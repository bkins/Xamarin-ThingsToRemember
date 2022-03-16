using System;
using System.Collections;
// using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Avails.Xamarin.Interfaces;
using NLog;
using ThingsToRemember.Droid.Utilities;
using LogManager = NLog.LogManager;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
[assembly: Xamarin.Forms.Dependency(typeof(ILogger))]
namespace ThingsToRemember.Droid.Utilities
{
    public class MessageAndroid : IMessage, ILogger
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long)
                ?.Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short)
                ?.Show();
        }


        //BENDO: Continue to implement logging to LogCat (not sure if this is the best place/way.  Probably need to learn more about it.
        public void Log(LogLevel  level
                       , string    message
                       , Exception ex)
        {
            var catLog = LogManager.GetCurrentClassLogger();
            catLog.WithProperty("Tag"
                              , "Ins & Outs & Ooohs!");
            if (ex is null)
            {
                catLog.Info(message);
            }
            else
            {
                catLog.Error(ex, message);
            }
        }
        
        public void Log(LogLevel level,
                        object   value)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        IFormatProvider formatProvider,
                        object          value)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level,
                        string   message,
                        object   arg1,
                        object   arg2)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level,
                        string   message,
                        object   arg1,
                        object   arg2,
                        object   arg3)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        IFormatProvider formatProvider,
                        string          message,
                        bool            argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level,
                        string   message,
                        bool     argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        IFormatProvider formatProvider,
                        string          message,
                        char            argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level,
                        string   message,
                        char     argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        IFormatProvider formatProvider,
                        string          message,
                        byte            argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level,
                        string   message,
                        byte     argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        IFormatProvider formatProvider,
                        string          message,
                        string          argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level,
                        string   message,
                        string   argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        IFormatProvider formatProvider,
                        string          message,
                        int             argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level,
                        string   message,
                        int      argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        IFormatProvider formatProvider,
                        string          message,
                        long            argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level,
                        string   message,
                        long     argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        IFormatProvider formatProvider,
                        string          message,
                        float           argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level,
                        string   message,
                        float    argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        IFormatProvider formatProvider,
                        string          message,
                        double          argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level,
                        string   message,
                        double   argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        IFormatProvider formatProvider,
                        string          message,
                        decimal         argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level,
                        string   message,
                        decimal  argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        IFormatProvider formatProvider,
                        string          message,
                        object          argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level,
                        string   message,
                        object   argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        IFormatProvider formatProvider,
                        string          message,
                        sbyte           argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level,
                        string   message,
                        sbyte    argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        IFormatProvider formatProvider,
                        string          message,
                        uint            argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level,
                        string   message,
                        uint     argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        IFormatProvider formatProvider,
                        string          message,
                        ulong           argument)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level,
                        string   message,
                        ulong    argument)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel level)
        {
            throw new NotImplementedException();
        }

        public void Log(LogEventInfo   logEvent)
        {
            throw new NotImplementedException();
        }

        public void Log(Type         wrapperType,
                        LogEventInfo logEvent)
        {
            throw new NotImplementedException();
        }

        public void Log<T>(LogLevel level,
                           T        value)
        {
            throw new NotImplementedException();
        }

        public void Log<T>(LogLevel        level,
                           IFormatProvider formatProvider,
                           T               value)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel            level,
                        LogMessageGenerator messageFunc)
        {
            throw new NotImplementedException();
        }

        public void LogException(LogLevel  level,
                                 string    message,
                                 Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        Exception       exception,
                        string          message,
                        params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        Exception       exception,
                        IFormatProvider formatProvider,
                        string          message,
                        params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        IFormatProvider formatProvider,
                        string          message,
                        params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel level
                      , string   message)
        {
            throw new NotImplementedException();
        }

        public void Log(LogLevel        level,
                        string          message,
                        params object[] args)
        {
            throw new NotImplementedException();
        }
        
        public void Log<TArgument>(LogLevel        level,
                                   IFormatProvider formatProvider,
                                   string          message,
                                   TArgument       argument)
        {
            throw new NotImplementedException();
        }

        public void Log<TArgument>(LogLevel  level,
                                   string    message,
                                   TArgument argument)
        {
            throw new NotImplementedException();
        }

        public void Log<TArgument1, TArgument2>(LogLevel        level,
                                                IFormatProvider formatProvider,
                                                string          message,
                                                TArgument1      argument1,
                                                TArgument2      argument2)
        {
            throw new NotImplementedException();
        }

        public void Log<TArgument1, TArgument2>(LogLevel   level,
                                                string     message,
                                                TArgument1 argument1,
                                                TArgument2 argument2)
        {
            throw new NotImplementedException();
        }

        public void Log<TArgument1, TArgument2, TArgument3>(LogLevel        level,
                                                            IFormatProvider formatProvider,
                                                            string          message,
                                                            TArgument1      argument1,
                                                            TArgument2      argument2,
                                                            TArgument3      argument3)
        {
            throw new NotImplementedException();
        }

        public void Log<TArgument1, TArgument2, TArgument3>(LogLevel   level,
                                                            string     message,
                                                            TArgument1 argument1,
                                                            TArgument2 argument2,
                                                            TArgument3 argument3)
        {
            throw new NotImplementedException();
        }

        public string Name { get; }

        public LogFactory Factory { get; }

        public event EventHandler<EventArgs> LoggerReconfigured;

        public void Swallow(Action  action)
        {
            throw new NotImplementedException();
        }

        public T Swallow<T>(Func<T> func)
        {
            throw new NotImplementedException();
        }

        public T Swallow<T>(Func<T> func,
                            T      fallback)
        {
            throw new NotImplementedException();
        }

        public void          Swallow(Task                              task)
        {
            throw new NotImplementedException();
        }

        public Task          SwallowAsync(Task                         task)
        {
            throw new NotImplementedException();
        }

        public Task          SwallowAsync(Func<Task>                   asyncAction)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> SwallowAsync<TResult>(Func<Task<TResult>> asyncFunc)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> SwallowAsync<TResult>(Func<Task<TResult>> asyncFunc
                                                 , TResult             fallback)
        {
            throw new NotImplementedException();
        }

        public void Trace(object value)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider,
                          object          value)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message,
                          object arg1,
                          object arg2)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message,
                          object arg1,
                          object arg2,
                          object arg3)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider,
                          string          message,
                          bool            argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message,
                          bool   argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider,
                          string          message,
                          char            argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message,
                          char   argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider,
                          string          message,
                          byte            argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message,
                          byte   argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider,
                          string          message,
                          string          argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message,
                          string argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider,
                          string          message,
                          int             argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message,
                          int    argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider,
                          string          message,
                          long            argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message,
                          long   argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider,
                          string          message,
                          float           argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message,
                          float  argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider,
                          string          message,
                          double          argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message,
                          double argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider,
                          string          message,
                          decimal         argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(string  message,
                          decimal argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider,
                          string          message,
                          object          argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message,
                          object argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider,
                          string          message,
                          sbyte           argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message,
                          sbyte  argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider,
                          string          message,
                          uint            argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message,
                          uint   argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider,
                          string          message,
                          ulong           argument)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message,
                          ulong  argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(object value)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider,
                          object          value)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message,
                          object arg1,
                          object arg2)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message,
                          object arg1,
                          object arg2,
                          object arg3)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider,
                          string          message,
                          bool            argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message,
                          bool   argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider,
                          string          message,
                          char            argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message,
                          char   argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider,
                          string          message,
                          byte            argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message,
                          byte   argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider,
                          string          message,
                          string          argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message,
                          string argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider,
                          string          message,
                          int             argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message,
                          int    argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider,
                          string          message,
                          long            argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message,
                          long   argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider,
                          string          message,
                          float           argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message,
                          float  argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider,
                          string          message,
                          double          argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message,
                          double argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider,
                          string          message,
                          decimal         argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(string  message,
                          decimal argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider,
                          string          message,
                          object          argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message,
                          object argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider,
                          string          message,
                          sbyte           argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message,
                          sbyte  argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider,
                          string          message,
                          uint            argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message,
                          uint   argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider,
                          string          message,
                          ulong           argument)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message,
                          ulong  argument)
        {
            throw new NotImplementedException();
        }

        public void Info(object value)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider,
                         object          value)
        {
            throw new NotImplementedException();
        }

        public void Info(string message,
                         object arg1,
                         object arg2)
        {
            throw new NotImplementedException();
        }

        public void Info(string message,
                         object arg1,
                         object arg2,
                         object arg3)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider,
                         string          message,
                         bool            argument)
        {
            throw new NotImplementedException();
        }

        public void Info(string message,
                         bool   argument)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider,
                         string          message,
                         char            argument)
        {
            throw new NotImplementedException();
        }

        public void Info(string message,
                         char   argument)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider,
                         string          message,
                         byte            argument)
        {
            throw new NotImplementedException();
        }

        public void Info(string message,
                         byte   argument)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider,
                         string          message,
                         string          argument)
        {
            throw new NotImplementedException();
        }

        public void Info(string message,
                         string argument)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider,
                         string          message,
                         int             argument)
        {
            throw new NotImplementedException();
        }

        public void Info(string message,
                         int    argument)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider,
                         string          message,
                         long            argument)
        {
            throw new NotImplementedException();
        }

        public void Info(string message,
                         long   argument)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider,
                         string          message,
                         float           argument)
        {
            throw new NotImplementedException();
        }

        public void Info(string message,
                         float  argument)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider,
                         string          message,
                         double          argument)
        {
            throw new NotImplementedException();
        }

        public void Info(string message,
                         double argument)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider,
                         string          message,
                         decimal         argument)
        {
            throw new NotImplementedException();
        }

        public void Info(string  message,
                         decimal argument)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider,
                         string          message,
                         object          argument)
        {
            throw new NotImplementedException();
        }

        public void Info(string message,
                         object argument)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider,
                         string          message,
                         sbyte           argument)
        {
            throw new NotImplementedException();
        }

        public void Info(string message,
                         sbyte  argument)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider,
                         string          message,
                         uint            argument)
        {
            throw new NotImplementedException();
        }

        public void Info(string message,
                         uint   argument)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider,
                         string          message,
                         ulong           argument)
        {
            throw new NotImplementedException();
        }

        public void Info(string message,
                         ulong  argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(object value)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider,
                         object          value)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message,
                         object arg1,
                         object arg2)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message,
                         object arg1,
                         object arg2,
                         object arg3)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider,
                         string          message,
                         bool            argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message,
                         bool   argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider,
                         string          message,
                         char            argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message,
                         char   argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider,
                         string          message,
                         byte            argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message,
                         byte   argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider,
                         string          message,
                         string          argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message,
                         string argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider,
                         string          message,
                         int             argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message,
                         int    argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider,
                         string          message,
                         long            argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message,
                         long   argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider,
                         string          message,
                         float           argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message,
                         float  argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider,
                         string          message,
                         double          argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message,
                         double argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider,
                         string          message,
                         decimal         argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(string  message,
                         decimal argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider,
                         string          message,
                         object          argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message,
                         object argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider,
                         string          message,
                         sbyte           argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message,
                         sbyte  argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider,
                         string          message,
                         uint            argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message,
                         uint   argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider,
                         string          message,
                         ulong           argument)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message,
                         ulong  argument)
        {
            throw new NotImplementedException();
        }

        public void Error(object value)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider,
                          object          value)
        {
            throw new NotImplementedException();
        }

        public void Error(string message,
                          object arg1,
                          object arg2)
        {
            throw new NotImplementedException();
        }

        public void Error(string message,
                          object arg1,
                          object arg2,
                          object arg3)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider,
                          string          message,
                          bool            argument)
        {
            throw new NotImplementedException();
        }

        public void Error(string message,
                          bool   argument)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider,
                          string          message,
                          char            argument)
        {
            throw new NotImplementedException();
        }

        public void Error(string message,
                          char   argument)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider,
                          string          message,
                          byte            argument)
        {
            throw new NotImplementedException();
        }

        public void Error(string message,
                          byte   argument)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider,
                          string          message,
                          string          argument)
        {
            throw new NotImplementedException();
        }

        public void Error(string message,
                          string argument)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider,
                          string          message,
                          int             argument)
        {
            throw new NotImplementedException();
        }

        public void Error(string message,
                          int    argument)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider,
                          string          message,
                          long            argument)
        {
            throw new NotImplementedException();
        }

        public void Error(string message,
                          long   argument)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider,
                          string          message,
                          float           argument)
        {
            throw new NotImplementedException();
        }

        public void Error(string message,
                          float  argument)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider,
                          string          message,
                          double          argument)
        {
            throw new NotImplementedException();
        }

        public void Error(string message,
                          double argument)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider,
                          string          message,
                          decimal         argument)
        {
            throw new NotImplementedException();
        }

        public void Error(string  message,
                          decimal argument)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider,
                          string          message,
                          object          argument)
        {
            throw new NotImplementedException();
        }

        public void Error(string message,
                          object argument)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider,
                          string          message,
                          sbyte           argument)
        {
            throw new NotImplementedException();
        }

        public void Error(string message,
                          sbyte  argument)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider,
                          string          message,
                          uint            argument)
        {
            throw new NotImplementedException();
        }

        public void Error(string message,
                          uint   argument)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider,
                          string          message,
                          ulong           argument)
        {
            throw new NotImplementedException();
        }

        public void Error(string message,
                          ulong  argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(object value)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider,
                          object          value)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message,
                          object arg1,
                          object arg2)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message,
                          object arg1,
                          object arg2,
                          object arg3)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider,
                          string          message,
                          bool            argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message,
                          bool   argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider,
                          string          message,
                          char            argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message,
                          char   argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider,
                          string          message,
                          byte            argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message,
                          byte   argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider,
                          string          message,
                          string          argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message,
                          string argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider,
                          string          message,
                          int             argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message,
                          int    argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider,
                          string          message,
                          long            argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message,
                          long   argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider,
                          string          message,
                          float           argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message,
                          float  argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider,
                          string          message,
                          double          argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message,
                          double argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider,
                          string          message,
                          decimal         argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string  message,
                          decimal argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider,
                          string          message,
                          object          argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message,
                          object argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider,
                          string          message,
                          sbyte           argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message,
                          sbyte  argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider,
                          string          message,
                          uint            argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message,
                          uint   argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider,
                          string          message,
                          ulong           argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message,
                          ulong  argument)
        {
            throw new NotImplementedException();
        }

        public void Trace<T>(T value)
        {
            throw new NotImplementedException();
        }

        public void Trace<T>(IFormatProvider formatProvider,
                             T               value)
        {
            throw new NotImplementedException();
        }

        public void Trace(LogMessageGenerator messageFunc)
        {
            throw new NotImplementedException();
        }

        public void TraceException(string    message,
                                   Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Trace(Exception exception,
                          string    message)
        {
            throw new NotImplementedException();
        }

        public void Trace(Exception       exception,
                          string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Trace(Exception       exception,
                          IFormatProvider formatProvider,
                          string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider,
                          string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message)
        {
            throw new NotImplementedException();
        }

        public void Trace(string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Trace(string    message,
                          Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Trace<TArgument>(IFormatProvider formatProvider,
                                     string          message,
                                     TArgument       argument)
        {
            throw new NotImplementedException();
        }

        public void Trace<TArgument>(string    message,
                                     TArgument argument)
        {
            throw new NotImplementedException();
        }

        public void Trace<TArgument1, TArgument2>(IFormatProvider formatProvider,
                                                  string          message,
                                                  TArgument1      argument1,
                                                  TArgument2      argument2)
        {
            throw new NotImplementedException();
        }

        public void Trace<TArgument1, TArgument2>(string     message,
                                                  TArgument1 argument1,
                                                  TArgument2 argument2)
        {
            throw new NotImplementedException();
        }

        public void Trace<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider,
                                                              string          message,
                                                              TArgument1      argument1,
                                                              TArgument2      argument2,
                                                              TArgument3      argument3)
        {
            throw new NotImplementedException();
        }

        public void Trace<TArgument1, TArgument2, TArgument3>(string     message,
                                                              TArgument1 argument1,
                                                              TArgument2 argument2,
                                                              TArgument3 argument3)
        {
            throw new NotImplementedException();
        }

        public void Debug<T>(T value)
        {
            throw new NotImplementedException();
        }

        public void Debug<T>(IFormatProvider formatProvider,
                             T               value)
        {
            throw new NotImplementedException();
        }

        public void Debug(LogMessageGenerator messageFunc)
        {
            throw new NotImplementedException();
        }

        public void DebugException(string    message,
                                   Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Debug(Exception exception,
                          string    message)
        {
            throw new NotImplementedException();
        }

        public void Debug(Exception       exception,
                          string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Debug(Exception       exception,
                          IFormatProvider formatProvider,
                          string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider,
                          string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message)
        {
            throw new NotImplementedException();
        }

        public void Debug(string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Debug(string    message,
                          Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Debug<TArgument>(IFormatProvider formatProvider,
                                     string          message,
                                     TArgument       argument)
        {
            throw new NotImplementedException();
        }

        public void Debug<TArgument>(string    message,
                                     TArgument argument)
        {
            throw new NotImplementedException();
        }

        public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider,
                                                  string          message,
                                                  TArgument1      argument1,
                                                  TArgument2      argument2)
        {
            throw new NotImplementedException();
        }

        public void Debug<TArgument1, TArgument2>(string     message,
                                                  TArgument1 argument1,
                                                  TArgument2 argument2)
        {
            throw new NotImplementedException();
        }

        public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider,
                                                              string          message,
                                                              TArgument1      argument1,
                                                              TArgument2      argument2,
                                                              TArgument3      argument3)
        {
            throw new NotImplementedException();
        }

        public void Debug<TArgument1, TArgument2, TArgument3>(string     message,
                                                              TArgument1 argument1,
                                                              TArgument2 argument2,
                                                              TArgument3 argument3)
        {
            throw new NotImplementedException();
        }

        public void Info<T>(T value)
        {
            throw new NotImplementedException();
        }

        public void Info<T>(IFormatProvider formatProvider,
                            T               value)
        {
            throw new NotImplementedException();
        }

        public void Info(LogMessageGenerator messageFunc)
        {
            throw new NotImplementedException();
        }

        public void InfoException(string    message,
                                  Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Info(Exception exception,
                         string    message)
        {
            throw new NotImplementedException();
        }

        public void Info(Exception       exception,
                         string          message,
                         params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Info(Exception       exception,
                         IFormatProvider formatProvider,
                         string          message,
                         params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider,
                         string          message,
                         params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Info(string message)
        {
            throw new NotImplementedException();
        }

        public void Info(string          message,
                         params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Info(string    message,
                         Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Info<TArgument>(IFormatProvider formatProvider,
                                    string          message,
                                    TArgument       argument)
        {
            throw new NotImplementedException();
        }

        public void Info<TArgument>(string    message,
                                    TArgument argument)
        {
            throw new NotImplementedException();
        }

        public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider,
                                                 string          message,
                                                 TArgument1      argument1,
                                                 TArgument2      argument2)
        {
            throw new NotImplementedException();
        }

        public void Info<TArgument1, TArgument2>(string     message,
                                                 TArgument1 argument1,
                                                 TArgument2 argument2)
        {
            throw new NotImplementedException();
        }

        public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider,
                                                             string          message,
                                                             TArgument1      argument1,
                                                             TArgument2      argument2,
                                                             TArgument3      argument3)
        {
            throw new NotImplementedException();
        }

        public void Info<TArgument1, TArgument2, TArgument3>(string     message,
                                                             TArgument1 argument1,
                                                             TArgument2 argument2,
                                                             TArgument3 argument3)
        {
            throw new NotImplementedException();
        }

        public void Warn<T>(T value)
        {
            throw new NotImplementedException();
        }

        public void Warn<T>(IFormatProvider formatProvider,
                            T               value)
        {
            throw new NotImplementedException();
        }

        public void Warn(LogMessageGenerator messageFunc)
        {
            throw new NotImplementedException();
        }

        public void WarnException(string    message,
                                  Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Warn(Exception exception,
                         string    message)
        {
            throw new NotImplementedException();
        }

        public void Warn(Exception       exception,
                         string          message,
                         params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Warn(Exception       exception,
                         IFormatProvider formatProvider,
                         string          message,
                         params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider,
                         string          message,
                         params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message)
        {
            throw new NotImplementedException();
        }

        public void Warn(string          message,
                         params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Warn(string    message,
                         Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Warn<TArgument>(IFormatProvider formatProvider,
                                    string          message,
                                    TArgument       argument)
        {
            throw new NotImplementedException();
        }

        public void Warn<TArgument>(string    message,
                                    TArgument argument)
        {
            throw new NotImplementedException();
        }

        public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider,
                                                 string          message,
                                                 TArgument1      argument1,
                                                 TArgument2      argument2)
        {
            throw new NotImplementedException();
        }

        public void Warn<TArgument1, TArgument2>(string     message,
                                                 TArgument1 argument1,
                                                 TArgument2 argument2)
        {
            throw new NotImplementedException();
        }

        public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider,
                                                             string          message,
                                                             TArgument1      argument1,
                                                             TArgument2      argument2,
                                                             TArgument3      argument3)
        {
            throw new NotImplementedException();
        }

        public void Warn<TArgument1, TArgument2, TArgument3>(string     message,
                                                             TArgument1 argument1,
                                                             TArgument2 argument2,
                                                             TArgument3 argument3)
        {
            throw new NotImplementedException();
        }

        public void Error<T>(T value)
        {
            throw new NotImplementedException();
        }

        public void Error<T>(IFormatProvider formatProvider,
                             T               value)
        {
            throw new NotImplementedException();
        }

        public void Error(LogMessageGenerator messageFunc)
        {
            throw new NotImplementedException();
        }

        public void ErrorException(string    message,
                                   Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error(Exception exception,
                          string    message)
        {
            throw new NotImplementedException();
        }

        public void Error(Exception       exception,
                          string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Error(Exception       exception,
                          IFormatProvider formatProvider,
                          string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider,
                          string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Error(string message)
        {
            throw new NotImplementedException();
        }

        public void Error(string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Error(string    message,
                          Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error<TArgument>(IFormatProvider formatProvider,
                                     string          message,
                                     TArgument       argument)
        {
            throw new NotImplementedException();
        }

        public void Error<TArgument>(string    message,
                                     TArgument argument)
        {
            throw new NotImplementedException();
        }

        public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider,
                                                  string          message,
                                                  TArgument1      argument1,
                                                  TArgument2      argument2)
        {
            throw new NotImplementedException();
        }

        public void Error<TArgument1, TArgument2>(string     message,
                                                  TArgument1 argument1,
                                                  TArgument2 argument2)
        {
            throw new NotImplementedException();
        }

        public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider,
                                                              string          message,
                                                              TArgument1      argument1,
                                                              TArgument2      argument2,
                                                              TArgument3      argument3)
        {
            throw new NotImplementedException();
        }

        public void Error<TArgument1, TArgument2, TArgument3>(string     message,
                                                              TArgument1 argument1,
                                                              TArgument2 argument2,
                                                              TArgument3 argument3)
        {
            throw new NotImplementedException();
        }

        public void Fatal<T>(T value)
        {
            throw new NotImplementedException();
        }

        public void Fatal<T>(IFormatProvider formatProvider,
                             T               value)
        {
            throw new NotImplementedException();
        }

        public void Fatal(LogMessageGenerator messageFunc)
        {
            throw new NotImplementedException();
        }

        public void FatalException(string    message,
                                   Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Exception exception,
                          string    message)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Exception       exception,
                          string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Exception       exception,
                          IFormatProvider formatProvider,
                          string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider,
                          string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string          message,
                          params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string    message,
                          Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Fatal<TArgument>(IFormatProvider formatProvider,
                                     string          message,
                                     TArgument       argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal<TArgument>(string    message,
                                     TArgument argument)
        {
            throw new NotImplementedException();
        }

        public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider,
                                                  string          message,
                                                  TArgument1      argument1,
                                                  TArgument2      argument2)
        {
            throw new NotImplementedException();
        }

        public void Fatal<TArgument1, TArgument2>(string     message,
                                                  TArgument1 argument1,
                                                  TArgument2 argument2)
        {
            throw new NotImplementedException();
        }

        public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider,
                                                              string          message,
                                                              TArgument1      argument1,
                                                              TArgument2      argument2,
                                                              TArgument3      argument3)
        {
            throw new NotImplementedException();
        }

        public void Fatal<TArgument1, TArgument2, TArgument3>(string     message,
                                                              TArgument1 argument1,
                                                              TArgument2 argument2,
                                                              TArgument3 argument3)
        {
            throw new NotImplementedException();
        }

        public bool IsTraceEnabled { get; }

        public bool IsDebugEnabled { get; }

        public bool IsInfoEnabled { get; }

        public bool IsWarnEnabled { get; }

        public bool IsErrorEnabled { get; }

        public bool        IsFatalEnabled { get; }
        public IDictionary Properties     { get; }
    }
}