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

    /// <summary>Capital "L" connector glyph shown in the left gutter for non-latest thread emails.</summary>
    public GameObject threadConnectorL;

    /// <summary>Root LayoutElement (kept for reference; width is no longer changed for indenting).</summary>
    public LayoutElement layoutElement;

    /// <summary>Inner container holding the popup visuals; shifted right to indent non-latest thread emails.</summary>
    public RectTransform bodyRect;

    // Threading metadata.
    public string subject;
    public string threadKey;
    public int arrivalOrder;

    private const float ThreadIndent = 20.8f;
    private const float PopupHeight = 64f;
    /// <summary>Extra layout height after the last email in a thread so between-thread gap stays at 16px while within-thread spacing is 6px.</summary>
    private const float InterThreadExtraGap = 10f;

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

    /// <summary>
    /// Positions this popup within its thread: the latest email is flush-left with no
    /// connector, while older emails are indented (shifted right) with the "L" connector
    /// left behind at the thread's left margin.
    /// When <paramref name="addInterThreadGap"/> is true, adds extra height below so the
    /// gap before the next thread matches the original between-thread spacing.
    /// </summary>
    public void SetThreadPosition(bool isLatestInThread, bool addInterThreadGap = false)
    {
        if (bodyRect != null)
        {
            float x = isLatestInThread ? 0f : ThreadIndent;
            bodyRect.anchoredPosition = new Vector2(x, bodyRect.anchoredPosition.y);
        }

        if (threadConnectorL != null)
            threadConnectorL.SetActive(!isLatestInThread);

        if (layoutElement != null)
            layoutElement.preferredHeight = addInterThreadGap
                ? PopupHeight + InterThreadExtraGap
                : PopupHeight;
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