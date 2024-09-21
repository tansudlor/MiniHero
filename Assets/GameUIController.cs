using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    public GameObject GameOver;
    public GameObject HeroContainer;
    public GameObject EnemyContainer;
    public GameObject HeroStat;
    public GameObject EnemyStat;
    private GameController gameController;
    // Start is called before the first frame update
    public void Awake()
    {
        gameController = GameController.GetInstance();
        gameController.gameUIController = this;
    }
    //For Add Hero Stat that collect to HeroLine ScrollView
    public void AddHeroStat(Hero hero)
    {
        var charStatUI = Instantiate(HeroStat, HeroContainer.transform);
        var charStat = charStatUI.GetComponent<CharacterStat>();
        charStat.hero = hero;
        charStat.HealthStat = 0;
        charStat.AttackStat = 0;
        charStat.DefenseStat = 0;
    }
    //For Add Enemy Stat that spawn on Map  to ScrollView
    public void AddEnemyStat(Enemy enemy)
    {
        var charStatUI = Instantiate(EnemyStat, EnemyContainer.transform);
        var charStat = charStatUI.GetComponent<EnemyStat>();
        charStat.enemy = enemy;
        charStat.HealthStat = 0;
        charStat.AttackStat = 0;
        charStat.DefenseStat = 0;
    }

    //For Display GameOver UI
    public void GameOVer()
    {
        GameOver.SetActive(true);
        gameController.isOver = true;
        Time.timeScale = 0f;
    }
}
