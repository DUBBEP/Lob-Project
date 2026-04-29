using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobotomySelectionResponse : MonoBehaviour, ISelectionResponse
{
    [SerializeField] private GameObject lobotomyEffectsHolder;
    [Range(0, 100000)][SerializeField] private int lobotomyDefense;

    private List<ILobotomyEffect> _lobotomyEffects;
    private ILobotomyEffect _currentEffect;

    public static int selectionOcurrenceCounter { get; private set; } = 0;

    private void Start()
    {
        Debug.Log("Selection Occurances: " + selectionOcurrenceCounter);
        selectionOcurrenceCounter = 0;
        _lobotomyEffects = lobotomyEffectsHolder.GetComponents<ILobotomyEffect>().ToList();
    }

    public void OnSelect(Transform selection)
    {
        selectionOcurrenceCounter++;

        if (HandleEscalation())
            TryLobotomyEffect(selection);
        else
            return;
    }

    public void OnDeselect(Transform selection)
    {
        if (_currentEffect != null)
        {
            _currentEffect.StopEffect(selection);
        }
    }

    private bool HandleEscalation()
    {
        int roll = Random.Range(0, lobotomyDefense);
        if (roll < selectionOcurrenceCounter)
            return true;

        return false;
    }

    private void TryLobotomyEffect(Transform selection)
    {
        _currentEffect = SelectWeightedOptions(_lobotomyEffects);
        _currentEffect.StartEffect(selection);
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
