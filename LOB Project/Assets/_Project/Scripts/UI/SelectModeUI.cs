using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectModeUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _selectionDisplay;
    [SerializeField]
    private GameObject _displayPanel;

    public static SelectModeUI instance;

    private void Awake() { instance = this; }

    public void UpdateSelectionDisplay(int index)
    {
        switch (index)
        {
            case 0:
                _selectionDisplay.text = HighlightText();
                break;
            case 1:
                _selectionDisplay.text = OutlineText();
                break;
            case 2:
                _selectionDisplay.text = LobotomyText();
                break;
        }
    }

    private string LobotomyText()
    {
        _displayPanel.SetActive(false);
        _selectionDisplay.gameObject.SetActive(false);
        return "";
    }

    private string OutlineText()
    {
        return "Highlight Select\n" +
               "Outline Select <---\n" +
               "Lobotomy Select   ";
    }

    private string HighlightText()
    {
        return "Highlight Select <---\n" +
               "Outline Select\n" +
               "Lobotomy Select   ";

    }
}
