using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

// /////////////////////////////////////////////////////////////////////////////////////////
//                              More Effective Coroutines Pro
//                                        v2.03.3
// 
// This is an improved implementation of coroutines that boasts zero per-frame memory allocations,
// runs about twice as fast as Unity's built in coroutines and has a range of extra features.
// 
// For manual, support, or upgrade guide visit http://trinary.tech/
// 
// Created by Teal Rogers
// Trinary Software
// All rights preserved
// trinaryllc@gmail.com
// /////////////////////////////////////////////////////////////////////////////////////////

namespace MovementEffects
{
    public class Timing : MonoBehaviour
    {
        public enum DebugInfoType
        {
            None,
            SeperateCoroutines,
            SeperateTags
        }

        /// <summary>
        /// The time between calls to SlowUpdate.
        /// </summary>
        [Tooltip("How quickly the SlowUpdate segment ticks.")]
        public float TimeBetweenSlowUpdateCalls = 1f / 7f;
        /// <summary>
        /// The amount that each coroutine should be seperated inside the Unity profiler. NOTE: When the profiler window
        /// is not open this value is ignored and all coroutines behave as if "None" is selected.
        /// </summary>
        [Tooltip("How much data should be sent to the profiler window when it's open.")]
        public DebugInfoType ProfilerDebugAmount = DebugInfoType.SeperateCoroutines;
        /// <summary>
        /// Whether the manual timeframe should automatically trigger during the update segment.
        /// </summary>
        [Tooltip("When using manual timeframe, should it run automatically after the update loop or only when TriggerManualTimframeUpdate is called.")]
        public bool AutoTriggerManualTimeframe = true;
        /// <summary>
        /// The number of coroutines that are being run in the Update segment.
        /// </summary>
        [Tooltip("A count of the number of Update coroutines that are currently running."), Space(12)]
        public int UpdateCoroutines;
        /// <summary>
        /// The number of coroutines that are being run in the FixedUpdate segment.
        /// </summary>
        [Tooltip("A count of the number of FixedUpdate coroutines that are currently running.")]
        public int FixedUpdateCoroutines;
        /// <summary>
        /// The number of coroutines that are being run in the LateUpdate segment.
        /// </summary>
        [Tooltip("A count of the number of LateUpdate coroutines that are currently running.")]
        public int LateUpdateCoroutines;
        /// <summary>
        /// The number of coroutines that are being run in the SlowUpdate segment.
        /// </summary>
        [Tooltip("A count of the number of SlowUpdate coroutines that are currently running.")]
        public int SlowUpdateCoroutines;
        /// <summary>
        /// The number of coroutines that are being run in the RealtimeUpdate segment.
        /// </summary>
        [Tooltip("A count of the number of RealtimeUpdate coroutines that are currently running.")]
        public int RealtimeUpdateCoroutines;
        /// <summary>
        /// The number of coroutines that are being run in the EditorUpdate segment.
        /// </summary>
        [Tooltip("A count of the number of EditorUpdate coroutines that are currently running.")]
        public int EditorUpdateCoroutines;
        /// <summary>
        /// The number of coroutines that are being run in the EditorSlowUpdate segment.
        /// </summary>
        [Tooltip("A count of the number of EditorSlowUpdate coroutines that are currently running.")]
        public int EditorSlowUpdateCoroutines;
        /// <summary>
        /// The number of coroutines that are being run in the EndOfFrame segment.
        /// </summary>
        [Tooltip("A count of the number of EndOfFrame coroutines that are currently running.")]
        public int EndOfFrameCoroutines;
        /// <summary>
        /// The number of coroutines that are being run in the ManualTimeframe segment.
        /// </summary>
        [Tooltip("A count of the number of ManualTimeframe coroutines that are currently running.")]
        public int ManualTimeframeCoroutines;

        /// <summary>
        /// The time in seconds that the current segment has been running.
        /// </summary>
        [HideInInspector] 
        public double localTime;
        /// <summary>
        /// The time in seconds that the current segment has been running.
        /// </summary>
        public static float LocalTime { get { return (float)Instance.localTime; } }
        /// <summary>
        /// The amount of time in fractional seconds that elapsed between this frame and the last frame.
        /// </summary>
        [HideInInspector] 
        public float deltaTime;
        /// <summary>
        /// The amount of time in fractional seconds that elapsed between this frame and the last frame.
        /// </summary>
        public static float DeltaTime { get { return Instance.deltaTime; } }
        /// <summary>
        /// When defined, all errors from inside coroutines will be passed into this function instead of falling through to the Unity console.
        /// </summary>
        public System.Action<System.Exception> OnError;
        /// <summary>
        /// When defined, this function will be called every time manual timeframe needs to be set. The last manual timeframe time is passed in, and
        /// the new manual timeframe time needs to be returned. If this function is left as null, manual timeframe will be set to the current Time.time.
        /// </summary>
        public System.Func<double, double> SetManualTimeframeTime;
        /// <summary>
        /// Used for advanced coroutine control.
        /// </summary>
        public static System.Func<IEnumerator<float>, CoroutineHandle, IEnumerator<float>> ReplacementFunction;
        /// <summary>
        /// This event fires just before each segment is run.
        /// </summary>
        public static event System.Action OnPreExecute;
        /// <summary>
        /// You can use "yield return Timing.WaitForOneFrame;" inside a coroutine function to go to the next frame. 
        /// This is equalivant to "yeild return 0f;"
        /// </summary>
        public readonly static float WaitForOneFrame = 0f;


        private bool _runningUpdate;
        private bool _runningSlowUpdate;
        private bool _runningFixedUpdate;
        private bool _runningLateUpdate;
        private bool _runningRealtimeUpdate;
        private bool _runningEditorUpdate;
        private bool _runningEditorSlowUpdate;
        private int _nextUpdateProcessSlot;
        private int _nextLateUpdateProcessSlot;
        private int _nextFixedUpdateProcessSlot;
        private int _nextSlowUpdateProcessSlot;
        private int _nextRealtimeUpdateProcessSlot;
        private int _nextEditorUpdateProcessSlot;
        private int _nextEditorSlowUpdateProcessSlot;
        private int _nextEndOfFrameProcessSlot;
        private int _nextManualTimeframeProcessSlot;
        private double _lastUpdateTime;
        private double _lastLateUpdateTime;
        private double _lastFixedUpdateTime;
        private double _lastSlowUpdateTime;
        private double _lastRealtimeUpdateTime;
        private double _lastEditorUpdateTime;
        private double _lastEditorSlowUpdateTime;
        private double _lastManualTimeframeTime;
        private ushort _framesSinceUpdate;
        private ushort _expansions = 1;
        private byte _instanceID;
        private bool _EOFPumpRan;

        private readonly WaitForEndOfFrame _EOFWaitObject = new WaitForEndOfFrame();
        private readonly Dictionary<CoroutineHandle, HashSet<ProcessData>> _waitingTriggers = new Dictionary<CoroutineHandle, HashSet<ProcessData>>();
        private readonly Queue<System.Exception> _exceptions = new Queue<System.Exception>();
        private readonly Dictionary<CoroutineHandle, ProcessIndex> _handleToIndex = new Dictionary<CoroutineHandle, ProcessIndex>();
        private readonly Dictionary<ProcessIndex, CoroutineHandle> _indexToHandle = new Dictionary<ProcessIndex, CoroutineHandle>();
        private readonly Dictionary<ProcessIndex, string> _processTags = new Dictionary<ProcessIndex, string>();
        private readonly Dictionary<string, HashSet<ProcessIndex>> _taggedProcesses = new Dictionary<string, HashSet<ProcessIndex>>();
        private readonly Dictionary<ProcessIndex, int> _processLayers = new Dictionary<ProcessIndex, int>();
        private readonly Dictionary<int, HashSet<ProcessIndex>> _layeredProcesses = new Dictionary<int, HashSet<ProcessIndex>>();

        private IEnumerator<float>[] UpdateProcesses = new IEnumerator<float>[InitialBufferSizeLarge];
        private IEnumerator<float>[] LateUpdateProcesses = new IEnumerator<float>[InitialBufferSizeSmall];
        private IEnumerator<float>[] FixedUpdateProcesses = new IEnumerator<float>[InitialBufferSizeMedium];
        private IEnumerator<float>[] SlowUpdateProcesses = new IEnumerator<float>[InitialBufferSizeMedium];
        private IEnumerator<float>[] RealtimeUpdateProcesses = new IEnumerator<float>[InitialBufferSizeSmall];
        private IEnumerator<float>[] EditorUpdateProcesses = new IEnumerator<float>[InitialBufferSizeSmall];
        private IEnumerator<float>[] EditorSlowUpdateProcesses = new IEnumerator<float>[InitialBufferSizeSmall];
        private IEnumerator<float>[] EndOfFrameProcesses = new IEnumerator<float>[InitialBufferSizeSmall];
        private IEnumerator<float>[] ManualTimeframeProcesses = new IEnumerator<float>[InitialBufferSizeSmall];

        private bool[] UpdatePaused = new bool[InitialBufferSizeLarge];
        private bool[] LateUpdatePaused = new bool[InitialBufferSizeSmall];
        private bool[] FixedUpdatePaused = new bool[InitialBufferSizeMedium];
        private bool[] SlowUpdatePaused = new bool[InitialBufferSizeMedium];
        private bool[] RealtimeUpdatePaused = new bool[InitialBufferSizeSmall];
        private bool[] EditorUpdatePaused = new bool[InitialBufferSizeSmall];
        private bool[] EditorSlowUpdatePaused = new bool[InitialBufferSizeSmall];
        private bool[] EndOfFramePaused = new bool[InitialBufferSizeSmall];
        private bool[] ManualTimeframePaused = new bool[InitialBufferSizeSmall];

        private const ushort FramesUntilMaintenance = 64;
        private const int ProcessArrayChunkSize = 64;
        private const int InitialBufferSizeLarge = 256;
        private const int InitialBufferSizeMedium = 64;
        private const int InitialBufferSizeSmall = 8;

        private static readonly Dictionary<byte, Timing> ActiveInstances = new Dictionary<byte, Timing>();
        private static Timing _instance;

        public static Timing Instance
        {
            get
            {
                if (_instance == null || !_instance.gameObject)
                {
                    GameObject instanceHome = GameObject.Find("Movement Effects");
                    System.Type movementType =
                        System.Type.GetType("MovementEffects.Movement, MovementOverTime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");

                    if(instanceHome == null)
                    {
                        instanceHome = new GameObject { name = "Movement Effects" };
                        DontDestroyOnLoad(instanceHome);

                        if (movementType != null)
                            instanceHome.AddComponent(movementType);

                        _instance = instanceHome.AddComponent<Timing>();
                    }
                    else
                    {
                         if (movementType != null && instanceHome.GetComponent(movementType) == null) 
                            instanceHome.AddComponent(movementType);

                        _instance = instanceHome.GetComponent<Timing>() ?? instanceHome.AddComponent<Timing>();
                    }
                }

                return _instance;
            }

            set { _instance = value; }
        }

        void Awake()
        {
            if(_instance == null)
                _instance = this;
            else
                deltaTime = _instance.deltaTime;

            _instanceID = 0x01;
            while(ActiveInstances.ContainsKey(_instanceID))
                _instanceID++;

            if (_instanceID == 0x20)
            {
                GameObject.Destroy(gameObject);
                throw new System.OverflowException("You are only allowed 31 instances of MEC at one time.");
            }

            ActiveInstances.Add(_instanceID, this);
        }

        void OnDestroy()
        {
            if (_instance == this)
                _instance = null;

            ActiveInstances.Remove(_instanceID);
        }

        void OnEnable()
        {
            if(_nextEditorUpdateProcessSlot > 0 || _nextEditorSlowUpdateProcessSlot > 0)
                OnEditorStart();

            if(_nextEndOfFrameProcessSlot > 0)
                RunCoroutineSingletonOnInstance(_EOFPumpWatcher(), "MEC_EOFPumpWatcher");
        }

        void Update()
        {
            if (OnPreExecute != null)
                OnPreExecute();

            if (_lastSlowUpdateTime + TimeBetweenSlowUpdateCalls < Time.realtimeSinceStartup && _nextSlowUpdateProcessSlot > 0)
            {
                ProcessIndex coindex = new ProcessIndex { seg = Segment.SlowUpdate };
                _runningSlowUpdate = true;
                UpdateTimeValues(coindex.seg);

                for (coindex.i = 0; coindex.i < _nextSlowUpdateProcessSlot; coindex.i++)
                {
                    if (!SlowUpdatePaused[coindex.i] && SlowUpdateProcesses[coindex.i] != null && !(localTime < SlowUpdateProcesses[coindex.i].Current))
                    {
                        if (ProfilerDebugAmount != DebugInfoType.None)
                        {
                            Profiler.BeginSample(ProfilerDebugAmount == DebugInfoType.SeperateTags ? ("Processing Coroutine (Slow Update), " +
                                    (_processLayers.ContainsKey(coindex) ? "layer " + _processLayers[coindex] : "no layer") +
                                    (_processTags.ContainsKey(coindex) ? ", tag " + _processTags[coindex] : ", no tag"))
                                    : "Processing Coroutine (Slow Update)");
                        }

                        try
                        {
                            if (!SlowUpdateProcesses[coindex.i].MoveNext())
                            {
                                SlowUpdateProcesses[coindex.i] = null;
                            }
                            else if (SlowUpdateProcesses[coindex.i] != null && float.IsNaN(SlowUpdateProcesses[coindex.i].Current))
                            {
                                if (ReplacementFunction == null)
                                {
                                    SlowUpdateProcesses[coindex.i] = null;
                                }
                                else
                                {
                                    SlowUpdateProcesses[coindex.i] = ReplacementFunction(SlowUpdateProcesses[coindex.i], _indexToHandle[coindex]);

                                    ReplacementFunction = null;
                                    coindex.i--;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            SlowUpdateProcesses[coindex.i] = null;
                        }

                        if (ProfilerDebugAmount != DebugInfoType.None)
                            Profiler.EndSample();
                    }
                }

                _runningSlowUpdate = false;
            }

            if (_nextRealtimeUpdateProcessSlot > 0)
            {
                ProcessIndex coindex = new ProcessIndex { seg = Segment.RealtimeUpdate };
                _runningRealtimeUpdate = true;
                UpdateTimeValues(coindex.seg);

                for (coindex.i = 0; coindex.i < _nextRealtimeUpdateProcessSlot; coindex.i++)
                {
                    if (!RealtimeUpdatePaused[coindex.i] && RealtimeUpdateProcesses[coindex.i] != null && !(localTime < RealtimeUpdateProcesses[coindex.i].Current))
                    {
                        if (ProfilerDebugAmount != DebugInfoType.None)
                        {
                            Profiler.BeginSample(ProfilerDebugAmount == DebugInfoType.SeperateTags ? ("Processing Coroutine (Realtime Update), " +
                                    (_processLayers.ContainsKey(coindex) ? "layer " + _processLayers[coindex] : "no layer") +
                                    (_processTags.ContainsKey(coindex) ? ", tag " + _processTags[coindex] : ", no tag"))
                                    : "Processing Coroutine (Realtime Update)");
                        }

                        try
                        {
                            if (!RealtimeUpdateProcesses[coindex.i].MoveNext())
                            {
                                RealtimeUpdateProcesses[coindex.i] = null;
                            }
                            else if (RealtimeUpdateProcesses[coindex.i] != null && float.IsNaN(RealtimeUpdateProcesses[coindex.i].Current))
                            {
                                if (ReplacementFunction == null)
                                {
                                    RealtimeUpdateProcesses[coindex.i] = null;
                                }
                                else
                                {
                                    RealtimeUpdateProcesses[coindex.i] = ReplacementFunction(RealtimeUpdateProcesses[coindex.i], _indexToHandle[coindex]);

                                    ReplacementFunction = null;
                                    coindex.i--;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            RealtimeUpdateProcesses[coindex.i] = null;
                        }

                        if (ProfilerDebugAmount != DebugInfoType.None)
                            Profiler.EndSample();
                    }
                }

                _runningRealtimeUpdate = false;
            }

            if (_nextUpdateProcessSlot > 0)
            {
                ProcessIndex coindex = new ProcessIndex { seg = Segment.Update };
                _runningUpdate = true;
                UpdateTimeValues(coindex.seg);

                for (coindex.i = 0; coindex.i < _nextUpdateProcessSlot; coindex.i++)
                {
                    if (!UpdatePaused[coindex.i] && UpdateProcesses[coindex.i] != null && !(localTime < UpdateProcesses[coindex.i].Current))
                    {
                        if (ProfilerDebugAmount != DebugInfoType.None)
                        {
                            Profiler.BeginSample(ProfilerDebugAmount == DebugInfoType.SeperateTags ? ("Processing Coroutine, " +
                                    (_processLayers.ContainsKey(coindex) ? "layer " + _processLayers[coindex] : "no layer") +
                                    (_processTags.ContainsKey(coindex) ? ", tag " + _processTags[coindex] : ", no tag")) : "Processing Coroutine");
                        }

                        try
                        {
                            if (!UpdateProcesses[coindex.i].MoveNext())
                            {
                                UpdateProcesses[coindex.i] = null;
                            }
                            else if (UpdateProcesses[coindex.i] != null && float.IsNaN(UpdateProcesses[coindex.i].Current))
                            {
                                if (ReplacementFunction == null)
                                {
                                    UpdateProcesses[coindex.i] = null;
                                }
                                else
                                {
                                    UpdateProcesses[coindex.i] = ReplacementFunction(UpdateProcesses[coindex.i], _indexToHandle[coindex]);

                                    ReplacementFunction = null;
                                    coindex.i--;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            UpdateProcesses[coindex.i] = null;
                        }

                        if (ProfilerDebugAmount != DebugInfoType.None)
                            Profiler.EndSample();
                    }
                }

                _runningUpdate = false;
            }

            if (AutoTriggerManualTimeframe)
            {
                TriggerManualTimeframeUpdate();
            }
            else
            {
                if (++_framesSinceUpdate > FramesUntilMaintenance)
                {
                    _framesSinceUpdate = 0;

                    if (ProfilerDebugAmount != DebugInfoType.None)
                        Profiler.BeginSample("Maintenance Task");

                    RemoveUnused();

                    if (ProfilerDebugAmount != DebugInfoType.None)
                        Profiler.EndSample();
                }

                if (_exceptions.Count > 0)
                    throw _exceptions.Dequeue();
            }
        }

        void FixedUpdate()
        {
            if (OnPreExecute != null)
                OnPreExecute();

            if (_nextFixedUpdateProcessSlot > 0)
            {
                ProcessIndex coindex = new ProcessIndex { seg = Segment.FixedUpdate };
                _runningFixedUpdate = true;
                UpdateTimeValues(coindex.seg);

                for (coindex.i = 0; coindex.i < _nextFixedUpdateProcessSlot; coindex.i++)
                {
                    if (!FixedUpdatePaused[coindex.i] && FixedUpdateProcesses[coindex.i] != null && !(localTime < FixedUpdateProcesses[coindex.i].Current))
                    {
                        if (ProfilerDebugAmount != DebugInfoType.None)
                        {
                            Profiler.BeginSample(ProfilerDebugAmount == DebugInfoType.SeperateTags ? ("Processing Coroutine, " +
                                    (_processLayers.ContainsKey(coindex) ? "layer " + _processLayers[coindex] : "no layer") +
                                    (_processTags.ContainsKey(coindex) ? ", tag " + _processTags[coindex] : ", no tag")) : "Processing Coroutine");
                        }

                        try
                        {
                            if (!FixedUpdateProcesses[coindex.i].MoveNext())
                            {
                                FixedUpdateProcesses[coindex.i] = null;
                            }
                            else if (FixedUpdateProcesses[coindex.i] != null && float.IsNaN(FixedUpdateProcesses[coindex.i].Current))
                            {
                                if (ReplacementFunction == null)
                                {
                                    FixedUpdateProcesses[coindex.i] = null;
                                }
                                else
                                {
                                    FixedUpdateProcesses[coindex.i] = ReplacementFunction(FixedUpdateProcesses[coindex.i], _indexToHandle[coindex]);

                                    ReplacementFunction = null;
                                    coindex.i--;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            FixedUpdateProcesses[coindex.i] = null;
                        }

                        if (ProfilerDebugAmount != DebugInfoType.None)
                            Profiler.EndSample();
                    }
                }

                _runningFixedUpdate = false;
            }

            if (_exceptions.Count > 0)
                throw _exceptions.Dequeue();
        }

        void LateUpdate()
        {
            if (OnPreExecute != null)
                OnPreExecute();

            if (_nextLateUpdateProcessSlot > 0)
            {
                ProcessIndex coindex = new ProcessIndex { seg = Segment.LateUpdate };
                _runningLateUpdate = true;
                UpdateTimeValues(coindex.seg);

                for (coindex.i = 0; coindex.i < _nextLateUpdateProcessSlot; coindex.i++)
                {
                    if (!LateUpdatePaused[coindex.i] && LateUpdateProcesses[coindex.i] != null && !(localTime < LateUpdateProcesses[coindex.i].Current))
                    {
                        if (ProfilerDebugAmount != DebugInfoType.None)
                        {
                            Profiler.BeginSample(ProfilerDebugAmount == DebugInfoType.SeperateTags ? ("Processing Coroutine, " +
                                    (_processLayers.ContainsKey(coindex) ? "layer " + _processLayers[coindex] : "no layer") +
                                    (_processTags.ContainsKey(coindex) ? ", tag " + _processTags[coindex] : ", no tag")) : "Processing Coroutine");
                        }

                        try
                        {
                            if (!LateUpdateProcesses[coindex.i].MoveNext())
                            {
                                LateUpdateProcesses[coindex.i] = null;
                            }
                            else if (LateUpdateProcesses[coindex.i] != null && float.IsNaN(LateUpdateProcesses[coindex.i].Current))
                            {
                                if (ReplacementFunction == null)
                                {
                                    LateUpdateProcesses[coindex.i] = null;
                                }
                                else
                                {
                                    LateUpdateProcesses[coindex.i] = ReplacementFunction(LateUpdateProcesses[coindex.i], _indexToHandle[coindex]);

                                    ReplacementFunction = null;
                                    coindex.i--;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            LateUpdateProcesses[coindex.i] = null;
                        }

                        if (ProfilerDebugAmount != DebugInfoType.None)
                            Profiler.EndSample();
                    }
                }

                _runningLateUpdate = false;
            }

            if (_exceptions.Count > 0)
                throw _exceptions.Dequeue();
        }

        /// <summary>
        /// This will trigger an update in the manual timeframe segment. If the AutoTriggerManualTimeframeDuringUpdate variable is set to true
        /// then this function will be automitically called every Update, so you would normally want to set that variable to false before
        /// calling this function yourself.
        /// </summary>
        public void TriggerManualTimeframeUpdate()
        {
            if (OnPreExecute != null)
                OnPreExecute();

            if (_nextManualTimeframeProcessSlot > 0)
            {
                ProcessIndex coindex = new ProcessIndex { seg = Segment.ManualTimeframe };
                UpdateTimeValues(coindex.seg);

                for (coindex.i = 0; coindex.i < _nextManualTimeframeProcessSlot; coindex.i++)
                {
                    if (!ManualTimeframePaused[coindex.i] && ManualTimeframeProcesses[coindex.i] != null && 
                        !(localTime < ManualTimeframeProcesses[coindex.i].Current))
                    {
                        if (ProfilerDebugAmount != DebugInfoType.None)
                        {
                            Profiler.BeginSample(ProfilerDebugAmount == DebugInfoType.SeperateTags ? ("Processing Coroutine (Manual Timeframe), " +
                                    (_processLayers.ContainsKey(coindex) ? "layer " + _processLayers[coindex] : "no layer") +
                                    (_processTags.ContainsKey(coindex) ? ", tag " + _processTags[coindex] : ", no tag"))
                                    : "Processing Coroutine (Manual Timeframe)");
                        }

                        try
                        {
                            if (!ManualTimeframeProcesses[coindex.i].MoveNext())
                            {
                                ManualTimeframeProcesses[coindex.i] = null;
                            }
                            else if (ManualTimeframeProcesses[coindex.i] != null && float.IsNaN(ManualTimeframeProcesses[coindex.i].Current))
                            {
                                if (ReplacementFunction == null)
                                {
                                    ManualTimeframeProcesses[coindex.i] = null;
                                }
                                else
                                {
                                    ManualTimeframeProcesses[coindex.i] = ReplacementFunction(ManualTimeframeProcesses[coindex.i], _indexToHandle[coindex]);

                                    ReplacementFunction = null;
                                    coindex.i--;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            ManualTimeframeProcesses[coindex.i] = null;
                        }

                        if (ProfilerDebugAmount != DebugInfoType.None)
                            Profiler.EndSample();
                    }
                }
            }

            if (++_framesSinceUpdate > FramesUntilMaintenance)
            {
                _framesSinceUpdate = 0;

                if (ProfilerDebugAmount != DebugInfoType.None)
                    Profiler.BeginSample("Maintenance Task");

                RemoveUnused();

                if (ProfilerDebugAmount != DebugInfoType.None)
                    Profiler.EndSample();
            }

            if (_exceptions.Count > 0)
                throw _exceptions.Dequeue();
        }

        private bool OnEditorStart()
        {
#if UNITY_EDITOR
            if(EditorApplication.isPlayingOrWillChangePlaymode)
                return false;

            if(_lastEditorUpdateTime == 0d)
                _lastEditorUpdateTime = EditorApplication.timeSinceStartup;

            EditorApplication.update -= OnEditorUpdate;

            EditorApplication.update += OnEditorUpdate;

            return true;
#else
            return false;
#endif
        }

#if UNITY_EDITOR
        private void OnEditorUpdate()
        {
            if (OnPreExecute != null)
                OnPreExecute();

            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                for(int i = 0;i < _nextEditorUpdateProcessSlot;i++)
                    EditorUpdateProcesses[i] = null;
                _nextEditorUpdateProcessSlot = 0;
                for (int i = 0; i < _nextEditorSlowUpdateProcessSlot; i++)
                    EditorSlowUpdateProcesses[i] = null;
                _nextEditorSlowUpdateProcessSlot = 0;

                EditorApplication.update -= OnEditorUpdate;
                _instance = null;
            }

            if (_lastEditorSlowUpdateTime + TimeBetweenSlowUpdateCalls < EditorApplication.timeSinceStartup && _nextEditorSlowUpdateProcessSlot > 0)
            {
                ProcessIndex coindex = new ProcessIndex { seg = Segment.EditorSlowUpdate };
                _runningEditorSlowUpdate = true;
                UpdateTimeValues(coindex.seg);

                for (coindex.i = 0; coindex.i < _nextEditorSlowUpdateProcessSlot; coindex.i++)
                {
                    if (!EditorSlowUpdatePaused[coindex.i] && EditorSlowUpdateProcesses[coindex.i] != null && 
                        !(EditorApplication.timeSinceStartup < EditorSlowUpdateProcesses[coindex.i].Current))
                    {
                        try
                        {
                            if (!EditorSlowUpdateProcesses[coindex.i].MoveNext())
                            {
                                EditorSlowUpdateProcesses[coindex.i] = null;
                            }
                            else if (EditorSlowUpdateProcesses[coindex.i] != null && float.IsNaN(EditorSlowUpdateProcesses[coindex.i].Current))
                            {
                                if (ReplacementFunction == null)
                                {
                                    EditorSlowUpdateProcesses[coindex.i] = null;
                                }
                                else
                                {
                                    EditorSlowUpdateProcesses[coindex.i] = ReplacementFunction(EditorSlowUpdateProcesses[coindex.i], _indexToHandle[coindex]);

                                    ReplacementFunction = null;
                                    coindex.i--;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            EditorSlowUpdateProcesses[coindex.i] = null;
                        }
                    }
                }

                _runningEditorSlowUpdate = false;
            }

            if(_nextEditorUpdateProcessSlot > 0)
            {
                ProcessIndex coindex = new ProcessIndex { seg = Segment.EditorUpdate };
                _runningEditorUpdate = true;
                UpdateTimeValues(coindex.seg);

                for (coindex.i = 0; coindex.i < _nextEditorUpdateProcessSlot; coindex.i++)
                {
                    if (!EditorUpdatePaused[coindex.i] && EditorUpdateProcesses[coindex.i] != null && 
                        !(EditorApplication.timeSinceStartup < EditorUpdateProcesses[coindex.i].Current))
                    {
                        try
                        {
                            if (!EditorUpdateProcesses[coindex.i].MoveNext())
                            {
                                EditorUpdateProcesses[coindex.i] = null;
                            }
                            else if (EditorUpdateProcesses[coindex.i] != null && float.IsNaN(EditorUpdateProcesses[coindex.i].Current))
                            {
                                if (ReplacementFunction == null)
                                {
                                    EditorUpdateProcesses[coindex.i] = null;
                                }
                                else
                                {
                                    EditorUpdateProcesses[coindex.i] = ReplacementFunction(EditorUpdateProcesses[coindex.i], _indexToHandle[coindex]);

                                    ReplacementFunction = null;
                                    coindex.i--;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            EditorUpdateProcesses[coindex.i] = null;
                        }
                    }
                }

                _runningEditorUpdate = false;
            }

            if (++_framesSinceUpdate > FramesUntilMaintenance)
            {
                _framesSinceUpdate = 0;

                EditorRemoveUnused();
            }

            if (_exceptions.Count > 0)
                throw _exceptions.Dequeue();
        }
#endif

        private IEnumerator<float> _EOFPumpWatcher()
        {
            while (_nextEndOfFrameProcessSlot > 0)
            {
                if(!_EOFPumpRan)
                    base.StartCoroutine(_EOFPump());

                _EOFPumpRan = false;

                yield return 0f;
            }

            _EOFPumpRan = false;
        }

        private System.Collections.IEnumerator _EOFPump()
        {
            while(_nextEndOfFrameProcessSlot > 0)
            {
                yield return _EOFWaitObject;

                if (OnPreExecute != null)
                    OnPreExecute();

                ProcessIndex coindex = new ProcessIndex { seg = Segment.EndOfFrame };
                _EOFPumpRan = true;
                UpdateTimeValues(coindex.seg);

                for(coindex.i = 0;coindex.i < _nextEndOfFrameProcessSlot;coindex.i++)
                {
                    if (!EndOfFramePaused[coindex.i] && EndOfFrameProcesses[coindex.i] != null && !(localTime < EndOfFrameProcesses[coindex.i].Current))
                    {
                        if(ProfilerDebugAmount != DebugInfoType.None)
                        {
                            Profiler.BeginSample(ProfilerDebugAmount == DebugInfoType.SeperateTags ? ("Processing Coroutine, " +
                                    (_processLayers.ContainsKey(coindex) ? "layer " + _processLayers[coindex] : "no layer") +
                                    (_processTags.ContainsKey(coindex) ? ", tag " + _processTags[coindex] : ", no tag")) : "Processing Coroutine");
                        }

                        try
                        {
                            if(!EndOfFrameProcesses[coindex.i].MoveNext())
                            {
                                EndOfFrameProcesses[coindex.i] = null;
                            }
                            else if(EndOfFrameProcesses[coindex.i] != null && float.IsNaN(EndOfFrameProcesses[coindex.i].Current))
                            {
                                if(ReplacementFunction == null)
                                {
                                    EndOfFrameProcesses[coindex.i] = null;
                                }
                                else
                                {
                                    EndOfFrameProcesses[coindex.i] = ReplacementFunction(EndOfFrameProcesses[coindex.i], _indexToHandle[coindex]);

                                    ReplacementFunction = null;
                                    coindex.i--;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if(OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            EndOfFrameProcesses[coindex.i] = null;
                        }

                        if (ProfilerDebugAmount != DebugInfoType.None) 
                            Profiler.EndSample();
                    }
                }
            }
        }

        private void RemoveUnused()
        {
            var waitTrigsEnum = _waitingTriggers.GetEnumerator();
            while (waitTrigsEnum.MoveNext())
            {
                if (waitTrigsEnum.Current.Value.Count == 0)
                {
                    _waitingTriggers.Remove(waitTrigsEnum.Current.Key);
                    waitTrigsEnum = _waitingTriggers.GetEnumerator();
                    continue;
                }

                if (_handleToIndex.ContainsKey(waitTrigsEnum.Current.Key) && CoindexIsNull(_handleToIndex[waitTrigsEnum.Current.Key]))
                {
                    CloseWaitingProcess(waitTrigsEnum.Current.Key);
                    waitTrigsEnum = _waitingTriggers.GetEnumerator();
                }
            }

            ProcessIndex outer, inner;
            outer.seg = inner.seg = Segment.Update;
            for (outer.i = inner.i = 0; outer.i < _nextUpdateProcessSlot; outer.i++)
            {
                if (UpdateProcesses[outer.i] != null)
                {
                    if (outer.i != inner.i)
                    {
                        UpdateProcesses[inner.i] = UpdateProcesses[outer.i];
                        UpdatePaused[inner.i] = UpdatePaused[outer.i];
                        MoveGraffiti(outer, inner);
                    }
                    inner.i++;
                }
            }
            for (outer.i = inner.i; outer.i < _nextUpdateProcessSlot; outer.i++)
            {
                UpdateProcesses[outer.i] = null;
                UpdatePaused[outer.i] = false;
                RemoveGraffiti(outer);
            }

            UpdateCoroutines = _nextUpdateProcessSlot = inner.i;

            outer.seg = inner.seg = Segment.FixedUpdate;
            for (outer.i = inner.i = 0; outer.i < _nextFixedUpdateProcessSlot; outer.i++)
            {
                if (FixedUpdateProcesses[outer.i] != null)
                {
                    if (outer.i != inner.i)
                    {
                        FixedUpdateProcesses[inner.i] = FixedUpdateProcesses[outer.i];
                        FixedUpdatePaused[inner.i] = FixedUpdatePaused[outer.i];
                        MoveGraffiti(outer, inner);
                    }
                    inner.i++;
                }
            }
            for (outer.i = inner.i; outer.i < _nextFixedUpdateProcessSlot; outer.i++)
            {
                FixedUpdateProcesses[outer.i] = null;
                FixedUpdatePaused[outer.i] = false;
                RemoveGraffiti(outer);
            }

            FixedUpdateCoroutines = _nextFixedUpdateProcessSlot = inner.i;

            outer.seg = inner.seg = Segment.LateUpdate;
            for (outer.i = inner.i = 0; outer.i < _nextLateUpdateProcessSlot; outer.i++)
            {
                if (LateUpdateProcesses[outer.i] != null)
                {
                    if (outer.i != inner.i)
                    {
                        LateUpdateProcesses[inner.i] = LateUpdateProcesses[outer.i];
                        LateUpdatePaused[inner.i] = LateUpdatePaused[outer.i];
                        MoveGraffiti(outer, inner);
                    }
                    inner.i++;
                }
            }
            for (outer.i = inner.i; outer.i < _nextLateUpdateProcessSlot; outer.i++)
            {
                LateUpdateProcesses[outer.i] = null;
                LateUpdatePaused[outer.i] = false;
                RemoveGraffiti(outer);
            }

            LateUpdateCoroutines = _nextLateUpdateProcessSlot = inner.i;

            outer.seg = inner.seg = Segment.SlowUpdate;
            for (outer.i = inner.i = 0; outer.i < _nextSlowUpdateProcessSlot; outer.i++)
            {
                if (SlowUpdateProcesses[outer.i] != null)
                {
                    if (outer.i != inner.i)
                    {
                        SlowUpdateProcesses[inner.i] = SlowUpdateProcesses[outer.i];
                        SlowUpdatePaused[inner.i] = SlowUpdatePaused[outer.i];
                        MoveGraffiti(outer, inner);
                    }
                    inner.i++;
                }
            }
            for (outer.i = inner.i; outer.i < _nextSlowUpdateProcessSlot; outer.i++)
            {
                SlowUpdateProcesses[outer.i] = null;
                SlowUpdatePaused[outer.i] = false;
                RemoveGraffiti(outer);
            }

            SlowUpdateCoroutines = _nextSlowUpdateProcessSlot = inner.i;

            outer.seg = inner.seg = Segment.RealtimeUpdate;
            for (outer.i = inner.i = 0; outer.i < _nextRealtimeUpdateProcessSlot; outer.i++)
            {
                if (RealtimeUpdateProcesses[outer.i] != null)
                {
                    if (outer.i != inner.i)
                    {
                        RealtimeUpdateProcesses[inner.i] = RealtimeUpdateProcesses[outer.i];
                        RealtimeUpdatePaused[inner.i] = RealtimeUpdatePaused[outer.i];
                        MoveGraffiti(outer, inner);
                    }
                    inner.i++;
                }
            }
            for (outer.i = inner.i; outer.i < _nextRealtimeUpdateProcessSlot; outer.i++)
            {
                RealtimeUpdateProcesses[outer.i] = null;
                RealtimeUpdatePaused[outer.i] = false;
                RemoveGraffiti(outer);
            }

            RealtimeUpdateCoroutines = _nextRealtimeUpdateProcessSlot = inner.i;

            outer.seg = inner.seg = Segment.EndOfFrame;
            for (outer.i = inner.i = 0; outer.i < _nextEndOfFrameProcessSlot; outer.i++)
            {
                if (EndOfFrameProcesses[outer.i] != null)
                {
                    if (outer.i != inner.i)
                    {
                        EndOfFrameProcesses[inner.i] = EndOfFrameProcesses[outer.i];
                        EndOfFramePaused[inner.i] = EndOfFramePaused[outer.i];
                        MoveGraffiti(outer, inner);
                    }
                    inner.i++;
                }
            }
            for (outer.i = inner.i; outer.i < _nextEndOfFrameProcessSlot; outer.i++)
            {
                EndOfFrameProcesses[outer.i] = null;
                EndOfFramePaused[outer.i] = false;
                RemoveGraffiti(outer);
            }

            EndOfFrameCoroutines = _nextEndOfFrameProcessSlot = inner.i;

            outer.seg = inner.seg = Segment.ManualTimeframe;
            for (outer.i = inner.i = 0; outer.i < _nextManualTimeframeProcessSlot; outer.i++)
            {
                if (ManualTimeframeProcesses[outer.i] != null)
                {
                    if (outer.i != inner.i)
                    {
                        ManualTimeframeProcesses[inner.i] = ManualTimeframeProcesses[outer.i];
                        ManualTimeframePaused[inner.i] = ManualTimeframePaused[outer.i];
                        MoveGraffiti(outer, inner);
                    }
                    inner.i++;
                }
            }
            for (outer.i = inner.i; outer.i < _nextManualTimeframeProcessSlot; outer.i++)
            {
                ManualTimeframeProcesses[outer.i] = null;
                ManualTimeframePaused[outer.i] = false;
                RemoveGraffiti(outer);
            }

            ManualTimeframeCoroutines = _nextManualTimeframeProcessSlot = inner.i;
        }

        private void EditorRemoveUnused()
        {
            var waitTrigsEnum = _waitingTriggers.GetEnumerator();
            while (waitTrigsEnum.MoveNext())
            {
                if (_handleToIndex.ContainsKey(waitTrigsEnum.Current.Key) && CoindexIsNull(_handleToIndex[waitTrigsEnum.Current.Key]))
                {
                    CloseWaitingProcess(waitTrigsEnum.Current.Key);
                    waitTrigsEnum = _waitingTriggers.GetEnumerator();
                }
            }

            ProcessIndex outer, inner;
            outer.seg = inner.seg = Segment.EditorUpdate;
            for (outer.i = inner.i = 0; outer.i < _nextEditorUpdateProcessSlot; outer.i++)
            {
                if (EditorUpdateProcesses[outer.i] != null)
                {
                    if (outer.i != inner.i)
                    {
                        EditorUpdateProcesses[inner.i] = EditorUpdateProcesses[outer.i];
                        EditorUpdatePaused[inner.i] = EditorUpdatePaused[outer.i];
                        MoveGraffiti(outer, inner);
                    }
                    inner.i++;
                }
            }
            for (outer.i = inner.i; outer.i < _nextEditorUpdateProcessSlot; outer.i++)
            {
                EditorUpdateProcesses[outer.i] = null;
                EditorUpdatePaused[outer.i] = false;
                RemoveGraffiti(outer);
            }

            EditorUpdateCoroutines = _nextEditorUpdateProcessSlot = inner.i;

            outer.seg = inner.seg = Segment.EditorSlowUpdate;
            for (outer.i = inner.i = 0; outer.i < _nextEditorSlowUpdateProcessSlot; outer.i++)
            {
                if (EditorSlowUpdateProcesses[outer.i] != null)
                {
                    if (outer.i != inner.i)
                    {
                        EditorSlowUpdateProcesses[inner.i] = EditorSlowUpdateProcesses[outer.i];
                        EditorUpdatePaused[inner.i] = EditorUpdatePaused[outer.i];
                        MoveGraffiti(outer, inner);
                    }
                    inner.i++;
                }
            }
            for (outer.i = inner.i; outer.i < _nextEditorSlowUpdateProcessSlot; outer.i++)
            {
                EditorSlowUpdateProcesses[outer.i] = null;
                EditorSlowUpdatePaused[outer.i] = false;
                RemoveGraffiti(outer);
            }

            EditorSlowUpdateCoroutines = _nextEditorSlowUpdateProcessSlot = inner.i;
        }

        /// <summary>
        /// Run a new coroutine in the Update segment.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine)
        {
            return coroutine == null ? new CoroutineHandle()
                : Instance.RunCoroutineInternal(coroutine, Segment.Update, null, null, new CoroutineHandle(Instance._instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine in the Update segment.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="tag">An optional tag to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, string tag)
        {
            return coroutine == null ? new CoroutineHandle()
                : Instance.RunCoroutineInternal(coroutine, Segment.Update, null, tag, new CoroutineHandle(Instance._instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine in the Update segment.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="layer">An optional layer to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, int layer)
        {
            return coroutine == null ? new CoroutineHandle()
                : Instance.RunCoroutineInternal(coroutine, Segment.Update, layer, null, new CoroutineHandle(Instance._instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine in the Update segment.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="tag">An optional tag to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <param name="layer">An optional layer to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, int layer, string tag)
        {
            return coroutine == null ? new CoroutineHandle()
                : Instance.RunCoroutineInternal(coroutine, Segment.Update, layer, tag, new CoroutineHandle(Instance._instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="segment">The segment that the coroutine should run in.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, Segment segment)
        {
            return coroutine == null ? new CoroutineHandle()
                : Instance.RunCoroutineInternal(coroutine, segment, null, null, new CoroutineHandle(Instance._instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="segment">The segment that the coroutine should run in.</param>
        /// <param name="tag">An optional tag to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, Segment segment, string tag)
        {
            return coroutine == null ? new CoroutineHandle()
                 : Instance.RunCoroutineInternal(coroutine, segment, null, tag, new CoroutineHandle(Instance._instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="segment">The segment that the coroutine should run in.</param>
        /// <param name="layer">An optional layer to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, Segment segment, int layer)
        {
            return coroutine == null ? new CoroutineHandle()
                 : Instance.RunCoroutineInternal(coroutine, segment, layer, null, new CoroutineHandle(Instance._instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="segment">The segment that the coroutine should run in.</param>
        /// <param name="tag">An optional tag to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <param name="layer">An optional layer to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, Segment segment, int layer, string tag)
        {
            return coroutine == null ? new CoroutineHandle()
                 : Instance.RunCoroutineInternal(coroutine, segment, layer, tag, new CoroutineHandle(Instance._instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine on this Timing instance in the Update segment.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine)
        {
            return coroutine == null ? new CoroutineHandle()
                 : RunCoroutineInternal(coroutine, Segment.Update, null, null, new CoroutineHandle(_instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine on this Timing instance in the Update segment.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="tag">An optional tag to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, string tag)
        {
            return coroutine == null ? new CoroutineHandle()
                 : RunCoroutineInternal(coroutine, Segment.Update, null, tag, new CoroutineHandle(_instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine on this Timing instance in the Update segment.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="layer">An optional layer to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, int layer)
        {
            return coroutine == null ? new CoroutineHandle()
                 : RunCoroutineInternal(coroutine, Segment.Update, layer, null, new CoroutineHandle(_instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine on this Timing instance in the Update segment.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="layer">An optional layer to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <param name="tag">An optional tag to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, int layer, string tag)
        {
            return coroutine == null ? new CoroutineHandle()
                 : RunCoroutineInternal(coroutine, Segment.Update, layer, tag, new CoroutineHandle(_instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine on this Timing instance.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="segment">The segment that the coroutine should run in.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, Segment segment)
        {
            return coroutine == null ? new CoroutineHandle()
                 : RunCoroutineInternal(coroutine, segment, null, null, new CoroutineHandle(_instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine on this Timing instance.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="segment">The segment that the coroutine should run in.</param>
        /// <param name="tag">An optional tag to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, Segment segment, string tag)
        {
            return coroutine == null ? new CoroutineHandle()
                 : RunCoroutineInternal(coroutine, segment, null, tag, new CoroutineHandle(_instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine on this Timing instance.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="segment">The segment that the coroutine should run in.</param>
        /// <param name="layer">An optional layer to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, Segment segment, int layer)
        {
            return coroutine == null ? new CoroutineHandle()
                 : RunCoroutineInternal(coroutine, segment, layer, null, new CoroutineHandle(_instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine on this Timing instance.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="segment">The segment that the coroutine should run in.</param>
        /// <param name="layer">An optional layer to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <param name="tag">An optional tag to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <returns>The coroutine's handle, which can be used for Wait and Kill operations.</returns>
        public CoroutineHandle RunCoroutineOnInstance(IEnumerator<float> coroutine, Segment segment, int layer, string tag)
        {
            return coroutine == null ? new CoroutineHandle() 
                : RunCoroutineInternal(coroutine, segment, layer, tag, new CoroutineHandle(_instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine in the Update segment with the supplied tag unless there is already one or more coroutines running with that tag.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="tag">A tag to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="overwrite">True will kill any pre-existing coroutines. False will only start this coroutine if none exist.</param>
        /// <returns>The newly created or existing handle.</returns>
        public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, string tag, bool overwrite = false)
        {
            if (_instance == null || coroutine == null)
                return new CoroutineHandle();

            if (overwrite)
            {
                KillCoroutines(tag);
            }
            else if (_instance._taggedProcesses.ContainsKey(tag))
            {
                var indexEnum = _instance._taggedProcesses[tag].GetEnumerator();
                if (indexEnum.MoveNext())
                    return _instance._indexToHandle[indexEnum.Current];
            }

            return _instance.RunCoroutineInternal(coroutine, Segment.Update, null, tag, new CoroutineHandle(_instance._instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine in the Update segment with the supplied tag unless there is already one or more coroutines running with that tag.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="layer">A layer to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="overwrite">True will kill any pre-existing coroutines. False will only start this coroutine if none exist.</param>
        /// <returns>The newly created or existing handle.</returns>
        public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, int layer, bool overwrite = false)
        {
            if (_instance == null || coroutine == null)
                return new CoroutineHandle();

            if (overwrite)
            {
                KillCoroutines(layer);
            }
            else if (_instance._layeredProcesses.ContainsKey(layer))
            {
                var indexEnum = _instance._layeredProcesses[layer].GetEnumerator();
                if (indexEnum.MoveNext())
                    return _instance._indexToHandle[indexEnum.Current];
            }

            return _instance.RunCoroutineInternal(coroutine, Segment.Update, layer, null, new CoroutineHandle(_instance._instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine in the Update segment with the supplied tag unless there is already one or more coroutines running with that tag.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="layer">A layer to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="tag">A tag to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="overwrite">True will kill any pre-existing coroutines. False will only start this coroutine if none exist.</param>
        /// <returns>The newly created or existing handle.</returns>
        public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, int layer, string tag, bool overwrite = false)
        {
            if (_instance == null || coroutine == null)
                return new CoroutineHandle();

            if (overwrite)
            {
                KillCoroutines(layer, tag);
                return _instance.RunCoroutineInternal(coroutine, Segment.Update, layer, tag, new CoroutineHandle(_instance._instanceID), true);
            }

            if (!_instance._taggedProcesses.ContainsKey(tag) || !_instance._layeredProcesses.ContainsKey(layer))
                return _instance.RunCoroutineInternal(coroutine, Segment.Update, layer, tag, new CoroutineHandle(_instance._instanceID), true);

            var matchesEnum = _instance._taggedProcesses[tag].GetEnumerator();
            while (matchesEnum.MoveNext())
                if (_instance._processLayers.ContainsKey(matchesEnum.Current) && _instance._processLayers[matchesEnum.Current] == layer)
                    return _instance._indexToHandle[matchesEnum.Current];

            return _instance.RunCoroutineInternal(coroutine, Segment.Update, layer, tag, new CoroutineHandle(_instance._instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine with the supplied tag unless there is already one or more coroutines running with that tag.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="segment">The segment that the coroutine should run in.</param>
        /// <param name="layer">A layer to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="overwrite">True will kill any pre-existing coroutines. False will only start this coroutine if none exist.</param>
        /// <returns>The newly created or existing handle.</returns>
        public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, Segment segment, int layer, bool overwrite = false)
        {
            if (_instance == null || coroutine == null)
                return new CoroutineHandle();

            if (overwrite)
            {
                KillCoroutines(layer);
            }
            else if (_instance._layeredProcesses.ContainsKey(layer))
            {
                var indexEnum = _instance._layeredProcesses[layer].GetEnumerator();
                if (indexEnum.MoveNext())
                    return _instance._indexToHandle[indexEnum.Current];
            }

            return _instance.RunCoroutineInternal(coroutine, segment, layer, null, new CoroutineHandle(_instance._instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine with the supplied tag unless there is already one or more coroutines running with that tag.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="segment">The segment that the coroutine should run in.</param>
        /// <param name="tag">A tag to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="overwrite">True will kill any pre-existing coroutines. False will only start this coroutine if none exist.</param>
        /// <returns>The newly created or existing handle.</returns>
        public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, Segment segment, string tag, bool overwrite = false)
        {
            if (_instance == null || coroutine == null)
                return new CoroutineHandle();

            if (overwrite)
            {
                KillCoroutines(tag);
            }
            else if (_instance._taggedProcesses.ContainsKey(tag))
            {
                var indexEnum = _instance._taggedProcesses[tag].GetEnumerator();
                if (indexEnum.MoveNext())
                    return _instance._indexToHandle[indexEnum.Current];
            }

            return _instance.RunCoroutineInternal(coroutine, segment, null, tag, new CoroutineHandle(_instance._instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine with the supplied tag unless there is already one or more coroutines running with that tag.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="segment">The segment that the coroutine should run in.</param>
        /// <param name="layer">A layer to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="tag">A tag to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="overwrite">True will kill any pre-existing coroutines. False will only start this coroutine if none exist.</param>
        /// <returns>The newly created or existing handle.</returns>
        public static CoroutineHandle RunCoroutineSingleton(IEnumerator<float> coroutine, Segment segment, int layer, string tag, bool overwrite = false)
        {
            if (_instance == null || coroutine == null)
                return new CoroutineHandle();

            if (overwrite)
            {
                KillCoroutines(layer, tag);
                return _instance.RunCoroutineInternal(coroutine, segment, layer, tag, new CoroutineHandle(_instance._instanceID), true);
            }

            if (!_instance._taggedProcesses.ContainsKey(tag) || !_instance._layeredProcesses.ContainsKey(layer))
                return _instance.RunCoroutineInternal(coroutine, segment, layer, tag, new CoroutineHandle(_instance._instanceID), true);

            var matchesEnum = _instance._taggedProcesses[tag].GetEnumerator();
            while (matchesEnum.MoveNext())
                if (_instance._processLayers.ContainsKey(matchesEnum.Current) && _instance._processLayers[matchesEnum.Current] == layer)
                    return _instance._indexToHandle[matchesEnum.Current];

            return _instance.RunCoroutineInternal(coroutine, segment, layer, tag, new CoroutineHandle(_instance._instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine in the Update segment with the supplied tag unless there is already one or more coroutines running with that tag.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="layer">A layer to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="overwrite">True will kill any pre-existing coroutines. False will only start this coroutine if none exist.</param>
        /// <returns>The newly created or existing handle.</returns>
        public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, int layer, bool overwrite = false)
        {
            if (coroutine == null)
                return new CoroutineHandle();

            if (overwrite)
            {
                KillCoroutines(layer);
            }
            else if (_layeredProcesses.ContainsKey(layer))
            {
                var indexEnum = _layeredProcesses[layer].GetEnumerator();
                if (indexEnum.MoveNext())
                    return _indexToHandle[indexEnum.Current];
            }

            return RunCoroutineInternal(coroutine, Segment.Update, layer, null, new CoroutineHandle(_instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine in the Update segment with the supplied tag unless there is already one or more coroutines running with that tag.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="tag">A tag to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="overwrite">True will kill any pre-existing coroutines. False will only start this coroutine if none exist.</param>
        /// <returns>The newly created or existing handle.</returns>
        public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, string tag, bool overwrite = false)
        {
            if (coroutine == null)
                return new CoroutineHandle();

            if (overwrite)
            {
                KillCoroutines(tag);
            }
            else if (_taggedProcesses.ContainsKey(tag))
            {
                var indexEnum = _taggedProcesses[tag].GetEnumerator();
                if (indexEnum.MoveNext())
                    return _indexToHandle[indexEnum.Current];
            }

            return RunCoroutineInternal(coroutine, Segment.Update, null, tag, new CoroutineHandle(_instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine in the Update segment with the supplied tag unless there is already one or more coroutines running with that tag.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="layer">A layer to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="tag">A tag to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="overwrite">True will kill any pre-existing coroutines. False will only start this coroutine if none exist.</param>
        /// <returns>The newly created or existing handle.</returns>
        public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, int layer, string tag, bool overwrite = false)
        {
            if (coroutine == null)
                return new CoroutineHandle();

            if (overwrite)
            {
                KillCoroutines(layer, tag);
                return RunCoroutineInternal(coroutine, Segment.Update, layer, tag, new CoroutineHandle(_instanceID), true);
            }

            if (!_taggedProcesses.ContainsKey(tag) || !_layeredProcesses.ContainsKey(layer))
                return RunCoroutineInternal(coroutine, Segment.Update, layer, tag, new CoroutineHandle(_instanceID), true);

            var matchesEnum = _taggedProcesses[tag].GetEnumerator();
            while (matchesEnum.MoveNext())
                if (_processLayers.ContainsKey(matchesEnum.Current) && _processLayers[matchesEnum.Current] == layer)
                    return _indexToHandle[matchesEnum.Current];

            return RunCoroutineInternal(coroutine, Segment.Update, layer, tag, new CoroutineHandle(_instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine with the supplied tag unless there is already one or more coroutines running with that tag.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="segment">The segment that the coroutine should run in.</param>
        /// <param name="layer">A layer to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="overwrite">True will kill any pre-existing coroutines. False will only start this coroutine if none exist.</param>
        /// <returns>The newly created or existing handle.</returns>
        public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, Segment segment, int layer, bool overwrite = false)
        {
            if (coroutine == null)
                return new CoroutineHandle();

            if (overwrite)
            {
                KillCoroutines(layer);
            }
            else if (_layeredProcesses.ContainsKey(layer))
            {
                var indexEnum = _layeredProcesses[layer].GetEnumerator();
                if (indexEnum.MoveNext())
                    return _indexToHandle[indexEnum.Current];
            }

            return RunCoroutineInternal(coroutine, segment, layer, null, new CoroutineHandle(_instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine with the supplied tag unless there is already one or more coroutines running with that tag.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="segment">The segment that the coroutine should run in.</param>
        /// <param name="tag">A tag to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="overwrite">True will kill any pre-existing coroutines. False will only start this coroutine if none exist.</param>
        /// <returns>The newly created or existing handle.</returns>
        public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, Segment segment, string tag, bool overwrite = false)
        {
            if (coroutine == null)
                return new CoroutineHandle();

            if (overwrite)
            {
                KillCoroutines(tag);
            }
            else if (_taggedProcesses.ContainsKey(tag))
            {
                var indexEnum = _taggedProcesses[tag].GetEnumerator();
                if (indexEnum.MoveNext())
                    return _indexToHandle[indexEnum.Current];
            }

            return RunCoroutineInternal(coroutine, segment, null, tag, new CoroutineHandle(_instanceID), true);
        }

        /// <summary>
        /// Run a new coroutine with the supplied tag unless there is already one or more coroutines running with that tag.
        /// </summary>
        /// <param name="coroutine">The new coroutine's handle.</param>
        /// <param name="segment">The segment that the coroutine should run in.</param>
        /// <param name="layer">A layer to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="tag">A tag to attach to the coroutine, and to check for existing instances.</param>
        /// <param name="overwrite">True will kill any pre-existing coroutines. False will only start this coroutine if none exist.</param>
        /// <returns>The newly created or existing handle.</returns>
        public CoroutineHandle RunCoroutineSingletonOnInstance(IEnumerator<float> coroutine, Segment segment, int layer, string tag, bool overwrite = false)
        {
            if (coroutine == null) 
                return new CoroutineHandle();

            if (overwrite)
            {
                KillCoroutines(layer, tag);
                return RunCoroutineInternal(coroutine, segment, layer, tag, new CoroutineHandle(_instanceID), true);
            }

            if (!_taggedProcesses.ContainsKey(tag) || !_layeredProcesses.ContainsKey(layer))
                return RunCoroutineInternal(coroutine, segment, layer, tag, new CoroutineHandle(_instanceID), true);

            var matchesEnum = _taggedProcesses[tag].GetEnumerator();
            while (matchesEnum.MoveNext())
                if (_processLayers.ContainsKey(matchesEnum.Current) && _processLayers[matchesEnum.Current] == layer)
                    return _indexToHandle[matchesEnum.Current];

            return RunCoroutineInternal(coroutine, segment, layer, tag, new CoroutineHandle(_instanceID), true);
        }

        private CoroutineHandle RunCoroutineInternal(IEnumerator<float> coroutine, Segment segment, int? layer, string tag, CoroutineHandle handle, bool prewarm)
        {
            ProcessIndex slot = new ProcessIndex { seg = segment };

            if (_handleToIndex.ContainsKey(handle))
            {
                _indexToHandle.Remove(_handleToIndex[handle]);
                _handleToIndex.Remove(handle);
            }

            switch (segment)
            {
                case Segment.Update:

                    if (_nextUpdateProcessSlot >= UpdateProcesses.Length)
                    {
                        IEnumerator<float>[] oldProcArray = UpdateProcesses;
                        bool[] oldPausedArray = UpdatePaused;

                        UpdateProcesses = new IEnumerator<float>[UpdateProcesses.Length + (ProcessArrayChunkSize * _expansions++)];
                        UpdatePaused = new bool[UpdateProcesses.Length];

                        for (int i = 0;i < oldProcArray.Length;i++)
                        {
                            UpdateProcesses[i] = oldProcArray[i];
                            UpdatePaused[i] = oldPausedArray[i];
                        }
                    }

                    slot.i = _nextUpdateProcessSlot++;
                    UpdateProcesses[slot.i] = coroutine;

                    if (null != tag)
                        AddTag(tag, slot);

                    if (layer.HasValue)
                        AddLayer((int)layer, slot);

                    _indexToHandle.Add(slot, handle);
                    _handleToIndex.Add(handle, slot);

                    if (!_runningUpdate && prewarm)
                    {
                        try
                        {
                            _runningUpdate = true;
                            SetTimeValues(slot.seg);

                            if (!UpdateProcesses[slot.i].MoveNext())
                            {
                                UpdateProcesses[slot.i] = null;
                            }
                            else if (UpdateProcesses[slot.i] != null && float.IsNaN(UpdateProcesses[slot.i].Current))
                            {
                                if (ReplacementFunction == null)
                                {
                                    UpdateProcesses[slot.i] = null;
                                }
                                else
                                {
                                    UpdateProcesses[slot.i] = ReplacementFunction(UpdateProcesses[slot.i], _indexToHandle[slot]);

                                    ReplacementFunction = null;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            UpdateProcesses[slot.i] = null;
                        }
                        finally
                        {
                            _runningUpdate = false;
                        }
                    }

                    return handle;

                case Segment.FixedUpdate:

                    if (_nextFixedUpdateProcessSlot >= FixedUpdateProcesses.Length)
                    {
                        IEnumerator<float>[] oldProcArray = FixedUpdateProcesses;
                        bool[] oldPausedArray = FixedUpdatePaused;

                        FixedUpdateProcesses = new IEnumerator<float>[FixedUpdateProcesses.Length + (ProcessArrayChunkSize * _expansions++)];
                        FixedUpdatePaused = new bool[FixedUpdateProcesses.Length];

                        for (int i = 0; i < oldProcArray.Length; i++)
                        {
                            FixedUpdateProcesses[i] = oldProcArray[i];
                            FixedUpdatePaused[i] = oldPausedArray[i];
                        }
                    }

                    slot.i = _nextFixedUpdateProcessSlot++;
                    FixedUpdateProcesses[slot.i] = coroutine;

                    if (null != tag)
                        AddTag(tag, slot);

                    if (layer.HasValue)
                        AddLayer((int)layer, slot);

                    _indexToHandle.Add(slot, handle);
                    _handleToIndex.Add(handle, slot);

                    if (!_runningFixedUpdate && prewarm)
                    {
                        try
                        {
                            _runningFixedUpdate = true;
                            SetTimeValues(slot.seg);

                            if (!FixedUpdateProcesses[slot.i].MoveNext())
                            {
                                FixedUpdateProcesses[slot.i] = null;
                            }
                            else if (FixedUpdateProcesses[slot.i] != null && float.IsNaN(FixedUpdateProcesses[slot.i].Current))
                            {
                                if (ReplacementFunction == null)
                                {
                                    FixedUpdateProcesses[slot.i] = null;
                                }
                                else
                                {
                                    FixedUpdateProcesses[slot.i] = ReplacementFunction(FixedUpdateProcesses[slot.i], _indexToHandle[slot]);

                                    ReplacementFunction = null;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            FixedUpdateProcesses[slot.i] = null;
                        }
                        finally
                        {
                            _runningFixedUpdate = false;
                        }
                    }

                    return handle;

                case Segment.LateUpdate:

                    if (_nextLateUpdateProcessSlot >= LateUpdateProcesses.Length)
                    {
                        IEnumerator<float>[] oldProcArray = LateUpdateProcesses;
                        bool[] oldPausedArray = LateUpdatePaused;

                        LateUpdateProcesses = new IEnumerator<float>[LateUpdateProcesses.Length + (ProcessArrayChunkSize * _expansions++)];
                        LateUpdatePaused = new bool[LateUpdateProcesses.Length];

                        for (int i = 0; i < oldProcArray.Length; i++)
                        {
                            LateUpdateProcesses[i] = oldProcArray[i];
                            LateUpdatePaused[i] = oldPausedArray[i];
                        }
                    }

                    slot.i = _nextLateUpdateProcessSlot++;
                    LateUpdateProcesses[slot.i] = coroutine;

                    if (null != tag)
                        AddTag(tag, slot);

                    if (layer.HasValue)
                        AddLayer((int)layer, slot);

                    _indexToHandle.Add(slot, handle);
                    _handleToIndex.Add(handle, slot);

                    if (!_runningLateUpdate && prewarm)
                    {
                        try
                        {
                            _runningLateUpdate = true;
                            SetTimeValues(slot.seg);

                            if (!LateUpdateProcesses[slot.i].MoveNext())
                            {
                                LateUpdateProcesses[slot.i] = null;
                            }
                            else if (LateUpdateProcesses[slot.i] != null && float.IsNaN(LateUpdateProcesses[slot.i].Current))
                            {
                                if (ReplacementFunction == null)
                                {
                                    LateUpdateProcesses[slot.i] = null;
                                }
                                else
                                {
                                    LateUpdateProcesses[slot.i] = ReplacementFunction(LateUpdateProcesses[slot.i], _indexToHandle[slot]);

                                    ReplacementFunction = null;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            LateUpdateProcesses[slot.i] = null;
                        }
                        finally
                        {
                            _runningLateUpdate = false;
                        }
                    }

                    return handle;

                case Segment.SlowUpdate:

                    if (_nextSlowUpdateProcessSlot >= SlowUpdateProcesses.Length)
                    {
                        IEnumerator<float>[] oldProcArray = SlowUpdateProcesses;
                        bool[] oldPausedArray = SlowUpdatePaused;

                        SlowUpdateProcesses = new IEnumerator<float>[SlowUpdateProcesses.Length + (ProcessArrayChunkSize * _expansions++)];
                        SlowUpdatePaused = new bool[SlowUpdateProcesses.Length];

                        for (int i = 0; i < oldProcArray.Length; i++)
                        {
                            SlowUpdateProcesses[i] = oldProcArray[i];
                            SlowUpdatePaused[i] = oldPausedArray[i];
                        }
                    }

                    slot.i = _nextSlowUpdateProcessSlot++;
                    SlowUpdateProcesses[slot.i] = coroutine;

                    if (null != tag)
                        AddTag(tag, slot);

                    if (layer.HasValue)
                        AddLayer((int)layer, slot);

                    _indexToHandle.Add(slot, handle);
                    _handleToIndex.Add(handle, slot);

                    if (!_runningSlowUpdate && prewarm)
                    {
                        try
                        {
                            _runningSlowUpdate = true;
                            SetTimeValues(slot.seg);

                            if (!SlowUpdateProcesses[slot.i].MoveNext())
                            {
                                SlowUpdateProcesses[slot.i] = null;
                            }
                            else if (SlowUpdateProcesses[slot.i] != null && float.IsNaN(SlowUpdateProcesses[slot.i].Current))
                            {
                                if (ReplacementFunction == null)
                                {
                                    SlowUpdateProcesses[slot.i] = null;
                                }
                                else
                                {
                                    SlowUpdateProcesses[slot.i] = ReplacementFunction(SlowUpdateProcesses[slot.i], _indexToHandle[slot]);

                                    ReplacementFunction = null;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            SlowUpdateProcesses[slot.i] = null;
                        }
                        finally
                        {
                            _runningSlowUpdate = false;
                        }
                    }

                    return handle;

                case Segment.RealtimeUpdate:

                    if (_nextRealtimeUpdateProcessSlot >= RealtimeUpdateProcesses.Length)
                    {
                        IEnumerator<float>[] oldProcArray = RealtimeUpdateProcesses;
                        bool[] oldPausedArray = RealtimeUpdatePaused;

                        RealtimeUpdateProcesses = new IEnumerator<float>[RealtimeUpdateProcesses.Length + (ProcessArrayChunkSize * _expansions++)];
                        RealtimeUpdatePaused = new bool[RealtimeUpdateProcesses.Length];

                        for (int i = 0; i < oldProcArray.Length; i++)
                        {
                            RealtimeUpdateProcesses[i] = oldProcArray[i];
                            RealtimeUpdatePaused[i] = oldPausedArray[i];
                        }
                    }

                    slot.i = _nextRealtimeUpdateProcessSlot++;
                    RealtimeUpdateProcesses[slot.i] = coroutine;

                    if (null != tag)
                        AddTag(tag, slot);

                    if (layer.HasValue)
                        AddLayer((int)layer, slot);

                    _indexToHandle.Add(slot, handle);
                    _handleToIndex.Add(handle, slot);

                    if (!_runningRealtimeUpdate && prewarm)
                    {
                        try
                        {
                            _runningRealtimeUpdate = true;
                            SetTimeValues(slot.seg);

                            if (!RealtimeUpdateProcesses[slot.i].MoveNext())
                            {
                                RealtimeUpdateProcesses[slot.i] = null;
                            }
                            else if (RealtimeUpdateProcesses[slot.i] != null && float.IsNaN(RealtimeUpdateProcesses[slot.i].Current))
                            {
                                if (ReplacementFunction == null)
                                {
                                    RealtimeUpdateProcesses[slot.i] = null;
                                }
                                else
                                {
                                    RealtimeUpdateProcesses[slot.i] = ReplacementFunction(RealtimeUpdateProcesses[slot.i], _indexToHandle[slot]);

                                    ReplacementFunction = null;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            RealtimeUpdateProcesses[slot.i] = null;
                        }
                        finally
                        {
                            _runningRealtimeUpdate = false;
                        }
                    }

                    return handle;

                case Segment.EditorUpdate:

                    if (!OnEditorStart())
                        return new CoroutineHandle();

                    if (_nextEditorUpdateProcessSlot >= EditorUpdateProcesses.Length)
                    {
                        IEnumerator<float>[] oldProcArray = EditorUpdateProcesses;
                        bool[] oldPausedArray = EditorUpdatePaused;

                        EditorUpdateProcesses = new IEnumerator<float>[EditorUpdateProcesses.Length + (ProcessArrayChunkSize * _expansions++)];
                        EditorUpdatePaused = new bool[EditorUpdateProcesses.Length];

                        for (int i = 0; i < oldProcArray.Length; i++)
                        {
                            EditorUpdateProcesses[i] = oldProcArray[i];
                            EditorUpdatePaused[i] = oldPausedArray[i];
                        }
                    }

                    slot.i = _nextEditorUpdateProcessSlot++;
                    EditorUpdateProcesses[slot.i] = coroutine;

                    if (null != tag)
                        AddTag(tag, slot);

                    if (layer.HasValue)
                        AddLayer((int)layer, slot);

                    _indexToHandle.Add(slot, handle);
                    _handleToIndex.Add(handle, slot);

                    if (!_runningEditorUpdate && prewarm)
                    {
                        try
                        {
                            _runningEditorUpdate = true;
                            SetTimeValues(slot.seg);

                            if (!EditorUpdateProcesses[slot.i].MoveNext())
                            {
                                EditorUpdateProcesses[slot.i] = null;
                            }
                            else if (EditorUpdateProcesses[slot.i] != null && float.IsNaN(EditorUpdateProcesses[slot.i].Current))
                            {
                                if (ReplacementFunction == null)
                                {
                                    EditorUpdateProcesses[slot.i] = null;
                                }
                                else
                                {
                                    EditorUpdateProcesses[slot.i] = ReplacementFunction(EditorUpdateProcesses[slot.i], _indexToHandle[slot]);

                                    ReplacementFunction = null;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            EditorUpdateProcesses[slot.i] = null;
                        }
                        finally
                        {
                            _runningEditorUpdate = false;
                        }
                    }

                    return handle;

                case Segment.EditorSlowUpdate:

                    if (!OnEditorStart())
                        return new CoroutineHandle();

                    if (_nextEditorSlowUpdateProcessSlot >= EditorSlowUpdateProcesses.Length)
                    {
                        IEnumerator<float>[] oldProcArray = EditorSlowUpdateProcesses;
                        bool[] oldPausedArray = EditorSlowUpdatePaused;

                        EditorSlowUpdateProcesses = new IEnumerator<float>[EditorSlowUpdateProcesses.Length + (ProcessArrayChunkSize * _expansions++)];
                        EditorSlowUpdatePaused = new bool[EditorSlowUpdateProcesses.Length];

                        for (int i = 0; i < oldProcArray.Length; i++)
                        {
                            EditorSlowUpdateProcesses[i] = oldProcArray[i];
                            EditorSlowUpdatePaused[i] = oldPausedArray[i];
                        }
                    }

                    slot.i = _nextEditorSlowUpdateProcessSlot++;
                    EditorSlowUpdateProcesses[slot.i] = coroutine;

                    if (null != tag)
                        AddTag(tag, slot);

                    if (layer.HasValue)
                        AddLayer((int)layer, slot);

                    _indexToHandle.Add(slot, handle);
                    _handleToIndex.Add(handle, slot);

                    if (!_runningEditorSlowUpdate && prewarm)
                    {
                        try
                        {
                            _runningEditorSlowUpdate = true;
                            SetTimeValues(slot.seg);

                            if (!EditorSlowUpdateProcesses[slot.i].MoveNext())
                            {
                                EditorSlowUpdateProcesses[slot.i] = null;
                            }
                            else if (EditorSlowUpdateProcesses[slot.i] != null && float.IsNaN(EditorSlowUpdateProcesses[slot.i].Current))
                            {
                                if (ReplacementFunction == null)
                                {
                                    EditorSlowUpdateProcesses[slot.i] = null;
                                }
                                else
                                {
                                    EditorSlowUpdateProcesses[slot.i] = ReplacementFunction(EditorSlowUpdateProcesses[slot.i], _indexToHandle[slot]);

                                    ReplacementFunction = null;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            if (OnError == null)
                                _exceptions.Enqueue(ex);
                            else
                                OnError(ex);

                            EditorSlowUpdateProcesses[slot.i] = null;
                        }
                        finally
                        {
                            _runningEditorSlowUpdate = false;
                        }
                    }

                    return handle;

                case Segment.EndOfFrame:

                    if (_nextEndOfFrameProcessSlot >= EndOfFrameProcesses.Length)
                    {
                        IEnumerator<float>[] oldProcArray = EndOfFrameProcesses;
                        bool[] oldPausedArray = EndOfFramePaused;

                        EndOfFrameProcesses = new IEnumerator<float>[EndOfFrameProcesses.Length + (ProcessArrayChunkSize * _expansions++)];
                        EndOfFramePaused = new bool[EndOfFrameProcesses.Length];

                        for (int i = 0; i < oldProcArray.Length; i++)
                        {
                            EndOfFrameProcesses[i] = oldProcArray[i];
                            EndOfFramePaused[i] = oldPausedArray[i];
                        }
                    }

                    slot.i = _nextEndOfFrameProcessSlot++;
                    EndOfFrameProcesses[slot.i] = coroutine;

                    if (null != tag)
                        AddTag(tag, slot);

                    if (layer.HasValue)
                        AddLayer((int)layer, slot);

                    _indexToHandle.Add(slot, handle);
                    _handleToIndex.Add(handle, slot);

                    RunCoroutineSingletonOnInstance(_EOFPumpWatcher(), "MEC_EOFPumpWatcher");

                    return handle;

                case Segment.ManualTimeframe:

                    if (_nextManualTimeframeProcessSlot >= ManualTimeframeProcesses.Length)
                    {
                        IEnumerator<float>[] oldProcArray = ManualTimeframeProcesses;
                        bool[] oldPausedArray = ManualTimeframePaused;

                        ManualTimeframeProcesses = new IEnumerator<float>[ManualTimeframeProcesses.Length + (ProcessArrayChunkSize * _expansions++)];
                        ManualTimeframePaused = new bool[ManualTimeframeProcesses.Length];

                        for (int i = 0; i < oldProcArray.Length; i++)
                        {
                            ManualTimeframeProcesses[i] = oldProcArray[i];
                            ManualTimeframePaused[i] = oldPausedArray[i];
                        }
                    }

                    slot.i = _nextManualTimeframeProcessSlot++;
                    ManualTimeframeProcesses[slot.i] = coroutine;

                    if (null != tag)
                        AddTag(tag, slot);

                    if (layer.HasValue)
                        AddLayer((int)layer, slot);

                    _indexToHandle.Add(slot, handle);
                    _handleToIndex.Add(handle, slot);

                    return handle;

                default:
                    return new CoroutineHandle();
            }
        }

        /// <summary>
        /// This will kill all coroutines running on the main MEC instance and reset the context.
        /// </summary>
        /// <returns>The number of coroutines that were killed.</returns>
        public static int KillCoroutines()
        {
            return _instance == null ? 0 : _instance.KillCoroutinesOnInstance();
        }

        /// <summary>
        /// This will kill all coroutines running on the current MEC instance and reset the context.
        /// </summary>
        /// <returns>The number of coroutines that were killed.</returns>
        public int KillCoroutinesOnInstance()
        {
            int retVal = _nextUpdateProcessSlot + _nextLateUpdateProcessSlot + _nextFixedUpdateProcessSlot + _nextSlowUpdateProcessSlot +
                         _nextRealtimeUpdateProcessSlot + _nextEditorUpdateProcessSlot + _nextEditorSlowUpdateProcessSlot + 
                         _nextEndOfFrameProcessSlot + _nextManualTimeframeProcessSlot;

            UpdateProcesses = new IEnumerator<float>[InitialBufferSizeLarge];
            UpdatePaused = new bool[InitialBufferSizeLarge];
            UpdateCoroutines = 0;
            _nextUpdateProcessSlot = 0;

            LateUpdateProcesses = new IEnumerator<float>[InitialBufferSizeSmall];
            LateUpdatePaused = new bool[InitialBufferSizeSmall];
            LateUpdateCoroutines = 0;
            _nextLateUpdateProcessSlot = 0;

            FixedUpdateProcesses = new IEnumerator<float>[InitialBufferSizeMedium];
            FixedUpdatePaused = new bool[InitialBufferSizeMedium];
            FixedUpdateCoroutines = 0;
            _nextFixedUpdateProcessSlot = 0;

            SlowUpdateProcesses = new IEnumerator<float>[InitialBufferSizeMedium];
            SlowUpdatePaused = new bool[InitialBufferSizeMedium];
            SlowUpdateCoroutines = 0;
            _nextSlowUpdateProcessSlot = 0;

            RealtimeUpdateProcesses = new IEnumerator<float>[InitialBufferSizeSmall];
            RealtimeUpdatePaused = new bool[InitialBufferSizeSmall];
            RealtimeUpdateCoroutines = 0;
            _nextRealtimeUpdateProcessSlot = 0;

            EditorUpdateProcesses = new IEnumerator<float>[InitialBufferSizeSmall];
            EditorUpdatePaused = new bool[InitialBufferSizeSmall];
            EditorUpdateCoroutines = 0;
            _nextEditorUpdateProcessSlot = 0;

            EditorSlowUpdateProcesses = new IEnumerator<float>[InitialBufferSizeSmall];
            EditorSlowUpdatePaused = new bool[InitialBufferSizeSmall];
            EditorSlowUpdateCoroutines = 0;
            _nextEditorSlowUpdateProcessSlot = 0;

            EndOfFrameProcesses = new IEnumerator<float>[InitialBufferSizeSmall];
            EndOfFramePaused = new bool[InitialBufferSizeSmall];
            EndOfFrameCoroutines = 0;
            _nextEndOfFrameProcessSlot = 0;

            ManualTimeframeProcesses = new IEnumerator<float>[InitialBufferSizeSmall];
            ManualTimeframePaused = new bool[InitialBufferSizeSmall];
            ManualTimeframeCoroutines = 0;
            _nextManualTimeframeProcessSlot = 0;

            _processTags.Clear();
            _taggedProcesses.Clear();
            _processLayers.Clear();
            _layeredProcesses.Clear();
            _handleToIndex.Clear();
            _indexToHandle.Clear();
            _waitingTriggers.Clear();
            _expansions = (ushort)((_expansions / 2) + 1);

            ResetTimeCountOnInstance();

#if UNITY_EDITOR
            EditorApplication.update -= OnEditorUpdate;
#endif
            return retVal;
        }

        /// <summary>
        /// Kills the instance of the coroutine handle if it exists.
        /// </summary>
        /// <param name="handle">The handle of the coroutine to kill.</param>
        /// <returns>The number of coroutines that were found and killed (0 or 1).</returns>
        public static int KillCoroutines(CoroutineHandle handle)
        {
            return ActiveInstances.ContainsKey(handle.Key) ? GetInstance(handle.Key).KillCoroutinesOnInstance(handle) : 0;
        }

        /// <summary>
        /// Kills the instance of the coroutine handle on this Timing instance if it exists.
        /// </summary>
        /// <param name="handle">The handle of the coroutine to kill.</param>
        /// <returns>The number of coroutines that were found and killed (0 or 1).</returns>
        public int KillCoroutinesOnInstance(CoroutineHandle handle)
        {
            bool foundOne = false;

            if (_handleToIndex.ContainsKey(handle))
            {
                if (_waitingTriggers.ContainsKey(handle))
                    CloseWaitingProcess(handle);

                foundOne = CoindexExtract(_handleToIndex[handle]) != null;
                RemoveGraffiti(_handleToIndex[handle]);
            }

            return foundOne ? 1 : 0;
        }

        /// <summary>
        /// Kills all coroutines that have the given tag.
        /// </summary>
        /// <param name="tag">All coroutines with this tag will be killed.</param>
        /// <returns>The number of coroutines that were found and killed.</returns>
        public static int KillCoroutines(string tag)
        {
            return _instance == null ? 0 : _instance.KillCoroutinesOnInstance(tag);
        }

        /// <summary> 
        /// Kills all coroutines that have the given tag.
        /// </summary>
        /// <param name="tag">All coroutines with this tag will be killed.</param>
        /// <returns>The number of coroutines that were found and killed.</returns>
        public int KillCoroutinesOnInstance(string tag)
        {
            if (tag == null) return 0;
            int numberFound = 0;

            while (_taggedProcesses.ContainsKey(tag))
            {
                var matchEnum = _taggedProcesses[tag].GetEnumerator();
                matchEnum.MoveNext();

                if (CoindexKill(matchEnum.Current))
                {
                    if(_waitingTriggers.ContainsKey(_indexToHandle[matchEnum.Current]))
                        CloseWaitingProcess(_indexToHandle[matchEnum.Current]);

                    numberFound++;
                }

                RemoveGraffiti(matchEnum.Current);
            }

            return numberFound;
        }

        /// <summary>
        /// Kills all coroutines on the given layer.
        /// </summary>
        /// <param name="layer">All coroutines on this layer will be killed.</param>
        /// <returns>The number of coroutines that were found and killed.</returns>
        public static int KillCoroutines(int layer)
        {
            return _instance == null ? 0 : _instance.KillCoroutinesOnInstance(layer);
        }

        /// <summary> 
        /// Kills all coroutines on the given layer.
        /// </summary>
        /// <param name="layer">All coroutines on this layer will be killed.</param>
        /// <returns>The number of coroutines that were found and killed.</returns>
        public int KillCoroutinesOnInstance(int layer)
        {
            int numberFound = 0;

            while (_layeredProcesses.ContainsKey(layer))
            {
                var matchEnum = _layeredProcesses[layer].GetEnumerator();
                matchEnum.MoveNext();

                if (CoindexKill(matchEnum.Current))
                {
                    if (_waitingTriggers.ContainsKey(_indexToHandle[matchEnum.Current]))
                        CloseWaitingProcess(_indexToHandle[matchEnum.Current]);

                    numberFound++;
                }

                RemoveGraffiti(matchEnum.Current);
            }

            return numberFound;
        }

        /// <summary>
        /// Kills all coroutines with the given tag on the given layer.
        /// </summary>
        /// <param name="layer">All coroutines on this layer with the given tag will be killed.</param>
        /// <param name="tag">All coroutines with this tag on the given layer will be killed.</param>
        /// <returns>The number of coroutines that were found and killed.</returns>
        public static int KillCoroutines(int layer, string tag)
        {
            return _instance == null ? 0 : _instance.KillCoroutinesOnInstance(layer, tag);
        }

        /// <summary> 
        /// Kills all coroutines with the given tag on the given layer.
        /// </summary>
        /// <param name="layer">All coroutines on this layer with the given tag will be killed.</param>
        /// <param name="tag">All coroutines with this tag on the given layer will be killed.</param>
        /// <returns>The number of coroutines that were found and killed.</returns>
        public int KillCoroutinesOnInstance(int layer, string tag)
        {
            if (tag == null)
                return KillCoroutinesOnInstance(layer);
            if (!_layeredProcesses.ContainsKey(layer) || !_taggedProcesses.ContainsKey(tag))
                return 0;
            int count = 0;

            var indexesEnum = _taggedProcesses[tag].GetEnumerator();
            while(indexesEnum.MoveNext())
            {
                if (CoindexIsNull(indexesEnum.Current) || !_layeredProcesses[layer].Contains(indexesEnum.Current) || !CoindexKill(indexesEnum.Current))
                    continue;

                if (_waitingTriggers.ContainsKey(_indexToHandle[indexesEnum.Current]))
                    CloseWaitingProcess(_indexToHandle[indexesEnum.Current]);

                count++;
                RemoveGraffiti(indexesEnum.Current);

                if (!_taggedProcesses.ContainsKey(tag) || !_layeredProcesses.ContainsKey(layer))
                    break;

                indexesEnum = _taggedProcesses[tag].GetEnumerator();
            }

            return count;
        }

        /// <summary>
        /// Retrieves the MEC manager that corresponds to the supplied instance id.
        /// </summary>
        /// <param name="ID">The instance ID.</param>
        /// <returns>The manager, or null if not found.</returns>
        public static Timing GetInstance(byte ID)
        {
            return ActiveInstances.ContainsKey(ID) ? ActiveInstances[ID] : null;
        }

        /// <summary>
        /// Use "yield return Timing.WaitForSeconds(time);" to wait for the specified number of seconds.
        /// </summary>
        /// <param name="waitTime">Number of seconds to wait.</param>
        public static float WaitForSeconds(float waitTime)
        {
            if (float.IsNaN(waitTime)) waitTime = 0f;
            return LocalTime + waitTime;
        }

        /// <summary>
        /// Use "yield return timingInstance.WaitForSecondsOnInstance(time);" to wait for the specified number of seconds.
        /// </summary>
        /// <param name="waitTime">Number of seconds to wait.</param>
        public float WaitForSecondsOnInstance(float waitTime)
        {
            if (float.IsNaN(waitTime)) waitTime = 0f;
            return (float)localTime + waitTime;
        }

        private void UpdateTimeValues(Segment segment)
        {
            switch(segment)
            {
                case Segment.Update:
                    deltaTime = Time.deltaTime;
                    _lastUpdateTime += deltaTime;
                    localTime = _lastUpdateTime;
                    return;
                case Segment.LateUpdate:
                    deltaTime = Time.deltaTime;
                    _lastLateUpdateTime += deltaTime;
                    localTime = _lastLateUpdateTime;
                    return;
                case Segment.FixedUpdate:
                    deltaTime = Time.deltaTime;
                    _lastFixedUpdateTime += deltaTime;
                    localTime = _lastFixedUpdateTime;
                    return;
                case Segment.SlowUpdate:
                    if(_lastSlowUpdateTime == 0d)
                        deltaTime = TimeBetweenSlowUpdateCalls;
                    else
                        deltaTime = Time.realtimeSinceStartup - (float)_lastSlowUpdateTime;

                    localTime = _lastSlowUpdateTime = Time.realtimeSinceStartup;
                    return;
                case Segment.RealtimeUpdate:
                    deltaTime = Time.unscaledDeltaTime;
                    _lastRealtimeUpdateTime += deltaTime;
                    localTime = _lastRealtimeUpdateTime;
                    return;
#if UNITY_EDITOR
                case Segment.EditorUpdate:
                    deltaTime = (float)(EditorApplication.timeSinceStartup - _lastEditorUpdateTime);

                    if(deltaTime > Time.maximumDeltaTime)
                        deltaTime = Time.maximumDeltaTime;

                    localTime = _lastEditorUpdateTime = EditorApplication.timeSinceStartup;
                    return;
                case Segment.EditorSlowUpdate:
                    deltaTime = (float)(EditorApplication.timeSinceStartup - _lastEditorSlowUpdateTime);
                    localTime = _lastEditorSlowUpdateTime = EditorApplication.timeSinceStartup;
                    return;
#endif
                case Segment.EndOfFrame:
                    deltaTime = Time.deltaTime;
                    localTime = _lastUpdateTime;
                    return;
                case Segment.ManualTimeframe:
                    localTime = SetManualTimeframeTime == null ? Time.time : SetManualTimeframeTime(_lastManualTimeframeTime);
                    deltaTime = (float)(localTime - _lastManualTimeframeTime);

                    if (deltaTime > Time.maximumDeltaTime)
                        deltaTime = Time.maximumDeltaTime;

                    _lastManualTimeframeTime = localTime;
                    return;
            }
        }

        private void SetTimeValues(Segment segment)
        {
            switch (segment)
            {
                case Segment.Update:
                    deltaTime = Time.deltaTime;
                    localTime = _lastUpdateTime;
                    return;
                case Segment.LateUpdate:
                    deltaTime = Time.deltaTime;
                    localTime = _lastLateUpdateTime;
                    return;
                case Segment.FixedUpdate:
                    deltaTime = Time.deltaTime;
                    localTime = _lastFixedUpdateTime;
                    return;
                case Segment.SlowUpdate:
                    deltaTime = Time.realtimeSinceStartup - (float)_lastSlowUpdateTime;
                    localTime = _lastSlowUpdateTime;
                    return;
                case Segment.RealtimeUpdate:
                    deltaTime = Time.unscaledDeltaTime;
                    localTime = _lastRealtimeUpdateTime;
                    return;
#if UNITY_EDITOR
                case Segment.EditorUpdate:
                    deltaTime = (float)(EditorApplication.timeSinceStartup - _lastEditorUpdateTime);

                    if (deltaTime > Time.maximumDeltaTime)
                        deltaTime = Time.maximumDeltaTime;

                    localTime = _lastEditorUpdateTime;
                    return;
                case Segment.EditorSlowUpdate:
                    deltaTime = (float)(EditorApplication.timeSinceStartup - _lastEditorSlowUpdateTime);
                    localTime = _lastEditorSlowUpdateTime;
                    return;
#endif
                case Segment.EndOfFrame:
                    deltaTime = Time.deltaTime;
                    localTime = _lastUpdateTime;
                    return;
                case Segment.ManualTimeframe:
                    deltaTime = Time.deltaTime;
                    localTime = _lastManualTimeframeTime;
                    return;
            }
        }

        private double GetSegmentTime(Segment segment)
        {
            switch (segment)
            {
                case Segment.Update:
                    return _lastUpdateTime;
                case Segment.LateUpdate:
                    return _lastLateUpdateTime;
                case Segment.FixedUpdate:
                    return _lastFixedUpdateTime;
                case Segment.SlowUpdate:
                   return _lastSlowUpdateTime;
                case Segment.RealtimeUpdate:
                    return _lastRealtimeUpdateTime;
                case Segment.EditorUpdate:
                    return _lastEditorUpdateTime;
                case Segment.EditorSlowUpdate:
                    return _lastEditorSlowUpdateTime;
                case Segment.EndOfFrame:
                    return _lastUpdateTime;
                case Segment.ManualTimeframe:
                    return _lastManualTimeframeTime;
                default:
                    return 0d;
            }
        }

        /// <summary>
        /// Resets the value of LocalTime to zero (only for the Update, LateUpdate, FixedUpdate, and RealtimeUpdate loops).
        /// </summary>
        public void ResetTimeCountOnInstance()
        {
            localTime = 0d;

            _lastUpdateTime = 0d;
            _lastLateUpdateTime = 0d;
            _lastFixedUpdateTime = 0d;
            _lastRealtimeUpdateTime = 0d;

            _EOFPumpRan = false;
        }

        /// <summary>
        /// This will pause all coroutines running on the current MEC instance until ResumeCoroutines is called.
        /// </summary>
        /// <returns>The number of coroutines that were paused.</returns>
        public static int PauseCoroutines()
        {
            return _instance == null ? 0 : _instance.PauseCoroutinesOnInstance();
        }

        /// <summary>
        /// This will pause all coroutines running on this MEC instance until ResumeCoroutinesOnInstance is called.
        /// </summary>
        /// <returns>The number of coroutines that were paused.</returns>
        public int PauseCoroutinesOnInstance()
        {
            int count = 0;
            int i;
            for (i = 0;i < _nextUpdateProcessSlot;i++)
            {
                if (!UpdatePaused[i] && UpdateProcesses[i] != null)
                {
                    UpdatePaused[i] = true;
                    count++;
                }
            }

            for (i = 0; i < _nextLateUpdateProcessSlot; i++)
            {
                if (!LateUpdatePaused[i] && LateUpdateProcesses[i] != null)
                {
                    LateUpdatePaused[i] = true;
                    count++;
                }
            }

            for (i = 0; i < _nextFixedUpdateProcessSlot; i++)
            {
                if (!FixedUpdatePaused[i] && FixedUpdateProcesses[i] != null)
                {
                    FixedUpdatePaused[i] = true;
                    count++;
                }
            }

            for (i = 0; i < _nextSlowUpdateProcessSlot; i++)
            {
                if (!SlowUpdatePaused[i] && SlowUpdateProcesses[i] != null)
                {
                    SlowUpdatePaused[i] = true;
                    count++;
                }
            }

            for (i = 0; i < _nextRealtimeUpdateProcessSlot; i++)
            {
                if (!RealtimeUpdatePaused[i] && RealtimeUpdateProcesses[i] != null)
                {
                    RealtimeUpdatePaused[i] = true;
                    count++;
                }
            }

            for (i = 0; i < _nextEditorUpdateProcessSlot; i++)
            {
                if (!EditorUpdatePaused[i] && EditorUpdateProcesses[i] != null)
                {
                    EditorUpdatePaused[i] = true;
                    count++;
                }
            }

            for (i = 0; i < _nextEditorSlowUpdateProcessSlot; i++)
            {
                if (!EditorSlowUpdatePaused[i] && EditorSlowUpdateProcesses[i] != null)
                {
                    EditorSlowUpdatePaused[i] = true;
                    count++;
                }
            }

            for (i = 0; i < _nextEndOfFrameProcessSlot; i++)
            {
                if (!EndOfFramePaused[i] && EndOfFrameProcesses[i] != null)
                {
                    EndOfFramePaused[i] = true;
                    count++;
                }
            }

            for (i = 0; i < _nextManualTimeframeProcessSlot; i++)
            {
                if (!ManualTimeframePaused[i] && ManualTimeframeProcesses[i] != null)
                {
                    ManualTimeframePaused[i] = true;
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// This will pause any matching coroutines running on the current MEC instance until ResumeCoroutines is called.
        /// </summary>
        /// <param name="tag">Any coroutines with a matching tag will be paused.</param>
        /// <returns>The number of coroutines that were paused.</returns>
        public static int PauseCoroutines(string tag)
        {
            return _instance == null ? 0 : _instance.PauseCoroutinesOnInstance(tag);
        }

        /// <summary>
        /// This will pause any matching coroutines running on this MEC instance until ResumeCoroutinesOnInstance is called.
        /// </summary>
        /// <param name="tag">Any coroutines with a matching tag will be paused.</param>
        /// <returns>The number of coroutines that were paused.</returns>
        public int PauseCoroutinesOnInstance(string tag)
        {
            if (tag == null || !_taggedProcesses.ContainsKey(tag)) 
                return 0;

            int count = 0;
            var matchesEnum = _taggedProcesses[tag].GetEnumerator();

            while (matchesEnum.MoveNext())
                if (!CoindexIsNull(matchesEnum.Current) && !CoindexSetPause(matchesEnum.Current))
                    count++;

            return count;
        }

        /// <summary>
        /// This will pause any matching coroutines running on the current MEC instance until ResumeCoroutines is called.
        /// </summary>
        /// <param name="layer">Any coroutines on the matching layer will be paused.</param>
        /// <returns>The number of coroutines that were paused.</returns>
        public static int PauseCoroutines(int layer)
        {
            return _instance == null ? 0 : _instance.PauseCoroutinesOnInstance(layer);
        }

        /// <summary>
        /// This will pause any matching coroutines running on this MEC instance until ResumeCoroutinesOnInstance is called.
        /// </summary>
        /// <param name="layer">Any coroutines on the matching layer will be paused.</param>
        /// <returns>The number of coroutines that were paused.</returns>
        public int PauseCoroutinesOnInstance(int layer)
        {
            if (!_layeredProcesses.ContainsKey(layer))
                return 0;

            int count = 0;
            var matchesEnum = _layeredProcesses[layer].GetEnumerator();

            while (matchesEnum.MoveNext())
                if (!CoindexIsNull(matchesEnum.Current) && !CoindexSetPause(matchesEnum.Current))
                    count++;

            return count;
        }

        /// <summary>
        /// This will pause any matching coroutines running on the current MEC instance until ResumeCoroutines is called.
        /// </summary>
        /// <param name="layer">Any coroutines on the matching layer will be paused.</param>
        /// <param name="tag">Any coroutines with a matching tag will be paused.</param>
        /// <returns>The number of coroutines that were paused.</returns>
        public static int PauseCoroutines(int layer, string tag)
        {
            return _instance == null ? 0 : _instance.PauseCoroutinesOnInstance(layer, tag);
        }

        /// <summary>
        /// This will pause any matching coroutines running on this MEC instance until ResumeCoroutinesOnInstance is called.
        /// </summary>
        /// <param name="layer">Any coroutines on the matching layer will be paused.</param>
        /// <param name="tag">Any coroutines with a matching tag will be paused.</param>
        /// <returns>The number of coroutines that were paused.</returns>
        public int PauseCoroutinesOnInstance(int layer, string tag)
        {
            if (tag == null)
                return PauseCoroutinesOnInstance(layer);

            if (!_taggedProcesses.ContainsKey(tag) || !_layeredProcesses.ContainsKey(layer))
                return 0;

            int count = 0;
            var matchesEnum = _taggedProcesses[tag].GetEnumerator();

            while (matchesEnum.MoveNext())
                if (_processLayers.ContainsKey(matchesEnum.Current) && _processLayers[matchesEnum.Current] == layer 
                    && !CoindexIsNull(matchesEnum.Current) && !CoindexSetPause(matchesEnum.Current))
                        count++;

            return count;
        }

        /// <summary>
        /// This will pause any matching coroutines until ResumeCoroutines is called.
        /// </summary>
        /// <param name="handle">The handle of the coroutine to pause.</param>
        /// <returns>The number of coroutines that were paused (0 or 1).</returns>
        public static int PauseCoroutines(CoroutineHandle handle)
        {
            return ActiveInstances.ContainsKey(handle.Key) ? GetInstance(handle.Key).PauseCoroutinesOnInstance(handle) : 0;
        }

        /// <summary>
        /// This will pause any matching coroutines running on this MEC instance until ResumeCoroutinesOnInstance is called.
        /// </summary>
        /// <param name="handle">The handle of the coroutine to pause.</param>
        /// <returns>The number of coroutines that were paused (0 or 1).</returns>
        public int PauseCoroutinesOnInstance(CoroutineHandle handle)
        {
            return _handleToIndex.ContainsKey(handle) && !CoindexIsNull(_handleToIndex[handle]) && !CoindexSetPause(_handleToIndex[handle]) ? 1 : 0;
        }

        /// <summary>
        /// This resumes all coroutines on the current MEC instance if they are currently paused, otherwise it has
        /// no effect.
        /// </summary>
        /// <returns>The number of coroutines that were resumed.</returns>
        public static int ResumeCoroutines()
        {
            return _instance == null ? 0 : _instance.ResumeCoroutinesOnInstance();
        }

        /// <summary>
        /// This resumes all coroutines on this MEC instance if they are currently paused, otherwise it has no effect.
        /// </summary>
        /// <returns>The number of coroutines that were resumed.</returns>
        public int ResumeCoroutinesOnInstance()
        {
            int count = 0;
            int i;
            for (i = 0; i < _nextUpdateProcessSlot; i++)
            {
                if (UpdatePaused[i] && UpdateProcesses[i] != null)
                {
                    UpdatePaused[i] = false;
                    count++;
                }
            }

            for (i = 0; i < _nextLateUpdateProcessSlot; i++)
            {
                if (LateUpdatePaused[i] && LateUpdateProcesses[i] != null)
                {
                    LateUpdatePaused[i] = false;
                    count++;
                }
            }

            for (i = 0; i < _nextFixedUpdateProcessSlot; i++)
            {
                if (FixedUpdatePaused[i] && FixedUpdateProcesses[i] != null)
                {
                    FixedUpdatePaused[i] = false;
                    count++;
                }
            }

            for (i = 0; i < _nextSlowUpdateProcessSlot; i++)
            {
                if (SlowUpdatePaused[i] && SlowUpdateProcesses[i] != null)
                {
                    SlowUpdatePaused[i] = false;
                    count++;
                }
            }

            for (i = 0; i < _nextRealtimeUpdateProcessSlot; i++)
            {
                if (RealtimeUpdatePaused[i] && RealtimeUpdateProcesses[i] != null)
                {
                    RealtimeUpdatePaused[i] = false;
                    count++;
                }
            }

            for (i = 0; i < _nextEditorUpdateProcessSlot; i++)
            {
                if (EditorUpdatePaused[i] && EditorUpdateProcesses[i] != null)
                {
                    EditorUpdatePaused[i] = false;
                    count++;
                }
            }

            for (i = 0; i < _nextEditorSlowUpdateProcessSlot; i++)
            {
                if (EditorSlowUpdatePaused[i] && EditorSlowUpdateProcesses[i] != null)
                {
                    EditorSlowUpdatePaused[i] = false;
                    count++;
                }
            }

            for (i = 0; i < _nextEndOfFrameProcessSlot; i++)
            {
                if (EndOfFramePaused[i] && EndOfFrameProcesses[i] != null)
                {
                    EndOfFramePaused[i] = false;
                    count++;
                }
            }

            for (i = 0; i < _nextManualTimeframeProcessSlot; i++)
            {
                if (ManualTimeframePaused[i] && ManualTimeframeProcesses[i] != null)
                {
                    ManualTimeframePaused[i] = false;
                    count++;
                }
            }

            var waitingEnum = _waitingTriggers.GetEnumerator();
            while(waitingEnum.MoveNext())
            {
                int listCount = 0;
                var pausedList = waitingEnum.Current.Value.GetEnumerator();

                while(pausedList.MoveNext())
                {
                    if (_handleToIndex.ContainsKey(pausedList.Current.Handle) && !CoindexIsNull(_handleToIndex[pausedList.Current.Handle]))
                    {
                        CoindexSetPause(_handleToIndex[pausedList.Current.Handle]);
                        listCount++;
                    }
                    else
                    {
                        waitingEnum.Current.Value.Remove(pausedList.Current);
                        listCount = 0;
                        pausedList = waitingEnum.Current.Value.GetEnumerator();
                    }
                }

                count -= listCount;
            }

            return count;
        }

        /// <summary>
        /// This resumes any matching coroutines on the current MEC instance if they are currently paused, otherwise it has no effect.
        /// </summary>
        /// <param name="tag">Any coroutines previously paused with a matching tag will be resumend.</param>
        /// <returns>The number of coroutines that were resumed.</returns>
        public static int ResumeCoroutines(string tag)
        {
            return _instance == null ? 0 : _instance.ResumeCoroutinesOnInstance(tag);
        }

        /// <summary>
        /// This resumes any matching coroutines on this MEC instance if they are currently paused, otherwise it has no effect.
        /// </summary>
        /// <param name="tag">Any coroutines previously paused with a matching tag will be resumend.</param>
        /// <returns>The number of coroutines that were resumed.</returns>
        public int ResumeCoroutinesOnInstance(string tag)
        {
            if (tag == null || !_taggedProcesses.ContainsKey(tag))
                return 0;
            int count = 0;

            var indexesEnum = _taggedProcesses[tag].GetEnumerator();
            while (indexesEnum.MoveNext())
                if (!CoindexIsNull(indexesEnum.Current) && CoindexSetPause(indexesEnum.Current, false))
                    count++;

            var waitingEnum = _waitingTriggers.GetEnumerator();
            while (waitingEnum.MoveNext())
            {
                var pausedList = waitingEnum.Current.Value.GetEnumerator();
                while (pausedList.MoveNext())
                {
                    if (_handleToIndex.ContainsKey(pausedList.Current.Handle) && !CoindexIsNull(_handleToIndex[pausedList.Current.Handle])
                        && !CoindexSetPause(_handleToIndex[pausedList.Current.Handle]))
                            count--;
                }
            }

            return count;
        }

        /// <summary>
        /// This resumes any matching coroutines on the current MEC instance if they are currently paused, otherwise it has
        /// no effect.
        /// </summary>
        /// <param name="layer">Any coroutines previously paused on the matching layer will be resumend.</param>
        /// <returns>The number of coroutines that were resumed.</returns>
        public static int ResumeCoroutines(int layer)
        {
            return _instance == null ? 0 : _instance.ResumeCoroutinesOnInstance(layer);
        }

        /// <summary>
        /// This resumes any matching coroutines on this MEC instance if they are currently paused, otherwise it has no effect.
        /// </summary>
        /// <param name="layer">Any coroutines previously paused on the matching layer will be resumend.</param>
        /// <returns>The number of coroutines that were resumed.</returns>
        public int ResumeCoroutinesOnInstance(int layer)
        {
            if (!_layeredProcesses.ContainsKey(layer))
                return 0;
            int count = 0;

            var indexesEnum = _layeredProcesses[layer].GetEnumerator();
            while (indexesEnum.MoveNext())
                if (!CoindexIsNull(indexesEnum.Current) && CoindexSetPause(indexesEnum.Current, false))
                    count++;

            var waitingEnum = _waitingTriggers.GetEnumerator();
            while (waitingEnum.MoveNext())
            {
                var pausedList = waitingEnum.Current.Value.GetEnumerator();
                while (pausedList.MoveNext())
                {
                    if (_handleToIndex.ContainsKey(pausedList.Current.Handle) && !CoindexIsNull(_handleToIndex[pausedList.Current.Handle])
                        && !CoindexSetPause(_handleToIndex[pausedList.Current.Handle]))
                            count--;
                }
            }

            return count;
        }

        /// <summary>
        /// This resumes any matching coroutines on the current MEC instance if they are currently paused, otherwise it has
        /// no effect.
        /// </summary>
        /// <param name="layer">Any coroutines previously paused on the matching layer will be resumend.</param>
        /// <param name="tag">Any coroutines previously paused with a matching tag will be resumend.</param>
        /// <returns>The number of coroutines that were resumed.</returns>
        public static int ResumeCoroutines(int layer, string tag)
        {
            return _instance == null? 0 : _instance.ResumeCoroutinesOnInstance(layer, tag);
        }

        /// <summary>
        /// This resumes any matching coroutines on this MEC instance if they are currently paused, otherwise it has no effect.
        /// </summary>
        /// <param name="layer">Any coroutines previously paused on the matching layer will be resumend.</param>
        /// <param name="tag">Any coroutines previously paused with a matching tag will be resumend.</param>
        /// <returns>The number of coroutines that were resumed.</returns>
        public int ResumeCoroutinesOnInstance(int layer, string tag)
        {
            if (tag == null)
                return ResumeCoroutinesOnInstance(layer);
            if (!_layeredProcesses.ContainsKey(layer) || !_taggedProcesses.ContainsKey(tag))
                return 0;
            int count = 0;

            var indexesEnum = _taggedProcesses[tag].GetEnumerator();
            while (indexesEnum.MoveNext())
                if (!CoindexIsNull(indexesEnum.Current) && _layeredProcesses[layer].Contains(indexesEnum.Current) &&
                    CoindexSetPause(indexesEnum.Current, false))
                        count++;

            var waitingEnum = _waitingTriggers.GetEnumerator();
            while (waitingEnum.MoveNext())
            {
                var pausedList = waitingEnum.Current.Value.GetEnumerator();
                while (pausedList.MoveNext())
                {
                    if (_handleToIndex.ContainsKey(pausedList.Current.Handle) && !CoindexIsNull(_handleToIndex[pausedList.Current.Handle])
                        && !CoindexSetPause(_handleToIndex[pausedList.Current.Handle]))
                            count--;
                }
            }

            return count;
        }

        /// <summary>
        /// This will resume any matching coroutines.
        /// </summary>
        /// <param name="handle">The handle of the coroutine to resume.</param>
        /// <returns>The number of coroutines that were resumed (0 or 1).</returns>
        public static int ResumeCoroutines(CoroutineHandle handle)
        {
            return ActiveInstances.ContainsKey(handle.Key) ? GetInstance(handle.Key).ResumeCoroutinesOnInstance(handle) : 0;
        }

        /// <summary>
        /// This will resume any matching coroutines running on this MEC instance.
        /// </summary>
        /// <param name="handle">The handle of the coroutine to resume.</param>
        /// <returns>The number of coroutines that were resumed (0 or 1).</returns>
        public int ResumeCoroutinesOnInstance(CoroutineHandle handle)
        {
            var waitingEnum = _waitingTriggers.GetEnumerator();
            while(waitingEnum.MoveNext())
            {
                var pausedList = waitingEnum.Current.Value.GetEnumerator();
                while(pausedList.MoveNext())
                    if (pausedList.Current.Handle == handle)
                        return 0;
            }

            return _handleToIndex.ContainsKey(handle) && 
                !CoindexIsNull(_handleToIndex[handle]) && CoindexSetPause(_handleToIndex[handle], false) ? 1 : 0;
        }

        /// <summary>
        /// Returns the tag associated with the coroutine that the given handle points to, if it is running.
        /// </summary>
        /// <param name="handle">The handle to the coroutine.</param>
        /// <returns>The coroutine's tag, or null if there is no matching tag.</returns>
        public static string GetTag(CoroutineHandle handle)
        {
            Timing inst = GetInstance(handle.Key);
            return inst != null && inst._handleToIndex.ContainsKey(handle) && inst._processTags.ContainsKey(inst._handleToIndex[handle])
                 ? inst._processTags[inst._handleToIndex[handle]] : null;
        }

        /// <summary>
        /// Returns the layer associated with the coroutine that the given handle points to, if it is running.
        /// </summary>
        /// <param name="handle">The handle to the coroutine.</param>
        /// <returns>The coroutine's layer as a nullable integer, or null if there is no matching layer.</returns>
        public static int? GetLayer(CoroutineHandle handle)
        {
            Timing inst = GetInstance(handle.Key);
            return inst != null && inst._handleToIndex.ContainsKey(handle) && inst._processLayers.ContainsKey(inst._handleToIndex[handle])
                  ? inst._processLayers[inst._handleToIndex[handle]] : (int?)null;
        }

        /// <summary>
        /// Returns the segment that the coroutine with the given handle is running on.
        /// </summary>
        /// <param name="handle">The handle to the coroutine.</param>
        /// <returns>The coroutine's segment, or Segment.Invalid if it's not found.</returns>
        public static Segment GetSegment(CoroutineHandle handle)
        {
            Timing inst = GetInstance(handle.Key);
            return inst != null && inst._handleToIndex.ContainsKey(handle) ? inst._handleToIndex[handle].seg : Segment.Invalid;
        }

        /// <summary>
        /// Sets the coroutine that the handle points to to have the given tag.
        /// </summary>
        /// <param name="handle">The handle to the coroutine.</param>
        /// <param name="newTag">The new tag to assign, or null to clear the tag.</param>
        /// <param name="overwriteExisting">If set to false then the tag will not be changed if the coroutine has an existing tag.</param>
        /// <returns>Whether the tag was set successfully.</returns>
        public static bool SetTag(CoroutineHandle handle, string newTag, bool overwriteExisting = true)
        {
            Timing inst = GetInstance(handle.Key);
            if (inst == null || !inst._handleToIndex.ContainsKey(handle) || inst.CoindexIsNull(inst._handleToIndex[handle])
                || (!overwriteExisting && inst._processTags.ContainsKey(inst._handleToIndex[handle])))
                return false;

            if (newTag == null)
            {
                inst.RemoveTag(inst._handleToIndex[handle]);
                return true;
            }

            if (inst._processTags.ContainsKey(inst._handleToIndex[handle]))
            {
                if (inst._taggedProcesses[inst._processTags[inst._handleToIndex[handle]]].Count <= 1)
                    inst._taggedProcesses.Remove(inst._processTags[inst._handleToIndex[handle]]);
                else
                    inst._taggedProcesses[inst._processTags[inst._handleToIndex[handle]]].Remove(inst._handleToIndex[handle]);

                inst._processTags[inst._handleToIndex[handle]] = newTag;
            }
            else
            {
                inst._processTags.Add(inst._handleToIndex[handle], newTag);
            }

            if (inst._taggedProcesses.ContainsKey(newTag))
                inst._taggedProcesses[newTag].Add(inst._handleToIndex[handle]);
            else
                inst._taggedProcesses.Add(newTag, new HashSet<ProcessIndex> { inst._handleToIndex[handle] });

            return true;
        }

        /// <summary>
        /// Sets the coroutine that the handle points to to have the given layer.
        /// </summary>
        /// <param name="handle">The handle to the coroutine.</param>
        /// <param name="newLayer">The new tag to assign.</param>
        /// <param name="overwriteExisting">If set to false then the tag will not be changed if the coroutine has an existing tag.</param>
        /// <returns>Whether the layer was set successfully.</returns>
        public static bool SetLayer(CoroutineHandle handle, int newLayer, bool overwriteExisting = true)
        {
            Timing inst = GetInstance(handle.Key);
            if (inst == null || !inst._handleToIndex.ContainsKey(handle) || inst.CoindexIsNull(inst._handleToIndex[handle])
                || (!overwriteExisting && inst._processLayers.ContainsKey(inst._handleToIndex[handle])))
                return false;

            if (inst._processLayers.ContainsKey(inst._handleToIndex[handle]))
            {
                if (inst._layeredProcesses[inst._processLayers[inst._handleToIndex[handle]]].Count <= 1)
                    inst._layeredProcesses.Remove(inst._processLayers[inst._handleToIndex[handle]]);
                else
                    inst._layeredProcesses[inst._processLayers[inst._handleToIndex[handle]]].Remove(inst._handleToIndex[handle]);

                inst._processLayers[inst._handleToIndex[handle]] = newLayer;
            }
            else
            {
                inst._processLayers.Add(inst._handleToIndex[handle], newLayer);
            }

            if (inst._layeredProcesses.ContainsKey(newLayer))
                inst._layeredProcesses[newLayer].Add(inst._handleToIndex[handle]);
            else
                inst._layeredProcesses.Add(newLayer, new HashSet<ProcessIndex> { inst._handleToIndex[handle] });

            return true;
        }

        /// <summary>
        /// Sets the segment for the coroutine with the given handle.
        /// </summary>
        /// <param name="handle">The handle to the coroutine.</param>
        /// <param name="newSegment">The new segment to run the coroutine in.</param>
        /// <returns>Whether the segment was set successfully.</returns>
        public static bool SetSegment(CoroutineHandle handle, Segment newSegment)
        {
            Timing inst = GetInstance(handle.Key);
            if (inst == null || !inst._handleToIndex.ContainsKey(handle) || inst.CoindexIsNull(inst._handleToIndex[handle]))
                return false;

            inst.RunCoroutineInternal(inst.CoindexExtract(inst._handleToIndex[handle]), newSegment, inst._processLayers.ContainsKey(inst._handleToIndex[handle])
                ? inst._processLayers[inst._handleToIndex[handle]] : (int?)null, inst._processTags.ContainsKey(inst._handleToIndex[handle])
                ? inst._processTags[inst._handleToIndex[handle]] : null, handle, false);

            return true;
        }

        /// <summary>
        /// Sets the coroutine that the handle points to to have the given tag.
        /// </summary>
        /// <param name="handle">The handle to the coroutine.</param>
        /// <returns>Whether the tag was removed successfully.</returns>
        public static bool RemoveTag(CoroutineHandle handle)
        {
            return SetTag(handle, null);
        }

        /// <summary>
        /// Sets the coroutine that the handle points to to have the given layer.
        /// </summary>
        /// <param name="handle">The handle to the coroutine.</param>
        /// <returns>Whether the layer was removed successfully.</returns>
        public static bool RemoveLayer(CoroutineHandle handle)
        {
            Timing inst = GetInstance(handle.Key);
            if (inst == null || !inst._handleToIndex.ContainsKey(handle) || inst.CoindexIsNull(inst._handleToIndex[handle]))
                return false;

            inst.RemoveLayer(inst._handleToIndex[handle]);

            return true;
        }

        /// <summary>
        /// Tests to see if the handle you have points to a valid coroutine.
        /// </summary>
        /// <param name="handle">The handle to test.</param>
        /// <returns>Whether it's a valid coroutine.</returns>
        public static bool IsRunning(CoroutineHandle handle)
        {
            Timing inst = GetInstance(handle.Key);
            return inst != null && inst._handleToIndex.ContainsKey(handle) && !inst.CoindexIsNull(inst._handleToIndex[handle]);
        }

        /// <summary>
        /// Tests to see if the handle you have points to a paused coroutine.
        /// </summary>
        /// <param name="handle">The handle to test.</param>
        /// <returns>Whether it's a paused coroutine.</returns>
        public static bool IsPaused(CoroutineHandle handle)
        {
            Timing inst = GetInstance(handle.Key);
            return inst != null && inst._handleToIndex.ContainsKey(handle) && !inst.CoindexIsNull(inst._handleToIndex[handle]) && 
                !inst.CoindexIsPaused(inst._handleToIndex[handle]);
        }

        private void AddTag(string tag, ProcessIndex coindex)
        {
            _processTags.Add(coindex, tag);

            if(_taggedProcesses.ContainsKey(tag))
                _taggedProcesses[tag].Add(coindex);
            else
                _taggedProcesses.Add(tag, new HashSet<ProcessIndex> { coindex });
        }

        private void AddLayer(int layer, ProcessIndex coindex)
        {
            _processLayers.Add(coindex, layer);

            if (_layeredProcesses.ContainsKey(layer))
                _layeredProcesses[layer].Add(coindex);
            else
                _layeredProcesses.Add(layer, new HashSet<ProcessIndex> { coindex });
        }

        private void RemoveTag(ProcessIndex coindex)
        {
            if (_processTags.ContainsKey(coindex))
            {
                if (_taggedProcesses[_processTags[coindex]].Count > 1)
                    _taggedProcesses[_processTags[coindex]].Remove(coindex);
                else
                    _taggedProcesses.Remove(_processTags[coindex]);

                _processTags.Remove(coindex);
            }
        }

        private void RemoveLayer(ProcessIndex coindex)
        {
            if (_processLayers.ContainsKey(coindex))
            {
                if (_layeredProcesses[_processLayers[coindex]].Count > 1)
                    _layeredProcesses[_processLayers[coindex]].Remove(coindex);
                else
                    _layeredProcesses.Remove(_processLayers[coindex]);

                _processLayers.Remove(coindex);
            }
        }

        private void RemoveGraffiti(ProcessIndex coindex)
        {
            if (_processLayers.ContainsKey(coindex))
            {
                if (_layeredProcesses[_processLayers[coindex]].Count > 1)
                    _layeredProcesses[_processLayers[coindex]].Remove(coindex);
                else
                    _layeredProcesses.Remove(_processLayers[coindex]);

                _processLayers.Remove(coindex);
            }

            if (_processTags.ContainsKey(coindex))
            {
                if (_taggedProcesses[_processTags[coindex]].Count > 1)
                    _taggedProcesses[_processTags[coindex]].Remove(coindex);
                else
                    _taggedProcesses.Remove(_processTags[coindex]);

                _processTags.Remove(coindex);
            }

            if (_indexToHandle.ContainsKey(coindex))
            {
                _handleToIndex.Remove(_indexToHandle[coindex]);
                _indexToHandle.Remove(coindex);
            }
        }

        private void MoveGraffiti(ProcessIndex coindexFrom, ProcessIndex coindexTo)
        {
            RemoveGraffiti(coindexTo);

            int layer;
            if (_processLayers.TryGetValue(coindexFrom, out layer))
            {
                _layeredProcesses[layer].Remove(coindexFrom);
                _layeredProcesses[layer].Add(coindexTo);

                _processLayers.Add(coindexTo, layer);
                _processLayers.Remove(coindexFrom);
            }

            string tag;
            if (_processTags.TryGetValue(coindexFrom, out tag))
            {
                _taggedProcesses[tag].Remove(coindexFrom);
                _taggedProcesses[tag].Add(coindexTo);

                _processTags.Add(coindexTo, tag);
                _processTags.Remove(coindexFrom);
            }

            _handleToIndex[_indexToHandle[coindexFrom]] = coindexTo;
            _indexToHandle.Add(coindexTo, _indexToHandle[coindexFrom]);
            _indexToHandle.Remove(coindexFrom);
        }

        private IEnumerator<float> CoindexExtract(ProcessIndex coindex)
        {
            IEnumerator<float> retVal;

            switch (coindex.seg)
            {
                case Segment.Update:
                    retVal = UpdateProcesses[coindex.i];
                    UpdateProcesses[coindex.i] = null;
                    return retVal;
                case Segment.FixedUpdate:
                    retVal = FixedUpdateProcesses[coindex.i];
                    FixedUpdateProcesses[coindex.i] = null;
                    return retVal;
                case Segment.LateUpdate:
                    retVal = LateUpdateProcesses[coindex.i];
                    LateUpdateProcesses[coindex.i] = null;
                    return retVal;
                case Segment.SlowUpdate:
                    retVal = SlowUpdateProcesses[coindex.i];
                    SlowUpdateProcesses[coindex.i] = null;
                    return retVal;
                case Segment.RealtimeUpdate:
                    retVal = RealtimeUpdateProcesses[coindex.i];
                    RealtimeUpdateProcesses[coindex.i] = null;
                    return retVal;
                case Segment.EditorUpdate:
                    retVal = EditorUpdateProcesses[coindex.i];
                    EditorUpdateProcesses[coindex.i] = null;
                    return retVal;
                case Segment.EditorSlowUpdate:
                    retVal = EditorSlowUpdateProcesses[coindex.i];
                    EditorSlowUpdateProcesses[coindex.i] = null;
                    return retVal;
                case Segment.EndOfFrame:
                    retVal = EndOfFrameProcesses[coindex.i];
                    EndOfFrameProcesses[coindex.i] = null;
                    return retVal;
                case Segment.ManualTimeframe:
                    retVal = ManualTimeframeProcesses[coindex.i];
                    ManualTimeframeProcesses[coindex.i] = null;
                    return retVal;
                default:
                    return null;
            }
        }

        private bool CoindexIsNull(ProcessIndex coindex)
        {
            switch (coindex.seg)
            {
                case Segment.Update:
                    return UpdateProcesses[coindex.i] == null;
                case Segment.FixedUpdate:
                    return FixedUpdateProcesses[coindex.i] == null;
                case Segment.LateUpdate:
                    return LateUpdateProcesses[coindex.i] == null;
                case Segment.SlowUpdate:
                    return SlowUpdateProcesses[coindex.i] == null;
                case Segment.RealtimeUpdate:
                    return RealtimeUpdateProcesses[coindex.i] == null;
                case Segment.EditorUpdate:
                    return EditorUpdateProcesses[coindex.i] == null;
                case Segment.EditorSlowUpdate:
                    return EditorSlowUpdateProcesses[coindex.i] == null;
                case Segment.EndOfFrame:
                    return EndOfFrameProcesses[coindex.i] == null;
                case Segment.ManualTimeframe:
                    return ManualTimeframeProcesses[coindex.i] == null;
                default:
                    return true;
            }
        }

        private IEnumerator<float> CoindexPeek(ProcessIndex coindex)
        {
            switch (coindex.seg)
            {
                case Segment.Update:
                    return UpdateProcesses[coindex.i];
                case Segment.FixedUpdate:
                    return FixedUpdateProcesses[coindex.i];
                case Segment.LateUpdate:
                    return LateUpdateProcesses[coindex.i];
                case Segment.SlowUpdate:
                    return SlowUpdateProcesses[coindex.i];
                case Segment.RealtimeUpdate:
                    return RealtimeUpdateProcesses[coindex.i];
                case Segment.EditorUpdate:
                    return EditorUpdateProcesses[coindex.i];
                case Segment.EditorSlowUpdate:
                    return EditorSlowUpdateProcesses[coindex.i];
                case Segment.EndOfFrame:
                    return EndOfFrameProcesses[coindex.i];
                case Segment.ManualTimeframe:
                    return ManualTimeframeProcesses[coindex.i];
                default:
                    return null;
            }
        }

        private bool CoindexKill(ProcessIndex coindex)
        {
            bool retVal;

            switch (coindex.seg)
            {
                case Segment.Update:
                    retVal = UpdateProcesses[coindex.i] != null;
                    UpdateProcesses[coindex.i] = null;
                    return retVal;
                case Segment.FixedUpdate:
                    retVal = FixedUpdateProcesses[coindex.i] != null;
                    FixedUpdateProcesses[coindex.i] = null;
                    return retVal;
                case Segment.LateUpdate:
                    retVal = LateUpdateProcesses[coindex.i] != null;
                    LateUpdateProcesses[coindex.i] = null;
                    return retVal;
                case Segment.SlowUpdate:
                    retVal = SlowUpdateProcesses[coindex.i] != null;
                    SlowUpdateProcesses[coindex.i] = null;
                    return retVal;
                case Segment.RealtimeUpdate:
                    retVal = RealtimeUpdateProcesses[coindex.i] != null;
                    RealtimeUpdateProcesses[coindex.i] = null;
                    return retVal;
                case Segment.EditorUpdate:
                    retVal = UpdateProcesses[coindex.i] != null;
                    EditorUpdateProcesses[coindex.i] = null;
                    return retVal;
                case Segment.EditorSlowUpdate:
                    retVal = EditorSlowUpdateProcesses[coindex.i] != null;
                    EditorSlowUpdateProcesses[coindex.i] = null;
                    return retVal;
                case Segment.EndOfFrame:
                    retVal = EndOfFrameProcesses[coindex.i] != null;
                    EndOfFrameProcesses[coindex.i] = null;
                    return retVal;
                case Segment.ManualTimeframe:
                    retVal = ManualTimeframeProcesses[coindex.i] != null;
                    ManualTimeframeProcesses[coindex.i] = null;
                    return retVal;
                default:
                    return false;
            }
        }

        private bool CoindexSetPause(ProcessIndex coindex, bool newPausedState = true)
        {
            bool isPaused;

            switch (coindex.seg)
            {
                case Segment.Update:
                    isPaused = UpdatePaused[coindex.i];
                    UpdatePaused[coindex.i] = newPausedState;
                    return isPaused;
                case Segment.FixedUpdate:
                    isPaused = FixedUpdatePaused[coindex.i];
                    FixedUpdatePaused[coindex.i] = newPausedState;
                    return isPaused;
                case Segment.LateUpdate:
                    isPaused = LateUpdatePaused[coindex.i];
                    LateUpdatePaused[coindex.i] = newPausedState;
                    return isPaused;
                case Segment.SlowUpdate:
                    isPaused = SlowUpdatePaused[coindex.i];
                    SlowUpdatePaused[coindex.i] = newPausedState;
                    return isPaused;
                case Segment.RealtimeUpdate:
                    isPaused = RealtimeUpdatePaused[coindex.i];
                    RealtimeUpdatePaused[coindex.i] = newPausedState;
                    return isPaused;
                case Segment.EditorUpdate:
                    isPaused = EditorUpdatePaused[coindex.i];
                    EditorUpdatePaused[coindex.i] = newPausedState;
                    return isPaused;
                case Segment.EditorSlowUpdate:
                    isPaused = EditorSlowUpdatePaused[coindex.i];
                    EditorSlowUpdatePaused[coindex.i] = newPausedState;
                    return isPaused;
                case Segment.EndOfFrame:
                    isPaused = EndOfFramePaused[coindex.i];
                    EndOfFramePaused[coindex.i] = newPausedState;
                    return isPaused;
                case Segment.ManualTimeframe:
                    isPaused = ManualTimeframePaused[coindex.i];
                    ManualTimeframePaused[coindex.i] = newPausedState;
                    return isPaused;
                default:
                    return false;
            }
        }

        private bool CoindexIsPaused(ProcessIndex coindex)
        {
            switch (coindex.seg)
            {
                case Segment.Update:
                    return UpdatePaused[coindex.i];
                case Segment.FixedUpdate:
                    return FixedUpdatePaused[coindex.i];
                case Segment.LateUpdate:
                    return LateUpdatePaused[coindex.i];
                case Segment.SlowUpdate:
                    return SlowUpdatePaused[coindex.i];
                case Segment.RealtimeUpdate:
                    return RealtimeUpdatePaused[coindex.i];
                case Segment.EditorUpdate:
                    return EditorUpdatePaused[coindex.i];
                case Segment.EditorSlowUpdate:
                    return EditorSlowUpdatePaused[coindex.i];
                case Segment.EndOfFrame:
                    return EndOfFramePaused[coindex.i];
                case Segment.ManualTimeframe:
                    return ManualTimeframePaused[coindex.i];
                default:
                    return false;
            }
        }

        private void CoindexReplace(ProcessIndex coindex, IEnumerator<float> replacement)
        {
            switch (coindex.seg)
            {
                case Segment.Update:
                    UpdateProcesses[coindex.i] = replacement;
                    return;
                case Segment.FixedUpdate:
                    FixedUpdateProcesses[coindex.i] = replacement;
                    return;
                case Segment.LateUpdate:
                    LateUpdateProcesses[coindex.i] = replacement;
                    return;
                case Segment.SlowUpdate:
                    SlowUpdateProcesses[coindex.i] = replacement;
                    return;
                case Segment.RealtimeUpdate:
                    RealtimeUpdateProcesses[coindex.i] = replacement;
                    return;
                case Segment.EditorUpdate:
                    EditorUpdateProcesses[coindex.i] = replacement;
                    return;
                case Segment.EditorSlowUpdate:
                    EditorSlowUpdateProcesses[coindex.i] = replacement;
                    return;
                case Segment.EndOfFrame:
                    EndOfFrameProcesses[coindex.i] = replacement;
                    return;
                case Segment.ManualTimeframe:
                    ManualTimeframeProcesses[coindex.i] = replacement;
                    return;
            }
        }

        /// <summary>
        /// Use the command "yield return Timing.WaitUntilDone(newCoroutine);" to start a new coroutine and pause the
        /// current one until it finishes.
        /// </summary>
        /// <param name="newCoroutine">The coroutine to pause for.</param>
        public static float WaitUntilDone(IEnumerator<float> newCoroutine)
        {
            return WaitUntilDone(RunCoroutine(newCoroutine), true);
        }

        /// <summary>
        /// Use the command "yield return Timing.WaitUntilDone(newCoroutine);" to start a new coroutine and pause the
        /// current one until it finishes.
        /// </summary>
        /// <param name="newCoroutine">The coroutine to pause for.</param>
        /// <param name="tag">An optional tag to attach to the coroutine which can later be used to identify this coroutine.</param>
        public static float WaitUntilDone(IEnumerator<float> newCoroutine, string tag)
        {
            return WaitUntilDone(RunCoroutine(newCoroutine, tag), true);
        }

        /// <summary>
        /// Use the command "yield return Timing.WaitUntilDone(newCoroutine);" to start a new coroutine and pause the
        /// current one until it finishes.
        /// </summary>
        /// <param name="newCoroutine">The coroutine to pause for.</param>
        /// <param name="layer">An optional layer to attach to the coroutine which can later be used to identify this coroutine.</param>
        public static float WaitUntilDone(IEnumerator<float> newCoroutine, int layer)
        {
            return WaitUntilDone(RunCoroutine(newCoroutine, layer), true);
        }

        /// <summary>
        /// Use the command "yield return Timing.WaitUntilDone(newCoroutine);" to start a new coroutine and pause the
        /// current one until it finishes.
        /// </summary>
        /// <param name="newCoroutine">The coroutine to pause for.</param>
        /// <param name="layer">An optional layer to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <param name="tag">An optional tag to attach to the coroutine which can later be used to identify this coroutine.</param>
        public static float WaitUntilDone(IEnumerator<float> newCoroutine, int layer, string tag)
        {
            return WaitUntilDone(RunCoroutine(newCoroutine, layer, tag), true);
        }

        /// <summary>
        /// Use the command "yield return Timing.WaitUntilDone(newCoroutine);" to start a new coroutine and pause the
        /// current one until it finishes.
        /// </summary>
        /// <param name="newCoroutine">The coroutine to pause for.</param>
        /// <param name="segment">The segment that the new coroutine should run in.</param>
        public static float WaitUntilDone(IEnumerator<float> newCoroutine, Segment segment)
        {
            return WaitUntilDone(RunCoroutine(newCoroutine, segment), true);
        }

        /// <summary>
        /// Use the command "yield return Timing.WaitUntilDone(newCoroutine);" to start a new coroutine and pause the
        /// current one until it finishes.
        /// </summary>
        /// <param name="newCoroutine">The coroutine to pause for.</param>
        /// <param name="segment">The segment that the new coroutine should run in.</param>
        /// <param name="tag">An optional tag to attach to the coroutine which can later be used to identify this coroutine.</param>
        public static float WaitUntilDone(IEnumerator<float> newCoroutine, Segment segment, string tag)
        {
            return WaitUntilDone(RunCoroutine(newCoroutine, segment, tag), true);
        }

        /// <summary>
        /// Use the command "yield return Timing.WaitUntilDone(newCoroutine);" to start a new coroutine and pause the
        /// current one until it finishes.
        /// </summary>
        /// <param name="newCoroutine">The coroutine to pause for.</param>
        /// <param name="segment">The segment that the new coroutine should run in.</param>
        /// <param name="layer">An optional layer to attach to the coroutine which can later be used to identify this coroutine.</param>
        public static float WaitUntilDone(IEnumerator<float> newCoroutine, Segment segment, int layer)
        {
            return WaitUntilDone(RunCoroutine(newCoroutine, segment, layer), true);
        }

        /// <summary>
        /// Use the command "yield return Timing.WaitUntilDone(newCoroutine);" to start a new coroutine and pause the
        /// current one until it finishes.
        /// </summary>
        /// <param name="newCoroutine">The coroutine to pause for.</param>
        /// <param name="segment">The segment that the new coroutine should run in.</param>
        /// <param name="layer">An optional layer to attach to the coroutine which can later be used to identify this coroutine.</param>
        /// <param name="tag">An optional tag to attach to the coroutine which can later be used to identify this coroutine.</param>
        public static float WaitUntilDone(IEnumerator<float> newCoroutine, Segment segment, int layer, string tag)
        {
            return WaitUntilDone(RunCoroutine(newCoroutine, segment, layer, tag), true);
        }

        /// <summary>
        /// Use the command "yield return Timing.WaitUntilDone(otherCoroutine);" to pause the current 
        /// coroutine until otherCoroutine is done.
        /// </summary>
        /// <param name="otherCoroutine">The coroutine to pause for.</param>
        public static float WaitUntilDone(CoroutineHandle otherCoroutine)
        {
            return WaitUntilDone(otherCoroutine, true);
        }

        /// <summary>
        /// Use the command "yield return Timing.WaitUntilDone(otherCoroutine, false);" to pause the current 
        /// coroutine until otherCoroutine is done, supressing warnings.
        /// </summary>
        /// <param name="otherCoroutine">The coroutine to pause for.</param>
        /// <param name="warnOnIssue">Post a warning to the console if no hold action was actually performed.</param>
        public static float WaitUntilDone(CoroutineHandle otherCoroutine, bool warnOnIssue)
        {
            Timing inst = GetInstance(otherCoroutine.Key);

            if (inst != null && inst._handleToIndex.ContainsKey(otherCoroutine))
            {
                if (inst.CoindexIsNull(inst._handleToIndex[otherCoroutine]))
                    return 0f;

                if (!inst._waitingTriggers.ContainsKey(otherCoroutine))
                {
                    inst.CoindexReplace(inst._handleToIndex[otherCoroutine],
                        inst._StartWhenDone(otherCoroutine, inst.CoindexPeek(inst._handleToIndex[otherCoroutine])));
                    inst._waitingTriggers.Add(otherCoroutine, new HashSet<ProcessData>());
                }

                ReplacementFunction = (coptr, handle) =>
                {
                    if (handle == otherCoroutine)
                    {
                        if (warnOnIssue)
                            Debug.LogWarning("A coroutine attempted to wait for itself.");

                        return coptr;
                    }
                    if (handle.Key != otherCoroutine.Key)
                    {
                        if (warnOnIssue)
                            Debug.LogWarning("A coroutine attempted to wait for a coroutine running on a different MEC instance.");

                        return coptr;
                    }

                    inst._waitingTriggers[otherCoroutine].Add(new ProcessData
                    {
                        Handle = handle,
                        PauseTime = coptr.Current > inst.GetSegmentTime(inst._handleToIndex[handle].seg)
                            ? coptr.Current - (float)inst.GetSegmentTime(inst._handleToIndex[handle].seg) : 0f
                    });

                    inst.CoindexSetPause(inst._handleToIndex[handle]);

                    return coptr;
                };

                return float.NaN;
            }

            if (warnOnIssue)
                Debug.LogWarning("WaitUntilDone cannot hold: The coroutine handle that was passed in is invalid.\n" + otherCoroutine);

            return 0f;
        }

        private IEnumerator<float> _StartWhenDone(CoroutineHandle handle, IEnumerator<float> proc)
        {
            if (!_waitingTriggers.ContainsKey(handle)) yield break;

            try
            {
                if (proc.Current > localTime)
                    yield return proc.Current;

                while (proc.MoveNext())
                    yield return proc.Current;
            }
            finally
            {
                CloseWaitingProcess(handle);
            }
        }

        private void CloseWaitingProcess(CoroutineHandle handle)
        {
            if (!_waitingTriggers.ContainsKey(handle)) return;

            var tasksEnum = _waitingTriggers[handle].GetEnumerator();
            _waitingTriggers.Remove(handle);

            while (tasksEnum.MoveNext())
            {
                if (_handleToIndex.ContainsKey(tasksEnum.Current.Handle))
                {
                    ProcessIndex coIndex = _handleToIndex[tasksEnum.Current.Handle];

                    if (tasksEnum.Current.PauseTime > 0d)
                        CoindexReplace(coIndex, _InjectDelay(CoindexPeek(coIndex), (float)(GetSegmentTime(coIndex.seg) + tasksEnum.Current.PauseTime)));

                    CoindexSetPause(coIndex, false);
                }
            }
        }

        /// <summary>
        /// Use the command "yield return Timing.WaitUntilDone(wwwObject);" to pause the current 
        /// coroutine until the wwwObject is done.
        /// </summary>
        /// <param name="wwwObject">The www object to pause for.</param>
        public static float WaitUntilDone(WWW wwwObject)
        {
            if (wwwObject == null || wwwObject.isDone) return 0f;
            ReplacementFunction = (coptr, handle) => _StartWhenDone(wwwObject, coptr);

            return float.NaN;
        }

        private static IEnumerator<float> _StartWhenDone(WWW wwwObject, IEnumerator<float> pausedProc)
        {
            while (!wwwObject.isDone)
                yield return 0f;

            ReplacementFunction = delegate { return pausedProc; };
            yield return float.NaN;
        }

        /// <summary>
        /// Use the command "yield return Timing.WaitUntilDone(operation);" to pause the current 
        /// coroutine until the operation is done.
        /// </summary>
        /// <param name="operation">The operation variable returned.</param>
        public static float WaitUntilDone(AsyncOperation operation)
        {
            if (operation == null || operation.isDone) return 0f;
            ReplacementFunction = (coptr, handle) => _StartWhenDone(operation, coptr);
            return float.NaN;
        }

        private static IEnumerator<float> _StartWhenDone(AsyncOperation operation, IEnumerator<float> pausedProc)
        {
            while (!operation.isDone)
                yield return 0f;

            ReplacementFunction = delegate { return pausedProc; };
            yield return float.NaN;
        }

#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
        /// <summary>
        /// Use the command "yield return Timing.WaitUntilDone(operation);" to pause the current 
        /// coroutine until the operation is done.
        /// </summary>
        /// <param name="operation">The operation variable returned.</param>
        public static float WaitUntilDone(CustomYieldInstruction operation)
        {
            if (operation == null || !operation.keepWaiting) return 0f;
            ReplacementFunction = (coptr, handle) => _StartWhenDone(operation, coptr);
            return float.NaN;
        }

        private static IEnumerator<float> _StartWhenDone(CustomYieldInstruction operation, IEnumerator<float> pausedProc)
        {
            while (operation.keepWaiting)
                yield return 0f;

            ReplacementFunction = delegate { return pausedProc; };
            yield return float.NaN;
        }
#endif

        /// <summary>
        /// Use the command "yield return Timing.WaitUntilTrue(evaluatorFunc);" to pause the current 
        /// coroutine until the evaluator function returns true.
        /// </summary>
        /// <param name="evaluatorFunc">The evaluator function.</param>
        public static float WaitUntilTrue(System.Func<bool> evaluatorFunc)
        {
            if (evaluatorFunc == null || evaluatorFunc()) return 0f;
            ReplacementFunction = (coptr, handle) => _StartWhenDone(evaluatorFunc, false, coptr);
            return float.NaN;
        }

        /// <summary>
        /// Use the command "yield return Timing.WaitUntilFalse(evaluatorFunc);" to pause the current 
        /// coroutine until the evaluator function returns false.
        /// </summary>
        /// <param name="evaluatorFunc">The evaluator function.</param>
        public static float WaitUntilFalse(System.Func<bool> evaluatorFunc)
        {
            if (evaluatorFunc == null || !evaluatorFunc()) return 0f;
            ReplacementFunction = (coptr, handle) => _StartWhenDone(evaluatorFunc, true, coptr);
            return float.NaN;
        }

        private static IEnumerator<float> _StartWhenDone(System.Func<bool> evaluatorFunc, bool continueOn, IEnumerator<float> pausedProc)
        {
            while (evaluatorFunc() == continueOn)
                yield return 0f;

            ReplacementFunction = delegate { return pausedProc; };
            yield return float.NaN;
        }

        private static IEnumerator<float> _InjectDelay(IEnumerator<float> proc, float returnAt)
        {
            yield return returnAt;

            ReplacementFunction = delegate { return proc; };
            yield return float.NaN;
        }

        /// <summary>
        /// Keeps this coroutine from executing until UnlockCoroutine is called with a matching key.
        /// </summary>
        /// <param name="coroutine">The handle to the coroutine to be locked.</param>
        /// <param name="key">The key to use. A new key can be generated by calling "new CoroutineHandle(0)".</param>
        /// <returns>Whether the lock was successful.</returns>
        public bool LockCoroutine(CoroutineHandle coroutine, CoroutineHandle key)
        {
            if (coroutine.Key != _instanceID || key == new CoroutineHandle() || key.Key != 0)
                return false;

            if (!_waitingTriggers.ContainsKey(key))
                _waitingTriggers.Add(key, new HashSet<ProcessData>());

            _waitingTriggers[key].Add(new ProcessData { Handle = coroutine });
            CoindexSetPause(_handleToIndex[coroutine]);

            return true;
        }

        /// <summary>
        /// Unlocks a coroutine that has been locked, so long as the key matches.
        /// </summary>
        /// <param name="coroutine">The handle to the coroutine to be unlocked.</param>
        /// <param name="key">The key that the coroutine was previously locked with.</param>
        /// <returns>Whether the coroutine was successfully unlocked.</returns>
        public bool UnlockCoroutine(CoroutineHandle coroutine, CoroutineHandle key)
        {
            if (coroutine.Key != _instanceID || key == new CoroutineHandle() || 
                !_handleToIndex.ContainsKey(coroutine) || !_waitingTriggers.ContainsKey(key))
                return false;

            ProcessData wrappedCoroutine = new ProcessData { Handle = coroutine };
            _waitingTriggers[key].Remove(wrappedCoroutine);

            bool coroutineStillPaused = false;
            var triggersEnum = _waitingTriggers.GetEnumerator();
            while (triggersEnum.MoveNext())
                if (triggersEnum.Current.Value.Contains(wrappedCoroutine))
                    coroutineStillPaused = true;

            CoindexSetPause(_handleToIndex[coroutine], coroutineStillPaused);

            return true;
        }

        /// <summary>
        /// Use the command "yield return Timing.SwitchCoroutine(segment);" to switch this coroutine to
        /// the given segment on the default instance.
        /// </summary>
        /// <param name="newSegment">The new segment to run in.</param>
        public static float SwitchCoroutine(Segment newSegment)
        {
            ReplacementFunction = (coptr, handle) =>
            {
                Timing instance = GetInstance(handle.Key);
                ProcessIndex index = instance._handleToIndex[handle];
                instance.RunCoroutineInternal(coptr, newSegment, instance._processLayers.ContainsKey(index) ? instance._processLayers[index] : (int?)null,
                    instance._processTags.ContainsKey(index) ? instance._processTags[index] : null, handle, false);
                return null;
            };

            return float.NaN;
        }

        /// <summary>
        /// Use the command "yield return Timing.SwitchCoroutine(segment, tag);" to switch this coroutine to
        /// the given values.
        /// </summary>
        /// <param name="newSegment">The new segment to run in.</param>
        /// <param name="newTag">The new tag to apply, or null to remove this coroutine's tag.</param>
        public static float SwitchCoroutine(Segment newSegment, string newTag)
        {
            ReplacementFunction = (coptr, handle) =>
            {
                Timing instance = GetInstance(handle.Key);
                ProcessIndex index = instance._handleToIndex[handle];
                instance.RunCoroutineInternal(coptr, newSegment, 
                    instance._processLayers.ContainsKey(index) ? instance._processLayers[index] : (int?)null, newTag, handle, false);
                return null;
            };

            return float.NaN;
        }

        /// <summary>
        /// Use the command "yield return Timing.SwitchCoroutine(segment, layer);" to switch this coroutine to
        /// the given values.
        /// </summary>
        /// <param name="newSegment">The new segment to run in.</param>
        /// <param name="newLayer">The new layer to apply.</param>
        public static float SwitchCoroutine(Segment newSegment, int newLayer)
        {
            ReplacementFunction = (coptr, handle) =>
            {
                Timing instance = GetInstance(handle.Key);
                ProcessIndex index = instance._handleToIndex[handle];
                instance.RunCoroutineInternal(coptr, newSegment, newLayer,
                    instance._processTags.ContainsKey(index) ? instance._processTags[index] : null, handle, false);
                return null;
            };

            return float.NaN;
        }

        /// <summary>
        /// Use the command "yield return Timing.SwitchCoroutine(segment, layer, tag);" to switch this coroutine to
        /// the given values.
        /// </summary>
        /// <param name="newSegment">The new segment to run in.</param>
        /// <param name="newLayer">The new layer to apply.</param>
        /// <param name="newTag">The new tag to apply, or null to remove this coroutine's tag.</param>
        public static float SwitchCoroutine(Segment newSegment, int newLayer, string newTag)
        {
            ReplacementFunction = (coptr, handle) =>
            {
                GetInstance(handle.Key).RunCoroutineInternal(coptr, newSegment, newLayer, newTag, handle, false);
                return null;
            };

            return float.NaN;
        }

        /// <summary>
        /// Use the command "yield return Timing.SwitchCoroutine(tag);" to switch this coroutine to
        /// the given tag.
        /// </summary>
        /// <param name="newTag">The new tag to apply, or null to remove this coroutine's tag.</param>
        public static float SwitchCoroutine(string newTag)
        {
            ReplacementFunction = (coptr, handle) =>
            {
                Timing instance = GetInstance(handle.Key);
                instance.RemoveTag(instance._handleToIndex[handle]);
                if (newTag != null)
                    instance.AddTag(newTag, instance._handleToIndex[handle]);
                return coptr;
            };
            return float.NaN;
        }

        /// <summary>
        /// Use the command "yield return Timing.SwitchCoroutine(layer);" to switch this coroutine to
        /// the given layer.
        /// </summary>
        /// <param name="newLayer">The new layer to apply.</param>
        public static float SwitchCoroutine(int newLayer)
        {
            ReplacementFunction = (coptr, handle) =>
            {
                Timing instance = GetInstance(handle.Key);
                instance.RemoveLayer(instance._handleToIndex[handle]);
                instance.AddLayer(newLayer, instance._handleToIndex[handle]);
                return coptr;
            };

            return float.NaN;
        }

        /// <summary>
        /// Use the command "yield return Timing.SwitchCoroutine(layer, tag);" to switch this coroutine to
        /// the given tag.
        /// </summary>
        /// <param name="newLayer">The new layer to apply.</param>
        /// <param name="newTag">The new tag to apply, or null to remove this coroutine's tag.</param>
        public static float SwitchCoroutine(int newLayer, string newTag)
        {
            ReplacementFunction = (coptr, handle) =>
            {
                Timing instance = GetInstance(handle.Key);
                instance.RemoveLayer(instance._handleToIndex[handle]);
                instance.AddLayer(newLayer, instance._handleToIndex[handle]);
                instance.RemoveTag(instance._handleToIndex[handle]);
                if (newTag != null)
                    instance.AddTag(newTag, instance._handleToIndex[handle]);
                return coptr;
            };

            return float.NaN;
        }

        /// <summary>
        /// Calls the specified action after a specified number of seconds.
        /// </summary>
        /// <param name="delay">The number of seconds to wait before calling the action.</param>
        /// <param name="action">The action to call.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public static CoroutineHandle CallDelayed(float delay, System.Action action)
        {
            return action == null ? new CoroutineHandle() : RunCoroutine(Instance._DelayedCall(delay, action));
        }

        /// <summary>
        /// Calls the specified action after a specified number of seconds.
        /// </summary>
        /// <param name="delay">The number of seconds to wait before calling the action.</param>
        /// <param name="action">The action to call.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public CoroutineHandle CallDelayedOnInstance(float delay, System.Action action)
        {
            return action == null ? new CoroutineHandle() : RunCoroutineOnInstance(_DelayedCall(delay, action));
        }

        private IEnumerator<float> _DelayedCall(float delay, System.Action action)
        {
            yield return WaitForSecondsOnInstance(delay);

            action();
        }

        /// <summary>
        /// Calls the supplied action at the given rate for a given number of seconds.
        /// </summary>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="period">The amount of time between calls.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public static CoroutineHandle CallPeriodically(float timeframe, float period, System.Action action, System.Action onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutine(Instance._CallContinuously(timeframe, period, action, onDone), Segment.Update);
        }

        /// <summary>
        /// Calls the supplied action at the given rate for a given number of seconds.
        /// </summary>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="period">The amount of time between calls.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public CoroutineHandle CallPeriodicallyOnInstance(float timeframe, float period, System.Action action, System.Action onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutineOnInstance(_CallContinuously(timeframe, period, action, onDone), Segment.Update);
        }

        /// <summary>
        /// Calls the supplied action at the given rate for a given number of seconds.
        /// </summary>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="period">The amount of time between calls.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="timing">The timing segment to run in.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public static CoroutineHandle CallPeriodically(float timeframe, float period, System.Action action, Segment timing, System.Action onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutine(Instance._CallContinuously(timeframe, period, action, onDone), timing);
        }

        /// <summary>
        /// Calls the supplied action at the given rate for a given number of seconds.
        /// </summary>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="period">The amount of time between calls.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="timing">The timing segment to run in.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public CoroutineHandle CallPeriodicallyOnInstance(float timeframe, float period, System.Action action, Segment timing, System.Action onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutineOnInstance(_CallContinuously(timeframe, period, action, onDone), timing);
        }

        /// <summary>
        /// Calls the supplied action at the given rate for a given number of seconds.
        /// </summary>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public static CoroutineHandle CallContinuously(float timeframe, System.Action action, System.Action onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutine(Instance._CallContinuously(timeframe, 0f, action, onDone), Segment.Update);
        }

        /// <summary>
        /// Calls the supplied action at the given rate for a given number of seconds.
        /// </summary>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public CoroutineHandle CallContinuouslyOnInstance(float timeframe, System.Action action, System.Action onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutineOnInstance(_CallContinuously(timeframe, 0f, action, onDone), Segment.Update);
        }

        /// <summary>
        /// Calls the supplied action every frame for a given number of seconds.
        /// </summary>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="timing">The timing segment to run in.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public static CoroutineHandle CallContinuously(float timeframe, System.Action action, Segment timing, System.Action onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutine(Instance._CallContinuously(timeframe, 0f, action, onDone), timing);
        }

        /// <summary>
        /// Calls the supplied action every frame for a given number of seconds.
        /// </summary>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="timing">The timing segment to run in.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public CoroutineHandle CallContinuouslyOnInstance(float timeframe, System.Action action, Segment timing, System.Action onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutineOnInstance(_CallContinuously(timeframe, 0f, action, onDone), timing);
        }

        private IEnumerator<float> _CallContinuously(float timeframe, float period, System.Action action, System.Action onDone)
        {
            double startTime = localTime;
            while (localTime <= startTime + timeframe)
            {
                yield return WaitForSecondsOnInstance(period);

                action();
            }

            if (onDone != null)
                onDone();
        }

        /// <summary>
        /// Calls the supplied action at the given rate for a given number of seconds.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied action each period.</param>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="period">The amount of time between calls.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public static CoroutineHandle CallPeriodically<T>(T reference, float timeframe, float period, 
            System.Action<T> action, System.Action<T> onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutine(Instance._CallContinuously(reference, timeframe, period, action, onDone), Segment.Update);
        }

        /// <summary>
        /// Calls the supplied action at the given rate for a given number of seconds.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied action each period.</param>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="period">The amount of time between calls.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public CoroutineHandle CallPeriodicallyOnInstance<T>(T reference, float timeframe, float period, 
            System.Action<T> action, System.Action<T> onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutineOnInstance(_CallContinuously(reference, timeframe, period, action, onDone), Segment.Update);
        }

        /// <summary>
        /// Calls the supplied action at the given rate for a given number of seconds.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied action each period.</param>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="period">The amount of time between calls.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="timing">The timing segment to run in.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public static CoroutineHandle CallPeriodically<T>(T reference, float timeframe, float period, System.Action<T> action, 
            Segment timing, System.Action<T> onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutine(Instance._CallContinuously(reference, timeframe, period, action, onDone), timing);
        }

        /// <summary>
        /// Calls the supplied action at the given rate for a given number of seconds.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied action each period.</param>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="period">The amount of time between calls.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="timing">The timing segment to run in.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public CoroutineHandle CallPeriodicallyOnInstance<T>(T reference, float timeframe, float period, System.Action<T> action,
            Segment timing, System.Action<T> onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutineOnInstance(_CallContinuously(reference, timeframe, period, action, onDone), timing);
        }

        /// <summary>
        /// Calls the supplied action every frame for a given number of seconds.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied action each frame.</param>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public static CoroutineHandle CallContinuously<T>(T reference, float timeframe, System.Action<T> action, System.Action<T> onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutine(Instance._CallContinuously(reference, timeframe, 0f, action, onDone), Segment.Update);
        }

        /// <summary>
        /// Calls the supplied action every frame for a given number of seconds.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied action each frame.</param>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public CoroutineHandle CallContinuouslyOnInstance<T>(T reference, float timeframe, System.Action<T> action, System.Action<T> onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutineOnInstance(_CallContinuously(reference, timeframe, 0f, action, onDone), Segment.Update);
        }

        /// <summary>
        /// Calls the supplied action every frame for a given number of seconds.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied action each frame.</param>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="timing">The timing segment to run in.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public static CoroutineHandle CallContinuously<T>(T reference, float timeframe, System.Action<T> action, 
            Segment timing, System.Action<T> onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutine(Instance._CallContinuously(reference, timeframe, 0f, action, onDone), timing);
        }

        /// <summary>
        /// Calls the supplied action every frame for a given number of seconds.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied action each frame.</param>
        /// <param name="timeframe">The number of seconds that this function should run.</param>
        /// <param name="action">The action to call every frame.</param>
        /// <param name="timing">The timing segment to run in.</param>
        /// <param name="onDone">An optional action to call when this function finishes.</param>
        /// <returns>The handle to the coroutine that is started by this function.</returns>
        public CoroutineHandle CallContinuouslyOnInstance<T>(T reference, float timeframe, System.Action<T> action,
            Segment timing, System.Action<T> onDone = null)
        {
            return action == null ? new CoroutineHandle() : RunCoroutineOnInstance(_CallContinuously(reference, timeframe, 0f, action, onDone), timing);
        }

        private IEnumerator<float> _CallContinuously<T>(T reference, float timeframe, float period,
            System.Action<T> action, System.Action<T> onDone = null)
        {
            double startTime = localTime;
            while (localTime <= startTime + timeframe)
            {
                yield return WaitForSecondsOnInstance(period);

                action(reference);
            }

            if (onDone != null)
                onDone(reference);
        }

        private struct ProcessData : System.IEquatable<ProcessData>
        {
            public CoroutineHandle Handle;
            public float PauseTime;

            public bool Equals(ProcessData other)
            {
                return Handle == other.Handle;
            }

            public override bool Equals(object other)
            {
                if (other is ProcessData)
                    return Equals((ProcessData)other);
                return false;
            }

            public override int GetHashCode()
            {
                return Handle.GetHashCode();
            }
        }

        private struct ProcessIndex : System.IEquatable<ProcessIndex>
        {
            public Segment seg;
            public int i;

            public bool Equals(ProcessIndex other)
            {
                return seg == other.seg && i == other.i;
            }

            public override bool Equals(object other)
            {
                if (other is ProcessIndex)
                    return Equals((ProcessIndex)other);
                return false;
            }

            public static bool operator ==(ProcessIndex a, ProcessIndex b)
            {
                return a.seg == b.seg && a.i == b.i;
            }

            public static bool operator !=(ProcessIndex a, ProcessIndex b)
            {
                return a.seg != b.seg || a.i != b.i;
            }

            public override int GetHashCode()
            {
                return (((int)seg - 4) * (int.MaxValue / 7)) + i;
            }
        }

        [System.Obsolete("Unity coroutine function, use RunCoroutine instead.", true)]
        public new Coroutine StartCoroutine(System.Collections.IEnumerator routine) { return null; }

        [System.Obsolete("Unity coroutine function, use RunCoroutine instead.", true)]
        public new Coroutine StartCoroutine(string methodName, object value) { return null; }

        [System.Obsolete("Unity coroutine function, use RunCoroutine instead.", true)]
        public new Coroutine StartCoroutine(string methodName) { return null; }

        [System.Obsolete("Unity coroutine function, use RunCoroutine instead.", true)]
        public new Coroutine StartCoroutine_Auto(System.Collections.IEnumerator routine) { return null; }

        [System.Obsolete("Unity coroutine function, use KillCoroutine instead.", true)]
        public new void StopCoroutine(string methodName) {}

        [System.Obsolete("Unity coroutine function, use KillCoroutine instead.", true)]
        public new void StopCoroutine(System.Collections.IEnumerator routine) {}

        [System.Obsolete("Unity coroutine function, use KillCoroutine instead.", true)]
        public new void StopCoroutine(Coroutine routine) {}

        [System.Obsolete("Unity coroutine function, use KillAllCoroutines instead.", true)]
        public new void StopAllCoroutines() {}

        [System.Obsolete("Use your own GameObject for this.", true)]
        public new static void Destroy(UnityEngine.Object obj) {}

        [System.Obsolete("Use your own GameObject for this.", true)]
        public new static void Destroy(UnityEngine.Object obj, float f) {}

        [System.Obsolete("Use your own GameObject for this.", true)]
        public new static void DestroyObject(UnityEngine.Object obj) {}

        [System.Obsolete("Use your own GameObject for this.", true)]
        public new static void DestroyObject(UnityEngine.Object obj, float f) {}

        [System.Obsolete("Use your own GameObject for this.", true)]
        public new static void DestroyImmediate(UnityEngine.Object obj) {}

        [System.Obsolete("Use your own GameObject for this.", true)]
        public new static void DestroyImmediate(UnityEngine.Object obj, bool b) {}

        [System.Obsolete("Just.. no.", true)]
        public new static T FindObjectOfType<T>() where T : UnityEngine.Object { return null; }

        [System.Obsolete("Just.. no.", true)]
        public new static UnityEngine.Object FindObjectOfType(System.Type t) { return null; }

        [System.Obsolete("Just.. no.", true)]
        public new static T[] FindObjectsOfType<T>() where T : UnityEngine.Object { return null; }

        [System.Obsolete("Just.. no.", true)]
        public new static UnityEngine.Object[] FindObjectsOfType(System.Type t) { return null; }

        [System.Obsolete("Just.. no.", true)]
        public new static void print(object message) {}
    }

    public enum Segment
    {
        Invalid = -1,
        Update,
        FixedUpdate,
        LateUpdate,
        SlowUpdate,
        RealtimeUpdate,
        EditorUpdate,
        EditorSlowUpdate,
        EndOfFrame,
        ManualTimeframe
    }

    /// <summary>
    /// A handle for a MEC coroutine.
    /// </summary>
    public struct CoroutineHandle : System.IEquatable<CoroutineHandle>
    {
        private const byte ReservedSpace = 0x1F;
        private readonly static int[] NextIndex = 
            { ReservedSpace + 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        private readonly int _id;

        public byte Key { get { return (byte)(_id & ReservedSpace); } }

        public CoroutineHandle(byte ind)
        {
            if (ind > ReservedSpace)
                ind -= ReservedSpace;

            _id = NextIndex[ind] + ind;
            NextIndex[ind] += ReservedSpace + 1;
        }

        public bool Equals(CoroutineHandle other)
        {
            return _id == other._id;
        }

        public override bool Equals(object other)
        {
            if (other is CoroutineHandle)
                return Equals((CoroutineHandle)other);
            return false;
        }

        public static bool operator ==(CoroutineHandle a, CoroutineHandle b)
        {
            return a._id == b._id;
        }

        public static bool operator !=(CoroutineHandle a, CoroutineHandle b)
        {
            return a._id != b._id;
        }

        public override int GetHashCode()
        {
            return _id;
        }

        /// <summary>
        /// Get or set the corrosponding coroutine's tag. Null removes the tag or represents no tag assigned.
        /// </summary>
        public string Tag
        {
            get { return Timing.GetTag(this); }
            set { Timing.SetTag(this, value); }
        }

        /// <summary>
        /// Get or set the corrosponding coroutine's layer. Null removes the layer or represents no layer assigned.
        /// </summary>
        public int? Layer
        {
            get { return Timing.GetLayer(this); }
            set
            {
                if (value == null)
                    Timing.RemoveLayer(this);
                else
                    Timing.SetLayer(this, (int)value);
            }
        }

        /// <summary>
        /// Get or set the coorsponding coroutine's segment.
        /// </summary>
        public Segment Segment
        {
            get { return Timing.GetSegment(this); }
            set { Timing.SetSegment(this, value); }
        }

        /// <summary>
        /// Is true until the coroutine function ends or is killed. Setting this to false will kill the coroutine.
        /// </summary>
        public bool IsRunning
        {
            get { return Timing.IsRunning(this); }
            set { if(!value) Timing.KillCoroutines(this); }
        }

        /// <summary>
        /// Is true while the coroutine is paused (but not in a WaitUntilDone holding pattern). Setting this value will pause or resume the coroutine. 
        /// </summary>
        public bool IsPaused
        {
            get { return Timing.IsPaused(this); }
            set { if (value) Timing.PauseCoroutines(this); else Timing.ResumeCoroutines(this);}
        }

        /// <summary>
        /// Is true if this handle may have been a valid handle at some point. (i.e. is not an error handle or a key to a coroutine lock)
        /// </summary>
        public bool IsValid
        {
            get { return Key != 0; }
        }
    }
}

public static class MECExtensionMethods
{
    /// <summary>
    /// Adds a delay to the beginning of this coroutine.
    /// </summary>
    /// <param name="coroutine">The coroutine handle to act upon.</param>
    /// <param name="timeToDelay">The number of seconds to delay this coroutine.</param>
    /// <returns>The modified coroutine handle.</returns>
    public static IEnumerator<float> Delay(this IEnumerator<float> coroutine, float timeToDelay)
    {
        yield return MovementEffects.Timing.WaitForSeconds(timeToDelay);

        while (coroutine.MoveNext())
            yield return coroutine.Current;
    }

    /// <summary>
    /// Adds a delay to the beginning of this coroutine until a function returns true.
    /// </summary>
    /// <param name="coroutine">The coroutine handle to act upon.</param>
    /// <param name="condition">The coroutine will be paused until this function returns true.</param>
    /// <returns>The modified coroutine handle.</returns>
    public static IEnumerator<float> Delay(this IEnumerator<float> coroutine, System.Func<bool> condition)
    {
        while (!condition())
            yield return 0f;

        while (coroutine.MoveNext())
            yield return coroutine.Current;
    }

    /// <summary>
    /// Adds a delay to the beginning of this coroutine until a function returns true.
    /// </summary>
    /// <param name="coroutine">The coroutine handle to act upon.</param>
    /// <param name="data">A variable that will be passed into the condition function each time it is tested.</param>
    /// <param name="condition">The coroutine will be paused until this function returns true.</param>
    /// <returns>The modified coroutine handle.</returns>
    public static IEnumerator<float> Delay<T>(this IEnumerator<float> coroutine, T data, System.Func<T, bool> condition)
    {
        while (!condition(data))
            yield return 0f;

        while (coroutine.MoveNext())
            yield return coroutine.Current;
    }

    /// <summary>
    /// Cancels this coroutine when the supplied game object is destroyed or made inactive.
    /// </summary>
    /// <param name="coroutine">The coroutine handle to act upon.</param>
    /// <param name="gameObject">The GameObject to test.</param>
    /// <returns>The modified coroutine handle.</returns>
    public static IEnumerator<float> CancelWith(this IEnumerator<float> coroutine, GameObject gameObject)
    {
        while (gameObject && gameObject.activeInHierarchy && coroutine.MoveNext())
            yield return coroutine.Current;
    }

    /// <summary>
    /// Cancels this coroutine when the supplied game objects are destroyed or made inactive.
    /// </summary>
    /// <param name="coroutine">The coroutine handle to act upon.</param>
    /// <param name="gameObject1">The first GameObject to test.</param>
    /// <param name="gameObject2">The second GameObject to test</param>
    /// <returns>The modified coroutine handle.</returns>
    public static IEnumerator<float> CancelWith(this IEnumerator<float> coroutine, GameObject gameObject1, GameObject gameObject2)
    {
        while (gameObject1 && gameObject1.activeInHierarchy && gameObject2 && gameObject2.activeInHierarchy && coroutine.MoveNext())
            yield return coroutine.Current;
    }

    /// <summary>
    /// Cancels this coroutine when the supplied game objects are destroyed or made inactive.
    /// </summary>
    /// <param name="coroutine">The coroutine handle to act upon.</param>
    /// <param name="gameObject1">The first GameObject to test.</param>
    /// <param name="gameObject2">The second GameObject to test</param>
    /// <param name="gameObject3">The third GameObject to test.</param>
    /// <returns>The modified coroutine handle.</returns>
    public static IEnumerator<float> CancelWith(this IEnumerator<float> coroutine,
        GameObject gameObject1, GameObject gameObject2, GameObject gameObject3)
    {
        while (gameObject1 && gameObject1.activeInHierarchy && gameObject2 && gameObject2.activeInHierarchy &&
                gameObject3 && gameObject3.activeInHierarchy && coroutine.MoveNext())
            yield return coroutine.Current;
    }

    /// <summary>
    /// Cancels this coroutine when the supplied function returns false.
    /// </summary>
    /// <param name="coroutine">The coroutine handle to act upon.</param>
    /// <param name="condition">The test function. True for continue, false to stop.</param>
    /// <returns>The modified coroutine handle.</returns>
    public static IEnumerator<float> CancelWith(this IEnumerator<float> coroutine, System.Func<bool> condition)
    {
        if (condition == null) yield break;

        while (condition() && coroutine.MoveNext())
            yield return coroutine.Current;
    }

    /// <summary>
    /// Runs the supplied coroutine immediately after this one.
    /// </summary>
    /// <param name="coroutine">The coroutine handle to act upon.</param>
    /// <param name="nextCoroutine">The coroutine to run next.</param>
    /// <returns>The modified coroutine handle.</returns>
    public static IEnumerator<float> Append(this IEnumerator<float> coroutine, IEnumerator<float> nextCoroutine)
    {
        while (coroutine.MoveNext())
            yield return coroutine.Current;

        if (nextCoroutine != null)
            while (nextCoroutine.MoveNext())
                yield return nextCoroutine.Current;
    }

    /// <summary>
    /// Runs the supplied function immediately after this coroutine finishes.
    /// </summary>
    /// <param name="coroutine">The coroutine handle to act upon.</param>
    /// <param name="onDone">The action to run after this coroutine finishes.</param>
    /// <returns>The modified coroutine handle.</returns>
    public static IEnumerator<float> Append(this IEnumerator<float> coroutine, System.Action onDone)
    {
        while (coroutine.MoveNext())
            yield return coroutine.Current;

        if (onDone != null)
            onDone();
    }

    /// <summary>
    /// Runs the supplied coroutine immediately before this one.
    /// </summary>
    /// <param name="coroutine">The coroutine handle to act upon.</param>
    /// <param name="lastCoroutine">The coroutine to run first.</param>
    /// <returns>The modified coroutine handle.</returns>
    public static IEnumerator<float> Prepend(this IEnumerator<float> coroutine, IEnumerator<float> lastCoroutine)
    {
        if (lastCoroutine != null)
            while (lastCoroutine.MoveNext())
                yield return lastCoroutine.Current;

        while (coroutine.MoveNext())
            yield return coroutine.Current;
    }

    /// <summary>
    /// Runs the supplied function immediately before this coroutine starts.
    /// </summary>
    /// <param name="coroutine">The coroutine handle to act upon.</param>
    /// <param name="onStart">The action to run before this coroutine starts.</param>
    /// <returns>The modified coroutine handle.</returns>
    public static IEnumerator<float> Prepend(this IEnumerator<float> coroutine, System.Action onStart)
    {
        if (onStart != null)
            onStart();

        while (coroutine.MoveNext())
            yield return coroutine.Current;
    }

    /// <summary>
    /// Combines the this coroutine with another and runs them in a combined handle.
    /// </summary>
    /// <param name="coroutineA">The coroutine handle to act upon.</param>
    /// <param name="coroutineB">The coroutine handle to combine.</param>
    /// <returns>The modified coroutine handle.</returns>
    public static IEnumerator<float> Superimpose(this IEnumerator<float> coroutineA, IEnumerator<float> coroutineB)
    {
        return Superimpose(coroutineA, coroutineB, MovementEffects.Timing.Instance);
    }

    /// <summary>
    /// Combines the this coroutine with another and runs them in a combined handle.
    /// </summary>
    /// <param name="coroutineA">The coroutine handle to act upon.</param>
    /// <param name="coroutineB">The coroutine handle to combine.</param>
    /// <param name="instance">The timing instance that this will be run in, if not the default instance.</param>
    /// <returns>The modified coroutine handle.</returns>
    public static IEnumerator<float> Superimpose(this IEnumerator<float> coroutineA, IEnumerator<float> coroutineB, MovementEffects.Timing instance)
    {
        while (coroutineA != null || coroutineB != null)
        {
            if (coroutineA != null && !(instance.localTime < coroutineA.Current) && !coroutineA.MoveNext())
                coroutineA = null;

            if (coroutineB != null && !(instance.localTime < coroutineB.Current) && !coroutineB.MoveNext())
                coroutineB = null;

            if ((coroutineA != null && float.IsNaN(coroutineA.Current)) || (coroutineB != null && float.IsNaN(coroutineB.Current)))
                yield return float.NaN;
            else if (coroutineA != null && coroutineB != null)
                yield return coroutineA.Current < coroutineB.Current ? coroutineA.Current : coroutineB.Current;
            else if (coroutineA == null && coroutineB != null)
                yield return coroutineB.Current;
            else if (coroutineA != null)
                yield return coroutineA.Current;
        }
    }

    /// <summary>
    /// Uses the passed in function to change the return values of this coroutine.
    /// </summary>
    /// <param name="coroutine">The coroutine handle to act upon.</param>
    /// <param name="newReturn">A function that takes the current return value and returns the new return.</param>
    /// <returns>The modified coroutine handle.</returns>
    public static IEnumerator<float> Hijack(this IEnumerator<float> coroutine, System.Func<float, float> newReturn)
    {
        if (newReturn == null) yield break;

        while (coroutine.MoveNext())
            yield return newReturn(coroutine.Current);
    }
}

