using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System;

namespace HYPurchase
{
    public class HYSyncContext : MonoBehaviour
    {
        public static TaskScheduler unityTaskScheduler;
        public static int unityThread;
        public static SynchronizationContext unitySynchronizationContext;
        static public Queue<Action> runInUpdate = new Queue<Action>();

        public void Awake()
        {
            unitySynchronizationContext = SynchronizationContext.Current;
            unityThread = Thread.CurrentThread.ManagedThreadId;
            unityTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }
        public static bool isOnUnityThread => unityThread == Thread.CurrentThread.ManagedThreadId;
        public static void RunOnUnityThread(Action action)
        {
            if (unityThread == Thread.CurrentThread.ManagedThreadId)
            {
                action();
            }
            else
            {
                lock (runInUpdate)
                {
                    runInUpdate.Enqueue(action);
                }
            }
        }


        private void Update()
        {
            while (runInUpdate.Count > 0)
            {
                Action action = null;
                lock (runInUpdate)
                {
                    if (runInUpdate.Count > 0)
                        action = runInUpdate.Dequeue();
                }
                action?.Invoke();
            }
        }
    }
}