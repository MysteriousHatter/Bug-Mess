using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;

public class Player : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 3f;
    private int PlayerLives = 3;

    [SerializeField] private float slowDuration = 2f;
    [SerializeField] private float slowMotionFactor = 0.5f;

    private bool isInvinciable = false;
    [SerializeField] private float invinicableDuration = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletForce = 20f;

    void Update()
    {
        Movement();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation, this.transform.parent);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.right * bulletForce;
    }

    private void Movement()
    {
        // Get input from horizontal and vertical axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Create a movement vector based on input
        Vector3 movement = new Vector3(horizontal, vertical, 0f);

        // Normalize the movement vector to ensure consistent speed
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // Move the player
        transform.position += movement * playerSpeed * Time.deltaTime;

        // Rotate the player based on movement
        RotatePlayerTowardsMovement(movement);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Red") && !isInvinciable)
        {
            Debug.Log("Lose Life");
            Destroy(collision.gameObject);
            LifeManagement("Decrease");
        }
        else if (collision.gameObject.tag.Equals("Blue"))
        {
            Debug.Log("1UP");
            ActivateOneUp(collision);
        }
        else if (collision.gameObject.tag.Equals("Purple"))
        {
            Debug.Log("Invinciablity");
            ActivateInvinciability(collision);
        }
        else if(collision.gameObject.tag.Equals("Yellow"))
        {
            ActivateSlowMotion(collision);
        }
    }

    public void ActivateSlowMotion(Collision2D collision)
    {
        Destroy(collision.gameObject);
        StartCoroutine(SlowMotion());
    }

    public void ActivateInvinciability(Collision2D collision)
    {
        Destroy(collision.gameObject);
        StartCoroutine(BecomeInvinciable());
    }

    public void ActivateOneUp(Collision2D collision)
    {
        Destroy(collision.gameObject);
        LifeManagement("Increase");
    }

    IEnumerator SlowMotion()
    {
        // Iterate through all objects in the scene
        Bacteria[] allObjects = FindObjectsOfType<Bacteria>();

        foreach (Bacteria obj in allObjects)
        {
            float movementSpeed = obj.GetMovementSpeed();
            obj.SetMovementSpeed(movementSpeed * slowMotionFactor);
            if(obj.gameObject.CompareTag("Red"))
            {
                float chaseSpeed = obj.GetComponent<RedBacteria>().GetChaseSpeed();
                obj.GetComponent<RedBacteria>().SetChaseSpeed(chaseSpeed * slowMotionFactor);
            }
        }

        // Wait for the slow-motion duration
        yield return new WaitForSeconds(slowDuration);

        // Restore normal speed for non-player objects
        foreach (Bacteria obj in allObjects)
        {
            float speed = obj.GetMovementSpeed();
            obj.SetMovementSpeed(speed/slowMotionFactor);
            if (obj.gameObject.CompareTag("Red"))
            {
                float chaseSpeed = obj.GetComponent<RedBacteria>().GetChaseSpeed();
                obj.GetComponent<RedBacteria>().SetChaseSpeed(chaseSpeed/slowMotionFactor);
            }
        }
    }

    private void LifeManagement(string type)
    {
        if (type.Equals("Decrease"))
        {
            if (PlayerLives < 0)
            {
                Debug.Log("Game Over");
            }
            else { PlayerLives--; GameManager.instance.UpdateHealthUI(PlayerLives); }
        }
        else if (type.Equals("Increase"))
        {
            if (PlayerLives > 3) { PlayerLives = 3; }
            else { PlayerLives++; GameManager.instance.UpdateHealthUI(PlayerLives); }
        }
    }

    // Rotate the player based on the mouse position
    void RotatePlayerTowardsMovement(Vector3 movement)
    {
        // Check if the movement vector is not zero to avoid errors
        if (movement != Vector3.zero)
        {
            // Calculate the angle between the x-axis and the movement vector
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

            // Apply the rotation to the player
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    IEnumerator BecomeInvinciable()
    {
        isInvinciable = true;
        Color oldColor = this.gameObject.GetComponent<SpriteRenderer>().color;
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.75f, 0.8f);
        yield return new WaitForSeconds(invinicableDuration);
        this.gameObject.GetComponent<SpriteRenderer>().color = oldColor;
        isInvinciable = false;
    }
}
