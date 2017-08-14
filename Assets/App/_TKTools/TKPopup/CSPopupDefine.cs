using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class CSPopupDefine
    {
        public static readonly string BOSS_TIME_UP_POPUP_TITLE = "ボス撃退チャンス";
        public static readonly string BOSS_TIME_UP_POPUP_DESCRIPTION = "動画を見ることで制限時間が\nプラス5秒追加されます。\n動画を見ますか？";
        public static readonly string FAIRY_POPUP_TITLE = "貂蝉のプレゼント";

        public static readonly string FAIRY_POPUP_DESCRIPTION =
            "貂蝉からのプレゼント\n\n動画を見ることで、\n{0}をプレゼントを受け取ることができます。\n動画を見ますか？";

        public static readonly string FAIRY_SKILL_POPUP_DESCRIPTION =
            "貂蝉からのプレゼント\n\n動画を見ることで、\nスキル「{0}」を使用することができます。\n動画を見ますか？";

        public static readonly string FAIRY_REWARD_RECEIVE_POPUP_DESCRIPTION = "{0}:{1}をGETしました。\n受取を完了してください。";
        public static readonly string FAIRY_REWARD_RECEIVE_POPUP_TITLE = "{0}をGET!!";
        public static readonly string PRESTIGE_POPUP_TITLE = "プレステージ";

        public static readonly string PRESTIGE_POPUP_DESCRIPTION =
                "金印と神器だけを保持して、\nゲームをリスタートしますか？\n\n未知なるステージへ挑戦するためには\n神器の購入が必要不可欠になります。\nプレステージをすることで\n大量の金印をGETして神器を購入しましょう。\n\n現在ステージ：{0}\n獲得金印数 {1}個"
            ;

        public static readonly string NON_REWARD_PRESTIGE_POPUP_DESCRIPTION =
                "金印と神器だけを保持して、\nゲームをリスタートしますか？\n\n未知なるステージへ挑戦するためには\n神器の購入が必要不可欠になります。\nプレステージをすることで\n大量の金印をGETして神器を購入しましょう。\n\n現在ステージ：{0}"
            ;

        public static readonly string PRESTIGE_CONFIRM_POPUP_DESCRIPTION = "{0}でプレステージしますか？";
    }
}