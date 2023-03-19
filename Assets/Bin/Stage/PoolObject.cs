using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    private List<GameObject> poolObjectList = new List<GameObject>();
    private Transform _thisTR;
    private GameObject _poolGO;

    public void Starting(GameObject poolObject, int cont)
    {
        if (cont > poolObjectList.Count) cont = cont - poolObjectList.Count;
        else cont = 0;
        for (int i = 0; i < cont; i++)
        {
            _poolGO = poolObject;
            _thisTR = GetComponent<Transform>();
            GameObject obj = Instantiate(_poolGO, _thisTR.position, _thisTR.rotation, _thisTR);
            poolObjectList.Add(obj);
            obj.AddComponent<EnemyMob>();           
        }
        for (int i=0; i < poolObjectList.Count; i++)
        {
            poolObjectList[i].SetActive(false);
        }
    }

    public void InstantiateObj(Vector3 direction, Vector3 position, out Transform transformGO)
    {        
        for (int i=0; i < poolObjectList.Count; i++)
        {
            if (!poolObjectList[i].activeSelf)
            {
                transformGO = poolObjectList[i].transform;
                transformGO.position = position;
                transformGO.up = Vector3.zero - transformGO.position;                                    
                poolObjectList[i].SetActive(true);
                return;
            }
        }
        GameObject obj = Instantiate(_poolGO, position, _thisTR.rotation, _thisTR);
        transformGO = obj.transform;
        obj.AddComponent<EnemyMob>();
        transformGO.up = direction;       
        
        poolObjectList.Add(obj);
        obj.SetActive(true);
    }
}
