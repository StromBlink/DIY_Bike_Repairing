using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Tabtale.TTPlugins
{
    public class TTPLogger
    {
        private static TTPLogger _logger;
        private const string LOGFileName = "ttplugins.log";
        private const string LOGFolderName = "logs";
        public static string LOGLabel = "TTPLog::";
        private const string LOGTimestampFormat = "HH:mm:ss.fff ";
        private const int MaxMessagesInPacket = 5;
        private readonly string _logFilePath;
        private readonly List<string> _allMessages;
        private readonly List<string> _msgPacket;
        public static Action<string> onAddMessage;
        private static GameObject _loggerConsole;
        public static bool UseTimestamp = true;

        public static void Create()
        {
            _logger = new TTPLogger();
        }

        public static void Remove()
        {
            if (_logger != null)
            {
                _logger.Destroy();
                _logger = null;
            }
        }

        public static void Log(string message)
        {
#if TTP_DEV_MODE
            if (_logger == null)
            {
                Create();
            }

            _logger.LogMessage(message);
#endif
        }

        public static List<string> GetLogs()
        {
            if (_logger == null)
            {
                Create();
            }

            return _logger._allMessages;
        }

        public static string GetFilePath()
        {
            if (_logger == null)
            {
                return "";
            }

            return _logger._logFilePath;
        }

        private TTPLogger()
        {
            _allMessages = new List<string>();
            _msgPacket = new List<string>();
            SetupConsole();
            var logDir = Path.Combine(Application.persistentDataPath, LOGFolderName);
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            _logFilePath = Path.Combine(logDir, LOGFileName);
            if (File.Exists(_logFilePath))
            {
                File.Delete(_logFilePath);
            }

            Debug.Log("TTPLogger::Log file is " + _logFilePath);
            Application.logMessageReceivedThreaded += HandleLog;
        }

        private void SetupConsole()
        {
            if (!_loggerConsole)
            {
                _loggerConsole = Resources.Load<GameObject>("LoggerCanvas");
                _loggerConsole = GameObject.Instantiate(_loggerConsole);
                GameObject.DontDestroyOnLoad(_loggerConsole);
                var rectTransform = _loggerConsole.GetComponent<RectTransform>();
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
                var loggerParent = _loggerConsole.transform.Find("LoggerParent").gameObject;
                loggerParent.AddComponent<TTPConsoleViewController>();
                var msgListView = _loggerConsole.transform.Find("LoggerParent/Panel/VerticalListView").gameObject;
                var listView = msgListView.AddComponent<TTPLogLoggerListView>();
                var viewPort = _loggerConsole.transform.Find("LoggerParent/Panel/VerticalListView/Viewport").gameObject;
                listView.viewport = viewPort.GetComponent<RectTransform>();
                var logItem = _loggerConsole.transform.Find("LoggerParent/Panel/LogItem").gameObject;
                listView.itemPrefab = logItem.GetComponent<RectTransform>();
            }
        }

        private void Destroy()
        {
            _logger.StorePacket();
            Application.logMessageReceivedThreaded -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (logString.StartsWith(LOGLabel) && type == LogType.Log)
            {
                _logger.LogMessage(logString.Remove(0, LOGLabel.Length));
            }
        }

        private void LogMessage(string message)
        {
            var dt = DateTime.Now.ToString(LOGTimestampFormat);
            var logMsg = UseTimestamp ? dt + message : message;
            _allMessages.Add(logMsg);
            _msgPacket.Add(logMsg);
            if (onAddMessage != null)
            {
                onAddMessage(logMsg);
            }

            if (_msgPacket.Count > MaxMessagesInPacket) StorePacket();
        }

        private void StorePacket()
        {
            using (var sw = File.Exists(_logFilePath) ? File.AppendText(_logFilePath) : File.CreateText(_logFilePath))
            {
                foreach (var msg in _msgPacket)
                {
                    sw.WriteLine(msg);
                }
            }

            _msgPacket.Clear();
        }

        public static void FlushBuffer()
        {
            _logger.StorePacket();
        }
    }
}