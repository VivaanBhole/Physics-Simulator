using System;
using UnityEngine;
using UnityEngine.Events;

public class ConfigValue
{
    public UnityEvent<ConfigValue> ConfigChanged; 

    [Flags]
    public enum Type
    {
        Checkbox = 1,
        NumField = 2
    }

    public readonly Type type;
    private float[] valueRange = { 0, 0, 0 };
    public float Value
    {
        get
        {
            return valueRange[1];
        }
        set
        {
            bool inBounds = valueRange[0] <= value && value <= valueRange[2];

            if (inBounds)
                valueRange[1] = value;
            else if (value <= valueRange[0])
                valueRange[1] = valueRange[0];
            else if (value >= valueRange[2])
                valueRange[1] = valueRange[2];

            ConfigChanged?.Invoke(this);
        }
    }

    private bool _checkbox = false;
    public bool Checkbox 
    {
        get
        {
            return _checkbox;
        }
        set
        {
            _checkbox = value;
            ConfigChanged?.Invoke(this);
        }
    }

    public ConfigValue(Type flags, bool checkboxInitialState = false, float[] valueRange = null)
    {
        ConfigChanged = new();
        type = flags;
        Checkbox = checkboxInitialState;
        if (valueRange is not null)
            this.valueRange = valueRange; 
    }

    public override string ToString()
    {
        return $"Type: {type}, valueRange: {valueRange}, value: {Value}, checkbox: {Checkbox}";
    }

}
