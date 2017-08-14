using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKF
{
    public class TKLocalObjectManagerBase<TManager,TObject> : TKObjectManagerBase<TManager,TObject> 
        where TManager : TKLocalObjectManagerBase<TManager,TObject>
        where TObject : UnityEngine.Object
    {
        /// <summary>
        /// Preload the specified onSucceed.
        /// </summary>
        /// <param name="onSucceed">On succeed.</param>
        public override IEnumerator Load_(System.Action<bool> onSucceed)
        {
            for (int i = 0; i < _objectList.Count; i++)
            {
                var obj = _objectList[i];
                _cache.SafeAdd(obj.name, obj);
            }
            yield break;
        }
    }
}