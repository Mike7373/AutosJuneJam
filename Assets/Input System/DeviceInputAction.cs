using System;
using UnityEngine.InputSystem;

namespace Input
{
    public interface InputBinder
    {
        public T ReadValue<T>();
        public bool IsInProgress();
        public void Start(object obj);
        public void Perform(object obj);
        public void Cancel();
        public void Bind(CharacterInputAction c);
        public void Unbind();

    }

    public class AIInputBinder : InputBinder
    {
        object value;
        bool isInProgress;
        CharacterInputAction binded;

        public void Start(object obj)
        {
            isInProgress = true;
            value = obj;
            binded?.started?.Invoke(obj);
        }

        public void Perform(object o)
        {
            isInProgress = true;
            value = o;
            binded?.performed?.Invoke(value);
        }

        public void Cancel()
        {
            isInProgress = false;
            binded?.canceled?.Invoke();
        }

        public void Bind(CharacterInputAction c)
        {
            binded = c;
        }

        public void Unbind()
        {
            binded = null;
        }

        public T ReadValue<T>()
        {
            object val = value ?? default(T);
            return (T) val;
        }

        public bool IsInProgress()
        {
            return isInProgress;
        }
    }
    

    public class NoOpInputBinder : InputBinder
    {
        public T ReadValue<T>()
        {
            return default(T);
        }

        public bool IsInProgress()
        {
            return false;
        }

        public void Start(object obj)
        {
            return;
        }

        public void Perform(object obj)
        {
            return;
        }

        public void Cancel()
        {
            return;
        }

        public void Bind(CharacterInputAction c)
        {
        }

        public void Unbind()
        {
            
        }
    }

    // TODO: Gli handlers sull'azione wrappata vanno rimossi.

    public class DeviceInputBinder : InputBinder
    {
        InputAction wrapped;
        CharacterInputAction binded;

        public DeviceInputBinder(InputAction toWrap)
        {
            wrapped = toWrap;
        }
        
        public T ReadValue<T>()
        {
            object val = wrapped.ReadValueAsObject() ?? default(T);
            return (T) val;        }

        public bool IsInProgress()
        {
            return wrapped.IsInProgress();
        }

        public void Start(object obj)
        {
            throw new NotImplementedException();
        }

        public void Perform(object obj)
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Bind(CharacterInputAction c)
        {
            binded = c;
            wrapped.performed += WrappedOnperformed;
            wrapped.started += WrappedOnStarted;
            wrapped.canceled += WrappedOnCanceled;
        }

        public void Unbind()
        {
            wrapped.performed -= WrappedOnperformed;
            wrapped.started -= WrappedOnStarted;
            wrapped.canceled -= WrappedOnCanceled;
        }

        void WrappedOnperformed(InputAction.CallbackContext obj)
        {
            binded.performed?.Invoke(obj.ReadValueAsObject());
        }

        void WrappedOnStarted(InputAction.CallbackContext obj)
        {
            binded.started?.Invoke(obj.ReadValueAsObject());
        }

        void WrappedOnCanceled(InputAction.CallbackContext obj)
        {
            binded.canceled?.Invoke();
        }
    }

    public class CharacterInputAction
    {
        public Action<object> performed;
        public Action<object> started;
        public Action    canceled;

        InputBinder binder;

        public CharacterInputAction(InputBinder inputBinder)
        {
            Bind(inputBinder);
        }

        public T ReadValue<T>()
        {
            return binder.ReadValue<T>();
        }

        public bool IsInProgress()
        {
            return binder.IsInProgress();
        }
        
        public void Start(object obj)
        {
            binder.Start(obj);
        }

        public void Perform(object obj)
        {
            binder.Perform(obj);
        }

        public void Cancel()
        {
            binder.Cancel();
        }

        public void Bind(InputBinder b)
        {
            binder?.Unbind();
            binder = b;
            b.Bind(this);
        }

    }
    


    
}