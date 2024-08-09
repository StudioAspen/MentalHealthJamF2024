using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] ResourceManager owner;

    [SerializeField] private RectTransform goalBarTransform;
    [SerializeField] private RectTransform physicalFillTransform;
    [SerializeField] private RectTransform mentalFillTransform;
    [SerializeField] private RectTransform financialFillTransform;

    // Update is called once per frame
    void Update() {
        BarUpdate();
    }

    private void BarUpdate() {
        float maxHeight = goalBarTransform.sizeDelta.y;

        float physicalPercent = owner.currentPhysicalHealth / owner.maxGoal;
        float mentalPercent = owner.currentMentalHealth / owner.maxGoal;
        float financialPercent = owner.currentFinancialHealth / owner.maxGoal;

        Vector2 targetPhysicalSize = new Vector2(physicalFillTransform.sizeDelta.x, maxHeight * physicalPercent);
        physicalFillTransform.sizeDelta = Vector2.Lerp(physicalFillTransform.sizeDelta, targetPhysicalSize, 10f * Time.deltaTime);

        Vector2 targetMentalSize = new Vector2(mentalFillTransform.sizeDelta.x, maxHeight * mentalPercent);
        mentalFillTransform.localPosition = new Vector2(mentalFillTransform.localPosition.x, physicalFillTransform.localPosition.y + physicalFillTransform.sizeDelta.y);
        mentalFillTransform.sizeDelta = Vector2.Lerp(mentalFillTransform.sizeDelta, targetMentalSize, 10f * Time.deltaTime);

        Vector2 targetFinancialSize = new Vector2(financialFillTransform.sizeDelta.x, maxHeight * financialPercent);
        financialFillTransform.localPosition = new Vector2(financialFillTransform.localPosition.x, physicalFillTransform.localPosition.y + mentalFillTransform.sizeDelta.y + physicalFillTransform.sizeDelta.y);
        financialFillTransform.sizeDelta = Vector2.Lerp(financialFillTransform.sizeDelta, targetFinancialSize, 10f * Time.deltaTime);
    }
}
