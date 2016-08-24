using System;
using Weather.Common.EventArgs;

namespace Weather.Common.Interfaces
{
    public interface ILog
    {
        void Debug(string message);

        void Debug(string message, Exception exception);

        void Info(string message);

        void Info(string message, Exception exception);

        void Error(string message);

        void Error(string message, Exception exception);

        void SetDebugLevel();

        void SetInfoLevel();

        event EventHandler<DebugMessageArgs> DebugPanelMessage;
    }
}