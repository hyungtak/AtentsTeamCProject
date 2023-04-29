using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBox : Coin
{

    public float RotateMove = 120f;

    protected override void Update()
    {
        base.Update();
        transform.Rotate(Time.deltaTime * RotateMove * Vector3.up);
    }
}
