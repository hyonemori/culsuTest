using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using TKF;
using System;

namespace TKF
{
    public class TKAppStateManager : SingletonMonoBehaviour<TKAppStateManager>
    {
        /// <summary>
        /// The current orientation.
        /// </summary>
        [SerializeField]
        private DeviceOrientation _currentOrientation;

        public DeviceOrientation CurrentOrientation
        {
            get { return _currentOrientation; }
        }

        [SerializeField]
        private NetworkReachability _networkReachability;

        public NetworkReachability NetworkReachability
        {
            get { return _networkReachability; }
        }

        [SerializeField, DisableAttribute]
        private bool _isPause;

        public bool IsPause
        {
            get { return _isPause; }
        }

        /// <summary>
        /// App Event Handlers 
        /// </summary>
        public event Action<DeviceOrientation> OnDeviceOrientationChangeHandler;

        public event Action OnApplicationResumptionHandler;
        public event Action OnApplicationPauseHandler;
        public event Action OnApplicationQuitHandler;
        public event Action OnApplicationFocusOnHandler;
        public event Action OnApplicationFocusOffHandler;
        public event Action OnNotReachableNetworkHandler;
        public event Action OnEnableReachableNetworkHandler;

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
            //ネットワークの状況を監視
            _networkReachability = Application.internetReachability;
            this
                .ObserveEveryValueChanged(network => Application.internetReachability)
                .Subscribe
                (
                    network =>
                    {
                        //log
                        Debug.Log("NetworkReachability:" + network);
                        //set reachability
                        _networkReachability = network;
                        //call
                        OnNetworkReachabilityChange(network);
                    })
                .AddTo(gameObject);
            //デバイスの向きを監視
            _currentOrientation = Input.deviceOrientation;
            this
                .ObserveEveryValueChanged(orien => Input.deviceOrientation)
                .Subscribe
                (
                    orien =>
                    {
                        //log
                        Debug.Log("DeviceOrientation:" + orien);
                        //set reachability
                        _currentOrientation = orien;
                        //call
                        OnDeviceOrientationChangeHandler.SafeInvoke(orien);
                    })
                .AddTo(gameObject);
        }

        /// <summary>
        /// Raises the network reachability change event.
        /// </summary>
        /// <param name="network">Network.</param>
        private void OnNetworkReachabilityChange(NetworkReachability network)
        {
            // ネットワークの状態を出力
            switch (network)
            {
                case NetworkReachability.NotReachable:
                    Debug.Log("ネットワークには到達不可");
                    OnNotReachableNetworkHandler.SafeInvoke();
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    Debug.Log("キャリアデータネットワーク経由で到達可能");
                    OnEnableReachableNetworkHandler.SafeInvoke();
                    break;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    Debug.Log("Wifiまたはケーブル経由で到達可能");
                    OnEnableReachableNetworkHandler.SafeInvoke();
                    break;
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            //一時停止
            if (hasFocus)
            {
                Debug.Log("フォーカスされました");
                OnApplicationFocusOnHandler.SafeInvoke();
            }
            else
            {
                Debug.Log("フォーカスが外れました");
                OnApplicationFocusOffHandler.SafeInvoke();
            }
        }

        /// <summary>
        //一時停止or再開時
        /// </summary>
        /// <param name="pauseStatus">If set to <c>true</c> pause status.</param>
        private void OnApplicationPause(bool pauseStatus)
        {
            //一時停止
            if (pauseStatus)
            {
                Debug.Log("アプリが一時停止しました");
                _isPause = true;
                OnApplicationPauseHandler.SafeInvoke();
            }
            //再開時
            else
            {
                Debug.Log("アプリが再開しました");
                _isPause = false;
                OnApplicationResumptionHandler.SafeInvoke();
            }
        }

        /// <summary>
        //終了処理
        /// </summary>
        private void OnApplicationQuit()
        {
            Debug.Log("アプリが終了しました");
            OnApplicationQuitHandler.SafeInvoke();
        }
    }
}