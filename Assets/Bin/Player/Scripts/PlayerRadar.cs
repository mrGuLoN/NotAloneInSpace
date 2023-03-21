using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRadar : MonoBehaviour
{
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ControllerPlayer.singletonePlayer.ChangeEnemyList(1, collision.transform);
    }
}
