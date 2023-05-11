using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCamera : MonoBehaviour
{
    public Transform player;

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>().transform;
        }
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y+2, -7);
    }
}
