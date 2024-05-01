using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static Config Instance { get; set; } = null;

    public readonly Dictionary<string, ConfigValue> ConfigDict = new Dictionary<string, ConfigValue>
    {
        {"Fields Visible", new ConfigValue(ConfigValue.Type.Checkbox, true)},
        {"Gravity", new ConfigValue(ConfigValue.Type.Checkbox | ConfigValue.Type.NumField, true, new float[] {float.MinValue, Physics.gravity.y, 0}) },
        {"Time Scale",  new ConfigValue(flags: ConfigValue.Type.NumField, valueRange: new float[] {0, 1, 100})}

    };

    private void Awake()
    {

        if (Instance != null && Instance != this)
            Destroy(this);
        else
        {
            Instance = this;
            ConfigChangedHandlers.registerHandlers();
        }
    }
}


