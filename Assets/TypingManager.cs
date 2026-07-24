using UnityEngine.InputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TypingManager : MonoBehaviour
{
    // Prefs key versioned so older saved "Off" values don't stick as the apparent default.
    const string CaseSensitivePrefsKey = "TypingCaseSensitiveV2";

    static bool caseSensitive = true;
    static bool loaded;

    /// <summary>
    /// When true, typed letters must match the template's exact case.
    /// Persists across scenes via PlayerPrefs. Defaults to on.
    /// </summary>
    public static bool CaseSensitive
    {
        get
        {
            EnsureLoaded();
            return caseSensitive;
        }
        set
        {
            EnsureLoaded();
            caseSensitive = value;
            PlayerPrefs.SetInt(CaseSensitivePrefsKey, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStaticState()
    {
        caseSensitive = true;
        loaded = false;
    }

    static void EnsureLoaded()
    {
        if (loaded)
            return;

        loaded = true;
        caseSensitive = PlayerPrefs.GetInt(CaseSensitivePrefsKey, 1) == 1;
    }

    public TextMeshProUGUI templateText;
    public TextMeshProUGUI playerText;
    public Transform cursor;

    [SerializeField] private float cursorScrollPadding = 16f;
    [SerializeField] private float cursorYOffset = 1f;

    private string targetText = "";
    private string typedText = "";
    private int currentIndex = 0;

    private bool isTyping = false;
    private ScrollRect emailScrollRect;
    private Camera mainCamera;

    void Awake()
    {
        if (templateText != null)
            emailScrollRect = templateText.GetComponentInParent<ScrollRect>();

        var cameraSwitch = FindFirstObjectByType<CameraSwitchDebug>();
        if (cameraSwitch != null)
            mainCamera = cameraSwitch.mainCamera;

        SetCursorVisible(false);
    }

    void Update()
    {
        if (!isTyping) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Debug: F2 autocompletes the current player email
        if (keyboard.f2Key.wasPressedThisFrame)
        {
            CompleteTyping();
            return;
        }

        // Get all keys pressed this frame
        foreach (var key in keyboard.allKeys)
        {
            //Debug.Log("key: " + key.displayName + " wasPressedThisFrame: " + key.wasPressedThisFrame);
            if (key.wasPressedThisFrame)
            {
                //string keyString = key.displayName;

                //if (string.IsNullOrEmpty(keyString))
                //    continue;

                // Convert to char
                char c = TypedCharacter(keyboard, key.keyCode);
                Debug.Log("Is \\n? " + (c == '\n'));
                Debug.Log("keyString: " + c);
                Debug.Log("target char " + targetText[currentIndex]);
                Debug.Log("currentIndex "+ currentIndex);

                char expected = targetText[currentIndex];
                if (CharactersMatch(c, expected))
                {
                    // Use the template character so typed text always matches the target,
                    // even when case sensitivity is off.
                    typedText += expected;
                    currentIndex++;

                    playerText.text = typedText;
                }
                Debug.Log("typedText: " + typedText);

                if (typedText == targetText)
                {
                    CompleteTyping();
                    return;
                }
            }
        }
    }

    void LateUpdate()
    {
        bool showCursor = isTyping && IsMainCameraActive();
        SetCursorVisible(showCursor);
        if (showCursor)
            UpdateCursorPosition();
    }

    void CompleteTyping()
    {
        typedText = targetText;
        currentIndex = targetText.Length;
        playerText.text = typedText;
        isTyping = false;
        SetCursorVisible(false);
        Debug.Log("Typing complete!");
    }

    private static bool CharactersMatch(char typed, char expected)
    {
        if (CaseSensitive)
            return typed == expected;

        return char.ToLowerInvariant(typed) == char.ToLowerInvariant(expected);
    }

    private char TypedCharacter(Keyboard keyboard, Key keyCode)
    {
        bool shift =
            Keyboard.current.leftShiftKey.isPressed ||
            Keyboard.current.rightShiftKey.isPressed;

        bool caps = Keyboard.current.capsLockKey.isPressed;
        bool uppercase = shift ^ caps; // XOR: only one of them active

        return KeyToChar(keyCode, uppercase);
    }

    // Following function from CHAT-GPT
    private char KeyToChar(Key key, bool shift)
    {
        // Letters
        if (key >= Key.A && key <= Key.Z)
            return (char)((shift ? 'A' : 'a') + (key - Key.A));

        // Number row
        switch (key)
        {
            case Key.Digit0: return shift ? ')' : '0';
            case Key.Digit1: return shift ? '!' : '1';
            case Key.Digit2: return shift ? '@' : '2';
            case Key.Digit3: return shift ? '#' : '3';
            case Key.Digit4: return shift ? '$' : '4';
            case Key.Digit5: return shift ? '%' : '5';
            case Key.Digit6: return shift ? '^' : '6';
            case Key.Digit7: return shift ? '&' : '7';
            case Key.Digit8: return shift ? '*' : '8';
            case Key.Digit9: return shift ? '(' : '9';
        }

        // Numpad
        if (key >= Key.Numpad0 && key <= Key.Numpad9)
            return (char)('0' + (key - Key.Numpad0));

        switch (key)
        {
            case Key.Space: return ' ';
            case Key.Tab: return '\t';
            case Key.Enter: return '\n';

            case Key.Minus: return shift ? '_' : '-';
            case Key.Equals: return shift ? '+' : '=';
            case Key.LeftBracket: return shift ? '{' : '[';
            case Key.RightBracket: return shift ? '}' : ']';
            case Key.Backslash: return shift ? '|' : '\\';
            case Key.Semicolon: return shift ? ':' : ';';
            //case Key.Quote: return shift ? '"' : '\'';

            case Key.Quote:
                return shift ? '@' : '\'';
            case Key.Digit2:
                return shift ? '"' : '2';

            case Key.Comma: return shift ? '<' : ',';
            case Key.Period: return shift ? '>' : '.';
            case Key.Slash: return shift ? '?' : '/';
            case Key.Backquote: return shift ? '~' : '`';

            case Key.NumpadPeriod: return '.';
            case Key.NumpadDivide: return '/';
            case Key.NumpadMultiply: return '*';
            case Key.NumpadMinus: return '-';
            case Key.NumpadPlus: return '+';
            case Key.NumpadEquals: return '=';
        }

        return ' ';
    }

    public void BeginTyping(string text)
    {
        Debug.Log("BeginTyping called with: " + text);
      

        targetText = text;
        typedText = "";
        currentIndex = 0;
        isTyping = true;

        templateText.text = targetText;
        playerText.text = "";

        Debug.Log("TEMPLATE SET TO: " + templateText.text);

        templateText.gameObject.SetActive(true);
        templateText.enabled = true;
        SetCursorVisible(IsMainCameraActive());
    }

    public bool IsTypingDone()
    {
        return typedText == targetText;
    }

    public void Disable()
    {
        //templateText.text = "";
        //templateText.gameObject.SetActive(false);
        isTyping = false;
        SetCursorVisible(false);
    }

    public void Enable()
    {
        // prevent any existing game object hijacking when return pressed during typing
        EventSystem.current.SetSelectedGameObject(null);
        templateText.gameObject.SetActive(true);
    }

    private bool IsMainCameraActive()
    {
        return mainCamera != null && mainCamera.gameObject.activeInHierarchy;
    }

    private void SetCursorVisible(bool visible)
    {
        if (cursor == null) return;
        cursor.gameObject.SetActive(visible);
    }

    private void UpdateCursorPosition()
    {
        if (cursor == null || templateText == null)
            return;

        templateText.ForceMeshUpdate(true);
        TMP_TextInfo textInfo = templateText.textInfo;
        if (textInfo.characterCount == 0)
            return;

        // Use line ascender/descender for Y so the caret stays level across letters.
        TMP_CharacterInfo anchorChar;
        float x;
        if (currentIndex >= textInfo.characterCount)
        {
            anchorChar = textInfo.characterInfo[textInfo.characterCount - 1];
            x = anchorChar.topRight.x;
        }
        else
        {
            anchorChar = textInfo.characterInfo[currentIndex];
            x = anchorChar.topLeft.x;
        }

        TMP_LineInfo line = textInfo.lineInfo[anchorChar.lineNumber];
        float y = (line.ascender + line.descender) * 0.5f;
        Vector3 localPoint = new Vector3(x, y, 0f);

        Vector3 worldPoint = templateText.rectTransform.TransformPoint(localPoint);
        Vector3 localInParent = cursor.parent.InverseTransformPoint(worldPoint);
        localInParent.y += cursorYOffset;
        localInParent.z = cursor.localPosition.z;
        cursor.localPosition = localInParent;

        KeepCursorInView(cursor.position);
    }

    private void KeepCursorInView(Vector3 cursorWorldPoint)
    {
        if (emailScrollRect == null)
            emailScrollRect = templateText.GetComponentInParent<ScrollRect>();
        if (emailScrollRect == null || emailScrollRect.viewport == null || emailScrollRect.content == null)
            return;

        RectTransform viewport = emailScrollRect.viewport;
        RectTransform content = emailScrollRect.content;

        float contentHeight = content.rect.height;
        float viewportHeight = viewport.rect.height;
        if (contentHeight <= viewportHeight)
            return;

        Vector3 caretInViewport = viewport.InverseTransformPoint(cursorWorldPoint);
        float overflow = contentHeight - viewportHeight;
        float normalized = emailScrollRect.verticalNormalizedPosition;

        if (caretInViewport.y < viewport.rect.yMin + cursorScrollPadding)
        {
            float delta = (viewport.rect.yMin + cursorScrollPadding) - caretInViewport.y;
            emailScrollRect.verticalNormalizedPosition = Mathf.Clamp01(normalized - delta / overflow);
        }
        else if (caretInViewport.y > viewport.rect.yMax - cursorScrollPadding)
        {
            float delta = caretInViewport.y - (viewport.rect.yMax - cursorScrollPadding);
            emailScrollRect.verticalNormalizedPosition = Mathf.Clamp01(normalized + delta / overflow);
        }
    }
}