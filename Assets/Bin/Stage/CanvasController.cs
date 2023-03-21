using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private Text costUpdateRadar, costUpdateSpeed, costUpdateDamage, score, stageProgress;
    [SerializeField] private Scrollbar energy, stageProgressScroll;
    [SerializeField] private ShopSO shopSO;
    [SerializeField] private GameObject startPannel;

    public static CanvasController singletonCanvas { get; private set; }

    private int _scoreInt, _updateRadarInt, _updateSpeedInt, _updateDamageInt, _numberStage, _stageDec;
    private int _lvlRadar;

    private void Awake()
    {
        singletonCanvas = this;
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        ControllerPlayer.singletonePlayer.gameObject.SetActive(false);
        ControllerEnemy.controllerEnemySingletone.gameObject.SetActive(false);
        startPannel.SetActive(true);
    }

    public void UpdateRadar()
    {        
        if (_updateRadarInt <= _scoreInt && _lvlRadar < 2)
        {
            _scoreInt -= _updateRadarInt;
            score.text = _scoreInt.ToString();
            ControllerPlayer.singletonePlayer.UpdateRadar(shopSO.upRadar);
            _updateRadarInt += _updateRadarInt * (int)shopSO.percentUpPriceRadar / 100;
            costUpdateRadar.text = _updateRadarInt.ToString();
            _lvlRadar++;
        }
        else
        {
            costUpdateRadar.text = "MAX";
        }
    }

    public void UpdateSpeed()
    {
        if (_updateSpeedInt <= _scoreInt)
        {
            _scoreInt -= _updateSpeedInt;
            score.text = _scoreInt.ToString();
            ControllerPlayer.singletonePlayer.UpdateSpeedFire(shopSO.upSpeed);
            _updateSpeedInt += _updateSpeedInt * (int)shopSO.percentUpPriceSpeed / 100;
            costUpdateSpeed.text = _updateSpeedInt.ToString();
        }
    }

    public void UpdateDamage()
    {
        if (_updateDamageInt <= _scoreInt)
        {
            _scoreInt -= _updateDamageInt;
            score.text = _scoreInt.ToString();
            ControllerPlayer.singletonePlayer.UpdateDamage(shopSO.upDamage);
            _updateDamageInt += _updateDamageInt * (int)shopSO.percentUpPriceDamage / 100;
            costUpdateDamage.text = _updateDamageInt.ToString();
        }
    }

    public void UpdateScore(int update)
    {      
        _scoreInt += update;
        score.text = _scoreInt.ToString();
    }

    public void UpdateHealth(int maxHealth, int CurrentHealth)
    {
        energy.size =  (float)CurrentHealth / (float)maxHealth;
        if (energy.size <= 0.01f) energy.size = 0.05f;
    }

    public void UpdateStage(int updateEnemy, int maxEnemy)
    {
        stageProgressScroll.size = (float)updateEnemy / (float)maxEnemy;
        if (stageProgressScroll.size <= 0.01f) stageProgressScroll.size = 0.05f;
        
    }

    public void UpdateNumberStage()
    {
        _numberStage++;
        stageProgress.text = "Stage: " + _numberStage;
        _stageDec++;
        if (_stageDec >= 10)
        {
            _stageDec = 0;
            ControllerEnemy.controllerEnemySingletone.UpdateMobeStage();
        }
    }

    public void Dead()
    {
        ControllerEnemy.controllerEnemySingletone.enabled = false;
        ControllerPlayer.singletonePlayer.enabled = false;
        startPannel.SetActive(true);
    }

    public void Starting()
    {
        _stageDec = 0;
        _scoreInt = 0;
        _lvlRadar = 0;

        _updateRadarInt = shopSO.startPriceRadar;
        _updateSpeedInt = shopSO.startPriceSpeed;
        _updateDamageInt = shopSO.startPriceDamage;

        score.text = _scoreInt.ToString();
        costUpdateRadar.text = _updateRadarInt.ToString();
        costUpdateDamage.text = _updateDamageInt.ToString();
        costUpdateSpeed.text = _updateSpeedInt.ToString();

        ControllerPlayer.singletonePlayer.gameObject.SetActive(true);
        ControllerPlayer.singletonePlayer.enabled = true;
        ControllerPlayer.singletonePlayer.SetupPlayerSOParametrs();

        ControllerEnemy.controllerEnemySingletone.gameObject.SetActive(true);
        ControllerEnemy.controllerEnemySingletone.enabled = true;
        ControllerEnemy.controllerEnemySingletone.SetupEnemySOParametrs();

        startPannel.SetActive(false);
        _stageDec = 0;
        _numberStage = 0;
        UpdateNumberStage();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
