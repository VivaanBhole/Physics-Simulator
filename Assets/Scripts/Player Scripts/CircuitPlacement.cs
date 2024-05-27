using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitPlacement : Placement
{
    public override IEnumerator PlaceElement(GameObject element)
    {
        element.name = Time.frameCount + "";
        Collider collider = element.GetComponentInChildren<Collider>();
        Transform elementTransform = element.transform;


        collider.isTrigger = true;

        CanInteract = false;
        Mode = PlacingMode.Place;
        ConnectionPoint.isVisible = true;

        CircuitElement circuitElement = element.GetComponentInChildren<CircuitElement>();
        if (!circuitElement)
        {
            Debug.Log("invalid selection for this scene");
            yield break;
        }


        while ((!Input.GetMouseButton(1)) && Mode == PlacingMode.Place)
        {
            elementTransform.localEulerAngles = RoundDown(_pRotation, _pRotationRes);

            elementTransform.position = circuitElement.GetDesiredPos(transform.position + transform.forward * _pDistance);

            yield return new WaitForSeconds(_pUpdateTime);
        }

        if (Mode != PlacingMode.Place)
        {
            Destroy(element);
        }
        else
        {
            Mode = PlacingMode.None;
            collider.isTrigger = false;
        }

        if (circuitElement)
        {
            circuitElement.AddToCircuit();
        }

        CanInteract = true;
        ConnectionPoint.isVisible = false;

    }
}
