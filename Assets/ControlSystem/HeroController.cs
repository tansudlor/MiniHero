using minihero.character;
using minihero.game;
using minihero.gamestructer;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace minihero.controller
{

    public class HeroController : MonoBehaviour
    {
        private GameController gameController;
        private Vector3 currentDirection = Vector3.zero;
        private GameObject heroControl = null;
        // Start is called before the first frame update
        void Start()
        {

            gameController = GameController.GetInstance();
            StartCoroutine(WaitForFirstHero());
        }

        IEnumerator WaitForFirstHero()
        {
            yield return new WaitUntil(() => gameController.FirstHero != null);
            heroControl = gameController.FirstHero;
            heroControl.name = "FirstHero";
            Debug.Log(heroControl);
        }

        // Update is called once per frame
        void Update()
        {

            if (heroControl == null)
            {
                return;
            }

            bool CanMove = true;

            for (int i = 0; i < gameController.HeroList.Count; i++)
            {

                if (gameController.HeroList[i].State == HeroState.ALONEMOVE || gameController.HeroList[i].State == HeroState.GROUPMOVE)
                {
                    CanMove = false;
                    break;
                }
            }


            if (!CanMove)
            {
                return;
            }

            if (gameController.isOver)
            {
                return;
            }



            if (Input.GetAxisRaw("Vertical") > 0 && currentDirection != Vector3.down)
            {
                MoveToPosition(Vector3.up);
            }
            else if (Input.GetAxisRaw("Vertical") < 0 && currentDirection != Vector3.up)
            {
                MoveToPosition(Vector3.down);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0 && currentDirection != Vector3.right)
            {
                MoveToPosition(Vector3.left);
            }
            else if (Input.GetAxisRaw("Horizontal") > 0 && currentDirection != Vector3.left)
            {
                MoveToPosition(Vector3.right);
            }

        }


        void MoveToPosition(Vector3 direction)
        {
            currentDirection = direction;
            var hero = heroControl.GetComponent<Hero>();
            hero.PreviousPosition = hero.CurrentPosition;
            gameController.RemovePositionCharacter(InvertStructer.Instance.Vector2ToInt2(hero.PreviousPosition));
            int2 spatialPos = InvertStructer.Instance.Vector2ToInt2(hero.CurrentPosition + direction);
            MoveLine();
            var isContain = gameController.CheckCellContain(spatialPos);
            if (isContain.isStore)
            {
                if (isContain.isEnemy)
                {
                    //Attack Enemy
                    bool isFinsh = true;
                    string battleEnd = "";
                    //Fight Unitill any Character die
                    while (isFinsh)
                    {
                        var battleResult = gameController.StartBattle(gameController.HashMap[spatialPos], hero, spatialPos);
                        isFinsh = battleResult.isFinish;
                        battleEnd = battleResult.character;
                    }
                    if (battleEnd == "enemy")
                    {
                        heroControl.GetComponent<Hero>().FinishPosition = hero.CurrentPosition + direction;
                        heroControl.GetComponent<Hero>().GroupMove();
                        gameController.StoreCharacter(spatialPos, heroControl, heroControl.GetComponent<Character>());
                    }
                    else if (battleEnd == "hero")
                    {
                        SwapHeroControl();
                    }
                    else if (battleEnd == "all")
                    {
                        SwapHeroControl();
                    }

                }
                else //if cell contain Spawn Hero 
                {

                    heroControl.GetComponent<Hero>().FinishPosition = hero.CurrentPosition + direction;
                    heroControl.GetComponent<Hero>().GroupMove();
                    gameController.CollectHero(spatialPos);
                    gameController.StoreCharacter(spatialPos, heroControl, heroControl.GetComponent<Character>());
                }
            }
            else //Cell not contain any Character
            {
                heroControl.GetComponent<Hero>().FinishPosition = hero.CurrentPosition + direction;
                heroControl.GetComponent<Hero>().GroupMove();
                gameController.StoreCharacter(spatialPos, heroControl, heroControl.GetComponent<Character>());
            }

        }

        //Move Hero that not heroControl
        public void MoveLine()
        {

            for (int i = 1; i < gameController.HeroList.Count; i++)
            {
                var prevHero = gameController.HeroList[i - 1];
                var currentHero = gameController.HeroList[i];
                var prevHeroPos = prevHero.PreviousPosition;
                currentHero.PreviousPosition = currentHero.CurrentPosition;
                currentHero.FinishPosition = prevHeroPos;
                currentHero.GroupMove();
                gameController.RemovePositionCharacter(InvertStructer.Instance.Vector2ToInt2(currentHero.PreviousPosition));
                gameController.StoreCharacter(InvertStructer.Instance.Vector2ToInt2(currentHero.FinishPosition), currentHero.gameObject, currentHero);
            }

        }

        //Swap heroControl to next hero in Herolist
        void SwapHeroControl()
        {
            gameController.HeroList.Remove(heroControl.GetComponent<Hero>());
            Destroy(heroControl);

            if (gameController.HeroList.Count == 0)
            {

                gameController.gameUIController.GameOVer();
                return;
            }

            heroControl = gameController.HeroList[0].gameObject;
            heroControl.GetComponent<Hero>().Crown.gameObject.SetActive(true);
        }
    }
}
