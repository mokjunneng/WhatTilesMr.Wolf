﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PowerupSpawner : NetworkBehaviour
{
    public GameObject[] powerUpPrefabs;

    public float cooldownInterval = 5f;

    [SerializeField]
    private float cooldownTimer = 0f;

    public bool spawnable = true;
    private bool cooldown;

    private float mapHeight = 5f;
    private float mapWidth = 5f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (spawnable)
        {
            Spawn();
            pauseSpawn();

            startCoolDownTimer();
            
        }

        if(cooldownTimer > cooldownInterval)
        {
            cooldownTimer = 0f;
            startSpawn();
            stopCoolDownTimer();
        }

        if (cooldown)
        {
            cooldownTimer += Time.deltaTime;
        }
    }

    void Spawn()
    {

        Vector3 position = new Vector3(transform.position.x + Random.Range(-mapWidth / 2, mapWidth / 2), 0.2f, transform.position.z + Random.Range(-mapHeight / 2, mapHeight / 2));

        GameObject powerUpPrefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
        GameObject powerUp = (GameObject)Instantiate(powerUpPrefab, position, new Quaternion());
        NetworkServer.Spawn(powerUp);

    }

    public void startSpawn()
    {
        spawnable = true;
    }

    public void pauseSpawn()
    {
        spawnable = false;
    }

    public void startCoolDownTimer()
    {
        cooldown = true;
    }

    public void stopCoolDownTimer()
    {
        cooldown = false;
    }
}
