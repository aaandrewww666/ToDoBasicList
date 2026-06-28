using System;
using System.Threading.Tasks;

namespace ToDoBasicList.Presentation.Extensions
{
    /// <summary>
    /// Helpers for running a <see cref="Task"/> without awaiting it, centralizing the single
    /// needed <c>async void</c> in one place so an unhandled exception can never crash the app.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Runs the task without awaiting it. Any exception is routed to
        /// <paramref name="onException"/> (or swallowed when none is provided) instead of
        /// becoming a fatal unobserved exception.
        /// </summary>
        public static async void SafeFireAndForget(this Task task, Action<Exception>? onException = null)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                onException?.Invoke(ex);
            }
        }
    }
}
