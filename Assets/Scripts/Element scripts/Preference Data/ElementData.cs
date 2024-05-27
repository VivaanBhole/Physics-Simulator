using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class ElementData : ScriptableObject
{
    public enum ElementType
    {
        Sphere, Point, Plane, Cylinder, PointVoltmeter,
        Wire, CircuitVoltmeter, Switch, Inductor, Resistor, Capacitor
    }

    public enum ElementProperties
    {
        Radius, Width, Length, Hollow, Conductive, Charge, Finite,
        Resistance, Capacitance, Inductance,
        LinearChargeDensity, AreaChargeDensity, VolumeChargeDensity
    }
    [SerializeField] public List<ElementProperties> Properties;
    [SerializeField] public List<float> PropertyValues;

    [SerializeField] public ElementType Type;
    [SerializeField] public Sprite Icon;
    [SerializeField] public string Name;
    [SerializeField] public GameObject Prefab;


    public void SetPropertyDefault(ElementProperties ep, float val)
    {
        if (Properties.Contains(ep))
        {
            PropertyValues[Properties.IndexOf(ep)] = val;
        }
        else
        {
            Properties.Add(ep);
            PropertyValues.Add(val);
        }
    }

    public GameObject GetDefaultObject()
    {
        Element element = Prefab.GetComponentInChildren<Element>();
        for(int i = 0; i < Properties.Count; i++)
        {
            switch (Properties[i])
            {
                case ElementProperties.Radius:
                    element.SetRadius(PropertyValues[i]);
                    break;
                case ElementProperties.Width:
                    element.SetWidth(PropertyValues[i]);
                    break;
                case ElementProperties.Length:
                    element.SetLength(PropertyValues[i]);
                    break;
                case ElementProperties.Hollow:
                    element.SetHollow(PropertyValues[i]);
                    break;
                case ElementProperties.Conductive:
                    element.SetConductivity(PropertyValues[i]);
                    break;
                case ElementProperties.Charge:
                    element.SetCharge(PropertyValues[i]);
                    break;
                case ElementProperties.Finite:
                    element.SetFinite(PropertyValues[i]);
                    break;
                case ElementProperties.LinearChargeDensity:
                    element.SetLinearChargeDensity(PropertyValues[i]);
                    break;
                case ElementProperties.AreaChargeDensity:
                    element.SetAreaChargeDensity(PropertyValues[i]);
                    break;
                case ElementProperties.VolumeChargeDensity:
                    element.SetVolumeChargeDensity(PropertyValues[i]);
                    break;
            }
        }
        return Prefab;
    }
}
