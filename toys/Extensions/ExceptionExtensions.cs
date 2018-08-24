using System;

namespace toys.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Get error message of an exception
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>
        /// a string of message
        /// </returns>
        public static string ToErrorMessage(this Exception ex)
        {
            return (ex == null)
                ? string.Empty
                : ex.Message + Environment.NewLine + ex.InnerException?.ToErrorMessage() + Environment.NewLine + ex.StackTrace;
        }

        /// <summary>
        /// To the exception message trim stacktrace.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static string ToExceptionMessageTrimStacktrace(this Exception ex)
        {
            var msg = ex.ToErrorMessage();
            if (string.IsNullOrWhiteSpace(msg))
                return string.Empty;

            var lastLineIdx = msg.LastIndexOf(":line", StringComparison.InvariantCultureIgnoreCase);
            var trace = lastLineIdx <= 0 ? string.Empty : msg.Substring(0, Math.Min(lastLineIdx + 10, msg.Length));

            return trace.Trim();
        }
    }
}
