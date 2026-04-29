using UnityEngine;

public class MouseScreenRayProvider : MonoBehaviour, IRayProvider
{
    public Ray CreateRay()
    {
        Debug.Log("Main Camera is " + Camera.main);
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}