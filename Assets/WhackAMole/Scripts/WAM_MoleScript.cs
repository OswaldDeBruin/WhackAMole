using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OzzysWhackAMole
{
    public class WAM_MoleScript : MonoBehaviour
    {
        enum MoleState {idle, movingUp, inUp, movingDown };
        MoleState myState = MoleState.idle;
        public GameObject MoleObject;
        public float TransitionTime = 0.5f;
        public Vector3 DownLocalPosition = Vector3.zero;
        public Vector3 UpLocalPosition = Vector3.zero;

        float stateStartTime = 0;

        void Start()
        {
            myState = MoleState.idle;
            MoveMole(DownLocalPosition);
        }

        void MoveMole(Vector3 position)
        {
            if (MoleObject != null)
            {
                MoleObject.transform.localPosition = position;
            }
        }

        

        void Update()
        {
            switch (myState)
            {
                case MoleState.movingUp:
                    MoveMole(
                        Vector3.Lerp(
                            DownLocalPosition,
                            UpLocalPosition, 
                            (Time.time-stateStartTime)/TransitionTime
                        )
                    );
                    break;
                case MoleState.inUp:
                    MoveMole(UpLocalPosition);
                    break;
                case MoleState.movingDown:
                    MoveMole(
                        Vector3.Lerp(
                            UpLocalPosition,
                            DownLocalPosition,
                            (Time.time - stateStartTime) / TransitionTime
                        )
                    );
                    break;
                default:
                    break;
            }
        }

        public void MoveMoleUp()
        {
            if (myState == MoleState.idle || myState == MoleState.movingDown)
            {
                myState = MoleState.movingUp;
                stateStartTime = Time.time;
            }
        }

        public void MoveMoleDown()
        {
            if (myState == MoleState.inUp || myState == MoleState.movingUp)
            {
                myState = MoleState.movingDown;
                stateStartTime = Time.time;
            }
        }

        public void MoleHammerHit()
        {
            if (myState!= MoleState.idle)
            {
                //Score
                MoveMoleDown();
            }
        }
    }
}