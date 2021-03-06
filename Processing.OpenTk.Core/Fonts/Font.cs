﻿using OpenTK.Graphics;
using Processing.OpenTk.Core.Textures;
using System;
using System.Collections.Generic;
using System.IO;
using TrueTypeSharp;


namespace Processing.OpenTk.Core
{
    public class Font
    {
        private readonly FontGenerator _generator;

        private Dictionary<char, PImage> _cache = new Dictionary<char, PImage>();

        private readonly float _size;
        private readonly Color4 _color;

        public Font(Func<FileStream> source, Color4 color, float size = 80)
        {
            using (var stream = source())
                _generator = new FontGenerator(new TrueTypeFont(stream));
            _size = size;
            _color = color;
        }

        public PImage this[char c]
        {
            get
            {
                if (!_cache.ContainsKey(c))
                    _cache[c] = _generator[c, _size, _color];
                return _cache[c];
            }
        }
    }
}
