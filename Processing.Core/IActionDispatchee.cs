using System;

namespace Processing.Core
{
    public interface IActionDispatchee
    {
        void Invoke(Action action);
    }
}
