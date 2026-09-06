using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleport : MonoBehaviour
{
    public XROrigin xr;
    public InputActionReference telButton;
    private bool inSkybox = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        telButton.action.Enable();
        telButton.action.performed += (ctx) => {
            if (!inSkybox) 
                xr.MoveCameraToWorldLocation(new(0, 26, 0));
            else
                xr.MoveCameraToWorldLocation(new(0, 1, 0));
            inSkybox = !inSkybox;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
