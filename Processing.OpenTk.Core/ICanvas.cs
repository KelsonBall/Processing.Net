using Processing.OpenTk.Core.Rendering;
using System;

namespace Processing.OpenTk.Core
{
    public interface ICanvas : IRenderer2d
    {
        new IStyle Style { get; set; }

        ulong FrameCount { get; set; }

        event Action<Canvas> Setup;

        event Action<Canvas> Draw;
    }
}
