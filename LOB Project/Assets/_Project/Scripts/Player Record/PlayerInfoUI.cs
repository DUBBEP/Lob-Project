using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerInfo;
    [SerializeField] private Image bgImage;

    private void Awake() => playerInfo = GetComponent<TMP_Text>();

    public void fillPlayerInfo(PlayerRecord item, int rank = 0, Color? color = null)
    {
        Color newColor = color ?? Color.white;

        if (newColor != Color.white)
            newColor = newColor + Color.grey;

        bgImage.color = newColor;
        playerInfo.text = $"{rank} <pos=10%>{item.username} <pos=75%>{item.score}";
    }
}
