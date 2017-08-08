using System.Collections;
using DG.Tweening;
using TKF;
using UnityEngine;

namespace Culsu
{
    public class YumitaiSkillController : CommonUIBase
    {
        [SerializeField]
        private Transform _arrowParent;

        [SerializeField]
        private Transform _rightArrowReferencePoint;

        [SerializeField]
        private Transform _leftArrowReferencePoint;

        [SerializeField]
        private Vector2 _startHeightRange;

        [SerializeField]
        private Vector2 _targetRangeX;

        [SerializeField]
        private Vector2 _targetRangeY;

        [SerializeField]
        private Transform _targetTransform;

        [Header("弓の数")]
        [SerializeField, Range(0, 1000)]
        private int _yumiNum;

        [SerializeField]
        private Vector2 _curveRangeY;

        [SerializeField]
        private Vector2 _moveDurationRange;

        [SerializeField]
        private Ease _easeType;

        /// <summary>
        /// Skill Coroutine
        /// </summary>
        private Coroutine _skillCoroutine;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="userData"></param>
        public void Initialize(CSUserData userData)
        {
            CSPlayerSkillManager.Instance.GetSkill<PlayerYumitaiSkill>().OnExecuteSkillHandler += OnExecuteSkill;
            CSPlayerSkillManager.Instance.GetSkill<PlayerYumitaiSkill>().OnEndSkillHandler += OnEndSkill;
        }

        /// <summary>
        /// Execute Skill Data
        /// </summary>
        /// <param name="skillData"></param>
        private void OnExecuteSkill(PlayerSkillBase yumitaiSkill)
        {
            _skillCoroutine = StartCoroutine(ArrowCreate_());
        }

        /// <summary>
        /// Arrow Create
        /// </summary>
        /// <returns></returns>
        private IEnumerator ArrowCreate_()
        {
            for (int i = 0; i < _yumiNum; i++)
            {
                //is right
                bool isRight = Random.Range(0, 2) == 0;
                //arrow parent
                Transform arrowPoint = isRight ? _rightArrowReferencePoint : _leftArrowReferencePoint;
                //y positoin
                float yPos = Random.Range(_startHeightRange.x, _startHeightRange.y);
                //arrow positoin
                Vector3 arrowPos = arrowPoint.localPosition + new Vector3(0, yPos);
                //arrow create
                var arrow = CSCommonUIManager.Instance.Create<YumitaiSkillArrow>
                (
                    _arrowParent,
                    arrowPos
                );
                //set scale
                arrow.rectTransform.SetLocalScaleX(isRight ? 1 : -1);
                //init
                arrow.Initialize();
                //random range y
                float randomRangeY = Random.Range(_curveRangeY.x, _curveRangeY.y);
                //mid pos
                Vector3 midPos = Vector3.Lerp
                    (
                        arrow.transform.localPosition,
                        _targetTransform.localPosition,
                        0.5f
                    )
                    +
                    new Vector3(0, randomRangeY, 0);
                //target positoin
                Vector3 targetPosition = _targetTransform.localPosition +
                    new Vector3
                    (
                        Random.Range(_targetRangeX.x, _targetRangeX.y),
                        Random.Range(_targetRangeY.x, _targetRangeY.y)
                    );
                //paths
                Vector3[] paths = new Vector3[]
                {
                    arrow.rectTransform.localPosition,
                    midPos,
                    targetPosition
                };
                //move duration
                float moveDuration = Random.Range(_moveDurationRange.x, _moveDurationRange.y);
                //move
                DOTween.Sequence()
                    .Append
                    (
                        arrow.rectTransform
                            .DOLocalPath(paths, moveDuration, PathType.CatmullRom)
                            .SetLookAt(0.01f)
                            .SetEase(_easeType)
                    )
                    .Append(arrow.ArrowImage.DOFade(0f, 0.2f))
                    .OnComplete
                    (
                        () =>
                        {
                            CSCommonUIManager.Instance.Remove(arrow);
                        }
                    );
                //wait
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnEndSkill
        (
            PlayerSkillBase yumitaiSkill
        )
        {
            if (_skillCoroutine != null)
            {
                StopCoroutine(_skillCoroutine);
            }
        }
    }
}