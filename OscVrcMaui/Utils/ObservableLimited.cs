using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OscVrcMaui.Utils
{
    //Just limited and observable variant of queue
    public class ObservableLimited<T> : Queue<T>, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private int _limit = 100;
        public ObservableLimited(int limit=100)
        {
            _limit = limit;
        }
        public ObservableLimited()
        {
         
        }
        public ObservableLimited(IEnumerable<T> collection, int limit = 100)
        {
            foreach (var item in collection)
                base.Enqueue(item);
            _limit = limit;
        }

        public ObservableLimited(List<T> list, int limit = 100)
        {
            foreach (var item in list)
                base.Enqueue(item);
            _limit = limit;
        }

  
        public new virtual void Enqueue(T item)
        {
            base.Enqueue(item);
            if (Count >= _limit) {
                base.Dequeue();
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));

            }
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
         
        }
        public new virtual T Dequeue()
        {
         
               var item = base.Dequeue();
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));

            return item;

        }
        public new virtual void Clear()
        {
            base.Clear();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }



        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;


        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.RaiseCollectionChanged(e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(e);
        }


        protected virtual event PropertyChangedEventHandler PropertyChanged;


        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
           
                this.CollectionChanged?.Invoke(this, e);
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
          
                this.PropertyChanged?.Invoke(this, e);
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { this.PropertyChanged += value; }
            remove { this.PropertyChanged -= value; }
        }

    }
}
