using UnityEngine;

public class LobotomyEffectChase : MonoBehaviour, ILobotomyEffect
{
    [SerializeField] private float followSpeed = 1f;
    private bool _effectIsActive;

    [SerializeField] private AudioSource _slidingAudio;
    [SerializeField] private Transform player;
    
    private Transform _selection;

    [Range(1, 100)][SerializeField] private float _selectionWeight;

    public float GetEffectSelectionPriority()
    {
        return _selectionWeight;
    }

    public void StartEffect(Transform selection)
    {
        _effectIsActive = true;
        _selection = selection;
        _slidingAudio.Play();
    }

    public void StopEffect(Transform selection)
    {
        float stopDelay = Random.Range(1f, 9f);

        Invoke(nameof(StopChase), stopDelay);
    }

    private void StopChase()
    {
        _effectIsActive = false;
        _selection = null;
        _slidingAudio.Stop();
    }

    private void Update()
    {
        if (_effectIsActive)
        {
            FollowPlayer(_selection);
        }
    }

    private void FollowPlayer(Transform selection)
    {
        selection.LookAt(player);
        selection.position = Vector3.Lerp(selection.position, player.position, Time.deltaTime * followSpeed);
    }
}
