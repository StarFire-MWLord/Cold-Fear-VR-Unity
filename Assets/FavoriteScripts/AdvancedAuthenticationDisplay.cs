using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdvancedAuthenticationDisplay : MonoBehaviour
{
    [Header("Text Component")]
    public TMP_Text textDisplay;

    [Header("GameObjects To Enable After Loading")]
    public List<GameObject> enableObjects = new List<GameObject>();

    [Header("GameObjects To Disable After Loading")]
    public List<GameObject> disableObjects = new List<GameObject>();

    [Header("Typing Settings")]
    public float typingSpeed = 0.04f;
    public float cursorBlinkSpeed = 0.5f;

    private bool showCursor = true;
    private int currentProgress = 0;

    private void Start()
    {
        StartCoroutine(CursorBlink());
        StartCoroutine(AuthenticationSequence());
    }

    IEnumerator CursorBlink()
    {
        while (true)
        {
            showCursor = !showCursor;
            yield return new WaitForSeconds(cursorBlinkSpeed);
        }
    }

    IEnumerator AuthenticationSequence()
    {
        yield return TypeAndLoad("Authenticating User", 0, 20, 3f);
        yield return TypeAndLoad("User Authenticated", 20, 25, 1f);

        yield return TypeAndLoad("Logging Into User Account", 25, 50, 3f);
        yield return TypeAndLoad("Loading User Inventory And Data", 50, 75, 3f);
        yield return TypeAndLoad("User Inventory And Data Loaded", 75, 80, 1f);

        yield return TypeAndLoad("Loading Game", 80, 100, 5f);

        // Enable objects
        foreach (GameObject obj in enableObjects)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        // Disable objects
        foreach (GameObject obj in disableObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    IEnumerator TypeAndLoad(string message, int startPercent, int endPercent, float duration)
    {
        textDisplay.text = "";

        // Type text
        foreach (char c in message)
        {
            textDisplay.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        float timer = 0f;
        int displayedProgress = startPercent;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;
            currentProgress = Mathf.Lerp(startPercent, endPercent, t) < endPercent
                ? Mathf.FloorToInt(Mathf.Lerp(startPercent, endPercent, t))
                : endPercent;

            // Animated dots
            int dotCount = Mathf.FloorToInt(timer * 2) % 4;
            string dots = new string('.', dotCount);

            string cursor = showCursor ? "_" : " ";

            textDisplay.text = message + dots + " [" + currentProgress + "%] " + cursor;

            yield return null;
        }

        currentProgress = endPercent;
        textDisplay.text = message + " [" + currentProgress + "%]";
    }
}