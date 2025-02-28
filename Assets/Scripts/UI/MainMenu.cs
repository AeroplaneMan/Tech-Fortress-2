using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // Loads the next scene in the build order and sets up a listener for scene loading
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.sceneLoaded += OnSceneLoaded; // Listen for when the new scene loads
    }

    // Called when a new scene is loaded to enable the player's weapons
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find weapons in the new scene
        PickUpSystem pickUp1 = GameObject.FindWithTag("LaserGun")?.GetComponent<PickUpSystem>();
        PickUpSystem pickUp2 = GameObject.FindWithTag("RocketLauncher")?.GetComponent<PickUpSystem>();

        // Enable them if found
        if (pickUp1 != null) pickUp1.enabled = true;
        if (pickUp2 != null) pickUp2.enabled = true;

        // Unsubscribe from event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("QUIT!");
    }
}
