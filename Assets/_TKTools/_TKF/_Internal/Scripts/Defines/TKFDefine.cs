using UnityEngine;
using System.Collections;

namespace TKF
{
    public class TKFDefine : MonoBehaviour
    {
        /// <summary>
        /// Delta time
        /// </summary>
        public static readonly float DELTA_TIME_60_FPS = 1f / 60f;

        /// <summary>
        /// User settings folder path
        /// </summary>
        public static readonly string USER_SETTINGS_FOLDER_PATH = "Assets/_TKTools/_TKUserSettings/";

        /// <summary>
        /// asset bundle version key 
        /// </summary>
        public static readonly string ASSET_BUNDLE_VERSION_KEY = "{0}_ASSET_BUNDLE_VERSION_KEY";

        /// <summary>
        /// asset bundle version key 
        /// </summary>
        public static readonly string MASTER_DATA_VERSION_KEY = "{0}_MASTER_DATA_VERSION_KEY";

        /// <summary>
        /// master data key
        /// </summary>
        public static readonly string MASTER_DATA_KEY = "{0}_MASTER_DATA_KEY";

        /// <summary>
        /// asset bundle version key 
        /// </summary>
        public static readonly string LOCALIZE_DATA_VERSION_KEY = "{0}_LOCALIZE_DATA_VERSION_KEY";

        /// <summary>
        /// master data key
        /// </summary>
        public static readonly string LOCALIZE_DATA_KEY = "{0}_LOCALIZE_DATA_KEY";

        /// <summary>
        /// table data version key 
        /// </summary>
        public static readonly string TABLE_DATA_VERSION_KEY = "{0}_TABLE_DATA_VERSION_KEY";

        /// <summary>
        /// table data key
        /// </summary>
        public static readonly string TABLE_DATA_KEY = "{0}_TABLE_DATA_KEY";

#if UNITY_EDITOR
        public static readonly string STREAMING_ASSETS_PATH = Application.dataPath + "/StreamingAssets";
#elif UNITY_IOS
        public static readonly string STREAMING_ASSETS_PATH = Application.dataPath + "/Raw";
#elif UNITY_ANDROID 
        public static readonly string STREAMING_ASSETS_PATH = Application.dataPath + "!/assets/";
#else
        public static readonly string STREAMING_ASSETS_PATH = Application.streamingAssetsPath;
#endif


        /// <summary>
        /// DefineSymbolType
        /// </summary>
        public enum DefineSymbolType
        {
            NONE,
            SYMBOL_DEBUG,
            SYMBOL_STAGING,
            SYMBOL_RELEASE
        }

        /// <summary>
        /// Orientation.
        /// </summary>
        public enum Orientation
        {
            NONE,
            VERTICAL,
            HORIZONTAL,
        }

        /// <summary>
        /// Development type.
        /// </summary>
        public enum DevelopmentType
        {
            NONE,
            DEVELOP,
            STAGING,
            RELEASE
        }

        /// <summary>
        /// Operation symbol type.
        /// </summary>
        public enum OperationType
        {
            NONE,
            ADDITION,
            SUBTRACTION,
            MULTIPLICATION,
            DIVISION
        }

        /// <summary>
        /// Local Storage Path Type
        /// </summary>
        public enum LocalStoragePathType
        {
            DATA,
            PERSISTENT,
            CACHE
        }
    }
}