using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SImplePlayerInput : MonoBehaviour
{

    [SerializeField] SimplePlayerMovement playerMovement;


    [SerializeField] float XAxis;
    [SerializeField] float YAxis;


    void Update()
    {
        // input
        XAxis = Input.GetAxis("Horizontal");
        YAxis = Input.GetAxis("Vertical");


        Vector2 inputMovement = new Vector2(XAxis, YAxis); // construction vector mouvment

        playerMovement.Move(inputMovement); // déclencher movement ek movement 
    }
}
