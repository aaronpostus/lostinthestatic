using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    // The name of the scene to load
    public string allShards, notAllShards;
    public ShardNumReference shardCount;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Load the scene
            if (shardCount.Value == 5)
            {
                SceneManager.LoadScene(allShards);
            }
            else {
                SceneManager.LoadScene(notAllShards);
            }

        }
    }
}
