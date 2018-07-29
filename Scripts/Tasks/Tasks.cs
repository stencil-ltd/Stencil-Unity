using System;
using System.Threading.Tasks;
using UnityEngine;
using Util;

namespace Plugins.Tasks
{
    public static class Tasks
    {
        public static Task<T> Attempts<T>(this Task<T> task, int attempts, float? timeout = null)
        {
            Func<T> func = () =>
            {
                if (timeout.HasValue)
                    task.Wait(TimeSpan.FromSeconds(timeout.Value));
                else task.Wait();
                return task.Result;
            };
            return Attempts(func, attempts);
        }
        
        public static Task<T> Attempts<T>(Func<T> func, int attempts, TaskCompletionSource<T> tcs = null)
        {
            if (tcs == null)
                tcs = new TaskCompletionSource<T>();
            Task.Factory.StartNew(func).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    if (attempts == 1)
                        tcs.SetException(task.Exception.InnerExceptions);
                    else
                    {
                        Debug.LogWarning($"Task failed. Retrying {attempts}...");
                        Task.Factory.StartNew(func).ContinueWith(t =>
                        {
                            Attempts(func, attempts - 1,tcs);
                        });}
                }
                else
                    tcs.SetResult(task.Result);
            });
            return tcs.Task;
        } 
        
        public static Task<T> ContinueOnMain<T>(this Task<T> task, Action<Task<T>> action)
        {
            return task.ContinueWith(task1 =>
            {
                Objects.OnMain(() => action(task1));
                return task1.Result;
            });
        }
    }
}