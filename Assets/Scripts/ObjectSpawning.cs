using System.Collections.Generic;
using Math = System.Math;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectSpawning : MonoBehaviour
{
    public InputActionReference spawnButton;
    public InputActionReference clearButton;
    public TrackControllers track;
    private List<GameObject> spawnedObjects;
    private List<Vector3> velocities;
    private List <float> gravities;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnButton.action.Enable();
        spawnButton.action.performed += (ctx) =>
        {
            GameObject cube = Resources.Load<GameObject>("Cube");
            RaycastHit target = track.getRightRay();
            spawnedObjects.Add(Instantiate(cube, target.point, Quaternion.identity));
            float gravity = Random.Range(3f, 50f);
            gravities.Add(gravity);
            velocities.Add(getStartVelocity(target.point, track.getRightOrientation(), gravity));
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
        float distance = (float) Math.Sqrt(Math.Pow(position.x, 2) + Math.Pow(position.y - 7.5, 2) + Math.Pow(position.z, 2));
        float xComp = -position.x;
        float yComp = -(position.y - 7.5f);
        float zComp = -position.z;
        float vel = (float) Math.Sqrt(gravity / distance);

        float z = (-xComp * rotation.x + -yComp * rotation.y) / zComp;
        Vector3 v = new(rotation.x, rotation.y, z);
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
