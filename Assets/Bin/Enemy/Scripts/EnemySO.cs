using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Stage/New EnemySO")]
public class EnemySO : ScriptableObject
{
    [SerializeField] private int _startCont;
    [SerializeField] private int _enemyInMin;
    [SerializeField] private int _score;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _enemyGO;
    [SerializeField] private int _health;

    public int health => _health;
    public int score => _score;
    public int startCont => _startCont;
    public float speespeedResp => _enemyInMin / 60f;   
    public float speed => _speed;
    public float timerRespawn => _enemyInMin / 60f; 
    public GameObject enemyGO => _enemyGO;
}
