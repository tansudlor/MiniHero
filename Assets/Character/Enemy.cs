using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace minihero.character
{
    public class Enemy : Character
    {
        public TextMeshPro NumberText;
        public string NumberSpawn;
        // Start is called before the first frame update
        void Start()
        {
            //For set order number spawn 
            NumberText.text = NumberSpawn;
        }

    }
}
