using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TKF
{
    /// <summary>
    /// Weak reference cache.
    /// ValueはWeakReferenceで保持される
    /// </summary>
    public class WeakReferenceDictionary<TKey, TValue> where TValue : class
    {
        private Dictionary<TKey, WeakReference> _cahce = new Dictionary<TKey, WeakReference>();
        private int _prevGcCollectionCount = -1;
        private readonly int _removeNullValueCount;

        /// <summary>
        /// Determines whether this instance is null or empty.
        /// </summary>
        /// <returns><c>true</c> if this instance is null or empty; otherwise, <c>false</c>.</returns>
        public bool IsNullOrEmpty()
        {
            return _cahce.IsNullOrEmpty();
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get
            {
                return _cahce.Count(_ => _.Value.Target != null);
            }
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public IEnumerable<TKey> Keys
        {
            get
            {
                return _cahce.Where(_ => _.Value.Target != null).Select(_ => _.Key);
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public IEnumerable<TValue> Values
        {
            get
            {
                return _cahce.Values.Where(_ => _.Target != null).Select(_ => _.Target as TValue);
            }
        }

        public WeakReferenceDictionary(int removeNullValueCount = 100)
        {
            _removeNullValueCount = removeNullValueCount;
        }

        /// <summary>
        /// キャッシュ登録
        /// valueがnullの場合は登録されない
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void Add(TKey key, TValue value)
        {
            // キャッシュ数が一定以上になった場合かつ
            // GC.Collectが発生していた場合はCacheのValueがnullのものは削除しておく
            if (_cahce.Count >= _removeNullValueCount)
            {
                int gcCollectionCount = System.GC.CollectionCount(0);

                if (gcCollectionCount != _prevGcCollectionCount)
                {
                    _prevGcCollectionCount = gcCollectionCount;
                    RemoveNullValueCache();
                }
            }

            // valueがnullの場合は登録しない
            if (value == null)
            {
                return;
            }

            WeakReference weakRef;

            // 既に同じkeyが存在する場合は上書き
            if (_cahce.TryGetValue(key, out weakRef))
            {
                weakRef.Target = value;
                return;
            }

            weakRef = CreateWeakReference(value);
            _cahce.Add(key, weakRef);
        }

        /// <summary>
        /// キーが含まれているかどうかチェックします
        ///
        /// キーが含まれていたとしてもref.Targetがnullである可能性があるので
        /// その場合はキーが含まれていないと判定します
        /// </summary>
        /// <returns><c>true</c>, if key was containsed, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public bool ContainsKey(TKey key)
        {
            return _cahce.ContainsKey(key) && _cahce[key].Target != null;
        }

        /// <summary>
        /// キャッシュ取得
        /// </summary>
        /// <returns><c>true</c>, if get value was tryed, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = null;

            if (_cahce.Count == 0)
            {
                return false;
            }

            WeakReference weakRef;

            if (_cahce.TryGetValue(key, out weakRef))
            {
                value = weakRef.Target as TValue;
            }

            return value != null;
        }

        /// <summary>
        /// キャッシュ削除
        /// </summary>
        /// <param name="key">Key.</param>
        public bool Remove(TKey key)
        {
            return _cahce.Remove(key);
        }

        /// <summary>
        /// 全キャッシュ削除
        /// </summary>
        public void Clear()
        {
            _cahce.Clear();
        }

        /// <summary>
        /// キャッシュValueがnullのものを削除する
        /// </summary>
        private void RemoveNullValueCache()
        {
            if (_cahce.Count == 0)
            {
                return;
            }

            TKey[] removeKeys =
                _cahce.Where(_ => _.Value.Target == null).Select(_ => _.Key).ToArray();

            foreach (var removeKey in removeKeys)
            {
                Remove(removeKey);
            }
        }

        private WeakReference CreateWeakReference(TValue target)
        {
            return new WeakReference(target);
        }
    }
}