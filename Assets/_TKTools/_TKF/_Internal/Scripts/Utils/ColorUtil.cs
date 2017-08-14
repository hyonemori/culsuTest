using UnityEngine;
using System.Text;
using UnityEngine.UI;

namespace TKF
{
	public class ColorUtil
	{
		/// <summary>
		/// 構造体Colorから16進数文字列を返却
		/// </summary>
		/// <returns>The to string.</returns>
		/// <param name="c">C.</param>
		public static string ColorToHexString (
			Color c,
			bool isContainAlpha = true, 
			bool isAddSharp = true)
		{
			int r = (int)(c.r * 255f);
			int g = (int)(c.g * 255f);
			int b = (int)(c.b * 255f);
			int a = (int)(c.a * 255f);
			StringBuilder sb = new StringBuilder ();

			if (isAddSharp) {
				sb.Append ("#");
			}

			sb.Append (r.ToString ("X2"));
			sb.Append (g.ToString ("X2"));
			sb.Append (b.ToString ("X2"));

			if (isContainAlpha) {
				sb.Append (a.ToString ("X2"));
			}

			return sb.ToString ().ToLower ();
		}

		/// <summary>
		/// 0~255カラー指定から構造体Colorを返却
		/// </summary>
		/// <returns>The to color.</returns>
		/// <param name="r">The red component.</param>
		/// <param name="g">The green component.</param>
		/// <param name="b">The blue component.</param>
		/// <param name="a">The alpha component.</param>
		public static Color RgbaToColor (int r, int g, int b, int a = 255)
		{
			return new Color (
				(float)r / 255f,
				(float)g / 255f,
				(float)b / 255f,
				(float)a / 255f);
		}


		/// <summary>
		/// Colors to hex.
		/// </summary>
		/// <returns>The to hex.</returns>
		/// <param name="color">Color.</param>
		public static string ColorToHex (Color32 color)
		{
			string hex = color.r.ToString ("X2") + color.g.ToString ("X2") + color.b.ToString ("X2");
			return hex;
		}

		/// <summary>
		/// Hexs the color of the to.
		/// </summary>
		/// <returns>The to color.</returns>
		/// <param name="hex">Hex.</param>
		public static Color HexToColor (string hex)
		{
			byte r, g, b, a = 255;

			if (hex.StartsWith ("#")) {
				if (byte.TryParse (hex.Substring (1, 2), System.Globalization.NumberStyles.HexNumber, null, out r) == false
				    || byte.TryParse (hex.Substring (3, 2), System.Globalization.NumberStyles.HexNumber, null, out g) == false
				    || byte.TryParse (hex.Substring (5, 2), System.Globalization.NumberStyles.HexNumber, null, out b) == false) {
					Debug.LogError ("16進数ではない値が含まれています:" + hex);
					return Color.white;
				}

				if (hex.Length >= 8
				    && byte.TryParse (hex.Substring (7, 2), System.Globalization.NumberStyles.HexNumber, null, out a) == false) {
					Debug.LogError ("16進数ではない値が含まれています:" + hex);
					return Color.white;
				}
			} else {
				if (byte.TryParse (hex.Substring (0, 2), System.Globalization.NumberStyles.HexNumber, null, out r) == false
				    || byte.TryParse (hex.Substring (2, 2), System.Globalization.NumberStyles.HexNumber, null, out g) == false
				    || byte.TryParse (hex.Substring (4, 2), System.Globalization.NumberStyles.HexNumber, null, out b) == false) {
					Debug.LogError ("16進数ではない値が含まれています:" + hex);
					return Color.white;
				}

				if (hex.Length >= 7
				    && byte.TryParse (hex.Substring (6, 2), System.Globalization.NumberStyles.HexNumber, null, out a) == false) {
					Debug.LogError ("16進数ではない値が含まれています:" + hex);
					return Color.white;
				}
			}

			return new Color32 (r, g, b, a);
		}

		/// Sets the color recursive.
		/// </summary>
		/// <param name="gameObject">GameObject.</param>
		/// <param name="color">Color.</param>
		public static void SetColorRecursive (GameObject gameObject, Color color)
		{
			foreach (var child in gameObject.GetComponentsInChildren<Graphic>()) {
				float originalAlpha = child.color.a;
				child.color = new Color (color.r, color.g, color.b, originalAlpha);
			}
		}
	}
}