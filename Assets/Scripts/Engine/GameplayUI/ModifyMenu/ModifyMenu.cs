using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ElementData;

public class ModifyMenu : MonoBehaviour
{
    public static ModifyMenu Instance { get; set; } = null;

    public Dictionary<string, ConfigValue> ConfigDict = new Dictionary<string, ConfigValue>();

    [SerializeField] GameObject TextModule;
    [SerializeField] GameObject CheckModule;
    [SerializeField] GameObject ValueModule;

    [SerializeField] GameObject panel;

    private Element element;

    private int nextPos;
    int NextPos{
        get
        {
            return nextPos -= 30;
        }
        set
        {
            nextPos = value;
        }
    }



    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
        {
            Instance = this;
        }
    }

    public void Open(Element element)
    {
        transform.localScale = Vector3.one;
        NextPos = 300;

        this.element = element;

        ElementData data = element.Data;

        for(int i = 0; i < data.Properties.Count; i++)
        {
            AddModule(data.Properties[i], data.PropertyValues[i]);
        }
        AddTextModule("Velocity", () => element.Velocity.ToString() );
        AddTextModule("Acceleration", () => element.Acceleration.ToString());
        AddTextModule("MagneticForce", () => element.MagneticForce.ToString());
        AddTextModule("ElectricForce", () => element.ElectricForce.ToString());
    }

    public void Close()
    {
        transform.localScale = Vector3.zero;
        this.element = null;
        ConfigDict.Clear();
        foreach(Transform t in panel.transform)
        {
            Destroy(t.gameObject);
        }
    }


    private void AddModule(ElementProperties property, float value)
    {
        switch (property)
        {
            case ElementProperties.Conductive:
                AddCheckModule(value, "Conductive");
                ConfigDict["Conductive"].ConfigChanged.AddListener((ConfigValue) => element.SetConductivity(ConfigValue.Checkbox ? 1 : 0));
                break;
            case ElementProperties.Hollow:
                AddCheckModule(value, "Hollow");
                ConfigDict["Hollow"].ConfigChanged.AddListener((ConfigValue) => element.SetHollow(ConfigValue.Checkbox ? 1 : 0));
                break;
            case ElementProperties.Mass:
                AddValueModule(value, "Mass", 0.1f, float.MaxValue);
                ConfigDict["Mass"].ConfigChanged.AddListener((ConfigValue) => element.SetMass(ConfigValue.Value));
                break;
            case ElementProperties.Radius:
                AddValueModule(value, "Radius", 0, 100);
                ConfigDict["Radius"].ConfigChanged.AddListener((ConfigValue) => element.SetRadius(ConfigValue.Value));
                break;
            case ElementProperties.Width:
                AddValueModule(value, "Width", 0, 100);
                ConfigDict["Width"].ConfigChanged.AddListener((ConfigValue) => element.SetWidth(ConfigValue.Value));
                break;
            case ElementProperties.Length:
                AddValueModule(value, "Length", 0, 100);
                ConfigDict["Length"].ConfigChanged.AddListener((ConfigValue) => element.SetLength(ConfigValue.Value));
                break;
            case ElementProperties.Charge:
                AddValueModule(value, "Charge", float.MinValue, float.MaxValue);
                ConfigDict["Charge"].ConfigChanged.AddListener((ConfigValue) => element.SetCharge(ConfigValue.Value));
                break;
            case ElementProperties.PoleStrength:
                AddValueModule(value, "PoleStrength", 0, float.MaxValue);
                ConfigDict["PoleStrength"].ConfigChanged.AddListener((ConfigValue) => element.SetPoleStrength(ConfigValue.Value));
                break;
        }
    }

    private void AddCheckModule(float value, string name)
    {
        GameObject module = Instantiate(CheckModule, panel.transform, false);
        module.transform.localPosition = new Vector3(0, NextPos, 0);
        module.name = name;
        CheckBox checkbox = module.GetComponentInChildren<CheckBox>();

        checkbox.Label.text = name;
        checkbox.Name = name;

        ConfigDict.Add(name, new ConfigValue(ConfigValue.Type.Checkbox, value > 0));
    }

    private void AddValueModule(float value, string name, float min, float max)
    {
        GameObject module = Instantiate(ValueModule, panel.transform, false);
        module.transform.localPosition = new Vector3(-40, NextPos, 0);
        module.name = name;
        ValueField valueField = module.GetComponentInChildren<ValueField>();

        valueField.Label.text = name;
        valueField.Name = name;


        ConfigDict.Add(name, new ConfigValue(flags: ConfigValue.Type.NumField, valueRange: new float[] { min, value, max }));
    }

    private void AddTextModule(string name, Measurement.GetAssignedMeasurement getAssignedMeasurement)
    {
        GameObject module = Instantiate(TextModule, panel.transform, false);
        module.transform.localPosition = new Vector3(-120, NextPos, 0);
        module.name = name;
        module.GetComponentInChildren<Measurement>().getAssignedMeasurement = () => name + ": " + getAssignedMeasurement;
    }
}