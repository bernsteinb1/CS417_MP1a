using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
// using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class TrackControllers : MonoBehaviour
{
    public InputActionReference leftControllerPosition, leftControllerRotation, rightControllerPosition, rightControllerRotation;
    private Vector3 leftPos, leftRot, rightPos, rightRot;
    public XROrigin xrOrigin;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        leftControllerPosition.action.Enable();
        leftControllerRotation.action.Enable();
        rightControllerPosition.action.Enable();
        rightControllerRotation.action.Enable();
    }

    void OnDisable()
    {
        leftControllerPosition.action.Disable();
        leftControllerRotation.action.Disable();
        rightControllerPosition.action.Disable();
        rightControllerRotation.action.Disable();
    }

    public RaycastHit getRightRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(rightPos, rightRot, out hit, 50))
        {
            return hit;
        }
        return new();
    }

    public RaycastHit getLeftRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(leftPos, leftRot, out hit, 50))
        {
            return hit;
        }
        return new();
    }

    public Vector3 getRightOrientation()
    {
        return rightRot;
    }

    // Update is called once per frame
    void Update()
    {
        Transform t = xrOrigin.transform;
        leftPos = t.TransformPoint(leftControllerPosition.action.ReadValue<Vector3>());
        rightPos = t.TransformPoint(rightControllerPosition.action.ReadValue<Vector3>());
        leftRot = t.rotation * leftControllerRotation.action.ReadValue<Quaternion>() * Vector3.forward;
        rightRot = t.rotation * rightControllerRotation.action.ReadValue<Quaternion>() * Vector3.forward;
    }
}
