using UnityEngine;

public class ClickerHint : MonoBehaviour
{
    [SerializeField] GameObject hintText;

    public void ToggleHint(bool toggle) => hintText.SetActive(toggle);
}
