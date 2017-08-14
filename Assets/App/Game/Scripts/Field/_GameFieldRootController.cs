using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class _GameFieldRootController : MonoBehaviour
    {
        [SerializeField]
        private EnemyGenerator _enemyGenerator;

        [SerializeField]
        private PlayerGenerator _playerGenerator;

        [SerializeField]
        private HeroGenerator _heroGenerator;

        [SerializeField]
        private TapArea _tapArea;

        [SerializeField]
        private YumitaiSkillController _yumitaiSkillController;

        [SerializeField]
        private KaminariSkillController _kaminariSkillController;

        [SerializeField]
        private FieldBgController _fieldBgController;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData data)
        {
            _tapArea.Initialize();
            //initialize generators
            _playerGenerator.Initialize(data);
            _heroGenerator.Initialize(data);
            _enemyGenerator.Initialize(data);
            _yumitaiSkillController.Initialize(data);
            _kaminariSkillController.Initialize(data);
            _fieldBgController.Initialize(data);
        }
    }
}