using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    [SerializeField] private Scrollbar targetScrollbar;
    [SerializeField] private float scrollSensitivity = 0.1f;

    void Update()
    {
        if (targetScrollbar == null) return;

        // Get the vertical scroll amount (-1, 0, or 1)
        float scrollInput = Input.mouseScrollDelta.y;

        if (scrollInput != 0)
        {
            // Update the scrollbar value, clamped between 0 and 1
            // Note: Use -= if you want "Natural" scrolling (inverted)
            targetScrollbar.value = Mathf.Clamp01(targetScrollbar.value + (scrollInput * scrollSensitivity));
        }
    }
}
