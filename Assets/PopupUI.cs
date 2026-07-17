using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PopupUI : MonoBehaviour
{
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI titleText;
    public GameObject alertMarker;
    public Button popupButton;

    /// <summary>Reply icon shown to the right of the popup while it is still repliable.</summary>
    public GameObject replyIcon;

    private bool isRead = false;
    private bool isIncomingEmail;
    public bool popupPressed = false;
    public SimpleInkDialogue dialogue;

    [SerializeField] private bool _isRepliable;

    /// <summary>True until the player has replied to this email. Drives the reply icon visibility.</summary>
    public bool isRepliable
    {
        get => _isRepliable;
        set
        {
            _isRepliable = value;
            UpdateReplyIcon();
        }
    }

    private string bodyText = string.Empty;

    public Action OnClickCallback;

    void Awake()
    {
        if (popupButton != null)
            popupButton.onClick.AddListener(OnPopupClicked);

        UpdateReplyIcon();
    }

    void UpdateReplyIcon()
    {
        if (replyIcon != null)
            replyIcon.SetActive(_isRepliable);
    }


    public void SetData(string speaker, string title, string body, bool incoming)
    {
        Debug.Log($"SetData incoming = {incoming}; body: " + body);
        if (speakerText != null)
            speakerText.text = speaker;

        if (titleText != null)
            titleText.text = title;
        
        bodyText = body;

        //Making sure Marker is visable when spawned
        if (alertMarker != null)
            alertMarker.SetActive(incoming);

        isRead = false;
    }

    void OnPopupClicked()
    {
        dialogue.popupPressed = true;

        MarkRead();
        if (OnClickCallback != null)
            OnClickCallback?.Invoke();
    }

    void MarkRead()
    {
        isRead = true;

        if (alertMarker != null)
            alertMarker.SetActive(!isRead);
    }
}