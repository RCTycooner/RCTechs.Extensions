using System;
using System.Threading;
using System.Threading.Tasks;

namespace RCTechs.Extensions
{
    public class ContinuationTask
    {
        private Task _task;
        public Task Task { get { return _task; } }

        private TaskScheduler _taskScheduler;
        public TaskScheduler TaskScheduler { get { return _taskScheduler; } }

        public ContinuationTask(Task task, TaskScheduler scheduler)
        {
            _task = task;
            _taskScheduler = scheduler;
        }
    }

    public class ContinuationTask<T>
    {
        private Task<T> _task;
        public Task<T> Task { get { return _task; } }

        private TaskScheduler _taskScheduler;
        public TaskScheduler TaskScheduler { get { return _taskScheduler; } }

        public ContinuationTask(Task<T> task, TaskScheduler scheduler)
        {
            _task = task;
            _taskScheduler = scheduler;
        }
    }

    public static class TaskExtensions
    {
        #region Task
        public static ContinuationTask HandleContinuation(this Task t, TaskScheduler scheduler)
        {
            return new ContinuationTask(t, scheduler);
        }
        public static ContinuationTask Success(this ContinuationTask task, Action<Task> action)
        {
            task.Task.ContinueWith(action, CancellationToken.None, TaskContinuationOptions.NotOnFaulted, task.TaskScheduler);
            return task;
        }

        public static ContinuationTask Error(this ContinuationTask task, Action<Task> action)
        {
            task.Task.ContinueWith(action, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, task.TaskScheduler);
            return task;
        }

        public static ContinuationTask Finally(this ContinuationTask task, Action<Task> action)
        {
            task.Task.ContinueWith(action, CancellationToken.None, TaskContinuationOptions.None, task.TaskScheduler);
            return task;
        }
        #endregion

        #region Task<T>
        public static ContinuationTask<T> HandleContinuation<T>(this Task<T> t, TaskScheduler scheduler)
        {
            return new ContinuationTask<T>(t, scheduler);
        }

        public static ContinuationTask<T> Success<T>(this ContinuationTask<T> task, Action<Task<T>> action)
        {
            task.Task.ContinueWith(action, CancellationToken.None, TaskContinuationOptions.NotOnFaulted, task.TaskScheduler);
            return task;
        }

        public static ContinuationTask<T> Error<T>(this ContinuationTask<T> task, Action<Task<T>> action)
        {
            task.Task.ContinueWith(action, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, task.TaskScheduler);
            return task;
        }

        public static ContinuationTask<T> Finally<T>(this ContinuationTask<T> task, Action<Task<T>> action)
        {
            task.Task.ContinueWith(action, CancellationToken.None, TaskContinuationOptions.None, task.TaskScheduler);
            return task;
        }
        #endregion
    }
}
