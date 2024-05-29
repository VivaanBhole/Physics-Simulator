using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueField : MonoBehaviour
{
    public string Name;
    [SerializeField] public TMP_InputField Toggle;
    [SerializeField] public Text Label;
    public float Value;
    void Start()
    {
        var configValue = ModifyMenu.Instance.ConfigDict[transform.name];

        var inputField = gameObject.GetComponentInChildren<TMP_InputField>();

        inputField.text = configValue.Value.ToString();

        // Inbuilt input validation, changes config according to value change
        inputField.onEndEdit.AddListener
        (
            (value) =>
            {
                float fVal;
                if (float.TryParse(value, out fVal))
                {
                    configValue.Value = fVal;
                }

                inputField.text = configValue.Value.ToString();
            }
        );
    }
}
