using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireController : MonoBehaviour
{
    private int _damage;
    private float _speedBullet;
    private LayerMask _enemyLayer;
    private Transform _playerTramsform;
    public void UpdateStartParametrs(PlayerSO playerSO, LayerMask enemyLayer)
    {
        _damage = playerSO.damage;
        _speedBullet = playerSO.bulletSpeed;
        _enemyLayer = enemyLayer;
        _playerTramsform = ControllerPlayer.singletonePlayer.transform;
    }

    // Update is called once per frame
    public void BulletMove(List<Transform> bulletsTransforms)
    {
        for (int i = 0; i < bulletsTransforms.Count; i++)
        {            

            bulletsTransforms[i].position += bulletsTransforms[i].up * _speedBullet * Time.fixedDeltaTime;

            //Проверяем попала ли пуля в противника
            RaycastHit2D hit = Physics2D.Raycast(bulletsTransforms[i].position, bulletsTransforms[i].up, 0.1f, _enemyLayer);

            if (hit.collider !=null)
            {
                hit.transform.gameObject.GetComponent<EnemyMob>().TakeDamage(_damage);
                bulletsTransforms[i].gameObject.SetActive(false);
                ControllerPlayer.singletonePlayer.ChangeBulletList(-1, bulletsTransforms[i]);                
                i--;
            }
            else if ( Vector3.Distance(_playerTramsform.position, bulletsTransforms[i].position) > 20)
            {
                bulletsTransforms[i].gameObject.SetActive(false);
                ControllerPlayer.singletonePlayer.ChangeBulletList(-1, bulletsTransforms[i]);
                i--;
            }
        }
    }

    public void UpdateDamage(int damage)
    {
        _damage += damage;
    }
}
