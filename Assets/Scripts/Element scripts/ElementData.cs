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
        Sphere, Point, Plane, Cylinder, PointVoltmeter, Magnet,
        Wire, CircuitVoltmeter, Switch, Inductor, Resistor, Capacitor
    }

    public enum ElementProperties
    {
        Mass, Radius, Width, Length, Hollow, Conductive, Charge,
        Resistance, Capacitance, Inductance, PoleStrength,
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
            }
        }
        return Prefab;
    }
}
