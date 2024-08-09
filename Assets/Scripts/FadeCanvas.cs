using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeCanvas : MonoBehaviour
{
    [SerializeField] private Image fadePanelImage;

    private bool started;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        fadePanelImage.gameObject.SetActive(false);
        fadePanelImage.color = Color.clear;
    }

    public void StartFade(float duration)
    {
        if (started) return;

        StartCoroutine(FadeCoroutine(duration));
    }

    private IEnumerator FadeCoroutine(float duration)
    {
        started = true;

        fadePanelImage.gameObject.SetActive(true);
        fadePanelImage.color = Color.clear;
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            fadePanelImage.color = Color.Lerp(Color.clear, Color.black, t/duration);
            yield return null;
        }
        fadePanelImage.color = Color.black;

        yield return SceneManager.LoadSceneAsync("Tetris", LoadSceneMode.Single);

        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            fadePanelImage.color = Color.Lerp(Color.black, Color.clear, t/duration);
            yield return null;
        }
        fadePanelImage.color = Color.clear;
        fadePanelImage.gameObject.SetActive(false);

        Destroy(gameObject);
    }
}
