using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseDatabase;

namespace FGFirebaseTableData
{
    [System.Serializable]
    public class FGFirebaseTableDataBase<TData, TElement> : FGFirebaseDataBase, ISerializationCallbackReceiver
        where TData : FGFirebaseTableDataBase<TData, TElement>
        where TElement : FGFirebaseTableElementBase
    {
        [SerializeField]
        protected List<TElement> _tableList
            = new List<TElement>();

        public List<TElement> TableList
        {
            get { return _tableList; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TKFirebaseDataTable.TKFirebaseDataTableBase"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public FGFirebaseTableDataBase(string id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Raises the after deserialize event.
        /// </summary>
        public void OnAfterDeserialize()
        {
            _OnAfterDeserialize();
        }

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
        /// Ons the before serialize.
        /// </summary>
        protected virtual void _OnBeforeSerialize()
        {
        }
    }
}