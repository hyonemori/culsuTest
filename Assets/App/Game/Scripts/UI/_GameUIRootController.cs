using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;

namespace Culsu
{
    public class _GameUIRootController : MonoBehaviour
    {
        [SerializeField]
        private CSModalViewManager _modalViewManager;

        [SerializeField]
        private StageProgressController _stageProgressController;

        [SerializeField]
        private EnemyProgressController _enemyProgressController;

        [SerializeField]
        private EnemyParameterController _enemyParameterController;

        [SerializeField]
        private ScreenFrameController _screenFrameController;

        [SerializeField]
        private NationFlagController _nationFlagController;

        [SerializeField]
        private CurrencyParameterManager _currencyParameterManager;

        [SerializeField]
        private DamageGenerator _damageGenerator;

        [SerializeField]
        private UpperLeftMenuController _upperLeftMenuController;

        [SerializeField]
        private StageCutinManager _stageCutinManager;

        [SerializeField]
        private FairyController _fairyController;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            _enemyProgressController.Initialize(userData);
            _stageProgressController.Initialize(userData);
            _enemyParameterController.Initialize(userData);
            _screenFrameController.Initialize(userData);
            _nationFlagController.Initialize(userData);
            _currencyParameterManager.Initialize(userData);
            _modalViewManager.Initialize(userData);
            _upperLeftMenuController.Initialize(userData);
            _damageGenerator.Initialize(userData);
            _fairyController.Initialize(userData);
        }
    }
}