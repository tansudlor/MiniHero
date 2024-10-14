using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace minihero.character
{
    public class Witch : Enemy
    {
        // Start is called before the first frame update
        void Start()
        {
            health = 2;
            attack = 50;
            defense = 100;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
