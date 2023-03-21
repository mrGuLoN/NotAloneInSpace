 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoolController : MonoBehaviour
{
    private PoolObject _poolObject;
    private float _currenTime, _deltaTime;
   
    private Transform _playerTransform;

    private void Awake()
    {
        _poolObject = gameObject.AddComponent<PoolObject>();
    }

    public void UpdateStartParametr(PlayerSO playerSO)
    {        
        _deltaTime = playerSO.speedFire;
        _currenTime = _deltaTime;      
        _poolObject.Starting(playerSO.bulletGO, 10);
        _playerTransform = ControllerPlayer.singletonePlayer.transform;
    }

    public void Instantiate(Transform enemyTransform)
    {
        _currenTime += Time.fixedDeltaTime;
        if (_currenTime >= _deltaTime)
        {
            Transform bulletTransform;
            _poolObject.InstantiateObj(enemyTransform.position - _playerTransform.position, _playerTransform.position, out bulletTransform);
            bulletTransform.up = enemyTransform.position - _playerTransform.position;
            ControllerPlayer.singletonePlayer.ChangeBulletList(1, bulletTransform);
            _currenTime = 0;
        }
    }

    public void UpdateFireSpeed(float fireSpeed)
    {
        _deltaTime = 60 / (fireSpeed + 60 / _deltaTime);
    }
}
