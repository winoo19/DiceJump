using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicesBackground : MonoBehaviour
{
    // List of dices (empty game objects) that will move around the background
    public GameObject[] dices;

    private float spawnDelay = 5f; // Time between each dice spawn

    private float currentTimer = 0f; // Current time since last dice spawn

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (currentTimer > spawnDelay)
        {
            SpawnDice();
            currentTimer = 0f;
        }
        else
        {
            currentTimer += Time.deltaTime;
        }
        
    }

    private void SpawnDice()
    {
        // Get a random dice from the list
        GameObject dice = dices[Random.Range(0, dices.Length)];

        // Get a random position inside the background
        Vector3 randomPos = new Vector3(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2),
                                        Random.Range(transform.position.y - transform.localScale.y / 2, transform.position.y + transform.localScale.y / 2),
                                        0);

        // Spawn the dice
        Instantiate(dice, randomPos, Quaternion.identity);
    }


}
