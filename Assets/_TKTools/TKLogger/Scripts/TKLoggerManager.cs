using UnityEngine;
using System.Collections;
using TKF;

namespace TKLogger
{
    /// <summary>
    /// TK log manager.
    /// Manager for log saved local.
    /// </summary>
    public class TKLoggerManager :SingletonMonoBehaviour<TKLoggerManager>
    {
        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
#if UNITY_EDITOR
            string logFilePath = "Logs/";
#else
            string logFilePath = System.IO.Path.Combine(Application.persistentDataPath, "logs");
#endif
            //ログの初期化
            Logger.LoggerInit(Application.productName, logFilePath, true);
        }

        /// <summary>
        /// Raises the enable event.
        /// </summary>
        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        /// <summary>
        /// Raises the disable event.
        /// </summary>
        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        /// <summary>
        /// Handles the log.
        /// </summary>
        /// <param name="output">Output.</param>
        /// <param name="stack">Stack.</param>
        /// <param name="type">Type.</param>
        private void HandleLog(string output, string stack, LogType type)
        {
            switch (type)
            {
                case LogType.Log:
                    Logger.LogInfo(output);
                    break;

                case LogType.Error:
                    Logger.LogError(output);
                    break;

                case LogType.Exception:
                    Logger.LogError(output);
                    Logger.LogError(stack);
                    break;

                case LogType.Warning:
                    Logger.LogWarning(output);
                    break;

                default:
                    break;
            }
        }
    }
}