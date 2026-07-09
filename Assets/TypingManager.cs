using UnityEngine.InputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TypingManager : MonoBehaviour
{
    public TextMeshProUGUI templateText;
    public TextMeshProUGUI playerText;

    private string targetText = "";
    private string typedText = "";
    private int currentIndex = 0;

    private bool isTyping = false;

    void Update()
    {
        if (!isTyping) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

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

                if (c == targetText[currentIndex])
                {
                    typedText += c;
                    currentIndex++;

                    playerText.text = typedText;
                }
                Debug.Log("typedText: " + typedText);

                if (typedText == targetText)
                {
                    isTyping = false;
                    Debug.Log("Typing complete!");
                    return;
                }
            }
        }
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
    }

    public bool IsTypingDone()
    {
        return typedText == targetText;
    }

    public void Disable()
    {
        //templateText.text = "";
        //templateText.gameObject.SetActive(false);
    }

    public void Enable()
    {
        // prevent any existing game object hijacking when return pressed during typing
        EventSystem.current.SetSelectedGameObject(null);
        templateText.gameObject.SetActive(true);
    }
}