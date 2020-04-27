using COVID19Tracker.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace COVID19Tracker.Library.Services
{
    public class Logger : ILogger
    {
        public struct LogDetails
        {
            public string Message;
            public Exception Exception;
            public IDictionary<string, string> Properties;

            public bool AppCenterTrackEvent;
        }

        bool _run = false;
        object _locker = new object();

        public string Prefix { get; set; } = "LOGGER";

        public IAppCenterLogger AppCenterLogger { get; set; } = null;

        Queue<LogDetails> LogStack { get; set; } = new Queue<LogDetails>();

        public void Start()
        {
            _run = true;
            //ThreadPool.QueueUserWorkItem()
            Task.Run(new Action(() => { Consume(); }));
        }

        public void Stop()
        {
            _run = false;
        }

        void Consume()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            while (_run)
            {
                lock (_locker)
                {
                    if (LogStack.Count != 0)
                    {
                        var log = LogStack.Dequeue();

                        Debug.WriteLine(log.Message);

                        if (log.AppCenterTrackEvent && AppCenterLogger != null)
                        {
                            // send log message to AppCenter
                            AppCenterLogger.TrackEvent(log.Message, log.Properties);
                        }

                        if (log.Exception != null && AppCenterLogger != null)
                        {
                            // report to app center logger
                            AppCenterLogger.TrackError(new Exception(log.Message, log.Exception));
                        }
                    }
                }
            }
        }

        public void LogError(Exception exception, string custom_message = "", IDictionary<string, string> properties = null)
        {
            Exception ex = exception;

            string indent = "";
            StringBuilder other_logs = new StringBuilder();
            StringBuilder messages = new StringBuilder();

            messages.AppendLine($"ERROR_MSG: {ex.Message} | DEV_MSG: {custom_message}");

            if (properties != null)
            {
                foreach (var key in properties.Keys)
                {
                    other_logs.AppendLine($"{key}: {properties[key]}");
                }

                messages.AppendLine($"Properties");
                messages.AppendLine(other_logs.ToString());
            }

            messages.AppendLine($"Stack Trace");
            // go through inner exceptions
            while (ex != null)
            {
                messages.AppendLine($"{indent}ERROR_MSG: {ex.Message}\r\n{indent}{ex.StackTrace}");
                indent += "\t";
                ex = ex.InnerException;
            }
            messages.AppendLine($" ----------- EOEx -----------");

            LogStack.Enqueue(new LogDetails()
            {
                Message = string.Join("\r\n", messages.ToString()),
                Exception = exception,
                Properties = properties
            });
        }

        public void Log(string message, IDictionary<string, string> properties = null, bool AppCenterTrackEvent = true)
        {
            StringBuilder other_logs = new StringBuilder();

            if (properties != null)
            {
                other_logs.AppendLine("\tProperties");

                foreach (var key in properties.Keys)
                {
                    other_logs.AppendLine($"\t{key}: {properties[key]}");
                }
            }

            string build_msg = $"{message}\r\n{other_logs}";

            var log = new LogDetails()
            {
                Message = build_msg,
                Exception = null,
                Properties = properties,
                AppCenterTrackEvent = AppCenterTrackEvent
            };

            //OnLog?.Invoke(this, log);

            LogStack.Enqueue(log);
        }
    }
}
