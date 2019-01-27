using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
    public PostProcessVolume volume;
    public Animator overlay;
    bool roundEnding = false;
    bool otherPlayerHasStartedDying = false;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach(Character player in players) {
            if(playerAmount < player.GetPlayerNumber()) {
                player.transform.parent.gameObject.SetActive(false);
            }
        }
        Countdown();
    }

    void StartRound() {        
        spawnManager.StartSpawning();
        timeStarted = timeLastWarmed = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(!otherPlayerHasStartedDying && Time.time > timeLastWarmed + kotatsuInterval && !roundEnding) {
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
        if(!roundEnding) {
            bool allEmpty = true;
            if(!spawnManager.IsEmpty()) {
                allEmpty = false;
                //Debug.Log("Spawners remain.");
            }
            foreach(Interactor i in permanentInteractors) {
                if(i.IsActive()) {
                    allEmpty = false;
                    //Debug.Log(i.name + " remains.");
                }
            }

            if(allEmpty) {
                CheckPoints();
            }
        }
    }

    public void StartDeathAnimation() {
        otherPlayerHasStartedDying = false;
        foreach(Character player in players) {
            if(playerAmount >= player.GetPlayerNumber() && player.DeathStarted() && !player.IsEliminated()) {
                otherPlayerHasStartedDying = true;
            }
            if(!otherPlayerHasStartedDying && playerAmount >= player.GetPlayerNumber() && player.IsDead() && !player.DeathStarted()) {
                otherPlayerHasStartedDying = true;
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
                player.Eliminate();
                player.transform.parent.gameObject.SetActive(false);
                allAlive = false;
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
                i.gameObject.SetActive(true);
                i.Reset();
            }
        }
        otherPlayerHasStartedDying = false;
        Countdown();
    }

    public void CheckPoints() {
        int lowestpoints = players[0].GetPoints();
        Character lowestPlayer = players[0];
        foreach(Character player in players) {
            if(!player.IsEliminated() && !player.IsDead()) {
                if(player.GetPoints() < lowestpoints) {
                    lowestPlayer = player;
                }
            }
        }
        roundEnding = true;
        lowestPlayer.Kill();
        StartDeathAnimation();
    }

    void Countdown() {
        DepthOfField DoF;
        volume.profile.TryGetSettings(out DoF);
        DoF.focusDistance.value = 1;
        overlay.Play("Countdown");
    }

    public void FinishCountdown() {
        DepthOfField DoF;
        volume.profile.TryGetSettings(out DoF);
        DoF.focusDistance.value = 10;
        StartRound();
    }
}
