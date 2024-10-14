using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using minihero.gameconfig;

namespace minihero.gamestructer
{
    public class InvertStructer 
    {
        public static InvertStructer Instance { get; private set; }
        public static InvertStructer GetInstance()
        {
            if (Instance == null)
            {
                Instance = new InvertStructer();
            }
            return Instance;
        }

        // Start is called before the first frame update
        public Vector2 Int2ToVector2(int2 pos) //Convert int2 to vector2
        {
            Vector2 vector2 = new Vector2(pos.x, pos.y);
            return vector2;
        }

        public int2 Vector2ToInt2(Vector2 pos)//Convert vector2 to int2
        {
            int x = (GridSize.GridWidth + ((int)pos.x % GridSize.GridWidth)) % GridSize.GridWidth;
            int y = (GridSize.GridHeight + ((int)pos.y % GridSize.GridWidth)) % GridSize.GridHeight;
            int2 int2 = new int2(x, y);
            return int2;

        }
    }
}
