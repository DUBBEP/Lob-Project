using System.Collections;
using UnityEditor;
using UnityEngine;

public class LobotomyEffectShatter : MonoBehaviour, ILobotomyEffect
{
    [SerializeField]
    private GameObject particleEffectPrefab;

    private float cooldown;
    public void StartEffect(Transform selection)
    {
        selection.gameObject.SetActive(false);
        GameObject particle = Instantiate(particleEffectPrefab, selection.position, selection.rotation);
        Destroy(particle, 2f);
    }

    public void StopEffect(Transform selection)
    {
        StartCoroutine(DelayActivateSelection(selection));
    }

    IEnumerator DelayActivateSelection(Transform Selection)
    {
        yield return new WaitForSeconds(2);
        Selection.gameObject.SetActive(true);
    }
}
