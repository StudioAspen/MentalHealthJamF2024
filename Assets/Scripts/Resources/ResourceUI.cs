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

    [SerializeField] private RectTransform physicalImage;
    [SerializeField] private RectTransform mentalImage;
    [SerializeField] private RectTransform financialImage;

    // Update is called once per frame
    void Update() {
        BarUpdate();
    }

    private void BarUpdate() {
        float maxHeight = goalBarTransform.sizeDelta.y;
        float width = goalBarTransform.sizeDelta.x;

        float physicalPercent = owner.currentPhysicalHealth / owner.maxGoal;
        float mentalPercent = owner.currentMentalHealth / owner.maxGoal;
        float financialPercent = owner.currentFinancialHealth / owner.maxGoal;

        Vector2 targetPhysicalSize = new Vector2(width, maxHeight * physicalPercent);
        physicalFillTransform.sizeDelta = Vector2.Lerp(physicalFillTransform.sizeDelta, targetPhysicalSize, 10f * Time.deltaTime);
        physicalImage.localPosition = mentalFillTransform.localPosition;

        Vector2 targetMentalSize = new Vector2(width, maxHeight * mentalPercent);
        mentalFillTransform.localPosition = new Vector2(mentalFillTransform.localPosition.x, physicalFillTransform.localPosition.y + physicalFillTransform.sizeDelta.y);
        mentalFillTransform.sizeDelta = Vector2.Lerp(mentalFillTransform.sizeDelta, targetMentalSize, 10f * Time.deltaTime);
        mentalImage.localPosition = financialFillTransform.localPosition;

        Vector2 targetFinancialSize = new Vector2(width, maxHeight * financialPercent);
        financialFillTransform.localPosition = new Vector2(financialFillTransform.localPosition.x, physicalFillTransform.localPosition.y + mentalFillTransform.sizeDelta.y + physicalFillTransform.sizeDelta.y);
        financialFillTransform.sizeDelta = Vector2.Lerp(financialFillTransform.sizeDelta, targetFinancialSize, 10f * Time.deltaTime);
        financialImage.localPosition = financialFillTransform.localPosition + financialFillTransform.sizeDelta.y * Vector3.up;
    }
}
