using System;
using System.Collections.Generic;
using UnityEngine;

namespace ImHooya
{
    public class HYMainThreadDispatcher : MonoBehaviour
    {
        private static HYMainThreadDispatcher instance;
        private static readonly Queue<Action> queue = new();

        // 🔥 메인 스레드에서 미리 생성
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (instance != null) return;

            var go = new GameObject(nameof(HYMainThreadDispatcher));
            DontDestroyOnLoad(go);
            instance = go.AddComponent<HYMainThreadDispatcher>();
        }

        public static void RunOnUnityThread(Action action)
        {
            if (action == null) return;

            // instance는 이미 메인 스레드에서 생성되어 있음
            lock (queue)
            {
                queue.Enqueue(action);
            }
        }

        private void Update()
        {
            lock (queue)
            {
                while (queue.Count > 0)
                {
                    queue.Dequeue()?.Invoke();
                }
            }
        }
    }
}