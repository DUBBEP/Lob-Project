using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LobotomyEffectResetPositions : MonoBehaviour, ILobotomyEffect
{
    [SerializeField] LightingSettings scaryLighting;
    [SerializeField] Material newSkybox;
    [SerializeField] private GameObject FlashBangEffect;
    [SerializeField] private Color fogColor;
    [Range(1, 100)][SerializeField] private float _selectionWeight;

    private List<ObjectStartState> _selectablesInScene = new List<ObjectStartState>();

    public float GetEffectSelectionPriority()
    {
        return _selectionWeight;
    }

    private void Start()
    {
        GameObject[] selectableObjects = GameObject.FindGameObjectsWithTag("Selectable");

        foreach (GameObject selectableObject in selectableObjects)
        {
            _selectablesInScene.Add(new ObjectStartState(selectableObject.transform, 
                selectableObject.transform.position, selectableObject.transform.rotation));
        }
    }

    public void StartEffect(Transform selection)
    {
        if (FlashBangEffect.activeSelf)
            return;

        foreach (ObjectStartState state in _selectablesInScene)
        {
            state.ResetState();
        }

        FlashBangEffect.SetActive(true);

        Lightmapping.lightingSettings = scaryLighting;

        RenderSettings.fogDensity = 0.12f;
        RenderSettings.fogColor = fogColor;
        RenderSettings.ambientIntensity = 0;
        RenderSettings.subtractiveShadowColor = Color.red;
        RenderSettings.ambientLight = Color.red;
        

        // turn off lights
        Light[] lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        foreach (Light light in lights)
        {
            if (light.type == LightType.Directional)
                light.gameObject.SetActive(false);
        }

        ChangeSkybox();
    }

    public void ChangeSkybox()
    {
        if (newSkybox != null)
        {
            RenderSettings.skybox = newSkybox;
            
            DynamicGI.UpdateEnvironment();
        }
    }

    public void StopEffect(Transform selection) { }
}
