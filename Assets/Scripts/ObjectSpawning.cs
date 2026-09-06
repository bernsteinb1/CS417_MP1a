using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectSpawning : MonoBehaviour
{
    public InputActionReference spawnButton;
    public TrackControllers track;
    public List<GameObject> spawnedObjects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnButton.action.Enable();
        spawnButton.action.performed += (ctx) =>
        {
            GameObject cube = Resources.Load<GameObject>("Cube");
            RaycastHit target = track.getRightRay();
            spawnedObjects.Add(Instantiate(cube, target.point, Quaternion.identity));
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
