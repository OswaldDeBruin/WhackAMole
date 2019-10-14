using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WAM_Utils
{
    public class CameraRotatingDirector : MonoBehaviour
    {
        //States to track camera in, boolean would suffice, but enum is more expandable
        enum CameraState { Idle, Rotating};
        CameraState myState = CameraState.Idle;//Initial state of the camera is waiting in position
        Quaternion StartRotation = new Quaternion();//angle at beginning of rotation
        Quaternion EndRotation = new Quaternion();//angle at end of rotation
        float StartTime = 0;//Start moment of rotation in Time.time
        public float RotateTime = 0.75f;//Time it takes for the camera to move
        public AnimationCurve RotateCurve = AnimationCurve.Linear(0, 0, 1, 1);//Easing curve for movement must start at 0 and end at 1

        //Unanimated rotation function
        public void InstantRotateTo(Vector3 eulerAngle)
        {
            myState = CameraState.Idle;
            transform.rotation = Quaternion.Euler(eulerAngle);
        }

        //Animated rotation
        public void RotateTo(Vector3 eulerAngle)
        {
            StartRotation = transform.rotation;//Move from current angle (even within a previous rotation
            EndRotation = Quaternion.Euler(eulerAngle);//To the given angle
            StartTime = Time.time;//Calculate from now
            myState = CameraState.Rotating;//Note that we are now in a rotating state
        }

        //function to shorten time calculation
        float PassedTimeInState()
        {
            return (Time.time - StartTime);
        }

        void Update()
        {
            if (myState == CameraState.Rotating)
            {
                if (PassedTimeInState() >= RotateTime)//If we reach the end of our rotation animation
                {
                    //we lock the camera in its end-state, in case we over/under shoot or the curve does not end at 1
                    transform.rotation = EndRotation;
                    myState = CameraState.Idle;//Note we are not rotating anymore
                }
                else
                {
                    //Standard lerp between start and end state
                    transform.rotation = Quaternion.Lerp(
                        StartRotation,
                        EndRotation,
                        RotateCurve.Evaluate(PassedTimeInState() / RotateTime)
                    );
                }
            }
        }
    }
}