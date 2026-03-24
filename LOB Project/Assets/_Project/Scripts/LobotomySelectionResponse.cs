using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LobotomySelectionResponse : MonoBehaviour, ISelectionResponse
{
    [SerializeField]
    private GameObject lobotomyEffectsHolder;

    private List<ILobotomyEffect> _lobotomyEffects;
    private int randomValue = 0;

    private void Start()
    {
        _lobotomyEffects = lobotomyEffectsHolder.GetComponents<ILobotomyEffect>().ToList();
    }

    public void OnSelect(Transform selection)
    {
        randomValue = Random.Range(0, _lobotomyEffects.Count);
        _lobotomyEffects[randomValue].StartEffect(selection);
    }

    public void OnDeselect(Transform selection)
    {
        _lobotomyEffects[randomValue].StopEffect(selection);
    }



}
