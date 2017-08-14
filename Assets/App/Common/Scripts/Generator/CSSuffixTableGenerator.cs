using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CielaSpike;
using System;
using TKF;
using Deveel.Math;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Culsu
{
    public class CSSuffixTableGenerator : MonoBehaviour
    {
        [SerializeField]
        private int _dataSize;
        [SerializeField]
        private string _resultText;

        /// <summary>
        /// Generate this instance.
        /// </summary>
        public void Generate()
        {
            Debug.Log("Suffix Table Create Start".Blue()); 
            //async start
            this.StartCoroutineAsync(Generate_(isSucceed =>
            {
                if (isSucceed)
                {
                    Debug.Log("Suffix Table Create Succeed".Green()); 
                }
            }));
        }

        /// <summary>
        // Generate this instance.
        /// </summary>
        private IEnumerator Generate_(Action<bool> onComplete = null)
        {
            BigInteger value = 1;
            for (int i = 0; i < _dataSize; i++)
            {
                value *= 10;
                int num = value.ToString().Length;
                if (i == _dataSize - 1)
                {
                    value.ToSuffixFromValue((integerOfDigits, suffix) =>
                    {
                        _resultText += "{";
                        _resultText += string.Format("{0},new CSSuffixData({1},\"{2}\")", num, integerOfDigits, suffix); 
                        _resultText += "}";
                    });
                }
                else
                {
                    value.ToSuffixFromValue((integerOfDigits, suffix) =>
                    {
                        _resultText += "{";
                        _resultText += string.Format("{0},new CSSuffixData({1},\"{2}\")", num, integerOfDigits, suffix); 
                        _resultText += "},\n";
                    });
                }
            }
            onComplete.SafeInvoke(true);
            yield break;
        }

        /* ---- ここから拡張コード ---- */
#if UNITY_EDITOR
        [CustomEditor(typeof(CSSuffixTableGenerator))] 
        public class CharacterEditor : Editor
        {
            /// <summary>
            /// Raises the inspector GU event.
            /// </summary>
            public override void OnInspectorGUI()
            {
                //draw default
                DrawDefaultInspector();
                // target は処理コードのインスタンスだよ！ 処理コードの型でキャストして使ってね！
                CSSuffixTableGenerator t = target as CSSuffixTableGenerator;
                //button
                if (GUILayout.Button("Generate"))
                {
                    t.Generate();
                }
            }
        }
#endif
    }
}
