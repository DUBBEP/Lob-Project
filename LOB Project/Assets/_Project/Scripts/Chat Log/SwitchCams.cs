using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCams : MonoBehaviour
{
    [SerializeField] private Camera player_camera;
    [SerializeField] private Camera computer_camera;
    [SerializeField] private SelectionManager selection_manager;
    [SerializeField] private Canvas computer_canvas;

    //private bool currentState;

    public void SwitchCameras()
    {
        // Toggle the enabled state of both cameras
        player_camera.enabled = !player_camera.enabled;
        computer_camera.enabled = !computer_camera.enabled;
        selection_manager.enabled = !selection_manager.enabled;
        computer_canvas.enabled = !computer_canvas.enabled;
    }


}
