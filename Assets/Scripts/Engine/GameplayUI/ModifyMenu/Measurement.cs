using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Measurement : MonoBehaviour
{
    public delegate string GetAssignedMeasurement();
    public GetAssignedMeasurement getAssignedMeasurement;
    public Text Text;
    void Update()
    {
        Text.text = getAssignedMeasurement();
    }
}
