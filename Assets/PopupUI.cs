using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupUI : MonoBehaviour
{
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;
    public GameObject alertMarker;
    public Button popupButton;

    private bool isRead = false;
    private bool isIncomingEmail;

    void Awake()
    {
        if (popupButton != null)
            popupButton.onClick.AddListener(OnPopupClicked);
    }


    public void SetData(string speaker, string title, string body, bool incoming)
    {
        Debug.Log($"SetData incoming = {incoming}");
        if (speakerText != null)
            speakerText.text = speaker;

        if (titleText != null)
            titleText.text = title;

        if (bodyText != null)
            bodyText.text = body;

        //Making sure Marker is visable when spawned
        if (alertMarker != null)
            alertMarker.SetActive(incoming);

        isRead = false;
    }

    void OnPopupClicked()
    {
        MarkRead();
    }

    void MarkRead()
    {
        isRead = true;

        if (alertMarker != null)
            alertMarker.SetActive(!isRead);
    }
}