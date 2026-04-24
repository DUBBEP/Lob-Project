﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    private IRayProvider _rayProvider;
    private ISelector _selector;
    private ISelectionResponse _selectionResponse;

    private Transform _currentSelection;

    private void Awake()
    {
        _selectionResponse = GetComponent<ISelectionResponse>();
        _selector = GetComponent<ISelector>();
        _rayProvider = GetComponent<IRayProvider>();
        SceneManager.LoadScene("Environment", LoadSceneMode.Additive);
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }

    private void Update()
    {
        _selector.Check(_rayProvider.CreateRay());
        var selection = _selector.GetSelection();


        if (IsNewSelection(selection))
        {
            if (_currentSelection != null) _selectionResponse.OnDeselect(_currentSelection);
            if (selection != null) _selectionResponse.OnSelect(selection);
        }

        _currentSelection = selection;
    }

    private bool IsNewSelection(Transform selection)
    {
        return _currentSelection != selection;
    }
}