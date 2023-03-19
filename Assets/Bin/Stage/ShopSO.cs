using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShopSO", menuName = "Stage/New ShopSO")]
public class ShopSO : ScriptableObject
{
    [SerializeField] private int _startPriceRadar, _startPriceSpeed, _startPriceDamage;
    [SerializeField] private float _percentUpPriceRadar, _percentUpPriceSpeed, _percentUpPriceDamage;
    [SerializeField] private int _upRadar, _upSpeed, _upDamage;

    public int startPriceRadar => _startPriceRadar;
    public int startPriceSpeed => _startPriceSpeed;
    public int startPriceDamage => _startPriceDamage;
    public int upRadar => _upRadar;
    public int upSpeed => _upSpeed;
    public int upDamage => _upDamage;

    public float percentUpPriceRadar => _percentUpPriceRadar;
    public float percentUpPriceSpeed => _percentUpPriceSpeed;

    public float percentUpPriceDamage => _percentUpPriceDamage;



}

