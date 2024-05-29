using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

public class CheckBox : MonoBehaviour
{
    public string Name;
    [SerializeField] public Toggle Toggle;
    [SerializeField] public Text Label;
    public bool IsChecked;
    void Start()
    {
        var configValue = ModifyMenu.Instance.ConfigDict[Name];

        gameObject.GetComponentInChildren<Toggle>().onValueChanged.AddListener
            (
                (isChecked) =>
                {
                    //Set Corresponding Config Boolean to Checkbox Value
                    configValue.Checkbox = isChecked;
                }
            );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckBoxChanged (bool val)
    {
        IsChecked = val;

    }
}
