using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGFirebaseDatabase
{
    [System.Serializable]
    public class FGFirebaseDataBase
    {
        [SerializeField]
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}