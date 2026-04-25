using UnityEngine;

[RequireComponent(typeof(ClickerHint))]
public class Clicker : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform targetObject;
    [SerializeField] private float max_distance;
    [SerializeField] private float lookThreshold;
    private bool isSwitched;
    private ClickerHint hint;

    private void Awake() => hint = GetComponent<ClickerHint>();

    public SwitchCams switcher;
    void Update()
    {
        hint.ToggleHint(false);

        if ((Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Tab)) && isSwitched)
        {
            switcher.SwitchCameras();
            isSwitched = false;
        }
        if (isSwitched) return;

        if (!CheckDistanceToTarget()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!CheckLookPercentage(ray)) return;
        hint.ToggleHint(true);

        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0))  && !isSwitched) // Left mouse button
        {
            switcher.SwitchCameras();
            isSwitched = true;
        }
    }

    private bool CheckDistanceToTarget()
    {
        if (Vector3.Distance(player.position, targetObject.position) <= max_distance)
        {
            return true;
        }

        return false;
    }

    private bool CheckLookPercentage(Ray ray)
    {
        float activeThreshold = 0f;

        if (Vector3.Distance(player.position, targetObject.position) < 1.5f)
            activeThreshold = lookThreshold * 0.7f;
        else
            activeThreshold = lookThreshold;

            Vector3 Vector1 = ray.direction;
        Vector3 Vector2 = targetObject.position - ray.origin;

        float lookPercentage = Vector3.Dot(Vector1.normalized, Vector2.normalized);

        if (lookPercentage > activeThreshold)
            return true;

        return false;
    }
}
