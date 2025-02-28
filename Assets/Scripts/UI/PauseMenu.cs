using System.Collections;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private PickUpSystem pickUp1;
    [SerializeField] private PickUpSystem pickUp2;
    [SerializeField] private LevelCompleteCheck levelComplete;
    [SerializeField] private PlayerDamage playerDamage;
    public GameObject gamePausedText;
    public TextMeshProUGUI enemiesKilledText;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private AudioSource audioSource1;
    public bool isPaused = false;

    void Update()
    {
        // Check if the Escape key is pressed to pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused && !(levelComplete.levelCompleted || playerDamage.Dead))
            {
                PauseGame();
            }
        }

        // If the game is paused, update UI elements
        if (isPaused)
        {
            gamePausedText.transform.Rotate(new Vector3(1, 1, 1) * rotationSpeed * Time.unscaledDeltaTime);
            enemiesKilledText.text = "Enemies Killed: " + levelComplete.numEnemiesDestroyed;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            StartCoroutine(PauseAudio());
        }
    }

    // Pauses the game and brings up the pause menu
    private void PauseGame()
    {
        isPaused = true;
        pauseScreen.SetActive(true);
        crosshair.SetActive(false);
        healthBar.SetActive(false);
        Time.timeScale = 0f;
    }

    // Coroutine to pause the game audio
    private IEnumerator PauseAudio()
    {
        audioSource1.Pause();
        yield return null;
    }

    // Resumes the game, hides the pause menu, and restores normal game state
    public void ResumeGame()
    {
        isPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        crosshair.SetActive(true);
        healthBar.SetActive(true);
        audioSource1.UnPause();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Quits the application
    public void Quit()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }
}
