using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKF
{
    [System.Serializable]
    public abstract class TKObjectBasedOnData<TObject,TData> :  ISerializationCallbackReceiver
        where TObject : TKObjectBasedOnData<TObject,TData>, new()
    {
        /// <summary>
        /// Create the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public static TObject Create(TData data)
        {
            TObject obj = new TObject(); 
            return obj.Update(data);
        }

        /// <summary>
        /// Update the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public virtual TObject Update(TData data)
        {
            OnCreateOrUpdate(data); 
            return this as TObject;
        }

        /// <summary>
        /// Raises the update event.
        /// </summary>
        /// <param name="data">Data.</param>
        protected abstract void OnCreateOrUpdate(TData data);

        /// <summary>
        /// Raises the after deserialize event.
        /// </summary>
        public void OnAfterDeserialize()
        {
            _OnAfterDeserialize();  
        }

        /// <summary>
        /// Ons the after deserialize.
        /// </summary>
        protected virtual void _OnAfterDeserialize()
        {
        }

        /// <summary>
        /// Raises the before serialize event.
        /// </summary>
        public void OnBeforeSerialize()
        {
            _OnBeforeSerialize();     
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Culsu.CSUserDataBase"/> class.
        /// </summary>
        protected virtual void _OnBeforeSerialize()
        {
            
        }
    }
}