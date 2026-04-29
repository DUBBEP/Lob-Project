using UnityEngine;

public class FlashController : MonoBehaviour
{
    public void SetInactive()
    {
        gameObject.SetActive(false);
    }

    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip1;
    [SerializeField] private AudioClip clip2;
    [SerializeField] private string animationStateName;

    private void OnEnable()
    {
        SetRandomClip();
    }

    public void SetRandomClip()
    {
        int roll = Random.Range(0, 10);

        if ( roll % 2 == 0)
        {
            audioSource.clip = clip1;
            SyncSpeed(clip1);
        }
        else
        {
            audioSource.clip = clip2;
            SyncSpeed(clip2);
        }
    }

    public void SyncSpeed(AudioClip clip)
    {
        if (audioSource.clip == null) return;

        // Get the length of the specific animation clip
        float animClipLength = GetAnimationClipLength(animationStateName);
        float audioLength = clip.length;

        // Calculate and apply speed
        float targetSpeed = animClipLength / audioLength;
        animator.speed = targetSpeed;

        audioSource.Play();
    }

    private float GetAnimationClipLength(string name)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name == name) return clip.length;
        }
        return 1.0f;
    }
}
