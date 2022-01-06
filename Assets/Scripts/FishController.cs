using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    //Layer Mask for only checking for Tank walls
    [SerializeField] private LayerMask tankLayer;
    //Rigid Body component of this fish
    private Rigidbody _fishRB;
    //Movement speed of this fish
    private float _moveSpeed = 70;
    
    void Start()
    {
        _fishRB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        //Check if the fish is close to a wall
        if(Physics.CheckSphere(transform.localPosition,1f,tankLayer))
        {
            transform.rotation = Quaternion.Euler(0, Random.Range(-170, 170), 0);
        }
        //Move the fish with a constant speed
        _fishRB.velocity = transform.forward * _moveSpeed * Time.deltaTime;
    }
}
