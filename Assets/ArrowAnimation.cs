using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
    public float delta = 0.1f;  // Amount to move left and right from the start point
    public float speed = 0.1f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 v = startPos;
        v.z += delta * Mathf.Sin(Time.time * speed);
        transform.position = v;
    }

    public void ResetPosition() {
        transform.position = startPos;
    }
}
