using System;
using System.Collections.Concurrent;

namespace Processing.Core
{
    public static class ConcurrentQueueExtensions
    {
        public static void Invoke(this ConcurrentQueue<Action> actions)
        {
            Action action;
            while (actions.TryDequeue(out action))
                action?.Invoke();
        }
    }
}
