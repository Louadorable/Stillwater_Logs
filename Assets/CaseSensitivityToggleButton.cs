using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main-menu toggle for whether TypingManager requires exact letter case.
/// </summary>
[RequireComponent(typeof(Button))]
public class CaseSensitivityToggleButton : MonoBehaviour
{
    [Tooltip("Label that shows On/Off (e.g. On/Off TMP under CapsSensButton).")]
    public TextMeshProUGUI statusLabel;

    void Awake()
    {
        EnsureLabel();
        GetComponent<Button>().onClick.AddListener(Toggle);
    }

    void OnEnable()
    {
        EnsureLabel();
        RefreshLabel();
    }

    public void Toggle()
    {
        TypingManager.CaseSensitive = !TypingManager.CaseSensitive;
        RefreshLabel();
    }

    void EnsureLabel()
    {
        if (statusLabel != null)
            return;

        foreach (var tmp in GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            if (tmp.gameObject.name.Contains("On/Off"))
            {
                statusLabel = tmp;
                return;
            }
        }
    }

    void RefreshLabel()
    {
        if (statusLabel != null)
            statusLabel.text = TypingManager.CaseSensitive ? "On" : "Off";
    }
}
