/*Note:
* This script is attached to the Tank gameobject
* Fish can spawn on top of one another, clipping needs to be fixed in the future
*/
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    //Transform Component to spawn fish from Prefabs folder
    [SerializeField] private Transform fishPrefab;
    //Player which is set from the heirarchy
    [SerializeField] private GameObject player;
    //UI score text
    [SerializeField] private TMP_Text scoreText;
    //UI game over Panel
    [SerializeField] private GameObject gameOverPanel;

    int score, maxFishNum;
    // Maintaining a list of all the spawned fishes
    List<Transform> fishData = new List<Transform>();
    //Spawnable Area
    private Vector3 maxArea;
    private Vector3 minArea;

    void OnEnable()
    {   //Registering Events
        MovePlayer.fishEaten += FishEaten;
        MovePlayer.gameOver += GameOver;
    }
    void OnDisable()
    {   //De-registering Events
        MovePlayer.fishEaten -= FishEaten;
        MovePlayer.gameOver -= GameOver;
    }
    void Start()
    {
        //Initializing values
        score = 0;
        maxFishNum = 20;
        maxArea = new Vector3(50, 15, 50);
        minArea = new Vector3(-50, 1, -50);


        for (int i = 0; i < maxFishNum; i++)
        {
            // Randomly set the x,y,z values for the fish position and the direction in which the fish are facing
            float x = Random.Range(minArea.x, maxArea.x);
            float y = Random.Range(minArea.y, maxArea.y);
            float z = Random.Range(minArea.z, maxArea.z);
            Quaternion fistRot = Quaternion.Euler(0, Random.Range(0f, 360.0f), 0);

            // Instantiate the fish and add them to the list of fish
            Vector3 position = new Vector3(x, y, z);
            var fish = Instantiate(fishPrefab, position, fistRot);
            fishData.Add(fish);
            //Try getting the Mesh renderer for each fish
            MeshRenderer fishMesh;
            if(fish.TryGetComponent<MeshRenderer>(out fishMesh)){
                Debug.Log("Successfully Got The Fish Mesh");
            }else{
                Debug.LogError("Error in getting the Mesh Renderer for "+fish.name);
            }
            // Make every 4th fish a sick fish
            if (i % 4 == 0)
            {
                fish.tag = "sick";
                fishMesh.material.color = Color.red;
            }
            else
            {
                fish.tag = "fish";
                fishMesh.material.color = Color.green;
            }

        }
        //Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Event that occurs when a fish is eaten
    void FishEaten()
    {
        score += 10;
        scoreText.text = score.ToString();
        respawnFish();
    }
    //Event when Game Over occurs
    void GameOver()
    {
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void respawnFish()
    {
        // Randomly set the x,y,z values for the fish position and the direction in which the fish are facing
        float x = Random.Range(minArea.x, maxArea.x);
        float y = Random.Range(minArea.y, maxArea.y);
        float z = Random.Range(minArea.z, maxArea.z);
        Quaternion fistRot = Quaternion.Euler(0, Random.Range(0f, 360.0f), 0);

        foreach (var fish in fishData)
        {
            if (!fish.gameObject.activeSelf)
            {
                //Reset the position of the fish to a new value
                fish.position = new Vector3(x, y, z);
                fish.rotation = fistRot;
                fish.gameObject.SetActive(true);
            }
        }
    }
}
