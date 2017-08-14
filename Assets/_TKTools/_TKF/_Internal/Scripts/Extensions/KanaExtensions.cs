using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;

namespace TKF
{
    public static class KanaExtensions
    {
        /** 濁音 */
        private static readonly string DAKUON_HANDAKUON = "がぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽ";
        private static readonly string DAKUON = "がぎぐげござじずぜぞだぢづでどばびぶべぼ";
        private static readonly string HANDAKUON = "ぱぴぷぺぽ";
 
        /** 清音 */
        private static readonly string SEION = "かきくけこさしすせそたちつてとはひふへほはひふへほ";

        /// <summary>
        /// Converts the dakuon2 seion.
        /// </summary>
        /// <returns>The dakuon2 seion.</returns>
        /// <param name="str">String.</param>
        public static string ToSeion(this string str)
        {
            int size = DAKUON_HANDAKUON.Length;
            string result = str;
            for (int i = 0; i < size; i++)
            {
                string s1 = DAKUON_HANDAKUON[i].ToString();
                string s2 = SEION[i].ToString();
                result = result.Replace(s1, s2);
                result = result.Replace(s1.ToKatakana(), s2.ToKatakana());
            }
            return result;
        }

        /// <summary>
        /// Determines if is sonant the specified str.
        /// </summary>
        /// <returns><c>true</c> if is sonant the specified str; otherwise, <c>false</c>.</returns>
        /// <param name="str">String.</param>
        public static bool IsSonant(this string str)
        {
            return str.IsDakuon() || str.IsHandakuon();  
        }

        /// <summary>
        /// Determines if is dakuon the specified str.
        /// </summary>
        /// <returns><c>true</c> if is dakuon the specified str; otherwise, <c>false</c>.</returns>
        /// <param name="str">String.</param>
        public static bool IsDakuon(this string str)
        {
            int size = DAKUON.Length;
            string result = str;
            for (int i = 0; i < size; i++)
            {
                string s1 = DAKUON[i].ToString();
                if (s1 == str || s1.ToKatakana() == str)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if is dakuon the specified str.
        /// </summary>
        /// <returns><c>true</c> if is dakuon the specified str; otherwise, <c>false</c>.</returns>
        /// <param name="str">String.</param>
        public static bool IsHandakuon(this string str)
        {
            int size = HANDAKUON.Length;
            string result = str;
            for (int i = 0; i < size; i++)
            {
                string s1 = HANDAKUON[i].ToString();
                if (s1 == str || s1.ToKatakana() == str)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if is dakuten the specified input.
        /// </summary>
        /// <returns><c>true</c> if is dakuten the specified input; otherwise, <c>false</c>.</returns>
        /// <param name="input">Input.</param>
        public static bool EnableDakuten(this string input)
        {
            // UTF-8 NFD において他のカナ文字と結合して、濁点/半濁点文字となるもの
            char dakuten = '\x3099';   // U+3099: COMBINING KATAKANA-HIRAGANA VOICED SOUND MARK

            // NFD風に結合した後、NFCに戻す。
            string added = (input + dakuten).Normalize(NormalizationForm.FormC);

            // NFCにない場合(＝文字と濁点の組み合わせが存在しない場合)文字化けして2文字になる
            // 結合前と同じ文字数を示した場合は、その組み合わせが存在する
            if (input.Length == added.Length)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Converts to dakuten.
        /// </summary>
        /// <returns>The to dakuten.</returns>
        /// <param name="input">Input.</param>
        public static string ToDakuten(this string input)
        {
            // UTF-8 NFD において他のカナ文字と結合して、濁点/半濁点文字となるもの
            char dakuten = '\x3099';   // U+3099: COMBINING KATAKANA-HIRAGANA VOICED SOUND MARK

            // NFD風に結合した後、NFCに戻す。
            string added = (input + dakuten).Normalize(NormalizationForm.FormC);

            // NFCにない場合(＝文字と濁点の組み合わせが存在しない場合)文字化けして2文字になる
            // 結合前と同じ文字数を示した場合は、その組み合わせが存在する
            if (input.Length == added.Length)
            {
                return added;
            }
            return input;
        }

        /// <summary>
        /// Enables the sonant.
        /// </summary>
        /// <returns><c>true</c>, if sonant was enabled, <c>false</c> otherwise.</returns>
        /// <param name="input">Input.</param>
        public static bool EnableSonant(this string input)
        {
            return SEION.Contains(input) || SEION.ToKatakana().Contains(input); 
        }

        /// <summary>
        /// Determines if is handakuten the specified input.
        /// </summary>
        /// <returns><c>true</c> if is handakuten the specified input; otherwise, <c>false</c>.</returns>
        /// <param name="input">Input.</param>
        public static bool EnableHandakuten(this string input)
        {
            // UTF-8 NFD において他のカナ文字と結合して、濁点/半濁点文字となるもの
            char handakuten = '\x309A';   // U+309A: COMBINING KATAKANA-HIRAGANA SEMI-VOICED SOUND MARK

            // NFD風に結合した後、NFCに戻す。
            string added = (input + handakuten).Normalize(NormalizationForm.FormC);

            // NFCにない場合(＝文字と濁点の組み合わせが存在しない場合)文字化けして2文字になる
            // 結合前と同じ文字数を示した場合は、その組み合わせが存在する
            if (input.Length == added.Length)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Converts to handakuten.
        /// </summary>
        /// <returns>The to handakuten.</returns>
        /// <param name="input">Input.</param>
        public static string ToHandakuten(this string input)
        {
            // UTF-8 NFD において他のカナ文字と結合して、濁点/半濁点文字となるもの
            char handakuten = '\x309A';   // U+309A: COMBINING KATAKANA-HIRAGANA SEMI-VOICED SOUND MARK

            // NFD風に結合した後、NFCに戻す。
            string added = (input + handakuten).Normalize(NormalizationForm.FormC);

            // NFCにない場合(＝文字と濁点の組み合わせが存在しない場合)文字化けして2文字になる
            // 結合前と同じ文字数を示した場合は、その組み合わせが存在する
            if (input.Length == added.Length)
            {
                return added;
            }
            return input;
        }

        /// <summary>
        /// Tos the katakana.
        /// </summary>
        /// <returns>The katakana.</returns>
        /// <param name="s">S.</param>
        public static string ToKatakana(this string s)
        {
            return new string(s.Select(c => (c >= 'ぁ' && c <= 'ゖ') ? (char)(c + 'ァ' - 'ぁ') : c).ToArray());
        }

        /// <summary>
        /// Tos the hiragana.
        /// </summary>
        /// <returns>The hiragana.</returns>
        /// <param name="s">S.</param>
        public static string ToHiragana(this string s)
        {
            return new string(s.Select(c => (c >= 'ァ' && c <= 'ヶ') ? (char)(c + 'ぁ' - 'ァ') : c).ToArray());
        }

        /// <summary>
        /// 指定した Unicode 文字が、ひらがなかどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c がひらがなである場合は true。それ以外の場合は false。</returns>
        public static bool IsHiragana(char c)
        {
            //「ぁ」～「より」までと、「ー」「ダブルハイフン」をひらがなとする
            return ('\u3041' <= c && c <= '\u309F')
            || c == '\u30FC' || c == '\u30A0';
        }
    }
}