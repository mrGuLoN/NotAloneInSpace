using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnController : MonoBehaviour
{
    private int _livedEnemy, _numberEnemy, _enemyHealth, _enemyAward, _respawnInt;
    private PoolObject _poolObject;
    private Transform _playerTransform;
    private ControllerEnemy _controllerEnemy;

    private float _currentTime, _timeToRespawn, _xRandom, _yRandom;
    private bool _respawnOn;
    private Vector3 _respawnPoint;

    private void Awake()
    {
        _poolObject = gameObject.AddComponent<PoolObject>();
    }
    // Start is called before the first frame update
    public void UpdateStartParametrs(int startNumberEnemy, int startEnemyHealth, int startEnemyAward, float timeToRespawn, GameObject enemyGameObject)
    {            

        _playerTransform = ControllerPlayer.singletonePlayer.transform;
        _livedEnemy = 0;
        _enemyAward = startEnemyAward;
        _enemyHealth = startEnemyHealth;
        _numberEnemy = startNumberEnemy;
        _timeToRespawn = timeToRespawn;
        _respawnInt = 0;

        _poolObject.Starting(enemyGameObject, _numberEnemy);

        _currentTime = _timeToRespawn;
        _respawnOn = true;
    }

    // Update is called once per frame
    public void Respawn(int livedEnemyInt)
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _timeToRespawn && _respawnOn)
        {
            _xRandom = Random.Range(-8.5f, 8.5f);
            _yRandom = Random.Range(-8.5f, 8.5f);
            _respawnPoint = _playerTransform.position + new Vector3(_xRandom, _yRandom, 0);
            _poolObject.InstantiateObj(_playerTransform.position, _respawnPoint, out Transform _objTransform);
            _objTransform.up = _playerTransform.position - _objTransform.position;
            if (_objTransform.gameObject.TryGetComponent<EnemyMob>(out EnemyMob enemyMob))
            {
                enemyMob.health = _enemyHealth;
                enemyMob.score = _enemyAward;
            }
           
            if (Vector3.Distance(_playerTransform.position, _objTransform.position) <= 7)
            {
                _objTransform.position -= _objTransform.up * 7f;
            }           
            _currentTime = 0;
            _respawnInt ++;
            ControllerEnemy.controllerEnemySingletone.ChangeEnemyList(1, _objTransform);
        }

        CheckRespawn(livedEnemyInt);
    }

    private void CheckRespawn(int livedEnemyInt)
    {
        if (_respawnInt >= _numberEnemy && _respawnOn)
        {
            _respawnOn = false;
            _respawnInt = 0;
        }
        else if (livedEnemyInt <= 0 && !_respawnOn)
        {
            _respawnOn = true;
            _numberEnemy++;
            CanvasController.singletonCanvas.UpdateNumberStage();
            ControllerEnemy.controllerEnemySingletone.currentNumberEnemy = _numberEnemy;
        }
    }

    public void UpdateParametrs(int updateHealth, int updateAward)
    {
        _enemyHealth += updateHealth;
        _enemyAward += (int)(updateAward * 0.5f);        
    }



}
