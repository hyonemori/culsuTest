using UnityEngine;
using System.Collections;

namespace TKF
{
	/// <summary>
	/// インデックスクラス 
	/// </summary>
	[System.Serializable]
	public struct MapIndex
	{
		/// <summary>
		/// Index Element 
		/// </summary>
		[SerializeField]
		public int x, y, z;

		/// <summary>
		/// Initializes a new instance of the <see cref="TKF.MapIndex"/> struct.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="z">The z coordinate.</param>
		public MapIndex (int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TKF.MapIndex"/> struct.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public MapIndex (int x, int y)
		{
			this.x = x;
			this.y = y;
			this.z = 0;
		}

		/// <summary>
		//objと自分自身が等価のときはtrueを返す
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="MapIndex"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current <see cref="MapIndex"/>;
		/// otherwise, <c>false</c>.</returns>
		public override bool Equals (object obj)
		{
			//objがnullか、型が違うときは、等価でない
			if (obj == null || this.GetType () != obj.GetType ()) {
				return false;
			}
			//この型が継承できないクラスや構造体であれば、次のようにできる
			//if (!(obj is TestClass))

			//Numberで比較する
			MapIndex index = (MapIndex)obj;
			return (x == index.x) && (y == index.y) && (z == index.z);
		}

		/// <summary>
		//Equalsがtrueを返すときに同じ値を返す
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode ()
		{
			return x + (10 * y) + (100 * z);
		}

		/// <summary>
		/// Log this instance.
		/// </summary>
		public Vector3 Log ()
		{
			return new Vector3 (x, y, z);
		}

		/// <param name="c1">C1.</param>
		/// <param name="c2">C2.</param>
		public static bool operator == (MapIndex c1, MapIndex c2)
		{
			//nullの確認（構造体のようにNULLにならない型では不要）
			//両方nullか（参照元が同じか）
			//(c1 == c2)とすると、無限ループ
			if (object.ReferenceEquals (c1, c2)) {
				return true;
			}
			//どちらかがnullか
			//(c1 == null)とすると、無限ループ
			if (((object)c1 == null) || ((object)c2 == null)) {
				return false;
			}

			return (c1.x == c2.x) && (c1.y == c2.y) && (c1.z == c2.z);
		}

		/// <param name="c1">C1.</param>
		/// <param name="c2">C2.</param>
		public static bool operator != (MapIndex c1, MapIndex c2)
		{
			return !(c1 == c2);
			//(c1 != c2)とすると、無限ループ
		}

		/// <param name="index_1">Index 1.</param>
		/// <param name="index_2">Index 2.</param>
		public static MapIndex operator+ (MapIndex index_1, MapIndex index_2)
		{
			return new MapIndex (index_1.x + index_2.x, index_1.y + index_2.y, index_1.z + index_2.z);
		}

		/// <param name="index_1">Index 1.</param>
		/// <param name="index_2">Index 2.</param>
		public static MapIndex operator- (MapIndex index_1, MapIndex index_2)
		{
			return new MapIndex (index_1.x - index_2.x, index_1.y - index_2.y, index_1.z - index_2.z);
		}
	}
}