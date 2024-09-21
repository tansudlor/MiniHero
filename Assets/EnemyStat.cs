using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Attack;
    public TextMeshProUGUI Defense;
    public TextMeshProUGUI NameText;
    public int HealthStat;
    public int AttackStat;
    public int DefenseStat;
    public Enemy enemy;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.health <= 0)
        {
            Destroy(gameObject);
        }

        NameText.text = enemy.NumberSpawn.ToString() + " :";
        Health.text = enemy.health.ToString();
        Attack.text = enemy.attack.ToString();
        Defense.text = enemy.defense.ToString();
    }
}
