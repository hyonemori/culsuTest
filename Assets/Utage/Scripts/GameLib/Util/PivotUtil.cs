//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;

namespace Utage
{
	public enum Pivot
	{
		/// <summary>右上</summary>
		TopLeft,
		/// <summary>上</summary>
		Top,
		/// <summary>左上</summary>
		TopRight,
		/// <summary>左</summary>
		Left,
		/// <summary>中央</summary>
		Center,
		/// <summary>右</summary>
		Right,
		/// <summary>左下</summary>
		BottomLeft,
		/// <summary>下</summary>
		Bottom,
		/// <summary>右下</summary>
		BottomRight,
	};

	/// <summary>
	/// ピボット処理
	/// </summary>
	public static class PivotUtil
	{
		public static Vector2 PivotEnumToVector2(Pivot pivot)
		{
			switch (pivot)
			{
				case Pivot.TopLeft:
					return new Vector2(0.0f, 1.0f);
				case Pivot.Left:
					return new Vector2(0.0f, 0.5f);
				case Pivot.BottomLeft:
					return new Vector2(0.0f, 0.0f);
				case Pivot.Top:
					return new Vector2(0.5f, 1.0f);
				case Pivot.Center:
					return new Vector2(0.5f, 0.5f);
				case Pivot.Bottom:
					return new Vector2(0.5f, 0.0f);
				case Pivot.TopRight:
					return new Vector2(1.0f, 1.0f);
				case Pivot.Right:
					return new Vector2(1.0f, 0.5f);
				case Pivot.BottomRight:
					return new Vector2(1.0f, 0.0f);
				default:
					return new Vector2(0.5f, 0.5f);
			}
		}
/*
		public static Vector3 GetPivotOffset(GameObject go, Vector2 pivot)
		{
			Renderer renderer = go.GetComponent<Renderer>();
			if (renderer)
			{
				return GetPivotOffset(renderer.bounds, pivot);
			}
			else
			{
				Debug.LogWarning("Not found render in " + go.name, go);
				return Vector3.zero;
			}
		}

		public static Vector3 GetPivotOffset(Bounds bounds, Vector2 pivot)
		{
			Vector2 size = bounds.size;
			return new Vector3(size.x*(pivot.x-0.5f), size.y*(pivot.y-0.5f), 0.0f);
		}*/
	}
}