using System.Collections.Generic;
using UnityEngine;
using minihero.gamestructer;
using minihero.character;
using minihero.game;
using minihero.gameconfig;

namespace minihero.grid
{

    public class GridSystem : MonoBehaviour
    {


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
            UnuseArea = gameController.UnuseArea;
            UsedArea = gameController.UsedArea;
            gameController.EnemySpawnList = EnemySpawnList;
            gameController.HeroSpawnList = HeroSpawnList;
            //Start Create Grid
            for (int i = 0; i < GridSize.GridWidth; i++)
            {
                for (int j = 0; j < GridSize.GridHeight; j++)
                {
                    int x = i;
                    int y = j;
                    Vector3 pos = new Vector3(x, y, 0);
                    int2 int2 = InvertStructer.Instance.Vector2ToInt2(pos);
                    //add to UnuseArea
                    UnuseArea.Add(int2);
                    Instantiate(Map, pos, Quaternion.identity, MapParent.transform);
                }
            }
            //Set Camera
            Camera.gameObject.transform.position = new Vector3((GridSize.GridWidth / 2), (GridSize.GridHeight / 2) - 1f, -10);
            Camera.orthographicSize = (GridSize.GridWidth / 2) + 1.5f;

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
            int2 int2Pos = InvertStructer.Instance.Vector2ToInt2(FirstHero.transform.position);
            gameController.RemovePositionCharacter(int2Pos);
            gameController.StoreCharacter(new int2(0, 0), FirstHero, heroClass);
            gameController.FirstHero = FirstHero;
            //Spawn Heros
            gameController.SpawnHero();


        }

    }
}