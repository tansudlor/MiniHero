using minihero.character;
using UnityEngine;

namespace minihero.game
{
    public struct HashMapData
    {
        public GameObject goData;
        public Character characterData;

        public HashMapData(GameObject goData, Character characterData)
        {
            this.goData = goData;
            this.characterData = characterData;
        }
    }
}
