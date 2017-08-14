using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpProgress : MonoBehaviour
{
    [SerializeField]
    private Image _progressImage;

    /// <summary>
    /// Initialize this instance.
    /// </summary>
    public void Initialize()
    {
    }

    /// <summary>
    /// Updates the value.
    /// </summary>
    /// <param name="ratio">Ratio.</param>
    public void UpdateValue(float ratio)
    {
        _progressImage.fillAmount = ratio;
    }
}
