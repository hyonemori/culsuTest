using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace TKF
{
    public static class TKCacheAndDelete
    {

        private const string CLEAR_CACHE_COMMAND_NAME = "Tools/TKTools/TKClearAndDelete/TKCacheClear";
        private const string DELETE_PLAYER_PREFS_COMMAND_NAME = "Tools/TKTools/TKClearAndDelete/TKPlayerPrefsDelete";

        [MenuItem(CLEAR_CACHE_COMMAND_NAME)]
        public static void ClearCache()
        {
            if (!CanClearCache())
            {
                return;
            }

            Caching.CleanCache();
        }

        [MenuItem(DELETE_PLAYER_PREFS_COMMAND_NAME)]
        public static void DeletePlayerPrefs()
        {
            if (!CanDeletePlayerPrefs())
            {
                return;
            }
            PlayerPrefs.DeleteAll();
        }

        [MenuItem(CLEAR_CACHE_COMMAND_NAME, true)]
        private static bool CanClearCache()
        {
            return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
        }

        [MenuItem(DELETE_PLAYER_PREFS_COMMAND_NAME, true)]
        private static bool CanDeletePlayerPrefs()
        {
            return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
        }
    }
}