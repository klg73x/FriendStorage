using FriendStorage.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FriendStorage.UI.Wrapper
{
    public class ModelWrapper<T> : Observable, IRevertibleChangeTracking
    {
        private Dictionary<string, object> _originalValues;
        public ModelWrapper(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            Model = model;
            _originalValues = new Dictionary<string, object>();
        }

        public T Model { get; private set; }

        public bool IsChanged { get { return _originalValues.Count > 0; } }

        public void AcceptChanges()
        {
            _originalValues.Clear();
            OnPropertyChanged(""); //This allows the whole view to take the new accepted values rather than iterating through all of what has changed. It is a bit of a cheat.
        }

        public void RejectChanges()
        {
            foreach (var originalValueEntry in _originalValues)
            {
                typeof(T).GetProperty(originalValueEntry.Key).SetValue(Model, originalValueEntry.Value);
            }
            _originalValues.Clear();
            OnPropertyChanged(""); //Again.. all databindings will now update their values with this
        }

        protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            var propertyInfo = Model.GetType().GetProperty(propertyName);
            return (TValue)propertyInfo.GetValue(Model);
        }

        protected TValue GetOriginalValue<TValue>(string propertyName)
        {
            return _originalValues.ContainsKey(propertyName)
                ? (TValue)_originalValues[propertyName]
                : GetValue<TValue>(propertyName);
        }

        protected bool GetIsChanged(string propertyName)
        {
            return _originalValues.ContainsKey(propertyName);
        }

        protected void SetValue<TValue>(TValue newValue,
            [CallerMemberName] string propertyName = null)
        {
            var propertyInfo = Model.GetType().GetProperty(propertyName);
            var currentValue = propertyInfo.GetValue(Model);
            if (!Equals(currentValue, newValue))
            {
                UpdateOriginalValue(currentValue, newValue, propertyName);
                propertyInfo.SetValue(Model, newValue);
                OnPropertyChanged(propertyName);
                OnPropertyChanged(propertyName + "IsChanged");
            }
        }

        private void UpdateOriginalValue(object currentValue, object newValue, string propertyName)
        {
            if (!_originalValues.ContainsKey(propertyName))
            {
                _originalValues.Add(propertyName, currentValue);
                OnPropertyChanged("IsChanged");
            }
            else
            {
                if (Equals(_originalValues[propertyName], newValue))
                {
                    _originalValues.Remove(propertyName);
                    OnPropertyChanged("IsChanged");
                }
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
