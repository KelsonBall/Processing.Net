using System;

namespace Processing.Core
{
    public interface IActionDispatchee
    {
        void InvokeDrawAction(Action action);
    }
}
