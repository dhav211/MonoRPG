using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace MonoRPG
{
    public class TilemapRenderer
    {
        List<List<Rectangle>> sourceRectangles;
        List<List<Rectangle>> destinationRectangles;
        Texture2D tileset;
        SpriteBatch spriteBatch;

        enum RendererState { EMPTY, LEVEL_LOADED }
        RendererState currentState = RendererState.EMPTY;

        public TilemapRenderer(SpriteBatch _spriteBatch)
        {
            spriteBatch = _spriteBatch;
        }

        ///<summary>
        /// These are all the required variables to render a tilemap to the screen.
        ///</summary>
        public void SetLevelToRender(List<List<Rectangle>> _sourceRectangles, List<List<Rectangle>> _destinationRectangles, Texture2D _tileset)
        {
            sourceRectangles = _sourceRectangles;
            destinationRectangles = _destinationRectangles;
            tileset = _tileset;
            currentState = RendererState.LEVEL_LOADED;
        }

        public void Draw()
        {
            if (currentState == RendererState.LEVEL_LOADED)
            {
                for(int i = 0; i < sourceRectangles.Count; ++i)
                {
                    for (int j = 0; j < sourceRectangles[i].Count; ++j)
                    {
                        Rectangle destinationRectangle = destinationRectangles[i][j];
                        Rectangle sourceRectangle = sourceRectangles[i][j];
                        spriteBatch.Draw(tileset, destinationRectangles[i][j], sourceRectangles[i][j], Color.White);
                    }
                }
            }
        }
    }
}