using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] ResourceManager owner;

    [SerializeField] Slider physical;
    [SerializeField] Slider mental;
    [SerializeField] Slider financial;
    [SerializeField] Slider goal;

    // Update is called once per frame
    void Update() {
        BarUpdate();
    }

    private void BarUpdate() {
        physical.value = owner.currentPhysicalHealth / owner.maxPhysicalHealth;
        mental.value = owner.currentMentalHealth / owner.maxMentalHealth;
        financial.value = owner.currentFinancialHealth / owner.maxFinancialHealth;
        goal.value = owner.currentGoal / owner.maxGoal;
    }
}
