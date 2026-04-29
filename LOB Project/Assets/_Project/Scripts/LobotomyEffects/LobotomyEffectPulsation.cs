using System.Collections;
using UnityEngine;

public class LobotomyEffectPulsation : MonoBehaviour, ILobotomyEffect
{
    [SerializeField] private GameObject heartSoundPrefab;
    [Range(0.1f, 2)][SerializeField] private float pulseRate;
    [Range(0.1f, 100)][SerializeField] private float _selectionWeight;

    public float GetEffectSelectionPriority()
    {
        return _selectionWeight;
    }

    public void StartEffect(Transform selection)
    {
        GameObject heartsfxObject = Instantiate(heartSoundPrefab, selection);

        AudioSource soundSource = heartsfxObject.GetComponent<AudioSource>();
        Destroy(heartsfxObject, soundSource.clip.length);

        StartCoroutine(DoPulsation(selection, soundSource.clip.length));
    }

    private IEnumerator DoPulsation(Transform selection, float length)
    {
        float timer = length;
        bool pulseOut = true;

        Vector3 startScale = selection.localScale;

        while (timer > 0)
        {
            if (pulseOut)
            {
                if (selection.localScale.magnitude > startScale.magnitude * 1.5f)
                    pulseOut = !pulseOut;

                selection.localScale += Vector3.one * Time.deltaTime * pulseRate;
            }
            else
            {
                if (selection.localScale.magnitude < startScale.magnitude)
                    pulseOut = !pulseOut;

                selection.localScale -= Vector3.one * Time.deltaTime;
            }

            yield return null;
        }
    }

    public void StopEffect(Transform selection) { }
}
