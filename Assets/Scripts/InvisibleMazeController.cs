using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleMazeController : MonoBehaviour
{
    [SerializeField] SplineComputer spline;
    [SerializeField] SplineFollower splineFollower;
    [SerializeField] GameObject player;
    [SerializeField] Transform respawnPosition;
    [SerializeField] float maxDistanceBeforeDeath;
    [SerializeField] float distanceAheadOfCar;
    [SerializeField] Radio radio;
    [SerializeField] StaticController staticController;
    private InvisibleMazeChannel channel;
    private bool trackPlayer;
    private void Start()
    {
        splineFollower.gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (!trackPlayer) return;

        if (radio.GetFrequency() != 98.3f) {
            radio.JumpToPreset(98.3f);
        }

        SplineSample sample = new SplineSample();
        Vector3 playerPos = player.transform.position;
        spline.Project(playerPos, ref sample);
        Vector3 pointPos = sample.position;
        float distance = Mathf.Sqrt(Mathf.Pow(playerPos.x - pointPos.x, 2) + Mathf.Pow(playerPos.z - pointPos.z, 2));
        Debug.Log(distance);
        channel.UpdateBalance(1 - (distance / maxDistanceBeforeDeath));

        
        splineFollower.SetPercent(sample.percent + distanceAheadOfCar);

        if (distance > maxDistanceBeforeDeath)
        {
            Lose();
        }
    }


    public void StartTrackingPlayer() {
        trackPlayer = true;
        splineFollower.gameObject.SetActive(true);
        radio.JumpToPreset(98.3f);
    }

    public void StopTrackingPlayer() {
        trackPlayer = false;
        splineFollower.gameObject.SetActive(false);
    }

    public void PassChannel(InvisibleMazeChannel channel) {
        this.channel = channel;
    }

    void Lose() {
        

        StartCoroutine(ReleaseOnNextStep());
    }

    private IEnumerator ReleaseOnNextStep() {
        Rigidbody rb = player.transform.GetComponent<Rigidbody>();

        if (rb == null) {
            player.transform.rotation = respawnPosition.rotation;
            player.transform.position = respawnPosition.position;
            yield return null;
        }

        rb.isKinematic = true;
        
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rb.interpolation = RigidbodyInterpolation.None;
        yield return new WaitForFixedUpdate();
        player.transform.rotation = respawnPosition.rotation;
        player.transform.position = respawnPosition.position;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        staticController.HotFix();
    }
}
