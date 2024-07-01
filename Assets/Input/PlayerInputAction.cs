using System;
using System.ComponentModel;
using UnityEngine.InputSystem;

namespace Input
{
    /*
     * Un'azione che può essere legata ad una InputAction, ma che può essere anche gestita
     * programmaticamente.
     *
     */
    public interface ICharacterAction<T>
    {
        public event Action<T> performed;
        public event Action<T> started;
        public event Action    canceled;

        public T ReadValue();
        public bool IsInProgress();

        public void Perform(T obj);
        public void Start(T obj);
        public void Cancel();
        
    }
    
    public class AInputAction<T> : ICharacterAction<T>
    {
        bool isInProgress;
        T value;

        public event Action<T> performed;
        public event Action<T> started;
        public event Action canceled;

        public T ReadValue()
        {
            return value;
        }

        public bool IsInProgress()
        {
            return isInProgress;
        }

        public void Perform(T obj)
        {
            isInProgress = true;
            value = obj;
            performed?.Invoke(obj);
        }

        public void Start(T obj)
        {
            isInProgress = true;
            value = obj;
            started?.Invoke(obj);
        }

        public void Cancel()
        {
            isInProgress = false;
            value = default(T);
            canceled?.Invoke();
        }
    }
    
    
    // TODO: Gli handlers sull'azione wrappata vanno rimossi.
    public class PlayerInputAction<T> : ICharacterAction<T> where T : struct
    {
        InputAction wrapped;
        PropertyChangedEventHandler a;
        
        event Action<T> _performed;
        event Action<T> _started;
        event Action _canceled;
        
        public event Action<T> performed
        {
            add => _performed += value;
            remove => _performed -= value;
        }
        
        public event Action<T> started
        {
            add => _started += value;
            remove => _started -= value;
        }
        
        public event Action canceled
        {
            add => _canceled += value;
            remove => _canceled -= value;
        }
        

        public PlayerInputAction(InputAction toWrap)
        {
            wrapped = toWrap;

            wrapped.performed += WrappedOnperformed;
            wrapped.started += WrappedOnStarted;
            wrapped.canceled += WrappedOnCanceled;
        }

        public void Perform(T obj)
        {
            _performed?.Invoke(obj);
        }

        public void Start(T obj)
        {
            _started?.Invoke(obj);
        }

        public void Cancel()
        {
            _canceled?.Invoke();
        }

        public T ReadValue()
        {
            return wrapped.ReadValue<T>();
        }

        public bool IsInProgress()
        {
            return wrapped.IsInProgress();
        }
        
        void WrappedOnperformed(InputAction.CallbackContext obj)
        {
            Perform(obj.ReadValue<T>());
        }

        void WrappedOnStarted(InputAction.CallbackContext obj)
        {
            Start(obj.ReadValue<T>());
        }

        void WrappedOnCanceled(InputAction.CallbackContext obj)
        {
            Cancel();
        }
        
    }
    
}