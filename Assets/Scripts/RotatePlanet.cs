using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    Transform t;
    float rotational_velocity = 15f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        t = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        t.Rotate(new Vector3(0, rotational_velocity * Time.deltaTime, 0));
    }
}
