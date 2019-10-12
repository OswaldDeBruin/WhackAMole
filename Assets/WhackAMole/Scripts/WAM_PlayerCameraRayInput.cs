using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OzzysWhackAMole
{
    public class WAM_PlayerCameraRayInput : MonoBehaviour
    {
        public Camera PlayerViewport;//The game's main camera
        public GameObject hitindication;

        private void Start()
        {
            if (PlayerViewport == null)//If we forgot to set the player camera
            {
                //We assume the first (active) camera we find is the game's main camera
                PlayerViewport = GameObject.FindObjectOfType<Camera>();
            }
        }

        void Update()
        {
            if (PlayerViewport != null)
            {
                //Using mouse input to detect touchscreen controls for now
                if (Input.GetMouseButtonDown(0))
                {
                    //Sending out player controls through raycast
                    RaycastHit hit;
                    var raycast = PlayerViewport.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(raycast, out hit))
                    {
                        var moleScript = hit.collider.gameObject.GetComponent<WAM_MoleScript>();
                        if (moleScript != null)
                        {
                            moleScript.MoleHammerHit();
                        }
                    }
                }
            }
        }
    }
}