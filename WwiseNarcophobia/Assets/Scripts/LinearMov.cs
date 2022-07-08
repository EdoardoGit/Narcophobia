using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMov : MonoBehaviour
{

    float timeCounter;

    public float speed;
    public float width;
    public float height;

    private void Start()
    {
        timeCounter = 0;
    }

    private void Update()
    {
        timeCounter += Time.deltaTime * speed;

        //float x = Mathf.Cos(timeCounter)*width;
        float z = Mathf.Cos(timeCounter) * height;

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
    }
}
