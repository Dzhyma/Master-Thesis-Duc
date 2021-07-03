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
    public Vector3 headBodyOffset;
    public Vector3 chairOffSet;
    // Start is called before the first frame update
    void Start()
    {
        headBodyOffset = transform.position - headContraint.position;
        chairOffSet = headBodyOffset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = headContraint.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward,
            Vector3.ProjectOnPlane(headContraint.forward, Vector3.up).normalized,Time.deltaTime* turnSmooth);
        //chair.transform.position = headContraint.position + headBodyOffset;
        var old_y = chair.transform.position.y;
        chair.transform.position = headContraint.position;// + chairOffSet;
        chair.transform.position = new Vector3(chair.transform.position.x, old_y, chair.transform.position.z);
        chair.transform.forward = Vector3.Lerp(chair.transform.forward,
            Vector3.ProjectOnPlane(headContraint.forward, Vector3.up).normalized, Time.deltaTime * turnSmooth);
        head.Map();
        rightHand.Map();
        leftHand.Map();
    }
}
