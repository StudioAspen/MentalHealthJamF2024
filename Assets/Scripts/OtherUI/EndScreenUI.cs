using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] GameObject endScreen;
    [SerializeField] RectTransform postItTransform;
    [SerializeField] TMP_Text endText;

    [SerializeField] TMP_Text physicalStatsText;
    [SerializeField] TMP_Text mentalStatsText;
    [SerializeField] TMP_Text financialStatsText;

    private void Start() {
        endScreen.SetActive(false);
    }

    public void ShowResultsEndScreen(float physicalPercent, float mentalPercent, float financialPercent) {
        endScreen.SetActive(true);

        float highestHealth = Mathf.Max(physicalPercent, mentalPercent, financialPercent);

        if (highestHealth == physicalPercent) endText.text = "Gym rat.";
        if (highestHealth == mentalPercent) endText.text = "You got your funny up... maybe it's time to get your money and body up?";
        if (highestHealth == financialPercent) endText.text = "You're rich now... lonely and mentally torn apart, but rich!";

        if(physicalPercent > 0.3f && physicalPercent < 0.3666f && mentalPercent > 0.3f && mentalPercent < 0.3666f && financialPercent > 0.3f && financialPercent < 0.3666f)
            endText.text = "Balanced as all things should be.";

        physicalStatsText.text = $": {Mathf.Round(physicalPercent * 10000f) / 100}%";
        mentalStatsText.text = $": {Mathf.Round(mentalPercent * 10000f) / 100}%";
        financialStatsText.text = $": {Mathf.Round(financialPercent * 10000f) / 100}%";
    }

    public void ShowEndScreen(string msg)
    {
        endScreen.SetActive(true);

        endText.text = msg;

        StartCoroutine(EndScreenAnimation());
    }

    private IEnumerator EndScreenAnimation()
    {
        postItTransform.localPosition = 1150f * Vector3.down;
        for(float t = 0; t < 1f; t += Time.unscaledDeltaTime)
        {
            float c4 = (2 * Mathf.PI) / 3;
            postItTransform.localPosition = 1150f * (1-(t == 0 ? 0 : t == 1 ? 1 : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1)) * Vector3.down;

            yield return null;
        }
        postItTransform.localPosition = Vector3.zero;
    }
}
