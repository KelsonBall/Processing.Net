using OpenTK.Graphics.OpenGL;
using System;

namespace Processing.OpenTk.Core.Textures
{
    public struct Texture2d
    {
        public readonly int Handle;

        public readonly int Width;
        public readonly int Height;

        public Texture2d(int[,] data)
        {
            Width = data.GetLength(0);
            Height = data.GetLength(1);

            Handle = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, Handle);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);

            GL.TexImage2D(TextureTarget.Texture2D,
                          0,
                          PixelInternalFormat.Rgba,
                          Width,
                          Height,
                          0,
                          PixelFormat.RgbaInteger,
                          PixelType.Int,
                          IntPtr.Zero);

            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, Width, Height, PixelFormat.RgbaInteger, PixelType.Int, data);


            //Release texture
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public static implicit operator int(Texture2d texture) => texture.Handle;
    }
}
