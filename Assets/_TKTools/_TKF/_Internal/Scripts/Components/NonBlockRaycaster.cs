using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TKF
{
    [RequireComponent(typeof(GraphicRaycaster))]
    public class NonBlockRaycaster : MonoBehaviour
    {
        [SerializeField]
        private GraphicRaycaster _selfRaycaster;

#if SYMBOL_DEBUG
        /// <summary>
        /// Start this instance.
        /// </summary>
        private void Start()
        {
            if (_selfRaycaster == null)
            {
                _selfRaycaster = GetComponent<GraphicRaycaster>();
            }
        }
	
        // Update is called once per frame
        private void Update()
        {
            if (_selfRaycaster.enabled == false)
            {
                _selfRaycaster.enabled = true;
            }
        }
#endif
    }
}
