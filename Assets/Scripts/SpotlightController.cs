using UnityEngine;
using System.Collections;

public class SpotlightController : MonoBehaviour
{
    public Transform[] gameObjectsToLookAt;
    public float lerpSpeed = 2f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 5f;

    private int currentIndex = 0;
    private Transform currentTarget;
    private bool isLerping = false;

    void Start()
    {
        StartCoroutine(LookAtTargets());
    }

    IEnumerator LookAtTargets()
    {
        while (true)
        {
            currentTarget = gameObjectsToLookAt[currentIndex];
            isLerping = true;
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            currentIndex = (currentIndex + 1) % gameObjectsToLookAt.Length;
        }
    }

    void Update()
    {
        if (isLerping)
        {
            Vector3 direction = currentTarget.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                isLerping = false;
            }
        }
    }
}