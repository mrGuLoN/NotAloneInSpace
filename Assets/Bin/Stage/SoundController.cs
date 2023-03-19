using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController soundController { get; private set; }

    [SerializeField] private AudioSource stageMusic, enemyBurn, enemyDead, playerTakeDamage, playerShoot;

    private void Awake()
    {
        soundController = this;
    }

    public void EnemyBurn()
    {
        enemyBurn.Play();
    }

    public void EnemyDead()
    {
        enemyDead.Play();
    }

    public void PlayerTakeDamage()
    {
        playerTakeDamage.Play();
    }

    public void PlayerShoot()
    {
        playerShoot.Play();
    }
}
