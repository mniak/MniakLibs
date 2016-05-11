using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mniak.Network
{
    internal static class AsyncExtensions
    {
        public static Task<TResult> AsApm<TResult>(this Task<TResult> task, AsyncCallback callback, object state)
        {
            var tcs = new TaskCompletionSource<TResult>(state);

            task.ContinueWith(delegate
            {
                if (task.IsFaulted) tcs.TrySetException(task.Exception.InnerExceptions);
                else if (task.IsCanceled) tcs.TrySetCanceled();
                else tcs.TrySetResult(task.Result);

                if (callback != null) callback(tcs.Task);
            }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);

            return tcs.Task;
        }
        public static Task<TResult> ToStartedTask<TResult>(this Func<TResult> func)
        {
            Task<TResult> task;
            task = new Task<TResult>(func);
            task.Start();
            return task;
        }
        public static TResult GetResult<TResult>(this IAsyncResult asyncResult)
        {
            try
            {
                return ((Task<TResult>)asyncResult).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
