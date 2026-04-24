using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LobotomySelectionResponse : MonoBehaviour, ISelectionResponse
{
    [SerializeField]
    private GameObject lobotomyEffectsHolder;

    private List<ILobotomyEffect> _lobotomyEffects;
    private ILobotomyEffect _currentEffect;

    private void Start()
    {
        _lobotomyEffects = lobotomyEffectsHolder.GetComponents<ILobotomyEffect>().ToList();
    }

    public void OnSelect(Transform selection)
    {
        _currentEffect = SelectWeightedOptions(_lobotomyEffects);
        _currentEffect.StartEffect(selection);
    }

    public void OnDeselect(Transform selection)
    {
        _currentEffect.StopEffect(selection);
    }

    private ILobotomyEffect SelectWeightedOptions(List<ILobotomyEffect> effects)
    {
        // Create a temporary, mutable list to remove selected options and ensure uniqueness
        List<ILobotomyEffect> pool = new List<ILobotomyEffect>(effects);

        float totalWeight = 0f;

        // Calculate the total weight of the remaining pool
        foreach (ILobotomyEffect item in pool)
        {
            // Get the base weight from the item's rarity
            float itemWeight = item.GetEffectSelectionPriority();

            totalWeight += itemWeight;
        }

        // Roll the Dice
        float randomPoint = Random.Range(0f, totalWeight);

        // Find the Winning Item
        ILobotomyEffect selectedItem = null;

        for (int i = 0; i < pool.Count; i++)
        {
            ILobotomyEffect currentItem = pool[i];
            float currentWeight = currentItem.GetEffectSelectionPriority();

            // Subtract the item's weight from the random point. 
            // The first item to make randomPoint <= 0 is the winner.
            if (randomPoint < currentWeight)
            {
                selectedItem = currentItem;
                break;
            }
            randomPoint -= currentWeight;
        }

        return selectedItem;
    }
}
