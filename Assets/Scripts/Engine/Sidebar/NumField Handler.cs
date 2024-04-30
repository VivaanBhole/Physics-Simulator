using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumFieldHandler : MonoBehaviour
{
    void Start()
    {
        var configValue = Config.Instance.ConfigDict[transform.parent.name];

        var inputField = gameObject.GetComponent<TMP_InputField>();

        inputField.text = ""+configValue.Value;

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
                
                inputField.text = ""+configValue.Value;
            }
        );
    }
}
