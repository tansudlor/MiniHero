using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public int GridWidth = 16;
    public int GridHeight = 16;
    
    public Camera Camera;
    public GameObject Map;
    public GameObject MapParent;
    public List<int2> UsedArea = new List<int2>();
    public List<int2> UnuseArea = new List<int2>();
    public List<GameObject> EnemySpawnList = new List<GameObject>();
    public List<GameObject> HeroSpawnList = new List<GameObject>();
    public GameObject FirstHero = null;

    private GameController gameController;
    
    void Start()
    {
        gameController = GameController.GetInstance();
        gameController.gridSystem = this;
        UnuseArea = gameController.UnuseArea;
        UsedArea = gameController.UsedArea;
        gameController.EnemySpawnList = EnemySpawnList;
        gameController.HeroSpawnList = HeroSpawnList;
        //Start Create Grid
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                int x = i;
                int y = j;
                Vector3 pos = new Vector3(x, y, 0);
                int2 int2 = gameController.Vector2ToInt2(pos);
                //add to UnuseArea
                UnuseArea.Add(int2);
                Instantiate(Map, pos, Quaternion.identity, MapParent.transform);
            }
        }
        //Set Camera
        Camera.gameObject.transform.position = new Vector3((GridWidth / 2) , (GridHeight / 2) - 1f, -10);
        Camera.orthographicSize = (GridWidth / 2) + 1.5f;

        //Spawn Enemys
        gameController.SpawnEnemy();

        //Spawn FirstHero
        FirstHero = gameController.SpawnHero();
        gameController.HeroList.Add(FirstHero.GetComponent<Hero>());
        var heroClass = FirstHero.GetComponent<Hero>();
        heroClass.FinishPosition = Vector3.zero;
        heroClass.CurrentPosition = heroClass.FinishPosition;
        heroClass.HeroName = "1 :";
        heroClass.Crown.gameObject.SetActive(true);
        heroClass.State = HeroState.GROUPMOVE;
        gameController.gameUIController.AddHeroStat(heroClass);
        int2 int2Pos = gameController.Vector2ToInt2(FirstHero.transform.position);
        gameController.RemovePositionCharacter(int2Pos);
        gameController.StoreCharacter(new int2(0,0), FirstHero, heroClass);

        //Spawn Heros
        gameController.SpawnHero();


    }



}
