using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

/// <summary>
/// Reliable camera switch for menu buttons. Works alongside Visual Scripting.
/// </summary>
[RequireComponent(typeof(Button))]
public class MenuCameraSwitchButton : MonoBehaviour
{
    [Tooltip("Camera GameObject to turn on (e.g. Main Camera).")]
    public GameObject enableCamera;

    [Tooltip("Camera GameObject to turn off (e.g. SettingsCam).")]
    public GameObject disableCamera;

    [Tooltip("Value written to scene variable WhatMainMenuAreWeOn? (empty = main menu).")]
    public string menuSectionValue = "";

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(SwitchCameras);
    }

    public void SwitchCameras()
    {
        if (enableCamera != null)
            enableCamera.SetActive(true);
        if (disableCamera != null)
            disableCamera.SetActive(false);

        Variables.ActiveScene.Set("WhatMainMenuAreWeOn?", menuSectionValue);
    }
}
