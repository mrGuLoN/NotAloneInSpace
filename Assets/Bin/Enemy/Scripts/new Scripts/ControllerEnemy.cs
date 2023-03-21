using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerEnemy : MonoBehaviour
{
    public static ControllerEnemy controllerEnemySingletone { get; private set; }

    [SerializeField] private EnemySO enemySO;
    [SerializeField] private LayerMask _playerLayerMask;

    [HideInInspector] public int currentNumberEnemy;

    
    private EnemyMoveController _enemyMoveController;
    private EnemyRespawnController _enemyRespawnController;
    private List<Transform> _aliveEnemyList;
    

    
    private void Awake()
    {
        controllerEnemySingletone = this;
        DontDestroyOnLoad(this);
        EnemyMob._enemyContChange += ChangeEnemyList;
    }

    void Start()
    {          
        _enemyMoveController = gameObject.AddComponent<EnemyMoveController>();
        _enemyRespawnController = gameObject.AddComponent<EnemyRespawnController>();

        currentNumberEnemy = enemySO.startCont;

        SetupEnemySOParametrs();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _enemyRespawnController.Respawn(_aliveEnemyList.Count);
        _enemyMoveController.MoveEnemy(_aliveEnemyList);
    }

    public void SetupEnemySOParametrs()
    {
        _aliveEnemyList = new List<Transform>();

        _enemyRespawnController.UpdateStartParametrs(enemySO.startCont, enemySO.health, enemySO.score, enemySO.timerRespawn, enemySO.enemyGO);
        _enemyMoveController.UpdateStartParametrs(enemySO.speed, _playerLayerMask);
    }

    public void ChangeEnemyList(int i, Transform enemyTransform)
    {
        if (i > 0) _aliveEnemyList.Add(enemyTransform);   
        else _aliveEnemyList.Remove(enemyTransform);
        CanvasController.singletonCanvas.UpdateStage(_aliveEnemyList.Count, currentNumberEnemy);
    }

    public void UpdateMobeStage()
    {
        _enemyMoveController.UpdateEnemySpeed(enemySO.speed*0.1f);
        _enemyRespawnController.UpdateParametrs(enemySO.health, enemySO.score);
    }
}
