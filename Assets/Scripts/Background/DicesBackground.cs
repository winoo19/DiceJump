using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicesBackground : MonoBehaviour
{
    // List of dices (empty game objects) that will move around the background
    public GameObject[] dices;

    private float spawnDelay = 0.5f; // Time between each dice spawn try
    private float expectedDiceTime = 2f; // Expected time between each dice spawn

    private float currentTimer; // Current time since last dice spawn

    private BoxCollider2D gameBorderCollider; // Limits of the game

    void Start()
    {
        gameBorderCollider = GameObject.Find("GameBorder").GetComponent<BoxCollider2D>();
        currentTimer = spawnDelay;
    }
    
    void Update()
    {
        if (currentTimer >= spawnDelay)
        {
            float random = Random.Range(0, expectedDiceTime / spawnDelay);
            if (random < 1)
            {
                SpawnDice();
            }
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

        // Spawn the dice in the down limits - margin, and in the x coordinate also randomly inside the limits
        Vector3 randomPos = new Vector3(
            Random.Range(gameBorderCollider.bounds.min.x, gameBorderCollider.bounds.max.x),
            gameBorderCollider.bounds.min.y - 1,
            0);
        // Spawn the dice and set its speed at random
        GameObject newDice = Instantiate(dice, randomPos, Quaternion.identity);
        newDice.GetComponent<diceMovement>().setSpeed(Random.Range(2f, 4f));
    }


}
