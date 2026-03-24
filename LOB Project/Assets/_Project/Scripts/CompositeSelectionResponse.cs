using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CompositeSelectionResponse : MonoBehaviour, ISelectionResponse, IChangeable
{
    [SerializeField] private GameObject selectionResponseHolder;

    private List<ISelectionResponse> _selectionResponses;
    private int _currentIndex;
    private Transform _selection;


    private void Start()
    {
        _selectionResponses = selectionResponseHolder.GetComponents<ISelectionResponse>().ToList();
        SelectModeUI.instance.UpdateSelectionDisplay(_currentIndex);

    }

    [ContextMenu("Next")]
    public void Next()
    {
        if (_selection != null) _selectionResponses[_currentIndex].OnDeselect(_selection);
        _currentIndex = (_currentIndex + 1) % _selectionResponses.Count;
        if (_selection != null) _selectionResponses[_currentIndex].OnSelect(_selection);

        SelectModeUI.instance.UpdateSelectionDisplay(_currentIndex);
    }



    public void OnDeselect(Transform selection)
    {
        _selection = null;
        if (HasSelection())
        {
            _selectionResponses[_currentIndex].OnDeselect(selection);
        }
    }



    public void OnSelect(Transform selection)
    {
        _selection = selection;
        if (HasSelection())
        {
            _selectionResponses[_currentIndex].OnSelect(selection);
        }
    }

    private bool HasSelection()
    {
        return _currentIndex > -1 && _currentIndex < _selectionResponses.Count;
    }
}
