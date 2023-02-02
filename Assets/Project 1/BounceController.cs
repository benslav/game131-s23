using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceController : MonoBehaviour
{
    public GameObject redSpawner, blueSpawner;
    public GameObject bouncePrefab;

    public float redSpawnRate = 0.1f, blueSpawnRate = 0.2f;
    private float redSpawnTimer = 0, blueSpawnTimer = 0;

    public int redSpawns = 100, blueSpawns = 50;
    public int redHealth = 50, blueHealth = 100;

    public Material redMaterial, blueMaterial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (redSpawns > 0) redSpawnTimer += Time.deltaTime;
        if (redSpawnTimer > redSpawnRate) {
            redSpawnTimer -= redSpawnRate;

            Vector3 range = redSpawner.transform.lossyScale;
            range.x -= 0.5f;
            range.z -= 0.5f;

            Vector3 spawnPosition = new Vector3(Random.Range(-range.x, range.x), 0.5f, Random.Range(-range.z, range.z));
            GameObject newRed = Instantiate(bouncePrefab);
            newRed.transform.position = spawnPosition + redSpawner.transform.position;
            newRed.name = "red";
            newRed.GetComponent<MeshRenderer>().material = redMaterial;
            newRed.GetComponent<Bouncer>().oomph = redHealth;

            redSpawns--;
        }

        if (blueSpawns > 0) blueSpawnTimer += Time.deltaTime;
        if (blueSpawnTimer >= blueSpawnRate) {
            blueSpawnTimer -= blueSpawnRate;

            Vector3 range = blueSpawner.transform.lossyScale;
            
            range.x -= 0.5f;
            range.z -= 0.5f;

            Vector3 spawnPosition = new Vector3(Random.Range(-range.x, range.x), 0.5f, Random.Range(-range.z, range.z));
            GameObject newBlue = Instantiate(bouncePrefab);
            newBlue.transform.position = spawnPosition + blueSpawner.transform.position;
            newBlue.name = "blue";
            newBlue.GetComponent<MeshRenderer>().material = blueMaterial;

            blueSpawns--;
        }
    }
}
