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
    public interface CharacterInputAction<T>
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
    
    public class AIInputAction<T> : CharacterInputAction<T> where T : struct
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
    public class DeviceInputAction<T> : CharacterInputAction<T> where T : struct
    {
        InputAction wrapped;
        PropertyChangedEventHandler a;
        
        public event Action<T> performed;
        public event Action<T> started;
        public event Action canceled;
        
        public DeviceInputAction(InputAction toWrap)
        {
            wrapped = toWrap;

            wrapped.performed += WrappedOnperformed;
            wrapped.started += WrappedOnStarted;
            wrapped.canceled += WrappedOnCanceled;
        }

        public void Perform(T obj)
        {
            performed?.Invoke(obj);
        }

        public void Start(T obj)
        {
            started?.Invoke(obj);
        }

        public void Cancel()
        {
            canceled?.Invoke();
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