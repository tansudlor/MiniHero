using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;


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

public struct int2
{
    public int x;
    public int y;

    public int2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class GameController
{
    public static GameController Instance { get; private set; }
    public List<int2> UsedArea = new List<int2>();
    public List<int2> UnuseArea = new List<int2>();
    public Dictionary<int2, HashMapData> HashMap = new Dictionary<int2, HashMapData>();
    public GridSystem gridSystem;
    public GameUIController gameUIController;
    public List<Hero> HeroList = new List<Hero>();
    public List<GameObject> EnemySpawnList = new List<GameObject>();
    public List<GameObject> HeroSpawnList = new List<GameObject>();
    public bool isOver = false;

    public int HeroCount = 1;
    public int EnemyCount = 0;
    public int EnemySpawnMin = 1;
    public int EnemySpawnMax = 3;
    public int HeroSpawnMin = 1;
    public int HeroSpawnMax = 2;

    private int healthMinStat = 5;
    private int healthMaxStat = 11;
    private int attackMinStat = 5;
    private int attackMaxStat = 11;
    private int defenseMinStat = 2;
    private int defenseMaxStat = 5;

    public static GameController GetInstance()
    {
        if (Instance == null)
        {
            Instance = new GameController();
        }
        return Instance;
    }

    public (bool isStore, bool isEnemy) CheckCellContain(int2 pos) //For Check that player move to is Contain Any Character
    {
        bool isStore = false;
        bool isEnemy = false;
        if (UsedArea.Contains(pos))
        {
            isStore = true;
            if (HashMap[pos].characterData is Enemy)
            {
                isEnemy = true;
            }

            else if (HashMap[pos].characterData is Hero)
            {
                if (HeroList.Contains(HashMap[pos].characterData))
                {

                    gameUIController.GameOVer();
                   
                }
            }

        }
        return (isStore, isEnemy);
    }

    public void StoreCharacter(int2 pos, GameObject gameObject, Character character) //For Store Character that spawn or move
    {
        UsedArea.Add(pos);
        UnuseArea.Remove(pos);
        HashMap[pos] = new HashMapData(gameObject, character);
    }

    public void RemovePositionCharacter(int2 pos)//For Remove Character that collect or die
    {
        UsedArea.Remove(pos);
        HashMap.Remove(pos);
        UnuseArea.Add(pos);
    }

    public Vector2 Int2ToVector2(int2 pos) //Convert int2 to vector2
    {
        Vector2 vector2 = new Vector2(pos.x, pos.y);
        return vector2;
    }

    public int2 Vector2ToInt2(Vector2 pos)//Convert vector2 to int2
    {
        int x = (gridSystem.GridWidth + ((int)pos.x % gridSystem.GridWidth)) % gridSystem.GridWidth;
        int y = (gridSystem.GridHeight + ((int)pos.y % gridSystem.GridWidth)) % gridSystem.GridHeight;
        int2 int2 = new int2(x, y);
        return int2;
       
    }

    public void SpawnEnemy() //For Spawn new Enemy and set config stat
    {
        EnemyCount++;
        var randomPos = RandomPosition();
        int randomIndexEnemy = Random.Range(0, EnemySpawnList.Count);
        GameObject randomEnemy = EnemySpawnList[randomIndexEnemy];
        GameObject enemyClone = GameObject.Instantiate(randomEnemy, Int2ToVector2(randomPos), Quaternion.identity);
        Character character = enemyClone.GetComponent<Character>();
        ((Enemy)character).NumberSpawn = EnemyCount + "";
        character.health = RandomStat(healthMinStat, healthMaxStat);
        character.attack = RandomStat(attackMinStat, attackMaxStat);
        character.defense = RandomStat(defenseMinStat, defenseMaxStat);
        StoreCharacter(randomPos, enemyClone, character);
        gameUIController.AddEnemyStat((Enemy)character);
    }

    public GameObject SpawnHero() //For Spawn new Hero and set config stat
    {
        var randomPos = RandomPosition();
        int randomIndexEnemy = Random.Range(0, HeroSpawnList.Count);
        GameObject randomHero = HeroSpawnList[randomIndexEnemy];
        GameObject heroClone = GameObject.Instantiate(randomHero, Int2ToVector2(randomPos), Quaternion.identity);
        Character character = heroClone.GetComponent<Character>();
        character.health = RandomStat(attackMinStat, attackMaxStat);
        character.attack = RandomStat(attackMinStat, attackMaxStat);
        character.defense = RandomStat(defenseMinStat, defenseMaxStat);
        StoreCharacter(randomPos, heroClone, character);
        return heroClone;
    }



    public (string character, bool isFinish) StartBattle(HashMapData enemyHashMapData, Character hero, int2 pos) //When Head Hero move to cell that have Enemy
    {
        Character enemy = enemyHashMapData.characterData;
        GameObject enemyGameObject = enemyHashMapData.goData;

        int damageToMonster = hero.attack - enemy.defense;
        int damageToHero = enemy.attack - hero.defense;

        int heroHealth = hero.health;
        int enemyHealth = enemy.health;

        if (damageToHero > 0)
        {
            heroHealth = hero.health - damageToHero;
        }

        if (damageToMonster > 0)
        {
            enemyHealth = enemy.health - damageToMonster;
        }

        enemy.health = enemyHealth;
        hero.gameObject.GetComponent<Character>().health = heroHealth;


        if (enemy.health <= 0 && hero.gameObject.GetComponent<Character>().health <= 0)
        {
            RemovePositionCharacter(Vector2ToInt2(enemyGameObject.transform.position));
            GameObject.Destroy(enemyGameObject);
            hero.gameObject.GetComponent<Character>().health = heroHealth;
            SpawnManyEnemy();
            return ("all", false);
        }


        if (enemy.health <= 0)
        {
            RemovePositionCharacter(Vector2ToInt2(enemyGameObject.transform.position));
            GameObject.Destroy(enemyGameObject);
            hero.gameObject.GetComponent<Character>().health = heroHealth;
            SpawnManyEnemy();
            return ("enemy", false);

        }

        else if (hero.gameObject.GetComponent<Character>().health <= 0)
        {
            return ("hero", false);
        }

        else if (enemy.health > 0)
        {
            return ("enemyNotDie", true);
        }

        return ("noone", false);
    }

    public void SpawnManyEnemy()
    {
        int randomNum = RandomStat(EnemySpawnMin, EnemySpawnMax);
        for (int i = 0; i < randomNum; i++)
        {
            SpawnEnemy();
        }

    }

    public void SpawnManyHero()
    {
        int randomNum = RandomStat(HeroSpawnMin, HeroSpawnMax);
        for (int i = 0; i < randomNum; i++)
        {
            SpawnHero();
        }

    }

    public void CollectHero(int2 pos) //When Player Collect Collectable Hero that spawn on Map
    {
        HeroCount++;
        //add to herolist
        Hero hero = (Hero)HashMap[pos].characterData;
        hero.HeroName = HeroCount + " :";
        hero.State = HeroState.GROUPMOVE;
        hero.CurrentPosition = HeroList[0].CurrentPosition;
        hero.FinishPosition = HeroList[HeroList.Count - 1].PreviousPosition;
        hero.AddToGroup();
        HeroList.Add(hero);
        HashMap[pos].goData.name = HeroList.Count + "";
        RemovePositionCharacter(pos);
        StoreCharacter(Vector2ToInt2(hero.FinishPosition), hero.gameObject, hero);
        gameUIController.AddHeroStat(hero);
        //spawn new hero
        SpawnManyHero();
    }

    int2 RandomPosition() 
    {
        int randomIndexPos = Random.Range(0, UnuseArea.Count);
        int2 randomPos = UnuseArea[randomIndexPos];
        return randomPos;
    }

    int RandomStat(int min, int max)
    {
        return Random.Range(min, max);
    }
}
