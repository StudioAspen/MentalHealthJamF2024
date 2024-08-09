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

        physicalFillTransform.sizeDelta = new Vector2(physicalFillTransform.sizeDelta.x, maxHeight * (owner.currentPhysicalHealth / owner.maxGoal));

        mentalFillTransform.position = new Vector2(mentalFillTransform.position.x, physicalFillTransform.position.y + physicalFillTransform.sizeDelta.y);
        mentalFillTransform.sizeDelta = new Vector2(mentalFillTransform.sizeDelta.x, maxHeight * (owner.currentMentalHealth/owner.maxGoal));

        financialFillTransform.position = new Vector2(financialFillTransform.position.x, physicalFillTransform.position.y + mentalFillTransform.sizeDelta.y + physicalFillTransform.sizeDelta.y);
        financialFillTransform.sizeDelta = new Vector2(financialFillTransform.sizeDelta.x, maxHeight * (owner.currentFinancialHealth / owner.maxGoal));
    }
}
