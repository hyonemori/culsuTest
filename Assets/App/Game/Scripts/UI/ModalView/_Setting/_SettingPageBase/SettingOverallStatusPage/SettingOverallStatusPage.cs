using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace Culsu
{
    public class SettingOverallStatusPage : SettingPageBase
    {
        #region Current

        [SerializeField]
        private SettingOverallStatusElement _currentDps;

        [SerializeField]
        private SettingOverallStatusElement _currentTotalHeroLevel;

        [SerializeField]
        private SettingOverallStatusElement _currentCriticalPersentage;

        [SerializeField]
        private SettingOverallStatusElement _currentGoldValue;

        #endregion

        #region Total

        [SerializeField]
        private SettingOverallStatusElement _totalGoldValue;

        [SerializeField]
        private SettingOverallStatusElement _totalPrestigeNum;

        [SerializeField]
        private SettingOverallStatusElement _totalTapNum;

        [SerializeField]
        private SettingOverallStatusElement _totalEnemyDeadNum;

        [SerializeField]
        private SettingOverallStatusElement _totalBossDeadNum;

        [SerializeField]
        private SettingOverallStatusElement _maxStageNum;

        [SerializeField]
        private SettingOverallStatusElement _releaseHeroNum;

        [SerializeField]
        private SettingOverallStatusElement _releaseSecretTreasureNum;

        #endregion

        /// <summary>
        /// On Show Began
        /// </summary>
        public override void OnShowBegan()
        {
            base.OnShowBegan();
            //user data
            CSUserData userData = CSUserDataManager.Instance.Data;
            //===Current===//
            _currentDps.Initialize("ダメージ毎秒", userData.AllHerosDps.SuffixStr);
            _currentTotalHeroLevel.Initialize
            (
                "仲間全体レベル",
                userData.CurrentNationUserHeroList.Sum(s => s.CurrentLevel).ToString()
            );
            float criticalPersentage = CSParameterEffectManager.Instance.GetEffectedValue
            (
                CSDefineDataManager.Instance.Data.RawData.DEFAULT_PLAYER_CRITICAL_PROBABILITY,
                CSParameterEffectDefine.PLAYER_CRITICAL_PROBABILITY_ADDITION_PERCENT
            );
            _currentCriticalPersentage.Initialize
            (
                "急所攻撃率",
                string.Format("{0}%", criticalPersentage)
            );
            _currentGoldValue.Initialize("五銖銭", userData.GoldNum.SuffixStr);
            //===Total===//
            _totalGoldValue.Initialize
            (
                "五銖銭獲得数",
                string.Format
                (
                    "{0}五銖銭",
                    CSTrophyManager.Instance.GetTrophy(CSTrophyDefine.TROPHY_STACK_GET_GOLD).CurrentValue.SuffixStr
                )
            );
            _totalPrestigeNum.Initialize
            (
                "プレステージ回数",
                string.Format
                (
                    "{0}回",
                    CSTrophyManager.Instance.GetTrophy
                        (CSTrophyDefine.TROPHY_STACK_PRESTIGE_NUM).CurrentValue.Value.ToString()
                )
            );
            _totalTapNum.Initialize
            (
                "タップ回数",
                string.Format
                (
                    "{0}タップ",
                    CSTrophyManager.Instance.GetTrophy
                        (CSTrophyDefine.TROPHY_STACK_TAP_NUM).CurrentValue.Value.ToString()
                )
            );
            _totalEnemyDeadNum.Initialize
            (
                "モンスター撃退数",
                string.Format
                (
                    "{0}体",
                    CSTrophyManager.Instance.GetTrophy
                        (CSTrophyDefine.TROPHY_STACK_KILL_MONSTER_NUM).CurrentValue.Value.ToString()
                )
            );
            _totalBossDeadNum.Initialize
            (
                "ボス撃退数",
                string.Format
                (
                    "{0}体",
                    CSTrophyManager.Instance.GetTrophy
                        (CSTrophyDefine.TROPHY_STACK_KILL_BOSS_NUM).CurrentValue.Value.ToString()
                )
            );
            _maxStageNum.Initialize
            (
                "最高ステージ到達",
                string.Format
                (
                    "{0}ステージ",
                    CSTrophyManager.Instance.GetTrophy
                        (CSTrophyDefine.TROPHY_MAX_CLEAR_STAGE_NUM).CurrentValue.Value.ToString()
                )
            );
            _releaseHeroNum.Initialize
            (
                "仲間解放人数",
                string.Format
                (
                    "{0}人",
                    CSTrophyManager.Instance.GetTrophy
                        (CSTrophyDefine.TROPHY_MAX_COLLECT_HERO).CurrentValue.Value.ToString()
                )
            );
            _releaseSecretTreasureNum.Initialize
            (
                "神器獲得数",
                string.Format
                (
                    "{0}コ",
                    CSTrophyManager.Instance.GetTrophy
                        (CSTrophyDefine.TROPHY_STACK_GET_SECRET_TREASURE).CurrentValue.Value.ToString()
                )
            );
        }
    }
}