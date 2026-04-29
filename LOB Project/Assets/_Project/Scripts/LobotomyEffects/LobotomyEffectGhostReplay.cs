using System.Collections.Generic;
using UnityEngine;

public class LobotomyEffectGhostReplay : MonoBehaviour, ILobotomyEffect
{
    private List<GhostRecord> ghosts = new List<GhostRecord>();

    private BackendGhostPlayerConnector connector;

    [Range(1, 100)][SerializeField] private float _selectionWeight;

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
        if (ghosts == null || ghosts.Count == 0) return null;

        return new ReplayData()
        {
            target = null,
            log = ghosts[Random.Range(0, ghosts.Count)].actions,
        };
    }

    public void StartEffect(Transform selection)
    {
        if (ghosts == null || ghosts.Count == 0) return;

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
