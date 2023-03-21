using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveController : MonoBehaviour
{
    private float _speed;
    private LayerMask _playerLayer;
    public void UpdateStartParametrs (float speedEnemy, LayerMask playerLayer)
    {
        _speed = speedEnemy;
        _playerLayer = playerLayer;
    }

    // Update is called once per frame
    public void MoveEnemy(List<Transform> enemyTransformList)
    {
        for (int i=0; i < enemyTransformList.Count;i++)
        {
            enemyTransformList[i].position += enemyTransformList[i].up * _speed * Time.fixedDeltaTime;
            CheckedPlayer(enemyTransformList[i]);
        }
    }

    private void CheckedPlayer(Transform transform)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 0.5f, _playerLayer);
        if (hit.collider != null)
        {
            Debug.Log(transform.name);
            ControllerPlayer.singletonePlayer.TakeDamage(transform);
            SoundController.soundController.PlayerTakeDamage();
            ControllerEnemy.controllerEnemySingletone.ChangeEnemyList(-1, transform);
            transform.gameObject.SetActive(false);           
        }
    }
    public void UpdateEnemySpeed(float updateSpeed)
    {
        _speed += updateSpeed;
    }
}
