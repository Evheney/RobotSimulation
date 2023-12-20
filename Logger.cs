using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Text;

namespace chip_counter
{
    public class Logger
    {
        private static Logger _instance = null;

        public ILog log;
        public RollingFileAppender rollingAppender;
        public PatternLayout layout;
        public log4net.Filter.LoggerMatchFilter lmf;

        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                }

                return _instance;
            }
        }

        public Logger(string Name)
        {
            string FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\LOG\\" + Name + "_"; //실행폴더 아래에 Log폴더


            Hierarchy hierarchy = (Hierarchy)LogManager.CreateRepository(Name + "Logger"); //LogManager.GetRepository();
            hierarchy.Configured = true;

            RollingFileAppender rollingAppender = new RollingFileAppender();
            rollingAppender.Name = Name + "logger";
            rollingAppender.File = FilePath; // 로그 파일 이름
            rollingAppender.AppendToFile = true;
            rollingAppender.Encoding = Encoding.UTF8;
            rollingAppender.StaticLogFileName = true;
            //rollingAppender.CountDirection = 1;
            rollingAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
            rollingAppender.LockingModel = new FileAppender.MinimalLock();
            rollingAppender.DatePattern = "yyyy-MM-dd'.LOG'"; // 날짜가 변경되면 이전 로그에 붙은 이름
            rollingAppender.StaticLogFileName = false;
            rollingAppender.MaxSizeRollBackups = 14;
            PatternLayout layout = new PatternLayout("%date [%-5level] : %message%newline");//로그 출력 포맷

            rollingAppender.Layout = layout;

            hierarchy.Root.AddAppender(rollingAppender);
            rollingAppender.ActivateOptions(); ;
            hierarchy.Root.Level = log4net.Core.Level.All;

            log = LogManager.GetLogger(Name + "Logger", Name + "logger");

        }

        public Logger()
        {
            string FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\LOG\\"; //실행폴더 아래에 Log폴더

            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Configured = true;

            RollingFileAppender rollingAppender = new RollingFileAppender();
            rollingAppender.Name = "logger";
            rollingAppender.File = FilePath; // 로그 파일 이름
            rollingAppender.AppendToFile = true;
            rollingAppender.Encoding = Encoding.UTF8;
            rollingAppender.StaticLogFileName = true;
            //rollingAppender.CountDirection = 1;
            rollingAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
            rollingAppender.LockingModel = new FileAppender.MinimalLock();
            rollingAppender.DatePattern = "yyyy-MM-dd'.LOG'"; // 날짜가 변경되면 이전 로그에 붙은 이름
            rollingAppender.StaticLogFileName = false;
            rollingAppender.MaxSizeRollBackups = 14;
            PatternLayout layout = new PatternLayout("%date [%-5level] : %message%newline");//로그 출력 포맷

            rollingAppender.Layout = layout;

            hierarchy.Root.AddAppender(rollingAppender);
            rollingAppender.ActivateOptions(); ;
            hierarchy.Root.Level = log4net.Core.Level.All;

            log = LogManager.GetLogger("logger");
            //Logger l = (Logger)log.Logger;
        }
        public void Add(string LogMsg)
        {
            Info(LogMsg);
        }

        public void Info(string log_msg)
        {
            log.Info(log_msg);
            System.Diagnostics.Debug.WriteLine(log_msg);
        }

        public void Error(string log_msg)
        {
            log.Error(log_msg);
            System.Diagnostics.Debug.WriteLine(log_msg);
        }
        public void Warn(string log_msg)
        {
            log.Warn(log_msg);
            System.Diagnostics.Debug.WriteLine(log_msg);
        }

        public void Debug(string log_msg)
        {
            log.Debug(log_msg);
            System.Diagnostics.Debug.WriteLine(log_msg);
        }
        public void Close()
        {
            LogManager.Shutdown();
        }
    }
}
