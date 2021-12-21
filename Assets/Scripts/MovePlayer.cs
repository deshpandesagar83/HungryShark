/*Note:
* This script should be attached to the player
*/
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // Character controller component attached to the player script
    private CharacterController controller;
    //Player speed able to be changed from the inspector
    [SerializeField] private float playerSpeed = 2.0f;
    //Delegate variable for the player based events
    public delegate void PlayerEvents();
    // Event when a wish is eaten and when the game is over
    public static event PlayerEvents fishEaten, gameOver;
    // Private game over variable
    private bool isGameOver;

    [SerializeField] private float smooth;
    void Start()
    {
        // Try to get the Character Controller component for the player
        if(gameObject.TryGetComponent<CharacterController>(out controller)){
            Debug.Log("Successfully got the Character Contoller component for "+ gameObject.name);
        }else{
            Debug.LogError("Error in getting the Character Contoller component for "+ gameObject.name);
        }   
    }


    void Update()
    {
        if (!isGameOver)
        {
            // Movement of the player with mouse only
            float yRot = playerSpeed * Input.GetAxis("Mouse X")*Time.deltaTime*smooth;
            float xRot = playerSpeed * Input.GetAxis("Mouse Y")*Time.deltaTime*smooth;
            transform.Rotate(xRot,yRot,0);
            controller.Move(-transform.forward * Time.deltaTime * playerSpeed);
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        //If the collider hit is a regular fish
        if (col.CompareTag("fish"))
        {
            fishEaten?.Invoke();
            col.gameObject.SetActive(false);
        }
        //If the collider hit is a sick fish
        if (col.CompareTag("sick"))
        {
            gameOver?.Invoke();
            isGameOver = true;
            col.gameObject.SetActive(false);
        }
    }
}
