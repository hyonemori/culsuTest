using UnityEngine;
using System.Collections;

namespace TKMaster
{
    [System.Serializable]
    public class RawDataBase : ISerializationCallbackReceiver
    {
        [SerializeField]
        private string id;

        public string Id
        {
            get { return id; }
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
        }
    }
}