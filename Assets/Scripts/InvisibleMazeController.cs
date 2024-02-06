using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleMazeController : MonoBehaviour
{
    [SerializeField] SplineComputer spline;
    [SerializeField] GameObject player;
    [SerializeField] Transform respawnPosition;
    [SerializeField] float maxDistanceBeforeDeath;
    public void StartTrackingPlayer() {
        UpdateTicker.Subscribe(UpdatePlayerPos);
    }
    public void StopTrackingPlayer() { 
        UpdateTicker.Unsubscribe(UpdatePlayerPos);
    }
    void UpdatePlayerPos() {
        SplineSample sample = new SplineSample();
        Vector3 playerPos = player.transform.position;
        spline.Project(playerPos, ref sample);
        Vector3 pointPos = sample.position;
        float distance = Mathf.Sqrt(Mathf.Pow(playerPos.x - pointPos.x, 2) + Mathf.Pow(playerPos.z - pointPos.z, 2));
        Debug.Log(distance);
        if (distance > maxDistanceBeforeDeath) {
            Lose();
        }
    }
    void Lose() {
        player.transform.position = respawnPosition.position;
    }
}
