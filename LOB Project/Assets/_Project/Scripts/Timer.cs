using UnityEngine;

public class Timer : MonoBehaviour
{
    public float elapsedTime { get; private set; } = 0f;

    private void Update() => elapsedTime += Time.deltaTime;
}
