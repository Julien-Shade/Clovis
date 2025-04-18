using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Move(Vector2 inputMovement)
    {
        Vector3 playerMovement = new Vector3(inputMovement.x, inputMovement.y, 0); // construction / conversion  vector2 en vector3 pour Translate

        transform.Translate(playerMovement * moveSpeed * Time.deltaTime); // player move !
    }

}
