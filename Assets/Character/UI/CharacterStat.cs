using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using minihero.character;

namespace minihero.character.ui
{
    public class CharacterStat : MonoBehaviour
    {
        public TextMeshProUGUI Health;
        public TextMeshProUGUI Attack;
        public TextMeshProUGUI Defense;
        public TextMeshProUGUI HeroName;
        public int HealthStat;
        public int AttackStat;
        public int DefenseStat;
        public Hero hero;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (hero.health <= 0)
            {
                Destroy(gameObject);
            }

            HeroName.text = hero.HeroName;
            Health.text = hero.health.ToString();
            Attack.text = hero.attack.ToString();
            Defense.text = hero.defense.ToString();
        }
    }
}