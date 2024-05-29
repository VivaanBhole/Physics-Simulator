using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Element
{
    // Start is called before the first frame update

    [SerializeField] public float PoleStrength, Length;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void SetLength(float l)
    {
        Length = l;
        transform.localScale += Vector3.right * (l - transform.localScale.x);
    }

    public override void SetPoleStrength(float d) {
        PoleStrength = d;
    }
}
