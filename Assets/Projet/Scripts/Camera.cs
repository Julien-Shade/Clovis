using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private Vector3 followOffset;
    [SerializeField] private float followSpeed;


    [SerializeField] private Transform lookAtTarget;
    [SerializeField] private Vector3 lookAtOffset = Vector3.up;
    [SerializeField] private float followSpeed2;



    void Update()
    {
        FollowBehavior();
        LootAtBehavior();
    }


    public void FollowBehavior()
    {
        //transform.position = followTarget.position + followOffset;

        Vector3 a = transform.position;
        Vector3 b = followTarget.position + followOffset;
        float t = followSpeed * Time.deltaTime; // temps en une frame

        //transform.position = b; // l'ancien

        transform.position = Vector3.Slerp(a, b, t);

    }

    public void LootAtBehavior()
    {
        //transform.LookAt(lookAtTarget.position + lookAtOffset);

        Vector3 lookAtForward = lookAtTarget.position - transform.position;

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(lookAtForward);

        float tt = followSpeed2 * Time.deltaTime;

        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, tt);
    }

}
