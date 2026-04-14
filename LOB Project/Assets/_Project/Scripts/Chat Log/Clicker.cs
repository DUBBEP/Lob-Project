using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    [SerializeField] private Clicker player;
    [SerializeField] private float max_distance;

    private GameObject clickedObject;
    private bool isSwitched;
    private float distance;

    public SwitchCams switcher;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isSwitched) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                clickedObject = hit.collider.gameObject;
                //Debug.Log("You clicked on " + clickedObject.name);
            }
            else
            { 
                clickedObject = null;
            }


            if (clickedObject != null)
            {
                distance = Vector3.Distance(player.transform.position, clickedObject.transform.position);
            }

            //Debug.Log("Distance from " + clickedObject + ": " + distance);

            if (distance <= max_distance)
            {
                Debug.Log("You Selected " + clickedObject + " at distance " + distance);
                switcher.SwitchCameras();
                isSwitched = true;
            }
                
        }

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Tab)) && isSwitched)
        {
            switcher.SwitchCameras();
            isSwitched = false;
        }

    }
}
