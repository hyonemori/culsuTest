using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace TKF
{
    public static class GameObjectExtensions
    {
#if UNITY_EDITOR
        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <returns>The component.</returns>
        /// <param name="go">Go.</param>
        /// <param name="toAdd">To add.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
        {
            return go.AddComponent<T>().GetCopyOf(toAdd) as T;
        }
#endif

        /// <summary>
        /// 自身に加え子階層のGameObjectのレイヤーを一度に設定します
        /// </summary>
        /// <param name="go">Go.</param>
        /// <param name="layer">Layer.</param>
        /// <param name="isChildren">If set to <c>true</c> is children.</param>
        public static void SetLayer(this GameObject go, int layer, bool isChildren = true)
        {
            go.layer = layer;

            if (isChildren)
            {
                foreach (Transform childTransform in go.transform)
                {
                    SetLayer(childTransform.gameObject, layer);
                }
            }
        }

        /// <summary>
        /// Safes the add component.
        /// </summary>
        /// <returns>The add component.</returns>
        /// <param name="go">Go.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T SafeAddComponent<T>(this GameObject go)
    where T : UnityEngine.Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
            {
                return go.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// 指定されたコンポーネントを返します 
        /// </summary>
        /// <returns>The get component.</returns>
        /// <param name="self">Self.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T SafeGetComponent<T>(this GameObject self) where T : Component
        {
            return self.GetComponent<T>() ?? self.AddComponent<T>();
        }

        public static T SafeGetComponent<T>(this Component self) where T : Component
        {
            return self.GetComponent<T>() ?? self.gameObject.AddComponent<T>();
        }

        /// <summary>
        ///すべての子オブジェクトを返します 
        /// </summary>
        /// <returns>The children.</returns>
        /// <param name="self">Self.</param>
        /// <param name="includeInactive">If set to <c>true</c> include inactive.</param>
        public static GameObject[] GetChildren(this GameObject self, bool includeInactive = false)
        {
            return self
            .GetComponentsInChildren<Transform>(includeInactive)
            .Where(c => c != self.transform)
            .Select(c => c.gameObject)
            .ToArray();
        }

        public static GameObject[] GetChildren(this Component self, bool includeInactive = false)
        {
            return self
            .GetComponentsInChildren<Transform>(includeInactive)
            .Where(c => c != self.transform)
            .Select(c => c.gameObject)
            .ToArray();
        }

        /// <summary>
        /// 孫オブジェクトを除くすべての子オブジェクトを返します
        /// </summary>
        /// <returns>The children without grandchildren.</returns>
        /// <param name="self">Self.</param>
        public static GameObject[] GetChildrenWithoutGrandchildren(this GameObject self)
        {
            var result = new List<GameObject>();
            foreach (Transform n in self.transform)
            {
                result.Add(n.gameObject);
            }
            return result.ToArray();
        }

        public static GameObject[] GetChildrenWithoutGrandchildren(this Component self)
        {
            var result = new List<GameObject>();
            foreach (Transform n in self.transform)
            {
                result.Add(n.gameObject);
            }
            return result.ToArray();
        }

        /// <summary>
        /// 自分自身を除くすべての子オブジェクトにアタッチされている指定されたコンポーネントをすべて返します
        /// </summary>
        /// <returns>The components in children without self.</returns>
        /// <param name="self">Self.</param>
        /// <param name="includeInactive">If set to <c>true</c> include inactive.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T[] GetComponentsInChildrenWithoutSelf<T>(this GameObject self, bool includeInactive = false) where T : Component
        {
            return self
            .GetComponentsInChildren<T>(includeInactive)
            .Where(c => self != c.gameObject)
            .ToArray();
        }

        public static T[] GetComponentsInChildrenWithoutSelf<T>(this Component self, bool includeInactive = false) where T : Component
        {
            return self
            .GetComponentsInChildren<T>(includeInactive)
            .Where(c => self.gameObject != c.gameObject)
            .ToArray();
        }

        /// <summary>
        /// 指定されたコンポーネントを削除します
        /// </summary>
        /// <param name="self">Self.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void RemoveComponent<T>(this GameObject self) where T : Component
        {
            Object.Destroy(self.GetComponent<T>());
        }

        public static void RemoveComponent<T>(this Component self) where T : Component
        {
            Object.Destroy(self.GetComponent<T>());
        }

        /// <summary>
        /// 指定されたコンポーネントを即座に削除します
        /// </summary>
        /// <param name="self">Self.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void RemoveComponentImmediate<T>(this GameObject self) where T : Component
        {
            GameObject.DestroyImmediate(self.GetComponent<T>());
        }

        public static void RemoveComponentImmediate<T>(this Component self) where T : Component
        {
            GameObject.DestroyImmediate(self.GetComponent<T>());
        }

        /// <summary>
        /// 指定されたコンポーネントをすべて削除します
        /// </summary>
        /// <param name="self">Self.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void RemoveComponents<T>(this GameObject self) where T : Component
        {
            foreach (var n in self.GetComponents<T>())
            {
                GameObject.Destroy(n);
            }
        }

        public static void RemoveComponents<T>(this Component self) where T : Component
        {
            foreach (var n in self.GetComponents<T>())
            {
                GameObject.Destroy(n);
            }
        }

        /// <summary>
        /// 指定されたコンポーネントがアタッチされている場合 true を返します
        /// </summary>
        /// <returns><c>true</c> if has component the specified self; otherwise, <c>false</c>.</returns>
        /// <param name="self">Self.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool HasComponent<T>(this GameObject self) where T : Component
        {
            return self.GetComponent<T>() != null;
        }

        public static bool HasComponent<T>(this Component self) where T : Component
        {
            return self.GetComponent<T>() != null;
        }

        /// <summary>
        /// 指定された名前で子オブジェクトを検索します
        /// </summary>
        /// <param name="self">Self.</param>
        /// <param name="name">Name.</param>
        public static Transform Find(this GameObject self, string name)
        {
            return self.transform.Find(name);
        }

        public static Transform Find(this Component self, string name)
        {
            return self.transform.Find(name);
        }

        /// <summary>
        /// 指定された名前で子オブジェクトを検索して GameObject 型で返します
        /// </summary>
        /// <returns>The game object.</returns>
        /// <param name="self">Self.</param>
        /// <param name="name">Name.</param>
        public static GameObject FindGameObject(this GameObject self, string name)
        {
            var result = self.transform.Find(name);
            return result != null ? result.gameObject : null;
        }

        public static GameObject FindGameObject(this Component self, string name)
        {
            var result = self.transform.Find(name);
            return result != null ? result.gameObject : null;
        }

        /// <summary>
        /// 指定された名前で子オブジェクトを検索して
        /// その子オブジェクトから指定されたコンポーネントを取得します
        /// </summary>
        /// <returns>The component.</returns>
        /// <param name="self">Self.</param>
        /// <param name="name">Name.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T FindComponent<T>(this GameObject self, string name) where T : Component
        {
            var t = self.transform.Find(name);
            if (t == null)
            {
                return null;
            }
            return t.GetComponent<T>();
        }

        public static T FindComponent<T>(this Component self, string name) where T : Component
        {
            var t = self.transform.Find(name);
            if (t == null)
            {
                return null;
            }
            return t.GetComponent<T>();
        }

        /// <summary>
        /// 指定された名前で子オブジェクトを深い階層まで検索して GameObject 型で返します
        /// </summary>
        /// <returns>The deep.</returns>
        /// <param name="self">Self.</param>
        /// <param name="name">Name.</param>
        /// <param name="includeInactive">If set to <c>true</c> include inactive.</param>
        public static GameObject FindDeep(this GameObject self, string name, bool includeInactive = false)
        {
            var children = self.GetComponentsInChildren<Transform>(includeInactive);
            foreach (var transform in children)
            {
                if (transform.name == name)
                {
                    return transform.gameObject;
                }
            }
            return null;
        }

        public static GameObject FindDeep(this Component self, string name, bool includeInactive = false)
        {
            var children = self.GetComponentsInChildren<Transform>(includeInactive);
            foreach (var transform in children)
            {
                if (transform.name == name)
                {
                    return transform.gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// 親オブジェクトを設定します
        /// </summary>
        /// <param name="self">Self.</param>
        /// <param name="parent">Parent.</param>
        public static void SetParent(this GameObject self, Component parent)
        {
            self.transform.SetParent(parent.transform);
        }

        public static void SetParent(this GameObject self, GameObject parent)
        {
            self.transform.SetParent(parent.transform);
        }

        public static void SetParent(this Component self, Component parent)
        {
            self.transform.SetParent(parent.transform);
        }

        public static void SetParent(this Component self, GameObject parent)
        {
            self.transform.SetParent(parent.transform);
        }

        /// <summary>
        /// 子オブジェクトが存在するかどうかを返します
        /// </summary>
        /// <returns><c>true</c> if has child the specified self; otherwise, <c>false</c>.</returns>
        /// <param name="self">Self.</param>
        public static bool HasChild(this GameObject self)
        {
            return 0 < self.transform.childCount;
        }

        public static bool HasChild(this Component self)
        {
            return 0 < self.transform.childCount;
        }

        /// <summary>
        /// 親オブジェクトが存在するかどうかを返します
        /// </summary>
        /// <returns><c>true</c> if has parent the specified self; otherwise, <c>false</c>.</returns>
        /// <param name="self">Self.</param>
        public static bool HasParent(this GameObject self)
        {
            return self.transform.parent != null;
        }

        public static bool HasParent(this Component self)
        {
            return self.transform.parent != null;
        }

        /// <summary>
        /// 指定されたインデックスの子オブジェクトを返します
        /// </summary>
        /// <returns>The child.</returns>
        /// <param name="self">Self.</param>
        /// <param name="index">Index.</param>
        public static GameObject GetChild(this GameObject self, int index)
        {
            var t = self.transform.GetChild(index);
            return t != null ? t.gameObject : null;
        }

        public static GameObject GetChild(this Component self, int index)
        {
            var t = self.transform.GetChild(index);
            return t != null ? t.gameObject : null;
        }

        /// <summary>
        /// 親オブジェクトを返します
        /// </summary>
        /// <returns>The parent.</returns>
        /// <param name="self">Self.</param>
        public static GameObject GetParent(this GameObject self)
        {
            var t = self.transform.parent;
            return t != null ? t.gameObject : null;
        }

        public static GameObject GetParent(this Component self)
        {
            var t = self.transform.parent;
            return t != null ? t.gameObject : null;
        }

        public static GameObject GetRoot(this GameObject self)
        {
            var root = self.transform.root;
            return root != null ? root.gameObject : null;
        }

        /// <summary>
        /// ルートとなるオブジェクトを返します.
        /// </summary>
        /// <returns>The root.</returns>
        /// <param name="self">Self.</param>
        public static GameObject GetRoot(this Component self)
        {
            var root = self.transform.root;
            return root != null ? root.gameObject : null;
        }

        /// <summary>
        /// レイヤー名を使用してレイヤーを設定します
        /// </summary>
        /// <param name="self">Self.</param>
        /// <param name="layerName">Layer name.</param>
        public static void SetLayer(this GameObject self, string layerName)
        {
            self.layer = LayerMask.NameToLayer(layerName);
        }

        public static void SetLayer(this Component self, string layerName)
        {
            self.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
}