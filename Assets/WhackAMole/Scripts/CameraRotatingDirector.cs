using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WAM_Utils
{
    public class CameraRotatingDirector : MonoBehaviour
    {
        //States to track camera in, boolean would suffice, but enum is more expandable
        enum CameraState { Idle, Rotating};
        CameraState myState = CameraState.Idle;
        Quaternion StartRotation = new Quaternion();
        Quaternion EndRotation = new Quaternion();
        float StartTime = 0;
        public float RotateTime = 0.75f;
        public AnimationCurve RotateCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public void InstantRotateTo(Vector3 eulerAngle)
        {
            myState = CameraState.Idle;
            transform.rotation = Quaternion.Euler(eulerAngle);
        }

        public void RotateTo(Vector3 eulerAngle)
        {
            StartRotation = transform.rotation;
            EndRotation = Quaternion.Euler(eulerAngle);
            StartTime = Time.time;
            myState = CameraState.Rotating;
        }

        float PassedTimeInState()
        {
            return (Time.time - StartTime);
        }

        void Update()
        {
            if (myState == CameraState.Rotating)
            {
                if (PassedTimeInState() >= RotateTime)
                {
                    transform.rotation = EndRotation;
                    myState = CameraState.Idle;
                }
                else
                {
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