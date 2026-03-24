using System.Collections.Generic;
using UnityEngine;

public class LobotomyEffectResetPositions : MonoBehaviour, ILobotomyEffect
{
    public struct startState
    {
        public Transform selectabe;
        public Vector3 position;
        public Vector3 rotation;
        public startState(Transform selectable, Vector3 position, Vector3 rotation)
        {
            this.selectabe = selectable;
            this.position = position;
            this.rotation = rotation;
        }

        public void ResetState()
        {
            selectabe.position = position;
            selectabe.rotation = Quaternion.Euler(rotation);
        }
    }

    [SerializeField]
    private GameObject FlashBangEffect;
    [SerializeField]
    private Color fogColor;


    private List<startState> _selectablesInScene = new List<startState>();

    private void Start()
    {
        GameObject[] selectableObjects = GameObject.FindGameObjectsWithTag("Selectable");

        foreach (GameObject selectableObject in selectableObjects)
        {
            _selectablesInScene.Add(new startState(selectableObject.transform, 
                selectableObject.transform.position, selectableObject.transform.rotation.eulerAngles));
        }
    }

    public void StartEffect(Transform selection)
    {
        if (FlashBangEffect.activeSelf)
            return;

        foreach (startState state in _selectablesInScene)
        {
            state.ResetState();
        }
        FlashBangEffect.SetActive(true);

        RenderSettings.fogDensity = 0.12f;
        RenderSettings.fogColor = fogColor;
        RenderSettings.ambientIntensity = 0;
        RenderSettings.subtractiveShadowColor = Color.red;
        RenderSettings.ambientLight = Color.red;
    }

    public void StopEffect(Transform selection)
    {
        return;
    }
}
