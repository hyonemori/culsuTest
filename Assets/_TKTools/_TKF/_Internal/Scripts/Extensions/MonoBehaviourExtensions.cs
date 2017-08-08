using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.ComponentModel;

namespace TKF
{
    /// <summary>
    /// Monobehaviourのextensions.
    /// </summary>
    public static class MonoBehaviourExtensions
    {
#region Generic

        /// <summary>
        /// AssetBundleからロードしたAssetをInstantiateするとき
        /// (EditorでShaderがpinkになる現象を、Shader.Findで解消する。※ Editorのみ有効)
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="behaviour">Behaviour.</param>
        /// <param name="prefab">Prefab.</param>
        public static T InstantiateAsset <T>(this MonoBehaviour behaviour, T prefab)
			where T : MonoBehaviour
        {
            T go = MonoBehaviour.Instantiate(prefab); 
#if UNITY_EDITOR
            ShaderFind(go);
#endif
            return go;
        }

        /// <summary>
        /// Shaders the find.
        /// </summary>
        /// <param name="go">Go.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private static void ShaderFind<T>(T go)
			where T : MonoBehaviour
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);
            if (renderers.IsNullOrEmpty())
            {
                return;
            }
            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];
                for (int j = 0; j < renderer.materials.Length; j++)
                {
                    var mt = renderer.materials[j];
                    mt.shader = Shader.Find(mt.shader.name);
                }
            }
        }

#endregion

        /// <summary>
        /// Instantiate the specified behaiviour, parent, position and isLocal.
        /// </summary>
        /// <param name="behaiviour">Behaiviour.</param>
        /// <param name="parent">Parent.</param>
        /// <param name="position">Position.</param>
        /// <param name="isLocal">If set to <c>true</c> is local.</param>
        public static GameObject InstantiateWithParent(this MonoBehaviour behaiviour,
                                                       GameObject prefab,
                                                       Transform parent = null,
                                                       Vector3 position = default(Vector3),
                                                       bool isLocal = true)
        {
            GameObject obj = MonoBehaviour.Instantiate(prefab); 
            if (parent != null)
            {
                obj.transform.SetParent(parent, false);
                if (isLocal)
                {
                    obj.transform.position = parent.TransformPoint(position);
                }
                else
                {
                    obj.transform.position = position; 
                }
            }
            return obj;
        }

        /// <summary>
        /// 安全にアクティブ状態を設定する 
        /// </summary>
        /// <param name="go">Go.</param>
        /// <param name="isActive">If set to <c>true</c> is active.</param>
        public static void SafeSetActive(this MonoBehaviour go, bool isActive)
        {
            if (go != null)
            {
                go.SetActive(isActive);
            }
        }

        /// <summary>
        /// AssetBundleからロードしたAssetをInstantiateするとき
        /// (EditorでShaderがpinkになる現象を、Shader.Findで解消する。※ Editorのみ有効)
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="behaviour">Behaviour.</param>
        /// <param name="prefab">Prefab.</param>
        /// <param name="position">Position.</param>
        /// <param name="rotation">Rotation.</param>
        public static GameObject InstantiateAsset(this MonoBehaviour behaviour,
                                                  GameObject prefab,
                                                  Vector3 position,
                                                  Quaternion rotation)
        {
            return _InstantiateAsset(prefab, position, rotation);
        }

        /// <summary>
        /// Instantiates the asset.
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="Behaviour">Behaviour.</param>
        /// <param name="prefab">Prefab.</param>
        public static GameObject InstantiateAsset(this MonoBehaviour Behaviour, GameObject prefab)
        {
            if (prefab == null)
            {
                return null;
            }
            GameObject go = MonoBehaviour.Instantiate(prefab);
#if UNITY_EDITOR
            ShaderFind(go);
#endif
            return go;
        }

        /// <summary>
        /// Instantiates the asset.
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="prefab">Prefab.</param>
        /// <param name="position">Position.</param>
        /// <param name="rotation">Rotation.</param>
        private static GameObject _InstantiateAsset(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null)
            {
                return null;
            }

            var go = MonoBehaviour.Instantiate(prefab, position, rotation) as GameObject;
#if UNITY_EDITOR
            ShaderFind(go);
#endif
            return go;
        }

        /// <summary>
        /// Shaderの再設定
        /// 注意：Editor Only
        ///
        /// メモ：
        /// http://docs.unity3d.com/ScriptReference/Shader.Find.html
        /// "Shader.Find will work only in the editor"
        ///
        /// </summary>
        /// <param name="go">Go.</param>
        private static void ShaderFind(GameObject go)
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);

            foreach (Renderer renderer in renderers)
            {
                foreach (var mt in renderer.materials)
                {
                    mt.shader = Shader.Find(mt.shader.name);
                }
            }
        }

        /// <summary>
        /// SetActive
        /// </summary>
        /// <param name="behaviour">Behaviour.</param>
        /// <param name="flg">If set to <c>true</c> flg.</param>
        public static void SetActive(this MonoBehaviour behaviour, bool flg)
        {
            behaviour.gameObject.SetActive(flg);
        }
    }
}