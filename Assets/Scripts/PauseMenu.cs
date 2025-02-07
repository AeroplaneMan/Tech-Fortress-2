using System.Collections;
using TMPro;
using UnityEngine;

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
        if (Input.GetKeyDown(KeyCode.Escape)) // Only allow pausing, not resuming
        {
            if (!isPaused && !(levelComplete.levelCompleted || playerDamage.Dead))
            {
                PauseGame();
            }
        }

        if (isPaused)
        {
            gamePausedText.transform.Rotate(new Vector3(1, 1, 1) * rotationSpeed * Time.unscaledDeltaTime);
            enemiesKilledText.text = "Enemies Killed: " + levelComplete.numEnemiesDestroyed;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            StartCoroutine(PauseAudio());
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        pauseScreen.SetActive(true);
        crosshair.SetActive(false);
        healthBar.SetActive(false);
        Time.timeScale = 0f;
    }

    private IEnumerator PauseAudio()
    {
        audioSource1.Pause();
        yield return null;
    }

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

    public void Quit()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }
}
