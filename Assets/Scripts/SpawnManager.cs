using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<Spawner> spawners;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartSpawning() {
        foreach(Spawner s in spawners) {
            s.Spawn();
        }
    }

    public void Reset() {
        foreach(Spawner s in spawners) {
            s.Reset();
        }
    }

    public bool IsEmpty() {        
        bool allEmpty = true;
        foreach(Spawner s in spawners) {
            if(!s.IsEmpty()) {
                allEmpty = false;
            }
        }

        return allEmpty;
    }
}
