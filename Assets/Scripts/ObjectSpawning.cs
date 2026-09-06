using System.Collections.Generic;
using Math = System.Math;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectSpawning : MonoBehaviour
{
    public InputActionReference spawnButton;
    public InputActionReference clearButton;
    public TrackControllers track;
    private List<GameObject> spawnedObjects = new();
    private List<Vector3> velocities = new();
    private List <float> gravities = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnButton.action.Enable();
        spawnButton.action.performed += (ctx) =>
        {
            GameObject cube = Resources.Load<GameObject>("Cube");
            RaycastHit target = track.getRightRay();
            float gravity = Random.Range(3f, 50f);
            Vector3 position = track.getRightPosition();
            spawnedObjects.Add(Instantiate(cube, position, Quaternion.identity));
            gravities.Add(gravity);
            velocities.Add(getStartVelocity(position, track.getRightOrientation(), gravity));
        };
        clearButton.action.Enable();
        clearButton.action.performed += (ctx) =>
        {
            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                Destroy(spawnedObjects[i]);
            }
            spawnedObjects.Clear();
            velocities.Clear();
            gravities.Clear();
        };
    }

    Vector3 getStartVelocity(Vector3 position, Vector3 rotation, float gravity)
    {
        // Calculate Orthogonal vector using Gram-Schmidt process
        float distance = (float) Math.Sqrt(Math.Pow(position.x, 2) + Math.Pow(position.y - 7.5, 2) + Math.Pow(position.z, 2));
        float vel = (float) Math.Sqrt(gravity / distance);

        Vector3 remove = new(-position.x, -(position.y - 7.5f), -position.z);
        Vector3 proj = Vector3.Dot(remove, rotation) / Vector3.Dot(remove, remove) * remove;

        // v is the velocity -- should have magnitude vel and be orthogonal to the vector from position to orbit point
        Vector3 v = rotation - proj;
        v /= v.magnitude;
        v *= vel;
        return v;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < spawnedObjects.Count; i++) {
            Vector3 position = spawnedObjects[i].GetComponent<Transform>().position;
            float distance = (float) Math.Sqrt(Math.Pow(position.x, 2) + Math.Pow(position.y - 7.5, 2) + Math.Pow(position.z, 2));
            float xComp = position.x / distance;
            float yComp = (position.y - 7.5f) / distance;
            float zComp = position.z / distance;
            float acceleration = (float) (-gravities[i] / Math.Pow(distance, 2));
            Vector3 velocityChange = new(xComp * acceleration, yComp * acceleration, zComp * acceleration);
            velocities[i] += velocityChange * Time.deltaTime;
            spawnedObjects[i].GetComponent<Transform>().Translate(velocities[i] * Time.deltaTime);
        }
    }
}
