using UnityEngine;

public interface ISelector
{
    Transform GetSelection();
    void Check(Ray ray);
}
