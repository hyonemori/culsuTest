using System.IO;
using System.Text;
using System;

public static class Logger
{
    private static StreamWriter _sw;
    private static FileInfo _fi;
    private static DateTime _dtToday;
    private static DateTime _currentDtToday;

    private static string _fileName;

    public static string FileName
    {
        get { return _fileName; }
    }

    private static string _filePath;

    public static string FilePath
    {
        get { return _filePath; }
    }

    private static bool _isAutoFlush;

    public static bool IsAutoFlush
    {
        get { return _isAutoFlush; }
    }

    private static StringBuilder LogTimeText
    {
        get { return SetDateTime(); }
    }

    public static void LoggerInit(string fileName, string filePath, bool isAutoFlush)
    {
        if (_sw != null)
        {
            _sw.Dispose();
        }
        _dtToday = DateTime.Today;
        _currentDtToday = DateTime.Today;

        if (System.IO.Directory.Exists(filePath) == false)
        {
            System.IO.Directory.CreateDirectory(filePath);
        }

        _fileName = fileName;
        _isAutoFlush = isAutoFlush;
        _filePath = filePath;
        fileName = fileName + _dtToday.ToString("yyyyMMdd") + ".txt";
        string fileFullPath = System.IO.Path.Combine(filePath, fileName);
        _fi = new FileInfo(fileFullPath);
        _sw = _fi.AppendText();
        _sw.AutoFlush = isAutoFlush;
    }

    public static void Log(object message)
    {
        InternalLog("[Debug]", message);
    }

    public static void LogFormat(string format, params object[] args)
    {
        InternalLogFormat("[Debug]", format, args);
    }

    public static void LogWarning(object message)
    {
        InternalLog("[Warn]", message);
    }

    public static void LogWarningFormat(string format, params object[] args)
    {
        InternalLogFormat("[Warn]", format, args);
    }

    public static void LogError(object message)
    {
        InternalLog("[Error]", message);
    }

    public static void LogErrorFormat(string format, params object[] args)
    {
        InternalLogFormat("[Error]", format, args);
    }

    public static void LogInfo(object message)
    {
        InternalLog("[Info]", message);
    }

    public static void LogInfoFormat(string format, params object[] args)
    {
        InternalLogFormat("[Info]", format, args);
    }

    private static void InternalLog(string logLevel, object logMessage)
    {
        StreamWriter writer = GetWriter();
        writer.WriteLine(LogTimeText.Append(logLevel).Append(logMessage));
    }

    private static void InternalLogFormat(string logLevel, string format, params object[] args)
    {
        StreamWriter writer = GetWriter();
        writer.WriteLine(LogTimeText.Append(logLevel).Append(string.Format(format, args)));
    }

    public static void SaveEnd()
    {
        _sw.Flush();
        using (_sw)
        {
            _sw.Close();
        }
    }

    private static StringBuilder SetDateTime()
    {
        StringBuilder sb = new StringBuilder();
        _dtToday = DateTime.Now;
        sb.Length = 0;
        sb.Append(_dtToday.ToString("[HH:mm:ss]"));
        return sb;
    }

    private static StreamWriter GetWriter()
    {
        if (IsDailyRotate())
        {
            _sw.Flush();
            _sw.Close();
            _fi = new FileInfo(_filePath + _fileName + _dtToday.ToString("yyyyMMdd") + ".txt");
            StreamWriter sw = _fi.AppendText();
            _sw = sw;
            _currentDtToday = _dtToday;
            return _sw;
        }

        return _sw;
    }

    private static bool IsDailyRotate()
    {
        _dtToday = DateTime.Today;
        return _currentDtToday.Date != _dtToday.Date;
    }
}