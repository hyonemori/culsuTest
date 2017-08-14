using System.Collections;
using System.Collections.Generic;
using Culsu;
using TKF;
using UnityEngine;

public class UpperLeftMenuController : MonoBehaviour, IInitializable<CSUserData>
{
    [SerializeField]
    private SettingButton _settingButton;

    [SerializeField]
    private HeroDictionaryButton _dictionaryButton;

    /// <summary>
    /// init
    /// </summary>
    /// <param name="data"></param>
    public void Initialize(CSUserData data)
    {
        _settingButton.Initialize(data);
        _dictionaryButton.Initialize(data);
    }
}