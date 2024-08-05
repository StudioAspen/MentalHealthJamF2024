using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [Header("Inital Random Value")]
    [SerializeField] [Range(0f,1f)] float minInitalPercent;
    [SerializeField] [Range(0f, 1f)] float maxInitalPercent;

    [Header("Current Values")]
    public float currentPhysicalHealth;
    public float currentMentalHealth;
    public float currentFinancialHealth;
    public float currentGoal;

    public UnityEvent OnReachGoal = new UnityEvent();

    bool draining = false;

    private void Start() {
        currentPhysicalHealth = maxPhysicalHealth * Mathf.Lerp(minInitalPercent, maxInitalPercent, Random.value);
        currentMentalHealth = maxMentalHealth * Mathf.Lerp(minInitalPercent, maxInitalPercent, Random.value);
        currentFinancialHealth = maxFinancialHealth * Mathf.Lerp(minInitalPercent, maxInitalPercent, Random.value);
        currentGoal = 0;
    }

    private void Update() {
        if(draining) {
            DrainUpdate();
        }
    }
    public void SetDraining(bool value) {
        draining = value;
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
        if(currentGoal >= maxGoal) {
            OnReachGoal.Invoke();
        }
    }

    private void DrainUpdate() {
        currentPhysicalHealth = Mathf.Max(0, currentPhysicalHealth - (physicalDrain * Time.deltaTime));
        currentMentalHealth = Mathf.Max(0, currentMentalHealth - (mentalDrain * Time.deltaTime));
        currentFinancialHealth = Mathf.Max(0, currentFinancialHealth - (financialDrain * Time.deltaTime));
    }
}
