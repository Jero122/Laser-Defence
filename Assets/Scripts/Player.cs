using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // config parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.5f;
    [SerializeField] int playerHealth = 200;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSFXVolume = 0.75f;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] [Range(0,1)]float shootSFXVolume = 0.25f;
    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserSpeed = 10f;
    [SerializeField] float laserFiringPeriod = 0.1f;

    private Rigidbody2D rb;
    private float deltaX;
    private float deltaY;
    Coroutine firingCoroutine;
    float xMin;
    float xMax;
    float yMin;
    float yMax;

  
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        deltaX = Input.GetAxis("Horizontal") * moveSpeed;
        deltaY = Input.GetAxis("Vertical") *  moveSpeed;
        Fire();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());

        }
        else if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject Laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            Laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSFXVolume);
            yield return new WaitForSeconds(laserFiringPeriod);
        }
    }

    private void Move()
    {
        

        var newXPos = transform.position.x + deltaX * Time.deltaTime;
        var newYPos = transform.position.y + deltaY * Time.deltaTime;

        rb.velocity = new Vector2(deltaX, deltaY);
        //transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).x;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).y;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (!damageDealer)
        {
            return;
        }
        ProcessHit(damageDealer);
    }


    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("HIT");
    }

    private void ProcessHit(DamageDealer damageDealer)
    {

        playerHealth -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (playerHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
        Level level = FindObjectOfType<Level>();
        level.LoadGameOver();
    }

    public int GetHealth()
    {
        return playerHealth;
    }
}
