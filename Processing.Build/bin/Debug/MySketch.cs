using System.Drawing;
using Processing.Core;


public Sketch()
{
    Size(400, 400);
}

public override void Draw()
{
    byte r = (byte)(FrameCount % 255);
    byte b = (byte)(255 - (FrameCount % 255));
    Background(r, 0, b);
}