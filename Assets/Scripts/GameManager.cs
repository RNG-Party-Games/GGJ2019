using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpawnManager spawnManager;
    public List<Character> players;
    public List<Kotatsu> kotatsus;
    public List<Interactor> permanentInteractors;
    public Interactor kotatsuPlug;
    public int playerAmount;
    public float kotatsuInterval, warmingValue;
    float timeStarted, timeLastWarmed;
    public static GameManager instance;
    bool roundEnding = false;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        spawnManager.StartSpawning();
        CheckDeath();
        timeStarted = timeLastWarmed = Time.time;
        foreach(Character player in players) {
            if(playerAmount < player.GetPlayerNumber()) {
                player.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > timeLastWarmed + kotatsuInterval && !roundEnding) {
            timeLastWarmed = Time.time;
            foreach(Character player in players) {
                if(kotatsuPlug.isActiveAndEnabled && player.IsSnapped() && playerAmount >= player.GetPlayerNumber() && !player.IsDead()) {
                    player.Warm(warmingValue);
                }
                else if(playerAmount >= player.GetPlayerNumber() && !player.IsDead()) {
                    player.Warm(-warmingValue);
                }
            }
            StartDeathAnimation();
        }
    }

    public void StartDeathAnimation() {
        foreach(Character player in players) {
            if(playerAmount >= player.GetPlayerNumber() && player.IsDead() && !player.DeathStarted()) {
                roundEnding = true;
                player.StartDeath();
                player.transform.parent.GetComponent<Animator>().Play("PlayerDeath");
            }
        }
    }

    public void CheckDeath() {
        bool allAlive = true;
        foreach(Character player in players) {
            if(!player.IsEliminated() && player.IsDead()) {
                player.transform.parent.gameObject.SetActive(false);
                allAlive = false;
                player.Eliminate();
            }
            else if(!player.IsEliminated()) {
                player.Reset();
            }
        }
        if(!allAlive) {
            spawnManager.Reset();
            roundEnding = false;
            foreach(Kotatsu k in kotatsus) {
                k.Reset();
            }
            foreach(Interactor i in permanentInteractors) {
                i.Reset();
            }
        }
    }
}
