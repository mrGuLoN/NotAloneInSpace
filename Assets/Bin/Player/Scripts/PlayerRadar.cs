using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRadar : MonoBehaviour
{
    public delegate void Radar(Transform _enemy);
    public static event Radar _enemiInRadar;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _enemiInRadar(collision.transform);
        Debug.Log("Inter");
    }
}
