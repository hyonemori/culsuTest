using System.Collections;
using System.Collections.Generic;
using TKURLScheme;
using UnityEngine;

namespace Culsu
{
    public class SettingScrollCharacterGoodsSegueElement : SettingScrollSegueElementBase
    {
        protected override void OnClick()
        {
            base.OnClick();
            TKURLSchemeManager.Instance.Open
                (CSLocalizeManager.Instance.GetString(TKLOCALIZE.SETTING_CHARACTER_GOODS_URL));
        }
    }
}