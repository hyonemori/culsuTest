using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

namespace Culsu
{
    public class TapArea : TKGraphicBase, IPointerDownHandler
    {
        [SerializeField]
        private bool _isTapped;

        [SerializeField]
        private int _tapFrameCount;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
            Observable.EveryLateUpdate()
                .Subscribe
                (
                    _ =>
                    {
                        int tapFrameInterval = TKAppInfomationManager.Instance.Fps < 40f ? 1 : 2;
                        if ((_tapFrameCount + tapFrameInterval) < Time.frameCount)
                        {
                            _isTapped = false;
                        }
                    })
                .AddTo(gameObject);
        }

        /// <summary>
        /// Raises the pointer down event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isTapped)
            {
                return;
            }
            //position
            Vector2 pos = default(Vector2);
            //convert screen to local point in rectangle
            RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                rectTransform,
                eventData.position,
                rootCanvas.worldCamera,
                out pos
            );
            //show effect
            CSShurikenParticleManager.Instance.Create<TapEffect>(rectTransform, pos).Show();
            //tap send
            CSGameManager.Instance.OnTap(eventData);
            //tapped
            _isTapped = true;
            //tap frame count
            _tapFrameCount = Time.frameCount;
        }
    }
}