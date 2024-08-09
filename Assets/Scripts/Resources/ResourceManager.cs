using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceManager : MonoBehaviour
{
    [Header("Max Values")]
    [SerializeField] public float maxGoal;

    [Header("Drain Rate")]
    [SerializeField] public float physicalDrain;
    [SerializeField] public float mentalDrain;
    [SerializeField] public float financialDrain;

    [Header("Current Values")]
    public float currentPhysicalHealth;
    public float currentMentalHealth;
    public float currentFinancialHealth;
    public float currentGoal;

    public UnityEvent OnReachGoal = new UnityEvent();

    bool draining = false;

    private void Start() {
        currentGoal = 0;
    }

    private void Update() {
        if(draining) {
            DrainUpdate();
        }

        currentGoal = Mathf.Min(maxGoal, currentPhysicalHealth + currentMentalHealth + currentFinancialHealth);
    }
    public void SetDraining(bool value) {
        draining = value;
    }
    public void AddPhysical(float amount) {
        float futureTotal = currentGoal + amount;
        float amtToSubtract = 0;

        if(futureTotal >= maxGoal)
        {
            amtToSubtract = futureTotal - maxGoal;
        }

        currentPhysicalHealth += amount - amtToSubtract;
    }
    public void AddMental(float amount) {
        float futureTotal = currentGoal + amount;
        float amtToSubtract = 0;

        if (futureTotal >= maxGoal)
        {
            amtToSubtract = futureTotal - maxGoal;
        }

        currentMentalHealth += amount - amtToSubtract;
    }
    public void AddFinancial(float amount) {
        float futureTotal = currentGoal + amount;
        float amtToSubtract = 0;

        if (futureTotal >= maxGoal)
        {
            amtToSubtract = futureTotal - maxGoal;
        }

        currentFinancialHealth += amount - amtToSubtract;
    }

    private void DrainUpdate() {
        currentPhysicalHealth = Mathf.Max(0, currentPhysicalHealth - (physicalDrain * Time.deltaTime));
        currentMentalHealth = Mathf.Max(0, currentMentalHealth - (mentalDrain * Time.deltaTime));
        currentFinancialHealth = Mathf.Max(0, currentFinancialHealth - (financialDrain * Time.deltaTime));
    }
}
