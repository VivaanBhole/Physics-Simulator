using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySliderHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() 
    {
        Slider[] sliders = gameObject.GetComponentsInChildren<Slider>();
        //This was the only way because assigning a refernece to a local variable would use that variable, not the value it was assigned when set
        sliders[0].onValueChanged.AddListener((val) => Settings.Instance.Sensitivity = val/100f);

        sliders[0].value = Settings.Instance.Sensitivity;
    }
}
