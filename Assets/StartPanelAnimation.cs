using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanelAnimation : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        GetComponent<RectTransform>().localPosition = 1150f * Vector3.down;
        for (float t = 0; t < 2f; t += Time.unscaledDeltaTime)
        {
            float parameter = t / 2f;
            float c4 = (2 * Mathf.PI) / 3;
            GetComponent<RectTransform>().localPosition = 1150f * (1 - (parameter == 0 ? 0 : parameter == 1 ? 1 : Mathf.Pow(2, -10 * parameter) * Mathf.Sin((parameter * 10f - 0.75f) * c4) + 1)) * Vector3.down;

            yield return null;
        }
        GetComponent<RectTransform>().localPosition = Vector3.zero;
    }
}
