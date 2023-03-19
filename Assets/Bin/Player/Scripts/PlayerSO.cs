using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PLayerSO", menuName = "Stage/New PlayerSO")]
public class PlayerSO : ScriptableObject
{
    [SerializeField] private int _health;
    [SerializeField] private int _bulletInMin;
    [SerializeField] private int _radiusFire;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private int _damage;
    [SerializeField] private GameObject _bullet;

    public int health => _health;
    public float speedFire => 60f/ _bulletInMin;
    public int radiusFire => _radiusFire;
    public int damage => _damage;
    public float bulletSpeed => _bulletSpeed;

    public GameObject bulletGO => _bullet;

}
