using System.Collections.Generic;
using UnityEngine;

public class ResponsiveSelector : MonoBehaviour, ISelector
{
    [SerializeField] private List<Selectable> selectables;
    [SerializeField] private float threshold;
    private Transform _selection;

    private void Start()
    {
        Selectable[] selectablesInScene = FindObjectsOfType<Selectable>();

        foreach (Selectable selectable in selectablesInScene)
            if (!selectables.Contains(selectable))
                selectables.Add(selectable);
    }

    public void Check(Ray ray)
    {
        _selection = null;

        var closest = 0f;
        for (int i = 0; i < selectables.Count; i++)
        {
            var Vector1 = ray.direction;
            var Vector2 = selectables[i].transform.position - ray.origin;

            var lookPercentage = Vector3.Dot(Vector1.normalized, Vector2.normalized);

            selectables[i].LookPercentage = lookPercentage;

            if (lookPercentage > threshold && lookPercentage > closest)
            {
                closest = lookPercentage;
                _selection = selectables[i].transform;
            }
        }

    }

    public Transform GetSelection()
    {
        return _selection;
    }
}
