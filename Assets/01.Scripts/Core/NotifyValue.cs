using System;

namespace Code.Core
{
    public class NotifyValue<T>
    {
        private T _value;

        public T Value
        {
            get => _value;

            set
            {
                if (Equals(_value, value))
                    return;
                
                _value = value;
                _onValueChanged?.Invoke(_value);
            }
        }

        private event Action<T> _onValueChanged;

        public NotifyValue(T initialValue = default) => _value = initialValue;

        public void Subscribe(Action<T> listener, bool invokeImmediately = true)
        {
            _onValueChanged += listener;

            if (invokeImmediately)
                listener(_value);
        }

        public void Unsubscribe(Action<T> listener) => _onValueChanged -= listener;

        public void Dispose() => _onValueChanged = null;
    }
}