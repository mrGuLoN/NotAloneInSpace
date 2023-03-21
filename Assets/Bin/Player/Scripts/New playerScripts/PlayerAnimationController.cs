using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimationController : MonoBehaviour
{
    private Transform _visualPlayerTransform;
    private Animator _animator;
    private GameObject _visualFire;
    private Vector3 _startVisualPosition;

    private void Start()
    {
        DOTween.Init();
    }
    public void UpdateStartParametr(Transform visualPlayerTransform, Transform visualFireTransform)
    {
        _visualPlayerTransform = visualPlayerTransform;
        _startVisualPosition = visualPlayerTransform.position;
        _animator = _visualPlayerTransform.gameObject.GetComponent<Animator>();
        _animator.SetBool("Dead", false);
        _visualFire = visualFireTransform.gameObject;
        _visualFire.SetActive(true);
    }

    // Update is called once per frame
    public void DamageAnimation()
    {
        _visualPlayerTransform.position = _startVisualPosition;
        _visualPlayerTransform.DOShakePosition(0.5f, 0.1f, 10, 360f, false, true, ShakeRandomnessMode.Full).OnComplete(ReturnTransform);
    }

    public void DeadAnimation()
    {
        _animator.SetBool("Dead", true);
        _visualPlayerTransform.DOShakePosition(0.5f, 0.1f, 10, 360f, false, true, ShakeRandomnessMode.Full).OnComplete(ReturnTransform);
        _visualFire.SetActive(false);
    }

    private void ReturnTransform()
    {
        _visualPlayerTransform.position = _startVisualPosition;
    }
}
