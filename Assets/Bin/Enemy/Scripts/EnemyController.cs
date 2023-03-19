using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private LayerMask _playerLayerMask;

    public static EnemyController enemySingleTone { get; private set; }

    private PoolObject _poolObject;
    private List<Transform> _enemyList = new List<Transform>();
    private Transform _objTransform, _playerTransform, _thisTR;
    private float _xRandom, _yRandom, _currentTime;
    private Vector3 _randomDirection;
    private bool _respawnOn = true;
    private int _liveEnemy, _startCont, _respawnInt;

    private int _currentEnemyHealth, _currentEnemyScore;
    private float _currenEnemySpeed;

    private void Awake()
    {
        EnemyMob._enemyContChange += EnemyContChange;
        enemySingleTone = this;
        _poolObject = this.gameObject.AddComponent<PoolObject>();
    }

    public void StartStarting()
    {
        _respawnInt = 0;
        _currenEnemySpeed = enemySO.speed;
        _currentEnemyHealth = enemySO.health;
        _currentEnemyScore = enemySO.score;
        _thisTR = GetComponent<Transform>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();


        _enemyList = new List<Transform>();

        _poolObject.Starting(enemySO.enemyGO, enemySO.startCont);
        _currentTime = enemySO.timerRespawn;
        _startCont = enemySO.startCont;
        _liveEnemy = 0;
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        _currentTime += Time.fixedDeltaTime;
        InstanceEnemy();
        if (_enemyList.Count > 0)
        {
            MoveEnemy();           
        }
        CheckRespawn();
    }

    private void InstanceEnemy()
    {
        if (_currentTime >= enemySO.timerRespawn && _respawnOn)
        {            
            _xRandom = Random.Range(-8.5f, 8.5f);
            _yRandom = Random.Range(-8.5f, 8.5f);
            _randomDirection = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0).normalized + new Vector3(_xRandom, _yRandom, 0);
            _poolObject.InstantiateObj(_playerTransform.position, _randomDirection, out _objTransform);
            _objTransform.gameObject.GetComponent<EnemyMob>().health = _currentEnemyHealth;
            _objTransform.gameObject.GetComponent<EnemyMob>().score = _currentEnemyScore;
            float distance = Vector3.Distance(_playerTransform.position, _objTransform.position);
            if (distance < 7f)
            {
                _objTransform.position -= _objTransform.up * 7f;
            }
            _enemyList.Add(_objTransform);
            _objTransform.up = (_playerTransform.position - _objTransform.position);            
            _currentTime = 0;
            _respawnInt++;
            _liveEnemy++;
            CanvasController.singletonCanvas.UpdateStage(_liveEnemy, _startCont);
        }
    }

    private void MoveEnemy()
    {
        for (int i = 0; i < _enemyList.Count; i++)
        {
            _enemyList[i].transform.position += _enemyList[i].up * _currenEnemySpeed * Time.fixedDeltaTime;
            CheckedPlayer(_enemyList[i]);
        }
    }

    private void CheckRespawn()
    {       
        if (_respawnInt >= _startCont && _respawnOn)
        {
            _respawnOn = false;
            _respawnInt = 0;
        }
        else if (_liveEnemy <= 0)
        {
            _respawnOn = true;
            _startCont++;
            CanvasController.singletonCanvas.UpdateNumberStage();
        }
    }

    public void UpDecStage()
    {
        _currentEnemyHealth += enemySO.health;
        _currentEnemyScore += (int)(enemySO.score * 0.5f);
        _currenEnemySpeed += 0.1f * enemySO.speed;
    }

   

    private void CheckedPlayer(Transform transform)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 0.5f, _playerLayerMask);
        if (hit.collider != null)
        {            
            PlayerController.singletonPlayer.TakeDamage(transform);
            SoundController.soundController.PlayerTakeDamage();
            transform.gameObject.SetActive(false);
            _enemyList.Remove(transform);
            _liveEnemy--;
            CanvasController.singletonCanvas.UpdateStage(_liveEnemy, _startCont);
        }
    }

    private void EnemyContChange(int i, Transform transform)
    {
        if (i<0)
        {
            _enemyList.Remove(transform);
            _liveEnemy--;
            CanvasController.singletonCanvas.UpdateStage(_liveEnemy, _startCont);
        }
    }

}
