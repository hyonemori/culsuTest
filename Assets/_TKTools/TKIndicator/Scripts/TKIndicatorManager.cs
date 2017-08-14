using UnityEngine;
using System.Collections;
using TKF;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace TKIndicator
{
    public class TKIndicatorManager : 
	LocalPoolableManagerBase<TKIndicatorManager,TKIndicatorBase,TKIndicatorPrefabReference>
    {
        [SerializeField]
        private Image _bgImage;
        [SerializeField]
        private GraphicRaycaster _selfGraphicRaycaster;
        [SerializeField]
        private Transform _parent;

        /// <summary>
        /// The graphic raycaster list.
        /// </summary>
        private List<GraphicRaycaster> _allGraphicRaycasterList = new List<GraphicRaycaster>();

        /// <summary>
        /// The graphic raycaster list.
        /// </summary>
        private List<GraphicRaycaster> _graphicRaycasterList;


        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Enables the raycaster.
        /// </summary>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public override void Initialize()
        {
            //self raycaster setting
            _selfGraphicRaycaster.enabled = false;
            //list init
            _graphicRaycasterList = ListPool<GraphicRaycaster>.Get();
        }

        /// <summary>
        /// Show the specified isRaycastBlock.
        /// </summary>
        /// <param name="isRaycastBlock">If set to <c>true</c> is raycast block.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T Create<T>(bool isRaycastBlock = true)
			where T : TKIndicatorBase
        {
            return Create<T>(isRaycastBlock, null);
        }

        /// <summary>
        /// Show the specified isRaycastBlock and onComplete.
        /// </summary>
        /// <param name="isRaycastBlock">If set to <c>true</c> is raycast block.</param>
        /// <param name="onComplete">On complete.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T Create<T>(
            bool isRaycastBlock,
            Action<T> onComplete = null
        )
			where T : TKIndicatorBase
        {
            //raycast setting
            _selfGraphicRaycaster.enabled = isRaycastBlock;
            //create 
            T indicator = Create<T>(_parent, default(Vector3));	
            //indicator show
            indicator.Show();
            //callback
            onComplete.SafeInvoke(indicator);
            //return
            return indicator;
        }

        /// <summary>
        /// Hide the specified indicator.
        /// </summary>
        /// <param name="indicator">Indicator.</param>
        public override void Remove(TKIndicatorBase indicator,bool isTransformChild = false)
        {
            //raycast setting
            _selfGraphicRaycaster.enabled = false;
            //indicator hide
            indicator.Hide();
            //base remove
            base.Remove(indicator);
        }

        /// <summary>
        /// Enables the raycaster.
        /// </summary>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        private void EnableRaycaster(bool enable)
        {
            if (enable == false)
            {
                //release
                ListPool<GraphicRaycaster>.Release(_graphicRaycasterList);
                for (int i = 0; i < _allGraphicRaycasterList.Count; i++)
                {
                    var raycaster = _allGraphicRaycasterList[i];
                    if (raycaster.enabled == true)
                    {
                        _graphicRaycasterList.Add(raycaster);
                        raycaster.enabled = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < _graphicRaycasterList.Count; i++)
                {
                    var raycaster = _graphicRaycasterList[i];
                    raycaster.enabled = true;
                }
            }
        }
    }
}