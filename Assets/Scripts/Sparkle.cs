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
        };
        action.action.canceled += (ctx) =>
        {
            buttonPressed = false;
            sparkle.Stop();
            if (currentSound != null) currentSound.Stop();
            if (currentParticle != null) currentParticle.Stop();
            timePassed = 0;
            currentFocus = null;
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
                currentFocus = hitObject;
                Transform newSound = currentFocus.GetComponent<Transform>().Find("Sound");
                Transform newParticle = currentFocus.GetComponent<Transform>().Find("Particle");
                if (newSound == null && newParticle == null)
                {
                    if (currentSound != null)
                        currentSound.Stop();
                    if (currentParticle != null)
                        currentParticle.Stop();
                    sparkle.Play();
                    sparkleSound.Play();
                }
                else {
                    sparkle.Stop();
                    if (newSound != null) 
                    {
                        Debug.Log("Sound should be playing");
                        currentSound = newSound.GetComponent<AudioSource>();
                        currentSound.Play();
                    }
                    if (newParticle != null)
                    {
                        Debug.Log("Particle should be playing");
                        currentParticle = newParticle.GetComponent<ParticleSystem>();
                        currentParticle.Play();
                    }
                }
            }

            if (sparkle.isEmitting)
            {
                timePassed += Time.deltaTime;
                if (timePassed >= (1f / sparkle.GetComponent<ParticleSystem>().emission.rateOverTime.constant))
                {
                    sparkleSound.GetComponent<AudioSource>().Play();
                    timePassed = 0;
                }
                sparkle.GetComponent<Transform>().SetPositionAndRotation(p.point, Quaternion.LookRotation(p.normal));
                sparkleSound.GetComponent<Transform>().SetPositionAndRotation(p.point, Quaternion.LookRotation(Vector3.up));
            }
        }
    }
}
