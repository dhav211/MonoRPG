using System.Collections.Generic;

namespace MonoRPG
{
    public class Level
    {
        public int width {get; set;}
        public int height {get; set;}
        public int offsetX {get; set;}
        public int offsetY {get; set;}
        public TilemapLayer[] layers { get; set; }

        public class TilemapLayer
        {
            public string name { get; set; }
            public int _eid { get; set; }
            public int offsetX { get; set; }
            public int offsetY { get; set; }
            public int gridCellWidth { get; set; }
            public int gridCellHeight { get; set; }
            public int gridCellsX { get; set; }
            public int gridCellsY { get; set; }
            public string tileset { get; set; }
            public List<List<List<int>>> dataCoords2D { get; set; }
            public List<LevelEntity> entities { get; set; }
        }

        public class LevelEntity
        {
            public string name { get; set; }
            public int id { get; set; }
            public int _eid { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int originX { get; set; }
            public int originY { get; set; }
            public LevelEntityValues values { get; set; }
        }

        public class LevelEntityValues
        {
            //Chests
            public bool isLocked { get; set; }
            public int chest_id { get; set; }
            public string keyRequired { get; set; }
        }
    }
}