using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    private void Update()
    {
        // If the player clicks the button, move to the next scene
        if (Input.GetMouseButtonDown(0) && GetComponent<Collider2D>().OverlapPoint(
            Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
    }
}
