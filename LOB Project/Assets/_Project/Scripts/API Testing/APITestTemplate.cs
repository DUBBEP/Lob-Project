using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;


/* This is a template file for API Testing.
 * It is incomplete and needs to be filled out with information for specific endpoints.
 * COPY AND PASTE THIS FILE, RENAME IT TO {FEATURE}APITest
 * Fill out any commented lines with the relevent information
 * 
 * Go to the API Test Scene. Add this script to an object 
 * and hook it up to the button and input field.
 * Test it and pray for success.
 */

public class APITestTemplate : MonoBehaviour
{
    // Replace type with your backend manager
    //[SerializeField] private BackendManager apiManager;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI urlDisplay;
    [SerializeField] private TMP_InputField urlField;

    private void Awake()
    {
        // uncomment
        // urlField.text = apiManager.GetURL();
        DisplayURL();
        // DebugObjects();
    }

    public void ChangeURL(TMP_InputField field)
    {
        // apiManager.SetURL(field.text);
        // DisplayURL();
    }

    public void DebugIndexMethod()
    {
        // DebugObjects();
    }

    private void DisplayURL()
    {
        // urlDisplay.text = "Current URL: " + apiManager.GetURL();
    }

    /*
    private async void DebugObjects()
    {
        // replace string with your container and make sure the index method is correct
        // ObjectWrapper[] items = await apiManager.GetIndexAsync();


        // if (items != null)
        {
            // Debug.Log("API Call was a success");
            // replace string with your object type
            // foreach (string item in items)
            {
                // Access the shape name from the full object data
                // Debug.Log("Object in DB: " + item);
            }
        }
        // else if (items == null)
        {
            // Debug.Log("Your request probably failed.");
        }
    }
    */
}