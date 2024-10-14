using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using minihero.gamestructer;


namespace minihero.character
{
    public enum HeroState
    {
        ALONE,
        GROUP,
        ALONEMOVE,
        GROUPMOVE
    }

    public class Hero : Character
    {
        public Vector3 FinishPosition;
        public Vector3 PreviousPosition;
        public HeroState State = HeroState.ALONE;
        public Vector3 CurrentPosition;
        public SpriteRenderer Crown;
        public string HeroName = "";
        private Vector3 movePerSec = Vector2.zero;
        private float moveDuration = 0;
        private float aloneMoveSpeed = 25;
        private float groupMovespeed = 5;
        // Start is called before the first frame update

        private void Update()
        {
            //To update of hero that attach to this script
            if (State == HeroState.ALONE)
            {
                return;
            }
            if (State == HeroState.ALONEMOVE)
            {
                if (moveDuration > 0)
                {
                    CurrentPosition += movePerSec * Time.deltaTime;
                    moveDuration -= Time.deltaTime;
                }
                else
                {

                    State = HeroState.GROUP;
                }

            }

            if (State == HeroState.GROUPMOVE)
            {
                if (moveDuration > 0)
                {
                    CurrentPosition += movePerSec * Time.deltaTime;
                    moveDuration -= Time.deltaTime;
                }
                else
                {

                    State = HeroState.GROUP;
                }
            }

            if (State == HeroState.GROUP)
            {
                CurrentPosition = FinishPosition;
            }


            int2 pos = InvertStructer.Instance.Vector2ToInt2(CurrentPosition);
            //Move Gameobject to position that should move to 
            gameObject.transform.position = new Vector3(pos.x + (CurrentPosition.x - (int)CurrentPosition.x), pos.y + (CurrentPosition.y - (int)CurrentPosition.y), 0);



        }

        //For Move Position of collect hero to end of Line
        public void AddToGroup()
        {
            State = HeroState.ALONEMOVE;
            movePerSec = (FinishPosition - CurrentPosition).normalized * aloneMoveSpeed;
            moveDuration = (FinishPosition - CurrentPosition).magnitude / movePerSec.magnitude;
        }

        //For Move Position of HeroLine
        public void GroupMove()
        {

            State = HeroState.GROUPMOVE;
            movePerSec = (FinishPosition - CurrentPosition).normalized * groupMovespeed;
            moveDuration = (FinishPosition - CurrentPosition).magnitude / movePerSec.magnitude;

        }


    }

}
