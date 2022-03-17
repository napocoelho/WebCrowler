using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace CrowlerLib
{
    public static class Dispatcher
    {
        public static Control Control { get; set; }
        
        private static void LoadControl()
        {
            if (Control == null && Application.OpenForms.Count > 0)
            {
                Control = Application.OpenForms[0];
            }
        }

        /*
        public static Action<object, EventArgs> Invoke(Action<object, EventArgs> action)
        {
            LoadControl();

            return (sender, evt) =>
                {
                    if (Control != null && Control.InvokeRequired)
                    {
                        Control.Invoke(action, sender, evt);
                    }
                    else
                    {
                        action(sender, evt);
                    }
                };
        }
        */

        public static void Run(Action action)
        {
            LoadControl();

            Action dispatcherAction = () =>
                {
                    if (Control != null && Control.InvokeRequired)
                    {
                        Control.Invoke(action);
                    }
                    else
                    {
                        action();
                    }
                };

            dispatcherAction();
        }
    }

    /*
    public class DispatcherResult
    {
        private Dispatcher Dispatcher { get; set; }

        public DispatcherResult(Dispatcher dispatcher, Action action)
        {
            this.Dispatcher = dispatcher;
        }

        public void WaitFinish()
        {

        }
    }
    */
}