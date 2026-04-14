using System.Collections.Generic;
using UnityEngine;

public class LobotomyEffectGhostReplay : MonoBehaviour, ILobotomyEffect
{
    private List<GhostRecord> ghosts = new List<GhostRecord>();

    private BackendGhostPlayerConnector connector;

    [SerializeField] private float _selectionWeight;

    public float GetEffectSelectionPriority()
    {
        return _selectionWeight;
    }

    private void Start()
    {
        connector = FindFirstObjectByType<BackendGhostPlayerConnector>();

        GetRandomRecords();
    }

    private async void GetRandomRecords()
    {
        ghosts = await connector.GetRandomRecordsAsync();
    }

    private ReplayData MakeRandomReplay()
    {
        return new ReplayData()
        {
            target = null,
            log = ghosts[Random.Range(0, ghosts.Count)].actions,
        };
    }

    public void StartEffect(Transform selection)
    {
        ReplayData newReplay = MakeRandomReplay();
        newReplay.target = selection;
        StateReplay replay = StateReplay.Instance;

        if (Random.Range(0, 2) == 0) 
            replay.StartReplay(newReplay); 
        else 
            replay.StartSmoothReplay(newReplay);
    }

    public void StopEffect(Transform selection) { }
}
