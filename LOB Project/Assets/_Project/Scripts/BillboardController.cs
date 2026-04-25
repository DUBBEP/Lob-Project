using UnityEngine;

public class BillboardController : MonoBehaviour
{
    private Transform cameraTransform;

    void Start() => cameraTransform = Camera.main.transform;

    void LateUpdate()
    {
        if (cameraTransform != null)
            transform.rotation = Quaternion.LookRotation(cameraTransform.forward);
    }
}