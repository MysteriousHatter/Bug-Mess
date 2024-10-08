using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleBacteria : Bacteria
{
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



}
