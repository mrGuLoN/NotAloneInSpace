using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public static PlayerController singletonPlayer { get; private set;}

    [SerializeField] private PlayerSO playerSO;
    [SerializeField] Transform playerVisualTransform, radiusFireTransform;
    [SerializeField] private LayerMask enemyLayer;


    private int _currentHealth;
    private float _bulletDeltaTime, _currentTime;
    private int _currentDamage;
    private int _radiusFire;
    private PoolObject _poolBullet;
    private List<Transform> _enemyTransform = new List<Transform>();
    private List<Transform> _bulletTransform = new List<Transform>();
    private Transform _thisTR;
    private Vector3 _playerVisualStartPosition;

    private void Awake()
    {
        singletonPlayer = this;
        PlayerRadar._enemiInRadar += AddEnemyListTR;
        EnemyMob._enemyContChange += RemoveListEnemyTransform;
        _poolBullet = this.gameObject.AddComponent<PoolObject>();
        _poolBullet.Starting(playerSO.bulletGO, 10);
        radiusFireTransform.gameObject.AddComponent<PlayerRadar>();
        radiusFireTransform.gameObject.AddComponent<Rigidbody2D>().gravityScale = 0;

        _playerVisualStartPosition = playerVisualTransform.position;
    }
    public void Starting()
    {       
        _thisTR = GetComponent<Transform>();
        GetComponentInChildren<Animator>().SetBool("Dead", false);

        _enemyTransform = new List<Transform>();
        _bulletTransform = new List<Transform>();

        _currentTime = playerSO.speedFire;
        _currentHealth = playerSO.health;
        _bulletDeltaTime = playerSO.speedFire;
        _currentDamage = playerSO.damage;
        _radiusFire = playerSO.radiusFire;

        UpRadiusFire(0);

        
        CanvasController.singletonCanvas.UpdateHealth(playerSO.health, _currentHealth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _currentTime += Time.deltaTime;
        if (_enemyTransform.Count>0) Fire();
        if (_bulletTransform.Count>0)
        {
            BulletMove();
        }
    }

    public void UpRadiusFire(int upRadius)
    {
        _radiusFire += upRadius;
        radiusFireTransform.localScale = new Vector3(_radiusFire, _radiusFire, 1);
    }

    private void AddEnemyListTR(Transform enemyTR)
    {
        _enemyTransform.Add(enemyTR);
    }

    private void Fire()
    {       
        if (_currentTime >= _bulletDeltaTime)
        {
            SoundController.soundController.PlayerShoot();
            Vector3 direction = _enemyTransform[0].position - _thisTR.position;
            Transform _objTransform;
            _poolBullet.InstantiateObj(direction.normalized, _thisTR.position, out _objTransform);
            _objTransform.up = direction.normalized;
            _bulletTransform.Add(_objTransform);
            _currentTime = 0;           
        }
    }
    private void BulletMove()
    {
        for (int i = 0; i < _bulletTransform.Count; i++)
        {
            _bulletTransform[i].position += _bulletTransform[i].up * playerSO.bulletSpeed * Time.deltaTime;
            BulletCheck(_bulletTransform[i], i,out i);
        }
    }

    private void BulletCheck(Transform bulletTransform, int inInt,out int i)
    {
        RaycastHit2D hit = Physics2D.Raycast(bulletTransform.position,bulletTransform.up,0.1f,enemyLayer);
        if (hit.collider != null)
        {           
            _bulletTransform.Remove(bulletTransform);           
            bulletTransform.gameObject.SetActive(false);
            hit.transform.gameObject.GetComponent<EnemyMob>().TakeDamage(_currentDamage);          

            i=inInt-1;
        }
        else if (Vector2.Distance(bulletTransform.position, _thisTR.position)>15f)
        {
            i = inInt-1;
            _bulletTransform.Remove(bulletTransform);           
            bulletTransform.gameObject.SetActive(false);
        }
        else
        {
            i = inInt;
        }
    }

    public void TakeDamage(Transform enemyTransform)
    {        
        playerVisualTransform.position = _playerVisualStartPosition;
        playerVisualTransform.DOShakePosition(0.5f, 0.1f, 10, 360f, false, true, ShakeRandomnessMode.Full).OnComplete(ReturnTransform);
        _enemyTransform.Remove(enemyTransform);
        CanvasController.singletonCanvas.UpdateHealth(playerSO.health,_currentHealth);
        _currentHealth--;
        if (_currentHealth <=0)
        {
            GetComponentInChildren<Animator>().SetBool("Dead", true);
            for (int i = 0; i < _bulletTransform.Count; i++)
            {
                _bulletTransform[i].gameObject.SetActive(false);
            }
            CanvasController.singletonCanvas.Dead();
        }
    }


    private void RemoveListEnemyTransform(int i, Transform _enemy)
    {      
        if (i < 0)
        _enemyTransform.Remove(_enemy);
    }

    private void ReturnTransform()
    {
        playerVisualTransform.position = _playerVisualStartPosition;
    }

    public void UpdateDamage(int update)
    {
        _currentDamage += update;
    }

    public void UpdateSpeed(float speed)
    {
        _bulletDeltaTime = 60/ (speed + 60/_bulletDeltaTime);
    }

  
}
