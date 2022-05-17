using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

namespace Solution_Tea.Helpers
{
    public class PropertyChangedNotification : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Fields
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();
        #endregion

        #region Protected

        protected void SetValue<T>(Expression<Func<T>> propertySelector, T value)
        {
            string propertyName = GetPropertyName(propertySelector);

            SetValue(propertyName, value);
        }

        protected void SetValue<T>(string propertyName, T value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            _values[propertyName] = value;
            NotifyPropertyChanged(propertyName);
        }

        protected T GetValue<T>(Expression<Func<T>> propertySelector)
        {
            string propertyName = GetPropertyName(propertySelector);

            return GetValue<T>(propertyName);
        }

        protected T GetValue<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            object value;
            if (!_values.TryGetValue(propertyName, out value))
            {
                value = default(T);
                _values.Add(propertyName, value);
            }

            return (T)value;
        }

      
        protected virtual string OnValidate(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            string error = string.Empty;
            object value = GetValue(propertyName);
            List<ValidationResult> results = new List<ValidationResult>(1);
            bool result = Validator.TryValidateProperty(
                value,
                new ValidationContext(this, null, null)
                {
                    MemberName = propertyName
                },
                results);

            if (!result)
            {
                ValidationResult validationResult = results.First();
                error = validationResult.ErrorMessage;
            }

            return error;
        }
        #endregion

        #region Public
        public ICommand ValidationCommand { get => new RelayCommand(x => Execute(), CanExecute); }
        #endregion

        protected void Execute()
        {
            foreach (KeyValuePair<string, object> item in _values)
            {
                OnValidate(item.Key);
            }
        }

        protected bool CanExecute(object sender)
        {
            return Validator.TryValidateObject(this, new ValidationContext(this, null, null), new List<ValidationResult>(), true) ?
                         true :
                         false;
        }

        #region Change Notification

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        protected void NotifyPropertyChanged<T>(Expression<Func<T>> propertySelector)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                string propertyName = GetPropertyName(propertySelector);
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Data Validation

        string IDataErrorInfo.Error
        {
            get
            {
                throw new NotSupportedException("IDataErrorInfo.Error is not supported, use IDataErrorInfo.this[propertyName] instead.");
            }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                return OnValidate(propertyName);
            }
        }

        #endregion

        #region Privates

        private string GetPropertyName(LambdaExpression expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException();
            }

            return memberExpression.Member.Name;
        }

        private object GetValue(string propertyName)
        {
            object value;
            if (!_values.TryGetValue(propertyName, out value))
            {
                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(GetType()).Find(propertyName, false);
                if (propertyDescriptor == null)
                {
                    throw new ArgumentException("Invalid property name", propertyName);
                }

                value = propertyDescriptor.GetValue(this);
                _values.Add(propertyName, value);
            }

            return value;
        }
        #endregion

        #region Debugging

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = $"Invalid property name: {propertyName}";

                if (ThrowOnInvalidPropertyName)
                {
                    throw new Exception(msg);
                }
                else
                {
                    Debug.Fail(msg);
                }
            }
        }

        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }
        #endregion
    }
}
