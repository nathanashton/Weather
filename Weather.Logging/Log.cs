using System;
using System.Reflection;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Weather.Common.EventArgs;
using ILog = Weather.Common.Interfaces.ILog;

namespace Weather.Logging
{
    public class Log : ILog
    {
        // ReSharper disable once InconsistentNaming
        private static readonly log4net.ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void SetDebugLevel()
        {
            ((Hierarchy) LogManager.GetRepository()).Root.Level = Level.Debug;
            ((Hierarchy) LogManager.GetRepository()).RaiseConfigurationChanged(EventArgs.Empty);
            log.Info("Logging level set at DEBUG");
        }

        public void SetInfoLevel()
        {
            ((Hierarchy) LogManager.GetRepository()).Root.Level = Level.Info;
            ((Hierarchy) LogManager.GetRepository()).RaiseConfigurationChanged(EventArgs.Empty);
            log.Info("Logging level set at INFO");
        }

        public event EventHandler<DebugMessageArgs> DebugPanelMessage;

        public void Debug(string message)
        {
            var level = ((Hierarchy) LogManager.GetRepository()).Root.Level;
            if (level == Level.Debug)
            {
                DebugPanelMessage?.Invoke(null, new DebugMessageArgs {Message = message});
            }
            log.Debug(message);
        }

        public void Debug(string message, Exception exception)
        {
            var level = ((Hierarchy) LogManager.GetRepository()).Root.Level;
            if (level == Level.Debug)
            {
                DebugPanelMessage?.Invoke(null, new DebugMessageArgs {Message = message});
            }
            log.Debug(message, exception);
        }

        public void Info(string message)
        {
            var level = ((Hierarchy) LogManager.GetRepository()).Root.Level;
            if (level == Level.Debug)
            {
                DebugPanelMessage?.Invoke(null, new DebugMessageArgs {Message = message});
            }
            log.Info(message);
        }

        public void Info(string message, Exception exception)
        {
            var level = ((Hierarchy) LogManager.GetRepository()).Root.Level;
            if (level == Level.Debug)
            {
                DebugPanelMessage?.Invoke(null, new DebugMessageArgs {Message = message});
            }
            log.Info(message, exception);
        }

        public void Error(string message)
        {
            var level = ((Hierarchy) LogManager.GetRepository()).Root.Level;
            if (level == Level.Debug)
            {
                DebugPanelMessage?.Invoke(null, new DebugMessageArgs {Message = message});
            }
            log.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            var level = ((Hierarchy) LogManager.GetRepository()).Root.Level;
            if (level == Level.Debug)
            {
                DebugPanelMessage?.Invoke(null, new DebugMessageArgs {Message = message});
            }
            log.Error(message, exception);
        }
    }
}