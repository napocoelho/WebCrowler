using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CrowlerLib
{
    [Serializable]
    public abstract class BindableBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        protected System.Collections.Generic.Dictionary<string, object> PropertyStore { get; private set; }


        #region Eventos;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                Dispatcher.Run(() => 
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                });
                
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public void OnPropertyChanging([CallerMemberName]string propertyName = "")
        {
            if (this.PropertyChanging != null)
            {
                Dispatcher.Run(() =>
                {
                    this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
                });
            }
        }

        public void OnAllPropertiesChanging()
        {
            Type type = this.GetType();
            System.Reflection.PropertyInfo[] propertiesInfo = type.GetProperties(System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance);

            foreach (System.Reflection.PropertyInfo info in propertiesInfo)
            {
                Dispatcher.Run(() =>
                {
                    this.OnPropertyChanging(info.Name);
                });
            }
        }

        public void OnAllPropertiesChanged()
        {
            Type type = this.GetType();
            System.Reflection.PropertyInfo[] propertiesInfo = type.GetProperties(System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance);

            foreach (System.Reflection.PropertyInfo info in propertiesInfo)
            {
                Dispatcher.Run(() =>
                {
                    this.OnPropertyChanged(info.Name);
                });
            }
        }

        #endregion Eventos;


        public BindableBase()
        {
            this.PropertyStore = new Dictionary<string, object>();
        }

        /// <summary>
        /// Obtém um valor da propriedade alvo.
        /// </summary>
        /// <typeparam name="T">Tipo de valor da propriedade</typeparam>
        /// <param name="propertyName">Nome da propriedade. Por padrão, deixar sem especificar</param>
        /// <returns>Retorna o valor da propriedade alvo.</returns>
        protected T Get<T>([CallerMemberName]string propertyName = null)
        {
            object oldObjectValue = null;
            T returnValue;

            if (!this.PropertyStore.TryGetValue(propertyName, out oldObjectValue))
            {
                returnValue = default(T);

                this.OnPropertyChanging(propertyName);
                this.PropertyStore[propertyName] = returnValue;
                this.OnPropertyChanged(propertyName);
            }
            else
            {
                returnValue = (T)oldObjectValue;
            }

            return returnValue;
        }

        /// <summary>
        /// Obtém um valor da propriedade alvo de forma assíncrona.
        /// </summary>
        /// <typeparam name="T">Tipo de valor da propriedade</typeparam>
        /// <param name="propertyName">Nome da propriedade. Por padrão, deixar sem especificar</param>
        /// <returns>Retorna o valor da propriedade alvo.</returns>
        protected T GetSync<T>([CallerMemberName]string propertyName = null)
        {
            lock (this)
            {
                return Get<T>(propertyName);
            }
        }

        /// <summary>
        /// Atribui um valor à propriedade alvo.
        /// </summary>
        /// <typeparam name="T">Tipo de valor da propriedade</typeparam>
        /// <param name="newValue">Valor a ser atribuido na propriedade</param>
        /// <param name="propertyName">Nome da propriedade. Por padrão, deixar sem especificar</param>
        /// <returns>Retorna True se a propriedade for alterada, False caso contrário. Caso o valor seja igual o existente, a propriedade não será alterada.</returns>
        protected bool Set<T>(T newValue, [CallerMemberName]string propertyName = null)
        {
            bool shouldModify = false;
            T oldValue = default(T);
            Type type = typeof(T);

            try
            {
                object oldObjectValue = null;
                object newObjectValue = newValue;


                // Obtendo valor antigo:
                if (!this.PropertyStore.TryGetValue(propertyName, out oldObjectValue))
                {
                    //this.PropertyStore[propertyName] = oldValue;
                    shouldModify = true;
                }
                else
                {
                    oldValue = (T)oldObjectValue;

                    // Comparando:
                    if (oldObjectValue == null && newObjectValue != null)
                    {
                        shouldModify = true;
                    }
                    else if (oldObjectValue != null && newObjectValue == null)
                    {
                        shouldModify = true;
                    }
                    else if (type.IsPrimitive)
                    {
                        shouldModify = (oldObjectValue != newObjectValue);
                    }
                    else
                    {
                        shouldModify = !object.ReferenceEquals(oldValue, newValue);
                    }
                }


                // Alterando valor, se necessário:
                if (shouldModify)
                {
                    this.OnPropertyChanging(propertyName);
                    this.PropertyStore[propertyName] = newValue;
                    this.OnPropertyChanged(propertyName);
                }

            }
            catch
            {
                shouldModify = true;
                this.OnPropertyChanging(propertyName);
                this.InitializeProperty(propertyName);
                this.OnPropertyChanged(propertyName);
            }

            return shouldModify;
        }

        /// <summary>
        /// Atribui um valor à propriedade alvo de forma assíncrona.
        /// </summary>
        /// <typeparam name="T">Tipo de valor da propriedade</typeparam>
        /// <param name="newValue">Valor a ser atribuido na propriedade</param>
        /// <param name="propertyName">Nome da propriedade. Por padrão, deixar sem especificar</param>
        /// <returns>Retorna True se a propriedade for alterada, False caso contrário. Caso o valor seja igual o existente, a propriedade não será alterada.</returns>
        protected bool SetSync<T>(T newValue, [CallerMemberName]string propertyName = null)
        {
            lock (this)
            {
                return this.Set<T>(newValue, propertyName);
            }
        }

        private void InitializeProperty(string propertyName)
        {
            if (!this.PropertyStore.ContainsKey(propertyName))
            {
                Type propertyType = this.GetType().GetProperty(propertyName).PropertyType;

                this.OnPropertyChanging(propertyName);

                if (propertyType.IsValueType)
                {
                    this.PropertyStore[propertyName] = Activator.CreateInstance(propertyType);
                }
                else
                {
                    this.PropertyStore[propertyName] = null;
                }

                this.OnPropertyChanged(propertyName);
            }
        }
        
        /// <summary>
        /// Desempenha um acesso tardio (Lazy Load) à propriedade alvo. É extremamente indicado para quando o valor da propriedade alvo necessitar de acesso adicional ao banco de dados.
        /// Esta técnica evita que vários relacionamentos sejam carregados antes de serem chamados, economizando chamadas ao banco de dados e evitando procedimentos não necessários.
        /// O procedimento especificado em [lazyCommand] será executado apenas na primeira vez em que houver uma chamada à propriedade alvo. 
        /// </summary>
        /// <param name="lazyCommand">Um closure a ser executado internamente. O closure será executado apenas no primeiro acesso à propriedade alvo</param>
        /// <param name="propertyName">Nome da propriedade alvo</param>
        /// <returns>Retorna um resultado do tipo especificado T</returns>
        protected T GetLazy<T>(Func<T> lazyCommand, [CallerMemberName]string propertyName = null)
        {
            object oldValue = null;
            T returnValue = default(T);

            if (!this.PropertyStore.TryGetValue(propertyName, out oldValue))
            {
                returnValue = lazyCommand();

                OnPropertyChanging();
                this.PropertyStore[propertyName] = returnValue;
                OnPropertyChanged();
            }
            else
            {
                returnValue = (T)oldValue;
            }

            return returnValue;
        }
    }
}