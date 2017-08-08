using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;
using TKF;
using UnityEngine.EventSystems;

namespace TKPopup
{
    /// <summary>
    /// Popup manager.
    /// </summary>
    public class TKPopupManagerBase : LocalPoolableManagerBase<TKPopupManagerBase, PopupBase, PopupPrefabReference>
    {
        /// <summary>
        /// このリストに登録されたGraphicRaycasterはポップアップ表示時にenableがfalseになります
        /// 理由：背後のCanvasのタッチ判定無効のため
        /// </summary>
        [SerializeField]
        protected List<GraphicRaycaster> _graphicRaycasterList;

        /// <summary>
        /// The popup background.
        /// </summary>
        [SerializeField]
        protected Image _popupBg;

        /// <summary>
        /// The popup canvas.
        /// </summary>
        [SerializeField]
        protected Canvas _popupCanvas;

        protected Transform _popupCanvasTransform
        {
            get { return _popupCanvas.transform; }
        }

        /// <summary>
        /// The hide popup canvas.
        /// </summary>
        [SerializeField]
        protected Canvas _hidePopupCanvas;

        protected Transform _hidePopupCanvasTransform
        {
            get { return _hidePopupCanvas.transform; }
        }

        /// <summary>
        /// The popup raycaster.
        /// </summary>
        protected GraphicRaycaster _popupRaycaster;

        /// <summary>
        /// The out of range popup view.
        /// </summary>
        [SerializeField]
        protected OutOfPopupRangeTapDetectionView _outOfRangePopupView;

        /// <summary>
        /// The duration of the open.
        /// </summary>
        [SerializeField, Range(0, 1)]
        protected float _openDuration = 0.3f;

        public float OpenDuration
        {
            get { return _openDuration; }
        }

        /// <summary>
        /// The duration of the close.
        /// </summary>
        [SerializeField, Range(0, 1)]
        protected float _closeDuration = 0.3f;

        public float CloseDuration
        {
            get { return _closeDuration; }
        }

        /// <summary>
        /// The show and close tween.
        /// </summary>
        protected Tween _openAndCloseTween;

        /// <summary>
        /// opne And Close Popup Bg Tween
        /// </summary>
        protected Tween _openAndClosePopupBgTween;

        /// <summary>
        /// The popup stack.
        /// </summary>
        protected Stack<PopupBase> _popupStack = new Stack<PopupBase>();

        /// <summary>
        /// The closing popup stack.
        /// </summary>
        protected Stack<PopupBase> _closingPopupStack = new Stack<PopupBase>();

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize()
        {
            //base init
            base.Initialize();
            //out of range pppup view init
            _outOfRangePopupView.Initialize(OnOutOfPopupRangeTappedHandler);
            //popup raycaster get
            _popupRaycaster = _popupCanvas.GetComponent<GraphicRaycaster>();
        }

        /// <summary>
        /// Create this instance.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T Create<T>()
            where T : PopupBase
        {
            //already exit popup
            if (_popupStack.Count >= 1)
            {
                //hide top layer popup
                HideTopLayerPopup();
            }
            else
            {
                //Raycalst Enable
                GraphicRaycasterEnable(false);
            }

            //Get Popup
            PopupBase popup = Get<T>();

            //Popup Initialize
            popup.Initialize(OnCloseBegan);

            //Popup Open
            popup.OpenBegan();

            //Popup Bg Enable
            _popupBg.enabled = true;

            //on show popup bg tween
            OnOpenPopupBgTween();

            //opne animation
            OnOpenPopupTween(popup)
                .OnComplete
                (
                    () =>
                    {
                        //Out Of Popup Range Tap View Enable
                        _outOfRangePopupView.Enable(true);
                        //open end
                        popup.OpenEnd();
                    });

            //Stack Push
            _popupStack.Push(popup);

            //Set Parent
            popup.transform.SetParent(_popupCanvas.transform, false);

            //return
            return popup as T;
        }

        /// <summary>
        /// Hides the top layer popup.
        /// </summary>
        protected void HideTopLayerPopup()
        {
            //Peek Popup
            PopupBase topLayerPopup = _popupStack.Peek();
            //Hide
            topLayerPopup.Hide();
            //set parent
            topLayerPopup.CachedTransform.SetParent(_hidePopupCanvasTransform);
            //set last sibling
            topLayerPopup.CachedTransform.SetAsLastSibling();
            //hide
            OnHidePopup(topLayerPopup);
        }

        /// <summary>
        /// Raises the close began event.
        /// </summary>
        protected void OnCloseBegan()
        {
            //Popup Touch Detection
            _popupRaycaster.enabled = false;
            //Stack Check
            if (_popupStack.IsNullOrEmpty() ||
                _openAndCloseTween.IsSafePlaying())
            {
                return;
            }
            //get stack
            PopupBase closingPopup = _popupStack.Pop();
            //close
            closingPopup.CloseBegan();
            //Add Closkg Popup
            _closingPopupStack.Push(closingPopup);
            //animation
            OnClosePopupTween(closingPopup)
                .OnStart
                (
                    () =>
                    {
                        //out of range popup false
                        _outOfRangePopupView.Enable(false);
                    })
                .OnComplete
                (
                    () =>
                    {
                        //out of range popup false
                        _outOfRangePopupView.Enable(true);
                        //close end
                        closingPopup.CloseEnd();
                        // on close
                        OnCloseFinished();
                    }
                );
        }

        /// <summary>
        /// Raises the out of popup range tapped handler event.
        /// </summary>
        protected void OnOutOfPopupRangeTappedHandler()
        {
            //Stack or Playing Check
            if (_popupStack.IsNullOrEmpty() ||
                _openAndCloseTween.IsSafePlaying())
            {
                return;
            }
            //get peek
            PopupBase topLayerPopup = _popupStack.Peek();
            //on out of range tapped
            topLayerPopup.OnOutOfRangeTapped();
        }

        /// <summary>
        /// Raises the close finished event.
        /// </summary>
        protected void OnCloseFinished()
        {
            //Popup Touch Detection
            _popupRaycaster.enabled = true;
            //Popup Remove
            if (_closingPopupStack.IsNullOrEmpty() == false &&
                _closingPopupStack.Peek() != null)
            {
                //closing popup
                PopupBase closingPopup = _closingPopupStack.Pop();
                //remove
                Remove(closingPopup);
            }

            //Popup Stack Check
            if (_popupStack.Count >= 1)
            {
                //Peek Popup
                PopupBase topLayerPopup = _popupStack.Peek();
                //Popup Show
                topLayerPopup.Show();
                //set parent
                topLayerPopup.CachedTransform.SetParent(_popupCanvasTransform);
                //popup show
                OnShowPopup(topLayerPopup);
                //return
                return;
            }
            //PopupFade
            OnClosePopupBgTween()
                .OnStart
                (
                    () =>
                    {
                        //out of range popup view disable
                        _outOfRangePopupView.Enable(false);
                    })
                .OnComplete
                (
                    () =>
                    {
                        //popup bg disable
                        _popupBg.enabled = false;
                        //Raycast Enable
                        GraphicRaycasterEnable(true);
                    }
                );
        }

        /// <summary>
        /// Raises the show popup tween event.
        /// </summary>
        protected virtual Tween OnOpenPopupTween(PopupBase popup)
        {
            _openAndCloseTween.SafeKill(true);
            _openAndClosePopupBgTween.SafeKill(true);
            return _openAndCloseTween = DOTween.Sequence()
                .Join(popup.CachedTransform.DOScale(Vector3.one, _openDuration).SetEase(Ease.OutBack))
                .Join(popup.CanvasGroup.DOFade(1, _openDuration));
        }

        /// <summary>
        /// Raises the close popup tween event.
        /// </summary>
        protected virtual Tween OnClosePopupTween(PopupBase popup)
        {
            _openAndCloseTween.SafeKill(true);
            _openAndClosePopupBgTween.SafeKill(true);
            return _openAndCloseTween = DOTween.Sequence()
                .Join(popup.CachedTransform.DOScale(Vector3.zero, _closeDuration).SetEase(Ease.InBack))
                .Join(popup.CanvasGroup.DOFade(0, _closeDuration));
        }

        /// <summary>
        /// Raises the shop popup event.
        /// </summary>
        /// <param name="popup">Popup.</param>
        protected virtual void OnShowPopup(PopupBase popup)
        {
            _openAndCloseTween.SafeKill(true);
            _openAndClosePopupBgTween.SafeKill(true);
            popup.CanvasGroup.alpha = 1;
        }

        /// <summary>
        /// Raises the shop popup event.
        /// </summary>
        /// <param name="popup">Popup.</param>
        protected virtual void OnHidePopup(PopupBase popup)
        {
            _openAndCloseTween.SafeKill(true);
            _openAndClosePopupBgTween.SafeKill(true);
        }

        /// <summary>
        /// Raises the show tween event.
        /// </summary>
        protected virtual Tween OnOpenPopupBgTween()
        {
            _openAndClosePopupBgTween.SafeKill(true);
            return _openAndClosePopupBgTween = _popupBg.DOFade(0.5f, 0.3f);
        }

        /// <summary>
        /// Raises the close tween event.
        /// </summary>
        protected virtual Tween OnClosePopupBgTween()
        {
            _openAndClosePopupBgTween.SafeKill(true);
            return _openAndClosePopupBgTween = _popupBg.DOFade(0f, 0.3f);
        }

        /// <summary>
        /// Graphics the raycaster enable.
        /// </summary>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        protected void GraphicRaycasterEnable(bool enable)
        {
            if (enable == false)
            {
                //get all graphic raycaster
                GraphicRaycaster[] raycasterAry =
                    GameObject.FindObjectsOfType(typeof(GraphicRaycaster)) as GraphicRaycaster[];
                for (int i = 0; i < raycasterAry.Length; i++)
                {
                    var raycaster = raycasterAry[i];
                    if (
                        raycaster == null ||
                        raycaster == _popupRaycaster ||
                        raycaster.enabled == false)
                    {
                        continue;
                    }
                    raycaster.enabled = false;
                    _graphicRaycasterList.SafeAdd(raycaster);
                }
            }
            else
            {
                for (int i = _graphicRaycasterList.Count - 1; i >= 0; i--)
                {
                    var gr = _graphicRaycasterList.SafeGetValue(i);
                    if (
                        gr == null ||
                        gr.gameObject == null)
                    {
                        _graphicRaycasterList.SafeRemove(gr);
                        continue;
                    }
                    gr.enabled = true;
                    _graphicRaycasterList.SafeRemove(gr);
                }
            }
        }
    }
}