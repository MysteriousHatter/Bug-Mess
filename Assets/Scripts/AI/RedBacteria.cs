using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBacteria : Bacteria
{

    private Transform playerTransform;

    [Header("EnemyAI Chase")]
    [SerializeField] private float chaseDistance = 5f;
    [SerializeField] private float chaseSpeed = 4f;

    private bool isFrozen = false;



    private void Start()
    {
        Initialize(this.gameObject);
        playerTransform = GameObject.FindWithTag("Player").transform;
        // Start the shrinking and enlarging process
        //StartCoroutine(ShrinkThenEnlarge());
    }
    protected override void Update()
    {
        // Call base class Update to handle size changes
        if (!isFrozen)
        {
            base.Update();
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
       float playerDistance = Vector2.Distance(playerTransform.position, GetPrefabType().transform.position);
        Debug.Log("The current player distance " +  playerDistance);
        if (playerDistance < chaseDistance) 
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, chaseSpeed * Time.deltaTime);
        }
        else
        {
            Move();
        }    
    }

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

    public void StartCorutineForFrozen()
    {
        if (!isFrozen) { StartCoroutine(FreezeMovement(0.8f)); }
    }
    IEnumerator FreezeMovement(float freezeDuration)
    {
        isFrozen = true;  // Stop movement
        yield return new WaitForSeconds(freezeDuration);  // Wait for the freeze duration
        isFrozen = false; // Resume movement
    }

}
