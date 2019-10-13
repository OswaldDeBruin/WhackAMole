using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OzzysWhackAMole
{
    //This script manages the mole going up or down and if it accepts a 'hit' of the hammer
    public class WAM_MoleScript : MonoBehaviour
    {
        enum MoleState {idle, movingUp, inUp, movingDown };//waiting states and transition states
        MoleState myState = MoleState.idle;//Current state
        public GameObject MoleObject;//The mole object to move to indicate if we can hit it
        public float TransitionTime = 0.5f;//Time length of the transition states
        public Vector3 DownLocalPosition = Vector3.zero;//Lower position of the mole object
        public Vector3 UpLocalPosition = Vector3.zero;//Upper position of the mole object
        public WAM_GameManager myGameManager;//Game manager to communicate to if the mole has been hit
        float stateStartTime = 0;//Time of the first frame of a state
        public AudioSource OnHitAudioSource;//Sound to play when hit
        public ParticleSystem OnHitParticleSystem;//Particle to play when mole is hit

        //This function sets the position of the mole object
        void MoveMole(Vector3 position)
        {
            if (MoleObject != null)
            {
                MoleObject.transform.localPosition = position;
            }
        }

        //public function for the game manager to manage set the mole's position to up
        public void MoveMoleUp()
        {
            //Only available when moving to and in down position
            if (myState == MoleState.idle || myState == MoleState.movingDown)
            {
                //Start transition state
                myState = MoleState.movingUp;
                stateStartTime = Time.time;
            }
        }

        //public function for the game manager to manage set the mole's position to down
        public void MoveMoleDown()
        {
            //Only available when moving to and in up position
            if (myState == MoleState.inUp || myState == MoleState.movingUp)
            {
                //Start transition state
                myState = MoleState.movingDown;
                stateStartTime = Time.time;
            }
        }

        //function for when we want to register a valid hit in the game manager
        private void CommunicateScoreHit()
        {
            if (myGameManager != null)
            {
                myGameManager.RegisterScoreHit();
            }
        }

        //Public function for the player input when hitting a mole
        public void MoleHammerHit()
        {
            //Only accept a hit when we are in or moving to the up position
            if (myState== MoleState.inUp || myState == MoleState.movingUp)
            {
                CommunicateScoreHit();
                if (OnHitAudioSource != null) OnHitAudioSource.Play();
                if (OnHitParticleSystem != null) OnHitParticleSystem.Play();
                MoveMoleDown();
            }
        }

        void Start()
        {
            myState = MoleState.idle;//init state
            MoveMole(DownLocalPosition);//init position
        }

        void Update()
        {
            //Handling states
            switch (myState)
            {
                case MoleState.movingUp:
                    //Move mole up
                    MoveMole(
                        Vector3.Lerp(
                            DownLocalPosition,
                            UpLocalPosition, 
                            (Time.time-stateStartTime)/TransitionTime
                        )
                    );
                    break;
                case MoleState.inUp:
                    //keep mole in the up position
                    MoveMole(UpLocalPosition);
                    break;
                case MoleState.movingDown:
                    //move mole down
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
    }
}