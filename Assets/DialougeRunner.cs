using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class SimpleInkDialogue : MonoBehaviour
{
    // Handles which state we are in (Player Email or NPC Email)
    public enum EmailState
    {
        Dialogue,
        IncomingEmail,
        PlayerEmail,
        Choice,
        Wait
    }

    private EmailState currentState = EmailState.Dialogue;

    //Email UI Assets
    public GameObject emailAssetsUI;
    public GameObject LogoAssetsUI;
    public bool popupPressed = false;

    //Ink Text Storage
    public TextAsset inkJSON;

    public TextMeshProUGUI dialogueText;

    public ScrollRect scrollRect;
    
    private string storedEmailText = "";

    private int waitTime = 0;
    private float waitUntil = -1;  // -1 is not set

    private string nextKnot = "email1";
   

    //Object Button
    public Button senderButton;
    public Button composeButton;
    public Button replyButton;
    private bool isRepliable = false;
    private PopupUI currentViewedPopup;
    private bool composeOn = false;
    public GameObject sendButtonHider;

    //Player Reply Option Text
    public Button OP1Button;
    public Button OP2Button;
    public TMP_Text OPText1;
    public TMP_Text OPText2;

    //Audio Source
    public AudioSource emailAlertSFX;

    //Current story line
    private string line;

    //Current tags
    private List<string> tags;

    //Handles Player Typing
    public TypingManager typingManager;

    //Who is the Speaker?
    public TextMeshProUGUI speakerText;
    private string currentSpeaker;

    //What is the Title?
    public TextMeshProUGUI titleText;
    private string currentTitle;

    //PopUps
    public GameObject EmailPopUp;
    public Transform EmailPopupContainer;
    public ScrollRect popupScrollRect;
    private int popupArrivalCounter = 0;

    public Animator AnimatorTester;
    public Image portraitImage;
    bool isIncomingEmailTag = false;

    public CharacterData[] characters;

    CharacterData GetCharacter(string characterName)
    {

        foreach (CharacterData character in characters)
        {
            if (character.characterName == characterName)
                return character;
        }

        return null;

    }

    private Story story;


    void Start()
    {
        Debug.Log("inkJSON.text: " + inkJSON.text);
        story = new Story(inkJSON.text);
        Debug.Log("story: " + story);
        story.ChoosePathString("default");
        ShowNextLine();
        senderButton.onClick.AddListener(OnSenderButtonClicked);
        composeButton.onClick.AddListener(OnComposeButtonClicked);
        replyButton.onClick.AddListener(OnComposeButtonClicked);
    }

    void OnComposeButtonClicked()
    {
        if (composeOn == false) return;
        Debug.Log("Composing!");

        // Once replied, this email stays non-repliable even if revisited via popup.
        if (currentViewedPopup != null)
            currentViewedPopup.isRepliable = false;
        isRepliable = false;
        replyButton.gameObject.SetActive(false);

        story.ChoosePathString(nextKnot);
        ShowNextLine();
    }

    void OnReplyButtonClicked()
    {
        Debug.Log("Replying!");
        if (composeOn == false) return;
        story.ChoosePathString(nextKnot);
        ShowNextLine();
    }

    void OnSenderButtonClicked()
    {
        Debug.Log("Sending!");
        SpawnPopup(currentSpeaker, currentTitle, line, isIncomingEmailTag, isRepliable);
        ShowNextLine();
    }

    void Update()
    {
        if (currentState == EmailState.PlayerEmail)
        {
            senderButton.interactable = typingManager.IsTypingDone();

            // ignore space during typing (or later submit email)
            return;
        }
        if (currentState == EmailState.Wait)
        {
            var now = Time.time;
            if (waitUntil != -1 && now >= waitUntil)
            {
                // change to next state
                waitUntil = -1;
                Debug.Log("The wait is over!");
                if(nextKnot != null)
                {
                    story.ChoosePathString(nextKnot);
                }
                ShowNextLine();
            }

        }
        if (currentState == EmailState.IncomingEmail)
        {
            ShowNextLine();
        }
      
        if (popupPressed == true)
        {
            LogoAssetsUI.SetActive(false);
            popupPressed = false;
        }

    }

    void ShowNextLine()
    {
        //Variable for Pop Up "Needs to be Read" marker
        isIncomingEmailTag = false;

        if (story == null) return;
        if (!story.canContinue) return;

        //line = story.Continue().trim();
        (line, tags) = GetWholeKnot();
        Debug.Log("tags: " + string.Join(", ", tags));

        HandleTags();

        Debug.Log("LINE: [" + line + "]");
        Debug.Log("STATE: " + currentState);
        //dialogueText.text = line;
        //SpawnPopup(currentSpeaker, currentTitle, line);

        if (currentState == EmailState.IncomingEmail)
        {
            typingManager.Disable();
            storedEmailText = line;

            // Spawn the inbox popup only — do not switch the right-hand viewer.
            // The viewer updates when the player clicks a popup.
            emailAlertSFX.Play();
            ShowEmail(line);
            return;
        }

        if (currentState == EmailState.PlayerEmail)
        {
            LogoAssetsUI.SetActive(false);
            typingManager.Enable();
            Debug.Log("PlayerEmail line: " + line);
            StartPlayerEmail(line);
            return;
        }

        if (currentState == EmailState.Wait)
        {
            typingManager.Disable();
            LogoAssetsUI.SetActive(true);
            var now = Time.time;
            if (waitUntil == -1)
            {
                waitUntil = now + (float)waitTime;
                Debug.Log("now is " + now);
                Debug.Log("set waitUntil to " + waitUntil);
            }
            else
            {
                if(now >= waitUntil)
                {
                    // change to next state
                    waitUntil = -1;
                    Debug.Log("The wait is over!");

                }
                return;
            }

        }

        if (currentState == EmailState.Choice)
        {
            Debug.Log("Choice state");
            LogoAssetsUI.SetActive(false);
            sendButtonHider.SetActive(false);
            typingManager.Disable();
            DisplayChoices();

            return;
        }

    }

    // Following function from CHAT-GPT
    private (string text, List<string> tags) GetWholeKnot()
    {
        bool autoplay = true;
        string text = "";
        List<string> tags = new List<string>();

        while (story.canContinue && autoplay)
        {
            string line = story.Continue().Trim();
            if (!string.IsNullOrEmpty(line))
            {
                text += line + "\n\n";
            }
            tags.AddRange(story.currentTags);

            // 🚨 Detect divert: we left the original knot/stitch
            if (tags.Contains("stop"))
            {
                autoplay = false;
            }
        }

        return (text.Trim(), tags);
    }

    void HandleTags()
    {
        Debug.Log("In HandleTags");
        string expressionValue = null;

        // Incoming emails only spawn a popup; keep the right-hand viewer on whatever
        // the player is currently reading until they click that popup.
        bool willShowIncoming = false;
        foreach (string tag in tags)
        {
            string[] split = tag.Split(':');
            if (split.Length == 2 && split[0].Trim() == "incomingEmail")
            {
                willShowIncoming = true;
                break;
            }
        }

        isRepliable = false;
        if (!willShowIncoming)
            replyButton.gameObject.SetActive(false);

        //foreach (string tag in story.currentTags)
        foreach (string tag in tags)
        {
            Debug.Log("currentTag: "+ tag);
            string[] splitTag = tag.Split(':');

            if (splitTag.Length != 2)
                continue;

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();



            if (tagKey == "compose_on")
            {
                composeOn = tagValue == "true";
                Debug.Log("ComposeOn:" + composeOn);
            }

            if (tagKey == "next_knot")
            {
                nextKnot = tagValue;
                Debug.Log("next_knot set to " + nextKnot);
            }

            if (tagKey == "time")
            {
                currentState = EmailState.Wait;
                waitTime = int.Parse(tagValue);
                Debug.Log("WaitTime!" + waitTime);
            }

            if (tagKey == "incomingEmail")
            {
                currentState = EmailState.IncomingEmail;
                isIncomingEmailTag = true;
                Debug.Log("Incoming email detected");
            }

            if (tagKey == "playerEmail")
            {
                currentState = EmailState.PlayerEmail;
                Debug.Log("Player email detected");
            }

            if (tagKey == "choice")
            {
                currentState = EmailState.Choice;
                Debug.Log("In Choice Mode");
            }

            if (tagKey == "speaker")
            {
                currentSpeaker = tagValue;

                if (!willShowIncoming)
                {
                    if (speakerText != null)
                        speakerText.text = tagValue;

                    CharacterData character = GetCharacter(currentSpeaker);

                    if (character != null)
                    {
                        portraitImage.sprite = character.defaultPortrait;
                    }

                    ResetExpression();
                }

                Debug.Log("Speaker changed to " + currentSpeaker);
            }

            if (tagKey == "expression")
            {
                expressionValue = tagValue;
            }

            if (tagKey == "animation")
            {
                if (!string.IsNullOrEmpty(tagValue))
                {
                    PlayAnimation(tagValue);
                }
            }

            if (tagKey == "title")
            {
                currentTitle = tagValue;

                if (!willShowIncoming && titleText != null)
                    titleText.text = tagValue;

                Debug.Log("Title changed to " + currentTitle);
            }

            if (tagKey == "repliable")
            {
                Debug.Log("tagRepliable " + tagValue);
                isRepliable = true;
                if (!willShowIncoming)
                    replyButton.gameObject.SetActive(true);
            }
        }

        if (!willShowIncoming && !string.IsNullOrEmpty(expressionValue))
        {
            ChangeExpression(expressionValue);
        }

    }

    void PlayAnimation(string animationName)
    {
        if (AnimatorTester == null)
        {
            Debug.LogWarning("AnimatorTester is not assigned.");
            return;
        }

        if (string.IsNullOrEmpty(animationName))
        {
            Debug.LogWarning("Animation name is empty.");
            return;
        }

        Debug.Log("Trying animation: " + animationName);

        AnimatorTester.SetTrigger(animationName);
    }


    void ChangeExpression(string expression)
    {
        CharacterData character = GetCharacter(currentSpeaker);

        if (character == null)
            return;

        foreach (ExpressionSprite expr in character.expressions)
        {
            if (expr.expressionName == expression)
            {
                portraitImage.sprite = expr.sprite;
                return;
            }
        }

        Debug.LogWarning(
            $"Expression '{expression}' not found for {currentSpeaker}"
        );
    }

    void ResetExpression()
    {
        CharacterData character = GetCharacter(currentSpeaker);

        if (character != null)
        {
            portraitImage.sprite = character.defaultPortrait;
        }
    }

    void ChangeToViewEmailState(string speaker, string title, string text, PopupUI popup)
    {
        Debug.Log("ChangeToViewEmailState: " + text);
        if (!string.IsNullOrEmpty(text))
        {
            currentViewedPopup = popup;
            storedEmailText = text;
            replaceDialogueText(text);
            titleText.text = title;
            speakerText.text = speaker;

            CharacterData character = GetCharacter(speaker);
            if (character != null)
                portraitImage.sprite = character.defaultPortrait;

            bool canReply = popup != null && popup.isRepliable;
            Debug.Log("ChangeToViewEmailState isRepliable: " + canReply);
            replyButton.gameObject.SetActive(canReply);
        }
    }

    void SpawnPopup(string speaker, string title, string text, bool isIncomingEmailTag, bool isRepliable)
    {
        GameObject go = Instantiate(EmailPopUp);
        PopupUI popup = go.GetComponent<PopupUI>();

        popup.dialogue = this;
        popup.isRepliable = isRepliable;
        popup.subject = title;
        popup.threadKey = NormalizeSubject(title);
        popup.arrivalOrder = ++popupArrivalCounter;

        popup.OnClickCallback = () => ChangeToViewEmailState(speaker, title, text, popup);
        popup.transform.SetParent(EmailPopupContainer, false);

        ReorderPopupThreads();

        popup.SetData(speaker, title, text, isIncomingEmailTag);

        Debug.Log("SpawnPopup CALLED");

        StartCoroutine(RefreshPopupScrollRoutine());
    }

    // Subjects are only ever the base subject or a "RE "-prefixed reply, so no regex is needed.
    string NormalizeSubject(string subject)
    {
        if (string.IsNullOrEmpty(subject)) return string.Empty;
        string s = subject.Trim();
        if (s.StartsWith("RE ", System.StringComparison.OrdinalIgnoreCase))
            s = s.Substring(3);
        return s.Trim();
    }

    // Groups popups by thread (normalized subject): the thread with the newest email sits on
    // top, and within each thread the newest email is on top. The latest email in a thread is
    // full-width; older ones are indented with the "L" connector.
    void ReorderPopupThreads()
    {
        List<PopupUI> popups = new List<PopupUI>();
        foreach (Transform child in EmailPopupContainer)
        {
            PopupUI p = child.GetComponent<PopupUI>();
            if (p != null)
                popups.Add(p);
        }

        int siblingIndex = 0;
        var groups = popups
            .GroupBy(p => p.threadKey)
            .OrderByDescending(g => g.Max(p => p.arrivalOrder))
            .ToList();

        for (int groupIndex = 0; groupIndex < groups.Count; groupIndex++)
        {
            List<PopupUI> thread = groups[groupIndex].OrderByDescending(p => p.arrivalOrder).ToList();
            bool isLastThread = groupIndex == groups.Count - 1;
            for (int i = 0; i < thread.Count; i++)
            {
                bool isLatestInThread = i == 0;
                bool isLastInThread = i == thread.Count - 1;
                // Restore full between-thread gap after this thread's last email (not after the final thread).
                bool addInterThreadGap = isLastInThread && !isLastThread;
                thread[i].SetThreadPosition(isLatestInThread, addInterThreadGap);
                thread[i].transform.SetSiblingIndex(siblingIndex++);
            }
        }
    }

    private IEnumerator RefreshPopupScrollRoutine()
    {
        yield return null;

        RectTransform containerRect = EmailPopupContainer as RectTransform;
        if (containerRect == null)
            containerRect = EmailPopupContainer.GetComponent<RectTransform>();

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(containerRect);

        ScrollRect popupScroll = popupScrollRect;
        if (popupScroll == null)
            popupScroll = EmailPopupContainer.GetComponentInParent<ScrollRect>();

        if (popupScroll != null && popupScroll.content != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(popupScroll.content);
            popupScroll.verticalNormalizedPosition = 1f;
        }
    }

    void ShowEmail(string line)
    {
        storedEmailText = line;

        Debug.Log("EMAIL CAPTURED: [" + storedEmailText + "]");

        SpawnPopup(currentSpeaker, currentTitle, line, isIncomingEmailTag, isRepliable);
    }


    void StartPlayerEmail(string line)
    {
        //currentState = EmailState.PlayerEmail;

        storedEmailText = line;

        Debug.Log("EMAIL SENT TO TYPING: [" + storedEmailText + "]");

        typingManager.BeginTyping(storedEmailText);
    }


    void DisplayChoices()
    {
        replaceDialogueText("");
        Debug.Log("DisplayChoices count = "+ story.currentChoices.Count);
        // Hide both buttons first
        OP1Button.gameObject.SetActive(false);
        OP2Button.gameObject.SetActive(false);

        // Remove any previous listeners
        OP1Button.onClick.RemoveAllListeners();
        OP2Button.onClick.RemoveAllListeners();

        if (story.currentChoices.Count > 0)
        {
            OP1Button.gameObject.SetActive(true);
            OPText1.text = story.currentChoices[0].text;
            OP1Button.onClick.AddListener(() => SelectChoice(0));
        }

        if (story.currentChoices.Count > 1)
        {
            OP2Button.gameObject.SetActive(true);
            OPText2.text = story.currentChoices[1].text;
            OP2Button.onClick.AddListener(() => SelectChoice(1));
        }

        void SelectChoice(int index)
        {
            OP1Button.gameObject.SetActive(false);
            OP2Button.gameObject.SetActive(false);

            story.ChooseChoiceIndex(index);

            story.Continue();
            ShowNextLine();
        }
    }


    private void replaceDialogueText(string newText)
    {
        StartCoroutine(ReplaceDialogueTextRoutine(newText));
    }

    private IEnumerator ReplaceDialogueTextRoutine(string newText)
    {
        TextMeshProUGUI templateText = dialogueText.transform.parent.GetComponent<TextMeshProUGUI>();
        if (templateText == null)
            yield break;

        EnsureTemplateTextFitsContent(templateText);

        templateText.text = newText;
        dialogueText.text = newText;

        templateText.ForceMeshUpdate(true);
        dialogueText.ForceMeshUpdate(true);

        // TMP preferred height is not ready until after the layout pass.
        yield return null;

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(templateText.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

        scrollRect.verticalNormalizedPosition = 1f;
    }

    private static void EnsureTemplateTextFitsContent(TextMeshProUGUI templateText)
    {
        ContentSizeFitter fitter = templateText.GetComponent<ContentSizeFitter>();
        if (fitter == null)
            fitter = templateText.gameObject.AddComponent<ContentSizeFitter>();

        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
} 