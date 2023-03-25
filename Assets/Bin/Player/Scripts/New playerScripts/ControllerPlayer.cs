using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayer : MonoBehaviour
{
    public static ControllerPlayer singletonePlayer;

    [SerializeField] private PlayerSO playerSO;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform visualPlayerTransform, visualFireTransform, radarTransform;

    private PlayerPoolController _playerPool;
    private PlayerFireController _playerFire;
    private PlayerAnimationController _playerAnimation;

    private int _currentHealth;

    private List<Transform> _enemyTransform;
    private List<Transform> _bulletTransform;

    private void Awake()
    {
        singletonePlayer = this;
        DontDestroyOnLoad(this);
        _playerAnimation = gameObject.AddComponent<PlayerAnimationController>();
        _playerFire = gameObject.AddComponent<PlayerFireController>();
        _playerPool = gameObject.AddComponent<PlayerPoolController>();

        radarTransform.gameObject.AddComponent<PlayerRadar>();
        radarTransform.gameObject.AddComponent<Rigidbody2D>().gravityScale = 0;
    }
    void Start()
    {
      

        SetupPlayerSOParametrs();
    }

    public void SetupPlayerSOParametrs()
    {
        _playerAnimation.UpdateStartParametr(visualPlayerTransform, visualFireTransform);
        _playerFire.UpdateStartParametrs(playerSO, enemyLayer);
        _playerPool.UpdateStartParametr(playerSO);

        _currentHealth = playerSO.health;

        radarTransform.localScale = new Vector3(2, 2, 1);

        _enemyTransform = new List<Transform>();
        _bulletTransform = new List<Transform>();

        CanvasController.singletonCanvas.UpdateHealth(playerSO.health, _currentHealth);
    }

    private void FixedUpdate()
    {
        if (_enemyTransform.Count > 0)
        {
            _playerPool.Instantiate(_enemyTransform[0]);
        }
        if (_bulletTransform.Count >0)
        {
            _playerFire.BulletMove(_bulletTransform);
        }
    }

    // Update is called once per frame
    public void ChangeBulletList(int i, Transform bulletTransform)
    {
        if (i > 0) _bulletTransform.Add(bulletTransform);
        else _bulletTransform.Remove(bulletTransform);        
    }

    public void ChangeEnemyList(int i, Transform enemyTransform)
    {
        if (i > 0) _enemyTransform.Add(enemyTransform);
        else _enemyTransform.Remove(enemyTransform);
    }

    public void TakeDamage(Transform enemyTransform)
    {
        _currentHealth--;
        _enemyTransform.Remove(enemyTransform);

        if (_currentHealth > 0)
        {
            _playerAnimation.DamageAnimation();
            CanvasController.singletonCanvas.UpdateHealth(playerSO.health, _currentHealth);           
        }
        else
        {
            _playerAnimation.DeadAnimation();
            for (int i = 0; i < _bulletTransform.Count; i++)
            {
                _bulletTransform[i].gameObject.SetActive(false);               
            }            
            
            CanvasController.singletonCanvas.Dead();
        }

        CanvasController.singletonCanvas.UpdateHealth(playerSO.health, _currentHealth);
    }

    public void UpdateRadar(int updateRadar)
    {
        radarTransform.localScale = new Vector3(radarTransform.localScale.x+ updateRadar, radarTransform.localScale.y + updateRadar, 1);
    }

    public void UpdateSpeedFire(float updateSpeed)
    {
        _playerPool.UpdateFireSpeed(updateSpeed);
    }

    public void UpdateDamage(int updateDamage)
    {
        _playerFire.UpdateDamage(updateDamage);
    }
}
