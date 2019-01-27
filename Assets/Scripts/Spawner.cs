using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<Interactor> spawnableItems;
    List<Interactor> spawnedItems;
    public int min, max;
    float minX, minZ, maxX, maxZ, y;

    void Awake()
    {
        Collider c = GetComponent<BoxCollider>();
        minX = c.bounds.min.x;
        minZ = c.bounds.min.z;
        maxX = c.bounds.max.x;
        maxZ = c.bounds.max.z;
        y = transform.position.y;
        spawnedItems = new List<Interactor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn() {
        int amountToSpawn = Random.Range(min, max);
        for(int i = 0; i < amountToSpawn; ++i) {
            int indexToSpawn = Random.Range(0, spawnableItems.Count);
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);
            Vector3 location = new Vector3(randomX, y, randomZ);
            Interactor newSpawn = Instantiate(spawnableItems[indexToSpawn], location, Quaternion.identity);
            newSpawn.transform.eulerAngles = new Vector3(newSpawn.transform.rotation.x - 90, newSpawn.transform.rotation.y, newSpawn.transform.rotation.z);
            spawnedItems.Add(newSpawn);
        }
    }

    public bool IsEmpty() {
        bool allInactive = true;
        foreach(Interactor s in spawnedItems) {
            if(s.gameObject.activeInHierarchy) {
                allInactive = false;
            }
        }
        return allInactive;
    }

    public void Reset() {
        foreach(Interactor i in spawnedItems) {
            Destroy(i.gameObject);
        }
        spawnedItems = new List<Interactor>();
        Spawn();
    }
}
