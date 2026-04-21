using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Clicker : MonoBehaviour
{
    [SerializeField] private Clicker player;
    [SerializeField] private float max_distance;
    //[SerializeField] private MouseLook m_MouseLook;

    //private Camera m_Camera;

    private GameObject clickedObject;
    private bool isSwitched;
    private float distance;

    public SwitchCams switcher;

    /*
    private void Start()
    {
        m_Camera = Camera.main;
        m_MouseLook.Init(transform, m_Camera.transform);
    }
    */

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0))  && !isSwitched) // Left mouse button
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
