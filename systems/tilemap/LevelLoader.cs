using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;

namespace MonoRPG
{
    
    public class LevelLoader
    {
        TilemapRenderer tilemapRenderer;
        TilemapEntityLoader entityLoader;
        MapGrid grid;
        EntityManager entityManager;
        

        public LevelLoader(TilemapRenderer _tilemapRenderer, TilemapEntityLoader _entityLoader, MapGrid _grid, EntityManager _entityManager)
        {
            tilemapRenderer = _tilemapRenderer;
            entityLoader = _entityLoader;
            grid = _grid;
            entityManager = _entityManager;
        }

        /*
        TODO:
            At the moment this is functional, however it will be more convenient to have a static SceneManager class that can just load levels by just typing in the
            scene name and it will go. This will probably involve structs called scenes that are are stored in a dictionary for easy access.
        */

        ///<summary>
        /// Load the level by giving the string path to the json file and then giving the texture of the the required tileset
        ///</summary>
        public void LoadLevel(string _levelPath, Texture2D _tileset)
        {
            Level level = new Level();
            List<Level.TilemapLayer> tilemapLayers = new List<Level.TilemapLayer>();
            List<Level.TilemapLayer> entityLayers = new List<Level.TilemapLayer>();
            List<List<Rectangle>> sourceRectangles = new List<List<Rectangle>>();
            List<List<Rectangle>> destinationRectangles = new List<List<Rectangle>>();

            using (StreamReader r = new StreamReader(_levelPath))
            {
                string levelPath = r.ReadToEnd();
                level = JsonConvert.DeserializeObject<Level>(levelPath);
                r.Close();
            }

            grid.FormGrid(level.width / 16, level.height / 16);  // TODO: remove the 16 and set it as a variable loaded from json

            SetTileMapAndEntityLayers(level, tilemapLayers, entityLayers);

            SetSourceAndDestinationRectangles(tilemapLayers, sourceRectangles, destinationRectangles);
            
            tilemapRenderer.SetLevelToRender(sourceRectangles, destinationRectangles, _tileset);

            LoadEntities(entityLayers);

            entityManager.RunPostInitialzation();
        }

        ///<summary>
        /// Seperate entity layers and tilemap layers by checking if datacords2d or entities list is null
        ///</summary>
        private void SetTileMapAndEntityLayers(Level _level, List<Level.TilemapLayer> _tilemapLayers, List<Level.TilemapLayer> _entityLayers)
        {
            foreach (Level.TilemapLayer layer in _level.layers)
            {
                if (layer.dataCoords2D != null)
                {
                    _tilemapLayers.Add(layer);
                }
                if (layer.entities != null)
                {
                    _entityLayers.Add(layer);
                }
            }
        }

        ///<summary>
        /// Set the source and desitination rectangles lists by looping through the datacoords2d lists.
        ///</summary>
        private void SetSourceAndDestinationRectangles(List<Level.TilemapLayer> _tilemapLayers, List<List<Rectangle>> _sourceRectangles, List<List<Rectangle>> _destinationRectangles)
        {
            int currentLayer = 0;

            foreach (Level.TilemapLayer layer in _tilemapLayers)
            {
                _sourceRectangles.Add(new List<Rectangle>());
                _destinationRectangles.Add(new List<Rectangle>());
                
                for (int i = 0; i < layer.dataCoords2D.Count; ++i)
                {
                    for (int j = 0; j < layer.dataCoords2D[i].Count; ++j)
                    {
                        int destinationX = 0;
                        int destinationY = 0;
                        int sourceX = 0;
                        int sourceY = 0;

                        for (int k = 0; k < layer.dataCoords2D[i][j].Count; ++k)
                        {
                            if (layer.dataCoords2D[i][j].Count > 1) // It is not a null square, so fill the desitination rects
                            {
                                if (k == 0)
                                {
                                    sourceX = layer.dataCoords2D[i][j][k] * 16;
                                }
                                if (k == 1)
                                {
                                    sourceY = layer.dataCoords2D[i][j][k] * 16;
                                }

                                destinationX = j * 16;
                                destinationY = i * 16;
                            }
                        }

                        if (layer.dataCoords2D[i][j].Count > 1)
                        {
                            _sourceRectangles[currentLayer].Add(new Rectangle(sourceX, sourceY, 16, 16));
                            _destinationRectangles[currentLayer].Add(new Rectangle(destinationX, destinationY, 16, 16));

                            if (layer.name == "Walls")
                            {
                                grid.SetGridNodeWalkablity(j, i, false);
                            }
                            if (layer.name == "Floor")
                            {
                                grid.SetGridNodeWalkablity(j, i, true);
                            }
                            // TODO: There is eventually be a collision layer for objects that aren't walls but still can't walk thru
                        }
                    }
                }

                currentLayer++;
            }
        }

        ///<summary>
        /// Load all entities by calling the LoadEntity method in the EntityLoader class while looping through each entity layer
        ///</summary>
        private void LoadEntities(List<Level.TilemapLayer> _entityLayers)
        {
            foreach(Level.TilemapLayer layer in _entityLayers)
            {
                foreach(Level.LevelEntity entity in layer.entities)
                {
                    entityLoader.LoadEntity(entity.name, new Vector2(entity.x, entity.y), entity.values);
                }
            }
        }
    }
}