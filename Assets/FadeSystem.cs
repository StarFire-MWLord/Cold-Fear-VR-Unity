
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeSystem : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad = "SceneName";

    [Header("Timing")]
    public float waitBeforeFade = 5f;
    public float fadeDuration = 2f;

    [Header("Fade Texture")]
    public Texture fadeTexture;

    private float alpha = 0f;

    void Start()
    {
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        yield return new WaitForSeconds(waitBeforeFade);

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    void OnGUI()
    {
        if (fadeTexture == null) return;

        GUI.color = new Color(0, 0, 0, alpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
    }
}
