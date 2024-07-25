using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Max Values")]
    [SerializeField] public float maxPhysicalHealth;
    [SerializeField] public float maxMentalHealth;
    [SerializeField] public float maxFinancialHealth;
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

    private void Start() {
        currentPhysicalHealth = maxPhysicalHealth;
        currentMentalHealth = maxMentalHealth;
        currentFinancialHealth = maxFinancialHealth;
        currentGoal = 0;
    }

    private void Update() {
        DrainUpdate();
    }

    public void AddPhysical(float amount) {
        currentPhysicalHealth = Mathf.Min(maxPhysicalHealth, currentPhysicalHealth + amount);
    }
    public void AddMental(float amount) {
        currentMentalHealth = Mathf.Min(maxMentalHealth, currentMentalHealth + amount);
    }
    public void AddFinancial(float amount) {
        currentFinancialHealth = Mathf.Min(maxFinancialHealth, currentFinancialHealth + amount);
    }
    public void AddGoal(float amount) {
        currentGoal = Mathf.Min(maxGoal, currentGoal + amount);
    }

    private void DrainUpdate() {
        currentPhysicalHealth -= physicalDrain * Time.deltaTime;
        currentMentalHealth -= mentalDrain * Time.deltaTime;
        currentFinancialHealth -= financialDrain * Time.deltaTime;
    }


}
