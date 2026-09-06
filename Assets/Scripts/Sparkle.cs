using System;
using System.Drawing;
using NUnit.Framework.Constraints;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sparkle : MonoBehaviour
{
    public InputActionReference action;
    public ParticleSystem sparkle;
    public TrackControllers track;
    public AudioSource soundEffect;
    private float timePassed = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            sparkle.Play();
        };
        action.action.canceled += (ctx) =>
        {
            sparkle.Stop();
            timePassed = 0;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (sparkle.isEmitting)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= (1f / sparkle.emission.rateOverTime.constant))
            {
                soundEffect.Play();
                timePassed = 0;
            }
            RaycastHit p = track.getRightRay();
            sparkle.GetComponent<Transform>().SetPositionAndRotation(p.point, Quaternion.LookRotation(p.normal));
            soundEffect.GetComponent<Transform>().SetPositionAndRotation(p.point, Quaternion.LookRotation(Vector3.up));
        }
    }
}
