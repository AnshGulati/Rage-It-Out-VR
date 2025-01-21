using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedShattering : MonoBehaviour
{
    public GameObject shatteredPrefab; // Shattered version of the object
    public float velocityNeededToShatter = 10f; // Threshold velocity to shatter
    public GameObject brokenVersion; // Broken version for Baton interaction

    private bool isHit = false; // Prevent multiple triggers
    private AudioSource audioSource;

    private void Start()
    {
        // Initialize the audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // Assign an audio clip in the inspector or dynamically if needed
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isHit) return; // Avoid duplicate shattering

        // Velocity-based shattering logic
        if (collision.gameObject.CompareTag("right hand") || collision.gameObject.CompareTag("left hand"))
        {
            Vector3 velocity = CalculateVelocity(collision.gameObject);
            float relativeVelocity = (velocity - GetComponent<Rigidbody>().velocity).magnitude;

            if (relativeVelocity > velocityNeededToShatter)
            {
                ShatterObject(); // Shatter based on velocity
                return;
            }
        }

        // Baton-based destruction logic
        if (collision.gameObject.CompareTag("Baton"))
        {
            ReplaceWithBrokenVersion();
            return;
        }

        // Velocity-based shattering for general collisions
        if (collision.relativeVelocity.magnitude > velocityNeededToShatter)
        {
            ShatterObject(); // Shatter based on collision velocity
        }
    }

    private Vector3 CalculateVelocity(GameObject obj)
    {
        HandPreviousPosition hand = obj.GetComponent<HandPreviousPosition>();
        if (hand != null)
        {
            Vector3 previousPosition = hand.GetPreviousPosition();
            return (obj.transform.position - previousPosition) / Time.deltaTime;
        }
        return Vector3.zero;
    }

    private void ShatterObject()
    {
        isHit = true;

        // Play sound
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        // Instantiate the shattered version
        GameObject shatteredInstance = Instantiate(shatteredPrefab, transform.position, transform.rotation);

        // Apply explosion force to the pieces
        foreach (Transform piece in shatteredInstance.transform)
        {
            Rigidbody rb = piece.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(200f, transform.position, 100f);
            }
        }

        // Destroy the original object
        Destroy(gameObject);
    }

    private void ReplaceWithBrokenVersion()
    {
        isHit = true;

        // Play sound
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        // Instantiate the broken version
        GameObject brokenInstance = Instantiate(brokenVersion, transform.position, transform.rotation);

        // Disable the original object's renderer
        GetComponent<MeshRenderer>().enabled = false;

        // Clean up the broken version after some time
        StartCoroutine(DestroyReplacement(brokenInstance, 4f));

        // Destroy the original object
        Destroy(gameObject, 4.5f);
    }

    private IEnumerator DestroyReplacement(GameObject replacement, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (replacement != null)
        {
            Destroy(replacement);
        }
    }
}
