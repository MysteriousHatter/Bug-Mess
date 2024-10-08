using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBacteria : Bacteria
{
    [SerializeField] private Vector3 sizeLength = new Vector3(0.5f, 0.5f,0.5f);
    [SerializeField] private float smallObjectThreshold = 1.0f;
    [SerializeField] private float largeObjectThreshold = 3.0f;

    private Timer timeManager => FindObjectOfType<Timer>();
    public override float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public override GameObject GetPrefabType()
    {
        return bacteriaPrefab;
    }
    public override void SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    private void Start()
    {
        Initialize(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Red"))
        {
            Debug.Log("Shrink");
            Destroy(collision.gameObject);
            ShrinkgOrEnlarge("Shrink");
        }
        else if (collision.gameObject.tag.Equals("Blue"))
        {
            Debug.Log("Grow");
            Destroy(collision.gameObject);
            ShrinkgOrEnlarge("Grow");
        }
        else if(collision.gameObject.tag.Equals("Blast"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void ShrinkgOrEnlarge(string type)
    {
        if (type.Equals("Shrink"))
        {
            transform.localScale -= sizeLength;
            // Ensure the object doesn't scale to negative values
            if (transform.localScale.x < 0.5f) transform.localScale = new Vector3(0.5f, transform.localScale.y, transform.localScale.z);
            if (transform.localScale.y < 0.5f) transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
            if (transform.localScale.z < 0.5f) transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0.5f);
        }
        else if(type.Equals("Grow"))
        {
            transform.localScale += sizeLength;

            // Ensure the object doesn't scale to above 3
            if (transform.localScale.x > 3f) transform.localScale = new Vector3(3f, transform.localScale.y, transform.localScale.z);
            if (transform.localScale.y > 3f) transform.localScale = new Vector3(transform.localScale.x, 3f, transform.localScale.z);
            if (transform.localScale.z > 3f) transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 3f);

        }
    }

    public void GetTimeValue()
    {
        // Get the size of the gameObject (we'll assume it's the scale of the object for this purpose)
        float objectSize = transform.localScale.magnitude;

        // Reference to the time manager (this assumes you have a TimeManager class)
        //TimeManager timeManager = FindObjectOfType<TimeManager>();

        // Check the size of the object and reduce time accordingly
        if (objectSize <= smallObjectThreshold)
        {
            // Object is considered small, reduce time by 5 seconds
            timeManager.ReduceTime(5.0f);
            Destroy(gameObject);
        }
        else if (objectSize >= largeObjectThreshold)
        {
            // Object is considered large, reduce time by 20 seconds
            timeManager.ReduceTime(20.0f);
            Destroy(gameObject);
        }
        else
        {
            // Object is between small and large, reduce time by 10 seconds as a medium case
            timeManager.ReduceTime(10.0f);
            Destroy(gameObject);
        }
    }
}
