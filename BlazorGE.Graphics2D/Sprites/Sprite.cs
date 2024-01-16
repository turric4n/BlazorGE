﻿#region Namespaces

using BlazorGE.Graphics.Assets;

#endregion

namespace BlazorGE.Graphics2D.Sprites
{
    public struct Sprite
    {
        #region Public Properties

        public int Height;
        public int SourceHeight;
        public int SourceWidth;
        public int SourceX;
        public int SourceY;
        public GraphicAsset SpriteSheet;
        public int Width;
        public int X;
        public int Y;

        #endregion

        #region Constructors

        public Sprite(GraphicAsset spriteSheet, int sourceX, int sourceY, int sourceWidth, int sourceHeight, int width, int height, int x = 0, int y = 0)
        {
            SpriteSheet = spriteSheet;
            SourceX = sourceX;
            SourceY = sourceY;
            SourceWidth = sourceWidth;
            SourceHeight = sourceHeight;
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        #endregion
    }
}
