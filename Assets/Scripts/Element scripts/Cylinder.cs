using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : Element
{

    [SerializeField] public float Radius, Length;
    [SerializeField] public bool Hollow, Conductive;
    [SerializeField] public GameObject hollowCylinder, SolidCylinder;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void SetRadius(float r)
    {
        Radius = r;
        transform.localScale = new Vector3(transform.localScale.x, Radius, Radius);
        Debug.Log("this is the new radius: "+r);
    }

    public override void SetLength(float l)
    {
        Length = l;
        transform.localScale += Vector3.right * (l - transform.localScale.x);
    }

    public override void SetHollow(float h)
    {
        Hollow = h > 0;
        hollowCylinder.SetActive(Hollow);
        SolidCylinder.SetActive(!Hollow);

    }

    public override void SetConductivity(float c)
    {
        Conductive = c > 0;
    }

}
