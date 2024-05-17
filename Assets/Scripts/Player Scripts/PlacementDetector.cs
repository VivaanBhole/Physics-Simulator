using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEditorInternal;
using UnityEngine;

public class PlacementDetector : MonoBehaviour
{
    public bool isValid;
    private Color _invalid = Color.red,
        _valid = Color.green,
        _ogColor;
    private int _overlaps;
    private Renderer _renderer;
    private bool _circuitElement = false;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _ogColor = _renderer.material.color;
        if(_overlaps>0)
            _renderer.material.color = _invalid;
        else
            _renderer.material.color = _valid;

        isValid = _overlaps <= 0;

        if (GetComponentInChildren<CircuitElement>())
            circuitOverride();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_overlaps);
    }

    private void OnDestroy()
    {
        _renderer.material.color = _ogColor;
    }
    private void OnTriggerEnter()
    {
        if (_circuitElement) return;

        _overlaps++;
        if (_overlaps > 0)
        {
            _renderer.material.color = _invalid;
            isValid = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("colliding with: "+other.name);
    }
    private void OnTriggerExit()
    {
        if (_circuitElement) return;

        _overlaps--;
        if (_overlaps == 0)
        {
            isValid = true;
            _renderer.material.color = _valid;
        }
    }

    public void circuitOverride()
    {
        isValid = true;
        _renderer.material.color = _valid;
        _circuitElement = true;
    }
}
