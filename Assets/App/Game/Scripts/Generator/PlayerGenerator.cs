using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System.Linq;
using UnityEngine.EventSystems;

namespace Culsu
{
    public class PlayerGenerator : CommonUIBase
    {
        [SerializeField, Disable]
        private PlayerBase _currentPlayer;

        [SerializeField, Disable]
        private DaichinoikariEffect _daichinoikariEffect;

        public PlayerBase CurrentPlayer
        {
            get { return _currentPlayer; }
        }


        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            //on tap handler
            CSGameManager.Instance.OnTapHandler += OnTap;
            //on kakusei skill attack handler
            CSPlayerSkillManager.Instance.GetSkill<PlayerKakuseiSkill>().OnAttackKakuseiSkillHandler +=
                OnAttackKakuseiSkill;
            CSPlayerSkillManager.Instance.GetSkill<PlayerDaichinoikariSkill>().OnExecuteSkillHandler +=
                OnExecuteDaichinoikari;
            CSPlayerSkillManager.Instance.GetSkill<PlayerDaichinoikariSkill>().OnEndSkillHandler +=
                OnEndDaichinoikari;
            //create daichinoikari effect
            _daichinoikariEffect = CSShurikenParticleManager.Instance.Create<DaichinoikariEffect>(CachedTransform);
            _daichinoikariEffect.Initialize();
            _daichinoikariEffect.gameObject.SetActive(false);
            //create player
            var playerData = userData.CurrentNationUserPlayerData;
            _currentPlayer = CSPlayerManager.Instance.Create(playerData.Id, CachedTransform);
            _currentPlayer.Initialize(playerData);
        }

        /// <summary>
        /// Raises the tap event.
        /// </summary>
        /// <param name="userData">User data.</param>
        private void OnTap(CSUserData userData, PointerEventData eventData)
        {
            _currentPlayer.OnTap();
        }

        /// <summary>
        /// On Attack Kakusei Skill
        /// </summary>
        /// <param name="kakuseiSkill"></param>
        private void OnAttackKakuseiSkill(PlayerKakuseiSkill kakuseiSkill)
        {
            _currentPlayer.OnTap();
        }

        /// <summary>
        /// On Execute Daichinoikari
        /// </summary>
        /// <param name="daichinoikari"></param>
        private void OnExecuteDaichinoikari(PlayerSkillBase daichinoikari)
        {
            _daichinoikariEffect.gameObject.SetActive(true);
            _daichinoikariEffect.Show();
        }

        /// <summary>
        /// On End Daichinoikari
        /// </summary>
        /// <param name="daichinoikari"></param>
        private void OnEndDaichinoikari(PlayerSkillBase daichinoikari)
        {
            _daichinoikariEffect.Hide();
            _daichinoikariEffect.gameObject.SetActive(false);
        }
    }
}