using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static Config Instance { get; set; } = null;

    private Dictionary<string, ConfigValue> configList = new Dictionary<string, ConfigValue>
    {
        {"Fields Visible", new ConfigValue(ConfigValue.Type.Checkbox, true)},
        {"Gravity", new ConfigValue(ConfigValue.Type.Checkbox | ConfigValue.Type.NumField, true, new float[] {0, 9.8f, -1}) },
        {"Time Scale",  new ConfigValue(flags: ConfigValue.Type.NumField, valueRange: new float[] {0, 1, -1})}

    };

    private void Awake()
    {

        if (Instance != null && Instance != this)
            Destroy(this);
        else
        {
            Instance = this;
        }
    }
}

