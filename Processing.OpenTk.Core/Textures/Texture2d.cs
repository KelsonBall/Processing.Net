using OpenTK.Graphics.OpenGL4;
using System;
using ImageSharp;
using System.Linq;
using System.IO;
using System.Collections.Generic;

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

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            
            unsafe
            {
                fixed (int* dataptr = &data[0, 0])
                {
                    GL.TexImage2D(TextureTarget.Texture2D,
                              0,
                              PixelInternalFormat.Rgba,
                              Width,
                              Height,
                              0,
                              PixelFormat.BgraInteger,
                              PixelType.Int,
                              IntPtr.Zero);
                    GL.TexSubImage2D(TextureTarget.Texture2D,
                              0,
                              0,
                              0,
                              Width,
                              Height,                              
                              PixelFormat.BgraInteger,
                              PixelType.Int,
                              (IntPtr)dataptr);

                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

                    GL.BindTexture(TextureTarget.Texture2D, 0);                    
                }

                GL.Disable(EnableCap.Texture2D);
                GL.Disable(EnableCap.DepthTest);
            }
            
            
            
        }

        public static implicit operator int(Texture2d texture) => texture.Handle;

        public static Texture2d FromFile(string file) => FromImage(Image.Load(file));
            
        public static Texture2d FromStream(Func<Stream> streamSource)
        {
            using (var stream = streamSource())
                return FromImage(Image.Load(stream));
        }

        public static Texture2d FromImage(Image<Rgba32> image)
        {
            var pixels = new int[image.Pixels.Length];
            int[,] data = new int[image.Height, image.Width];

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var i = x + (y * image.Width);
                    data[y, x] = image.Pixels[i].R << 24 | image.Pixels[i].G << 16 | image.Pixels[i].B << 8 | image.Pixels[i].A;
                }
            }

            return new Texture2d(data);
        }

        public static IEnumerable<T> GetEnumerable<T>(Span<T> span)
        {
            for (int i = 0; i < span.Length; i++)
            {
                yield return span[i];
            }
        }
    }
    
}
