using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace minihero.gameconfig
{
    public class GridSize
    {
        private static int gridWidth = 16;
        private static int gridHeight = 16;

        public static int GridWidth { get => gridWidth; private set => gridWidth = value; }
        public static int GridHeight { get => gridHeight; private set => gridHeight = value; }
    }
}

