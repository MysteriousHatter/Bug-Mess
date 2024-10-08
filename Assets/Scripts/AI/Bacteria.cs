
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bacteria : MonoBehaviour
{
    // Fields for movement speed, prefab, and size tracking
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected GameObject game_area;
    protected GameObject bacteriaPrefab;
    protected Vector2 initialScale;   // Store the initial scale of the bacteria
    [SerializeField] protected bool isShrinking = true; // Track if the bacteria is shrinking or enlarging
    protected Vector3 currentDirection;  // Current movement direction


    protected float scaleTimer = 0;
    protected bool isSlowed = false;  // Track if the object is currently slowed

    // Constructor or method to initialize the bacteria
    public virtual void Initialize(GameObject prefab)
    {
        bacteriaPrefab = prefab;
        initialScale = transform.localScale; // Set initial scale
        game_area = GameObject.FindGameObjectWithTag("Area");

        ChooseRandomDirection();
    }

    // Abstract method to get movement speed
    public abstract float GetMovementSpeed();

    public abstract void SetMovementSpeed(float speed);

    // Abstract method to get the prefab type
    public abstract GameObject GetPrefabType();

    // Update method to check for shrinking and enlarging behavior
    protected virtual void Update()
    {

    }
    protected void Move()
    {
        /** Move this ship forward per frame, if it gets too far from the game area, bounce off instead of destroying it **/

        transform.position += currentDirection * (Time.deltaTime * movementSpeed);

        float distance = Vector3.Distance(transform.position, game_area.transform.position);
        float radius = game_area.GetComponent<CompositeCollider2D>().bounds.extents.magnitude - 9f;
        if (distance > radius)
        {
            Debug.Log("bounce off");
            BounceOff();
        }
    }
    private void ChooseRandomDirection()
    {
        // Generate a random angle and convert it to a direction vector
        float randomAngle = Random.Range(0f, 360f);
        float radianAngle = randomAngle * Mathf.Deg2Rad;
        currentDirection = new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle), 0).normalized;

        // Optional: Rotate the ship visually to match the new random direction
        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    void BounceOff()
    {
        /** Instead of destroying the ship, we reflect its direction to simulate bouncing off **/

        // Get the direction from the center of the game area to the ship (normal to the boundary)
        Vector3 directionToCenter = (game_area.transform.position - transform.position).normalized;

        // Reflect the current direction against the normal (direction to center)
        currentDirection = Vector3.Reflect(currentDirection, directionToCenter);

        // Optional: Rotate the ship visually to match the new direction
        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }


}
