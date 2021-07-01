using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class VRMap {
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;


    public void Map() {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    
    }
}
public class VRRig : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;
    public float turnSmooth = 1.0f;
    public GameObject chair;

    public Transform headContraint;
    private Vector3 headBodyOffset;
    // Start is called before the first frame update
    void Start()
    {
        headBodyOffset = transform.position - headContraint.position;
        //chair.transform.position = headContraint.position + headBodyOffset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = headContraint.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward,
            Vector3.ProjectOnPlane(headContraint.forward, Vector3.up).normalized,Time.deltaTime* turnSmooth);
        //chair.transform.position = headContraint.position + headBodyOffset;
        chair.transform.forward = Vector3.Lerp(chair.transform.forward,
            Vector3.ProjectOnPlane(headContraint.forward, Vector3.up).normalized, Time.deltaTime * turnSmooth);
        head.Map();
        rightHand.Map();
        leftHand.Map();
    }
}
