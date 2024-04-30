using UnityEngine;
using UnityEngine.UI;


public class CheckboxHandler : MonoBehaviour
{
    void Start()
    {
        var configValue = Config.Instance.ConfigDict[transform.parent.name];

        gameObject.GetComponent<Toggle>().onValueChanged.AddListener
            (
                (isChecked) =>
                {
                    //Set Corresponding Config Boolean to Checkbox Value
                    configValue.Checkbox = isChecked;
                }
            );

    }
}
