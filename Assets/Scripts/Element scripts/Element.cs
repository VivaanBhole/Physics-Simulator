using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    //Mass, charge, isConuctive, isLocked, velocity, acceleration, forces

    public float Mass, Charge;

    // Initialize whenever creating an element
    public Rigidbody rb;

    public virtual void SetMass(float m) { Mass = m; }
    public virtual void SetCharge(float c) { Charge = c; }
    public virtual void SetRadius(float r) { }
    public virtual void SetLength(float l) { }
    public virtual void SetWidth(float w) { }
    public virtual void SetHollow(float h) { }
    public virtual void SetConductivity(float c) { }
    public virtual void SetAreaChargeDensity(float d) { }
    public virtual void SetVolumeChargeDensity(float d) { }
    public virtual void SetLinearChargeDensity(float d) { }
    public virtual void SetFinite(float f) { }
    public virtual Vector3 GetMagneticForce(Element other) { return Vector3.zero; }
    public virtual Vector3 GetElectricForce(Element other) { return Vector3.zero; }









}
