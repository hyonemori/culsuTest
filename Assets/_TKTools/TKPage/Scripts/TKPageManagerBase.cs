using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TKF;

namespace TKPage
{
    public class TKPageManagerBase<TPage> : SingletonMonoBehaviour<TKPageManagerBase<TPage>>
        where TPage : TKPageBase
    {
        [SerializeField]
        private GraphicRaycaster _graphicRaycaster;

        [SerializeField]
        private float _segueSpeed;

        public float SegueSpeed
        {
            get { return _segueSpeed; }
        }

        [SerializeField]
        private TPage _startUpPage;

        public TPage StartUpPage
        {
            get { return _startUpPage; }
        }

        [SerializeField]
        private List<TPage> _pages;

        [SerializeField, DisableAttribute]
        private TPage _currentPage;

        public TPage CurrentPage
        {
            get { return _currentPage; }
        }

        /// <summary>
        /// Opposit Direction Table
        /// </summary>
        private Dictionary<MoveDirection, MoveDirection> _oppositDirectionTable =
            new Dictionary<MoveDirection, MoveDirection>()
            {
                {MoveDirection.Down, MoveDirection.Up},
                {MoveDirection.Up, MoveDirection.Down},
                {MoveDirection.Left, MoveDirection.Right},
                {MoveDirection.Right, MoveDirection.Left},
                {MoveDirection.None, MoveDirection.None}
            };

        /// <summary>
        /// Widht
        /// </summary>
        protected float _width;

        /// <summary>
        /// Height
        /// </summary>
        protected float _height;

        /// <summary>
        /// Page Stack Log
        /// </summary>
        private Stack<TPage> _pageStackLog;

        /// <summary>
        /// Move Direction Stack Log
        /// </summary>
        private Stack<MoveDirection> _moveDirectionStackLog;

        /// <summary>
        /// ページ遷移開始時に呼ばれる
        /// </summary>
        public event Action<TPage, TPage> OnBeganPageSegueHandler;

        /// <summary>
        /// ページ遷移終了時に呼ばれる
        /// </summary>
        public event Action<TPage, TPage> OnEndPageSegueHandler;

        /// <summary>
        /// The root canvas.
        /// </summary>
        protected RectTransform _canvasRect;

        /// <summary>
        /// Class Name To Page
        /// </summary>
        protected Dictionary<Type, TPage> _typeToPage = new Dictionary<Type, TPage>();

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
            //get rect transform
            _canvasRect = GetComponent<RectTransform>();
            //witdh
            _width = _canvasRect.rect.width;
            //height
            _height = _canvasRect.rect.height;
            //page stack new
            _pageStackLog = new Stack<TPage>();
            //move direction stack new
            _moveDirectionStackLog = new Stack<MoveDirection>();
            //Initialize Page
            foreach (var page in _pages)
            {
                //init
                page.Initialize();
                if (page == _startUpPage)
                {
                    _startUpPage.OnShowBegan();
                    _currentPage = page;
                    _pageStackLog.Push(page);
                    _moveDirectionStackLog.Push(MoveDirection.None);
                }
                else
                {
                    page.CanvasGroup.alpha = 0;
                    page.CanvasGroup.blocksRaycasts = false;
                }
                //add 
                _typeToPage.Add(page.GetType(), page);
            }
        }

        /// <summary>
        /// １つ前のページに戻る
        /// </summary>
        public void Pop()
        {
            if (_pageStackLog.IsNullOrEmpty())
            {
                Debug.LogWarning("Do not back to page any more !");
                return;
            }
            TPage currentPage = _currentPage;
            TPage nextPage = _pageStackLog.Pop();
            if (currentPage == nextPage)
            {
                Debug.LogWarningFormat
                (
                    "Segue to Same Page CurrentPage:{0} NextPage:{1}",
                    currentPage.GetType(),
                    nextPage.GetType()
                );
                return;
            }
            MoveDirection direction = _oppositDirectionTable[_moveDirectionStackLog.Pop()];
            StartCoroutine(SegutAnimation_(currentPage, nextPage, direction));
        }


        /// <summary>
        /// Show the specified direction.
        /// </summary>
        /// <param name="direction">Direction.</param>
        public void Show<T>
        (
            MoveDirection showDirection = MoveDirection.None
        )
            where T : TPage
        {
            StartCoroutine(SegueCoroutine<T>(showDirection));
        }

        /// <summary>
        /// Segues the coroutine.
        /// </summary>
        /// <returns>The coroutine.</returns>
        /// <param name="pageType">Page type.</param>
        /// <param name="showDirection">Show direction.</param>
        protected IEnumerator SegueCoroutine<T>
        (
            MoveDirection showDirection
        )
            where T : TPage
        {
            TPage currentPage = _currentPage;
            TPage nextPage = null;
            Type type = typeof(T);
            //try get page
            if (_typeToPage.SafeTryGetValue(type, out nextPage) == false)
            {
                Debug.LogErrorFormat("Not Found Page Key:{0}", type);
                yield break;
            }
            if (currentPage == nextPage)
            {
                Debug.LogWarningFormat("Segue to Same Page Page:{0}", type);
                yield break;
            }
            //page stack
            _pageStackLog.Push(currentPage);
            //move direction stack
            _moveDirectionStackLog.Push(showDirection);
            //Segue
            yield return SegutAnimation_(currentPage, nextPage, showDirection);
        }

        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="nextPage"></param>
        /// <returns></returns>
        protected IEnumerator SegutAnimation_
        (
            TPage currentPage,
            TPage nextPage,
            MoveDirection showDirection
        )
        {
            //set current Page
            _currentPage = nextPage;
            //set graphic raycaster
            _graphicRaycaster.enabled = false;
            //call on began page
            OnBeganPageSegueHandler.SafeInvoke(currentPage, nextPage);
            //direction switch
            switch (showDirection)
            {
                case MoveDirection.None:
                    yield return DOTween.Sequence()
                        .OnStart
                        (
                            () =>
                            {
                                nextPage.CanvasGroup.alpha = 0f;
                                nextPage.rectTransform.localPosition = Vector3.zero;
                                currentPage.rectTransform.localPosition = Vector3.zero;
                                currentPage.OnCloseBegan();
                                nextPage.OnShowBegan();
                            })
                        .Join(nextPage.CanvasGroup.DOFade(1f, _segueSpeed))
                        .Join(currentPage.CanvasGroup.DOFade(0f, _segueSpeed))
                        .WaitForCompletion();
                    break;

                case MoveDirection.Down:
                    yield return DOTween.Sequence()
                        .OnStart
                        (
                            () =>
                            {
                                nextPage.CanvasGroup.alpha = 1f;
                                nextPage.rectTransform.SetLocalPositionY(_height);
                                nextPage.OnShowBegan();
                                currentPage.OnCloseBegan();
                            })
                        .Join(nextPage.rectTransform.DOLocalMoveY(0, _segueSpeed))
                        .Join(currentPage.rectTransform.DOLocalMoveY(-_height, _segueSpeed))
                        .SetEase(Ease.Linear)
                        .WaitForCompletion();
                    break;

                case MoveDirection.Up:
                    yield return DOTween.Sequence()
                        .OnStart
                        (
                            () =>
                            {
                                nextPage.CanvasGroup.alpha = 1f;
                                nextPage.rectTransform.SetLocalPositionY(-_height);
                                nextPage.OnShowBegan();
                                currentPage.OnCloseBegan();
                            })
                        .Join(nextPage.rectTransform.DOLocalMoveY(0, _segueSpeed))
                        .Join(currentPage.rectTransform.DOLocalMoveY(_height, _segueSpeed))
                        .SetEase(Ease.Linear)
                        .WaitForCompletion();
                    break;

                case MoveDirection.Left:
                    yield return DOTween.Sequence()
                        .OnStart
                        (
                            () =>
                            {
                                nextPage.CanvasGroup.alpha = 1f;
                                nextPage.rectTransform.SetLocalPositionX(_width);
                                nextPage.OnShowBegan();
                                currentPage.OnCloseBegan();
                            })
                        .Join(nextPage.rectTransform.DOLocalMoveX(0, _segueSpeed))
                        .Join(currentPage.rectTransform.DOLocalMoveX(-_width, _segueSpeed))
                        .SetEase(Ease.Linear)
                        .WaitForCompletion();
                    break;

                case MoveDirection.Right:
                    yield return DOTween.Sequence()
                        .OnStart
                        (
                            () =>
                            {
                                nextPage.CanvasGroup.alpha = 1f;
                                nextPage.rectTransform.SetLocalPositionX(-_width);
                                nextPage.OnShowBegan();
                                currentPage.OnCloseBegan();
                            })
                        .Join(nextPage.rectTransform.DOLocalMoveX(0, _segueSpeed))
                        .Join(currentPage.rectTransform.DOLocalMoveX(_width, _segueSpeed))
                        .SetEase(Ease.Linear)
                        .WaitForCompletion();
                    break;
            }
            currentPage.OnCloseEnded();
            nextPage.OnShowEnded();
            _graphicRaycaster.enabled = true;
            OnEndPageSegueHandler.SafeInvoke(currentPage, nextPage);
        }
    }
}