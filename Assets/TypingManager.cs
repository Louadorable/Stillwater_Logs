using UnityEngine.InputSystem;
using TMPro;
using UnityEngine;

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
                string keyString = key.displayName;

                if (string.IsNullOrEmpty(keyString))
                    continue;

                // Convert to char
                char c = TypedCharacter(keyboard, key.keyCode);
                Debug.Log("keyString: " + c);

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

        if (keyCode >= Key.A && keyCode <= Key.Z)
        {
            char c = (char)('a' + (keyCode - Key.A));

            if (uppercase)
                c = char.ToUpper(c);

            return c;
        }
        else
        {
            return (char)keyCode;
        }
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
        templateText.gameObject.SetActive(false);
    }

    public void Enable()
    {
        templateText.gameObject.SetActive(true);
    }
}