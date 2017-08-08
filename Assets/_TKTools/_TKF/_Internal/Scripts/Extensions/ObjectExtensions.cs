using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace TKF
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// Deeps the copy.
		/// </summary>
		/// <returns>The copy.</returns>
		/// <param name="target">Target.</param>
		public static object DeepCopy (this object target)
		{
			object result;
			BinaryFormatter b = new BinaryFormatter ();
			MemoryStream mem = new MemoryStream ();
		
			try {
				b.Serialize (mem, target);
				mem.Position = 0;
				result = b.Deserialize (mem);
			} finally {
				mem.Close ();
			}
		
			return result;
		}
	}
}