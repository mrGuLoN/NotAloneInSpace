using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScript : MonoBehaviour
{
    [SerializeField] private float _speedScroll;

    private Material _material;
    private float _currentScroll;
    void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        _currentScroll += _speedScroll * Time.deltaTime;
        _material.mainTextureOffset = new Vector2(_currentScroll, 0);        
    }
}
