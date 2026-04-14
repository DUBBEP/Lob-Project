using UnityEngine;

public struct ObjectStartState
{
    public Transform selectabe;
    public Vector3 position;
    public Quaternion rotation;
    public ObjectStartState(Transform selectable, Vector3 position, Quaternion rotation)
    {
        this.selectabe = selectable;
        this.position = position;
        this.rotation = rotation;
    }

    public void ResetState()
    {
        selectabe.position = position;
        selectabe.rotation = rotation;
    }
}