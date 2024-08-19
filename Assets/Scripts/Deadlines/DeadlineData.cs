using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DeadlineData", order = 1)]
public class DeadlineData : ScriptableObject {
    public string deadlineName;
    public Color color;
}
