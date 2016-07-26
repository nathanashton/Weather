using System.Runtime.CompilerServices;

namespace Weather.Common
{
    public static class MethodName
    {
        public static string Get([CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            return $"{fileName}({lineNumber}):{memberName}";
        }
    }
}