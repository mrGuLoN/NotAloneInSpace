using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyMob : MonoBehaviour
{   

    public delegate void EnemyContChange(int real, Transform transform);
    public static event EnemyContChange _enemyContChange;

    [HideInInspector] public int health, score;
    private Transform _visualTransform;
    
    private Animator _ani;
    private bool _dead = false;
    private CapsuleCollider2D _col;

    private void Awake()
    {
        _visualTransform = gameObject.GetComponentInChildren<SpriteRenderer>().transform;
        _ani = _visualTransform.gameObject.GetComponent<Animator>();
        
        
        if (this.gameObject.TryGetComponent<CapsuleCollider2D>(out CapsuleCollider2D collider2D))
        {
            _col = GetComponent<CapsuleCollider2D>();
        }
    }

    private void OnEnable()
    {       
        _dead = false;
        SoundController.soundController.EnemyBurn();
        if (_col != null) _col.enabled = true;
    }

    private void OnDisable()
    {
        
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if (health > 0)
        {
            _visualTransform.DOShakePosition(0.5f, 0.1f, 10, 360f, false, true, ShakeRandomnessMode.Full);
        }
        else if (_dead == false)
        {
            CanvasController.singletonCanvas.UpdateScore(score);
            _dead = true;
            _ani.SetBool("Dead", true);         
            if (_col != null) _col.enabled = false;
            ControllerPlayer.singletonePlayer.ChangeEnemyList(-1, this.transform);
            ControllerEnemy.controllerEnemySingletone.ChangeEnemyList(-1, this.transform);
            SoundController.soundController.EnemyDead();
            StartCoroutine(Destroy());
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }

  
}
