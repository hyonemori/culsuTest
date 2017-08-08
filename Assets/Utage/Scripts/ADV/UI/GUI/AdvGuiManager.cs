//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Utage
{
	/// <summary>
	/// GUIの管理
	/// </summary>
	[AddComponentMenu("Utage/ADV/GuiManager")]
	public class AdvGuiManager : MonoBehaviour, IAdvCustomSaveDataIO
	{
		//管理対象のUI
		[SerializeField]
		List<GameObject> guiObjects = new List<GameObject>();
		Dictionary<string, AdvGuiBase> objects = new Dictionary<string, AdvGuiBase>();
		public Dictionary<string, AdvGuiBase> Objects{ get{ return objects;}}

		//
		void Awake()
		{
			foreach( var item in guiObjects)
			{
				if (objects.ContainsKey(item.name))
				{
				}
				else
				{
					objects.Add(item.name, new AdvGuiBase(item));
				}
			}
		}


		internal bool TryGet(string name, out AdvGuiBase gui)
		{
			return this.objects.TryGetValue(name, out gui);
		}

		//セーブデータのキー
		public string SaveKey { get{return "GuiManager";} }

		//クリアする(初期状態に戻す)
		public void OnClear()
		{
			foreach (var item in objects.Values)
			{
				item.Reset();
			}
		}

		const int Version = 0;
		//書き込み
		public void OnWrite(System.IO.BinaryWriter writer)
		{
			writer.Write(Version);
			writer.Write(objects.Values.Count);
			foreach (var key in objects.Keys)
			{
				writer.Write(key);
				byte[] buffer = objects[key].ToBuffer();
				writer.Write(buffer.Length);
				writer.Write(buffer);
			}
		}

		//読み込み
		public void OnRead(System.IO.BinaryReader reader)
		{
			//バージョンチェック
			int version = reader.ReadInt32();
			if (version == Version)
			{
				int count = reader.ReadInt32();
				for (int i = 0; i < count; ++i)
				{
					string key = reader.ReadString();
					int bufferLen = reader.ReadInt32();
					byte[] buffer = reader.ReadBytes(bufferLen);
					AdvGuiBase gui;
					if (this.objects.TryGetValue(key, out gui))
					{
						gui.ReadBuffer(buffer);
					}
					else
					{
						Debug.LogError(key + " is not found in GuiManager");
					}
				}
			}
			else
			{
				Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, version));
			}
		}
	}
}