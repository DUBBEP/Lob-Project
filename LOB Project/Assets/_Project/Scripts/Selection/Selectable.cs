using TMPro;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lookPercentageLabel;

    [HideInInspector] public float LookPercentage;
    private void Update()
    {
        if (lookPercentageLabel != null)
            lookPercentageLabel.text = LookPercentage.ToString(format:"F3");
    }
}
