using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PowerupSpawner : NetworkBehaviour
{
    public GameObject[] powerUpPrefabs;

    [SerializeField]
    private float cooldownInterval;

    [SerializeField]
    private float cooldownTimer = 0f;

    public bool spawnable = true;
    private bool cooldown;

    private float mapHeight = 7f;
    private float mapWidth = 15f;
    private HexMap map;

    // Use this for initialization
    void Start()
    {

    }

    public override void OnStartServer()
    {
        map = GameObject.FindGameObjectWithTag("TileMap").GetComponent<HexMap>();
    }

    // Update is called once per frame
    void Update()
    {

        if (spawnable)
        {
            Spawn();
            pauseSpawn();

            //sets random spawn interval
            cooldownInterval = Random.Range(20, 30);
            startCoolDownTimer();

        }

        if (cooldownTimer > cooldownInterval)
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

    //Spawen power up on top of tiles
    void Spawn()
    {
        List<GameObject> currentTiles = map.tiles;
        Vector3 targetPosition = currentTiles[Random.Range(0, currentTiles.Count)].transform.position;
        targetPosition.y += .57f;
        targetPosition.z += .17f;  // adjust and modify if needed
        targetPosition.x -= .15f;  // adjust and modify if needed
        GameObject powerUpPrefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
        GameObject powerUp = (GameObject)Instantiate(powerUpPrefab, targetPosition, new Quaternion());
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
