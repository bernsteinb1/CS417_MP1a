using Unity.VisualScripting;
using UnityEngine;
using Math = System.Math;

public class Orbit : MonoBehaviour
{
    Vector3 velocity;
    const float gravity = 25f;
    Transform t;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        t = this.GetComponent<Transform>();
        // Vector3 position = t.position;
        // float distance = (float) Math.Sqrt(Math.Pow(position.x, 2) + Math.Pow(position.y - 7.5, 2) + Math.Pow(position.z, 2));
        // velocity = new Vector3(0, 0, (float) Math.Sqrt(gravity / distance));
        velocity = getStartVelocity();
    }

    Vector3 getStartVelocity()
    {
        Vector3 position = t.position;
        float distance = (float) Math.Sqrt(Math.Pow(position.x, 2) + Math.Pow(position.y - 7.5, 2) + Math.Pow(position.z, 2));
        float xComp = -position.x;
        float yComp = -(position.y - 7.5f);
        float zComp = -position.z;
        float vel = (float) Math.Sqrt(gravity / distance);

        float z = (-xComp * 0.5f + -yComp * 1f) / zComp;
        Vector3 v = new(0.5f, 1f, z);
        v /= v.magnitude;
        v *= vel;
        return v;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = t.position;
        float distance = (float) Math.Sqrt(Math.Pow(position.x, 2) + Math.Pow(position.y - 7.5, 2) + Math.Pow(position.z, 2));
        float xComp = position.x / distance;
        float yComp = (position.y - 7.5f) / distance;
        float zComp = position.z / distance;
        float acceleration = (float) (-gravity / Math.Pow(distance, 2));
        Vector3 velocityChange = new(xComp * acceleration, yComp * acceleration, zComp * acceleration);
        velocity += velocityChange * Time.deltaTime;
        t.Translate(velocity * Time.deltaTime);
    }
}
