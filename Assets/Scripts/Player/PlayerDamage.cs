using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public int health = 100;
    public PlayerMovement playerMove;
    public bool Dead;
    Rigidbody playerRB;
    Sliding sliding;
    AudioSource audioSource;
    [SerializeField] private GameOverUI gameOver;
    [SerializeField] private Camera cam;
    public float camFOVIn;
    [SerializeField] private float camFOVOut;
    [HideInInspector] public float countdown = 0f;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private PickUpSystem pickUp1;
    [SerializeField] private PickUpSystem pickUp2;
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            pickUp1.enabled = false;
            pickUp2.enabled = false;
            gameOverScreen.SetActive(true);
            StartCoroutine(PlayerDie());
        }
    }

    public IEnumerator PlayerDie()
    {
        Dead = true;
        yield return null;
        playerMove.Die();
        sliding.Die();
        //playerRB.AddRelativeTorque(Random.insideUnitSphere * 1000, ForceMode.Impulse);
        //playerRB.AddExplosionForce(50, enemy.position, 10f, 1f, ForceMode.Impulse);

        float elapsed = 0;
        countdown = camFOVIn + 1;

        while (elapsed < camFOVIn)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 135f, elapsed / camFOVIn);
            elapsed += Time.deltaTime;
            gameOver.Update();
            yield return null;
        }
    }
}