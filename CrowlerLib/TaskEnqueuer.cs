using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace CrowlerLib
{
    public class TaskEnqueuer : BindableBase
    {
        private object LOCK = new object();
        //private int maxSimultaneousThreads;
        
        private bool OnStop { get { return base.GetSync<bool>(); } set { base.SetSync(value); } }
        
        public ConcurrentQueue<Task> TasksQueue { get; private set; }
        public ConcurrentHashSet<Task> ProcessingTasks { get; private set; }

        public bool IsProcessing { get { return base.GetSync<bool>(); } private set { base.SetSync(value); } }
        public bool IsCompleted { get { return base.GetSync<bool>(); } private set { base.SetSync(value); } }

        public bool IsPaused { get { return base.GetSync<bool>(); } set { base.SetSync(value); } }
        public int MaxSimultaneousThreads { get { return base.GetSync<int>(); } set { base.SetSync(value); } }

        public int ProcessingTasksCount { get { return base.GetSync<int>(); } private set { base.SetSync(value); } }
        public int EnqueuedTasksCount { get { return base.GetSync<int>(); } private set { base.SetSync(value); } }


        public event EventHandler BeforeIsComplete;
        public event EventHandler AfterIsComplete;


        private void OnBeforeIsComplete()
        {
            if (BeforeIsComplete != null)
                BeforeIsComplete(this, new EventArgs());
        }

        private void OnAfterIsComplete()
        {
            if (AfterIsComplete != null)
                AfterIsComplete(this, new EventArgs());
        }


        public TaskEnqueuer()
        {
            this.TasksQueue = new ConcurrentQueue<Task>();
            this.ProcessingTasks = new ConcurrentHashSet<Task>();
            this.MaxSimultaneousThreads = 20;

            this.IsProcessing = false;
            this.OnStop = false;
        }

        public void Enqueue(Action action)
        {
            Task newTask = new Task(action);
            this.TasksQueue.Enqueue(newTask);
        }

        public void Enqueue(Task task)
        {
            this.TasksQueue.Enqueue(task);
        }

        public void Stop()
        {
            this.OnStop = true;
        }

        public void RunToComplete()
        {
            lock (LOCK)
            {
                if (this.IsProcessing)
                    return;

                this.Run(RunningMode.RunToComplete);
            }
        }

        public void RunUntilStop()
        {
            lock (LOCK)
            {
                if (this.IsProcessing)
                    return;

                this.Run(RunningMode.RunUntilStop);
            }
        }

        private void Run(RunningMode mode)
        {
            if (this.IsProcessing)
                return;

            try
            {
                this.IsPaused = false;
                this.OnStop = false;
                this.IsProcessing = true;
                this.IsCompleted = false;



                Action freeCompletedTasks = () =>
                    {
                        lock (LOCK)
                        {
                            // Requisitando mais espaço (verifica se há tasks finalizadas):
                            this.ProcessingTasks.RemoveWhere((task) =>
                            {
                                return task.IsCompleted;
                            });
                        }
                    };



                Task.Run(() =>
                    {
                        try
                        {
                            while (!this.OnStop)
                            {
                                if (this.IsPaused)
                                {
                                    Thread.Sleep(250);
                                    continue;
                                }


                                freeCompletedTasks();

                                // Procura e executa novas Tasks:
                                if (this.ProcessingTasks.Count < this.MaxSimultaneousThreads)
                                {
                                    Task newTask = null;

                                    if (this.TasksQueue.TryDequeue(out newTask))
                                    {
                                        this.ProcessingTasks.Add(newTask);
                                        newTask.Start();
                                    }
                                    else
                                    {
                                        freeCompletedTasks();
                                        Thread.Sleep(250);
                                    }
                                }

                                this.ProcessingTasksCount = this.ProcessingTasks.Count;
                                this.EnqueuedTasksCount = this.TasksQueue.Count;

                                Thread.Sleep(10);

                                // Verificando se não há mais Tasks requisitadas nem Tasks trabalhando e completa o trabalho;
                                if (mode == RunningMode.RunToComplete && this.ProcessingTasks.Count == 0 && this.TasksQueue.Count == 0)
                                {
                                    break;
                                }
                            }
                        }
                        finally
                        {
                            this.IsPaused = false;
                            this.OnStop = false;
                            this.IsProcessing = false;

                            this.OnBeforeIsComplete();
                            this.IsCompleted = true;
                            this.OnAfterIsComplete();
                        }
                    });


            }
            finally
            {

            }
        }
    }


    public enum RunningMode
    {
        /// <summary>
        /// Run while it exists Tasks running
        /// </summary>
        RunToComplete,

        /// <summary>
        /// Run until the method Stop is called
        /// </summary>
        RunUntilStop
    }
}