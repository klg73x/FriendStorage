using FriendStorage.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FriendStorage.UI.Wrapper
{
    public class ModelWrapper<T> : Observable
    {
        public ModelWrapper(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            Model = model;
        }

        public T Model { get; private set; }

        protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            var propertyInfo = Model.GetType().GetProperty(propertyName);
            return (TValue)propertyInfo.GetValue(Model);
        }

        protected void SetValue<TValue>(TValue value,
            [CallerMemberName] string propertyName = null)
        {
            var propertyInfo = Model.GetType().GetProperty(propertyName);
            var currentValue = propertyInfo.GetValue(Model);
            if (!Equals(currentValue, value))
            {
                propertyInfo.SetValue(Model, value);
                OnPropertyChanged(propertyName);
            }
        }

        protected void RegisterCollection<TWrapper, TModel>(ObservableCollection<TWrapper> wrapperCollection,
           List<TModel> modelCollection) where TWrapper : ModelWrapper<TModel>
        {
            wrapperCollection.CollectionChanged += (s, e) =>
            {
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems.Cast<TWrapper>())
                    {
                        modelCollection.Remove(item.Model);
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems.Cast<TWrapper>())
                    {
                        modelCollection.Add(item.Model);
                    }
                }
            };
        }
    }
}
