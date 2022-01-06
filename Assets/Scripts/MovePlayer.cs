/*Note:
* This script should be attached to the player
*/
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // Rigid Body component attached to the player script
    private Rigidbody _playerRB;
    //Player speed able to be changed from the inspector
    [SerializeField] private float _playerSpeed = 2.0f;
    //The rotation smoothing value
    [SerializeField] private float smooth;
    // Private game over variable
    private bool isGameOver;

    //Delegate variable for the player based events
    public delegate void PlayerEvents();
    // Event when a wish is eaten and when the game is over
    public static event PlayerEvents fishEaten, gameOver;

    void Start()
    {
        // Try to get the Character Controller component for the player
        if (gameObject.TryGetComponent<Rigidbody>(out _playerRB))
        {
            Debug.Log("Successfully got the Rigid Body component for " + gameObject.name);
        }
        else
        {
            Debug.LogError("Error in getting the Rigid Body component for " + gameObject.name);
        }

    }


    void Update()
    {
        if (!isGameOver)
        {
            // Movement of the player with mouse only
            float yRot = Input.GetAxis("Mouse X") * Time.deltaTime * smooth;
            float xRot = Input.GetAxis("Mouse Y") * Time.deltaTime * smooth;
            transform.localEulerAngles += new Vector3(xRot, yRot, 0);

        }
    }

    void FixedUpdate()
    {
        if (!isGameOver)
        {
            //Move player at constant velocity
            _playerRB.velocity = -transform.forward * _playerSpeed * Time.deltaTime;
        }else{
            _playerRB.velocity = Vector3.zero;
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

    private void OnCollisionEnter(Collision col)
    {
        //If the collider hit is a regular fish
        if (col.collider.CompareTag("fish"))
        {
            fishEaten?.Invoke();
            col.gameObject.SetActive(false);
        }
        //If the collider hit is a sick fish
        if (col.collider.CompareTag("sick"))
        {
            gameOver?.Invoke();
            isGameOver = true;
            col.gameObject.SetActive(false);
        }

    }
}
