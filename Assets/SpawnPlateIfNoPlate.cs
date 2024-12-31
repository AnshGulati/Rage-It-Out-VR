using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnPlateIfNoPlate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // SpawnObjectIfNoCollision();
    }

    public GameObject objectToSpawn;
    public BoxCollider colliderToCheck;

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Update");
        SpawnObjectIfNoCollision();
    }

    void SpawnObjectIfNoCollision()
    {
        // Get the bounds of the Box Collider to check
        Bounds bounds = colliderToCheck.bounds;

        // Check if there are any colliders within the bounds
        Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.extents, colliderToCheck.transform.rotation);
        // if (!colliders.Cast<Collider>().Any())
        if (colliders.Length == 1)
        {
            // If there are no colliders, spawn the object at the collider's position
            Instantiate(objectToSpawn, colliderToCheck.transform.position, objectToSpawn.transform.rotation);
            // Instantiate(objectToSpawn);
        }
    }

}
