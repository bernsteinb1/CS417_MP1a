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
    public AudioSource sparkleSound;
    public TrackControllers track;
    private float timePassed = 0;
    private bool buttonPressed = false;
    private GameObject currentFocus;
    private AudioSource currentSound;
    private ParticleSystem currentParticle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            buttonPressed = true;
            sparkle.Play();
        };
        action.action.canceled += (ctx) =>
        {
            buttonPressed = false;
            sparkle.Stop();
            timePassed = 0;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonPressed)
        {
            RaycastHit p = track.getRightRay();
            GameObject hitObject = p.collider.gameObject;
            if (hitObject != currentFocus)
            {
                currentSound.Stop();
                currentParticle.Stop();

                currentFocus = hitObject;
                currentSound = currentFocus.GetComponent<Transform>().Find("Sound").GetComponent<AudioSource>();
                currentParticle = currentFocus.GetComponent<Transform>().Find("Particle").GetComponent<ParticleSystem>();
                if (currentSound == null && currentParticle == null)
                {
                    currentSound = sparkleSound;
                    currentParticle = sparkle;
                }
                if (currentSound != null)
                    currentSound.Play();
                if (currentParticle != null)
                    currentParticle.Play();
            }

            if (currentSound == sparkle)
            {
                timePassed += Time.deltaTime;
                if (timePassed >= (1f / sparkle.emission.rateOverTime.constant))
                {
                    sparkleSound.Play();
                    timePassed = 0;
                }
                sparkle.GetComponent<Transform>().SetPositionAndRotation(p.point, Quaternion.LookRotation(p.normal));
                sparkleSound.GetComponent<Transform>().SetPositionAndRotation(p.point, Quaternion.LookRotation(Vector3.up));
            }
        }
    }
}
