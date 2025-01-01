using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedVersion : MonoBehaviour
{

    // this where you'll put in the broken version
    public GameObject brokenVersion;

    // create a boolean for whether or not a hit has occurred and set it to false
    public bool isHit = false;

    private AudioSource audioSource;

    

    private void Update()
    {
        //OnCollisionEnter(collision);

        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Baton") && !isHit) 
        {
            
            isHit = true;
            GameObject instantiatedBrokenVersion = Instantiate(brokenVersion, transform.position, transform.rotation);

            audioSource.Play();

            StartCoroutine(DestroyReplacement(instantiatedBrokenVersion, 4f));

           gameObject.GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, 4.5f);

             Destroy(brokenVersion.gameObject, 10f);

            
        }

    }

    /*private IEnumerator DestroyReplacement(GameObject replacement, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (replacement != null)
        {
            // Use DestroyImmediate to remove the replacement object immediately
            Destroy(replacement);

    }*/

    private IEnumerator DestroyReplacement(GameObject replacement, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (replacement != null)
        {
            // Use DestroyImmediate to remove the replacement object immediately
            replacement.SetActive(false);
        }


    }

}
