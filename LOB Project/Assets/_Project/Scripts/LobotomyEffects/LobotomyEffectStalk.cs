using System.Collections.Generic;
using UnityEngine;

public class LobotomyEffectStalk : MonoBehaviour, ILobotomyEffect
{
    [SerializeField]
    private float followSpeed = 1f;
    private bool _effectIsActive;

    [SerializeField]
    private Transform player;
    private List<Transform> _selections = new List<Transform>();
    private Transform _currentSelection;
    public void StartEffect(Transform selection)
    {
        _currentSelection = selection;
        _selections.Add(selection);
    }

    public void StopEffect(Transform selection)
    {
        _currentSelection = null;

        if (_selections.Contains(selection) && Random.Range(0, 10) == 0)
            _selections.Remove(selection);

        if (Random.Range(0, 100) == 0)
            _selections.Clear();
    }

    private void Update()
    {
        foreach (Transform selection in _selections)
        {
            if (selection == _currentSelection) continue;
            FollowPlayer(selection);
        }
    }

    private void FollowPlayer(Transform selection)
    {
        selection.LookAt(player);
        selection.position = Vector3.Lerp(selection.position, player.position, Time.deltaTime * followSpeed);
    }
}
