using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class ChangeLight : MonoBehaviour
{
    public InputActionReference button;
    public Light light;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.action.Enable();
        button.action.performed += (ctx) =>
        {
            light.color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
