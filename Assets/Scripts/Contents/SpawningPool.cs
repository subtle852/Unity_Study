using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    int _currentMonsterCount = 0;
    int _reserveMonsterCount = 0;
    [SerializeField]
    int _keepMonsterCount = 0;

    [SerializeField]
    Vector3 _spawnPos;
    [SerializeField]
    float _spawnRadius = 15.0f;
    [SerializeField]
    float _spawnTime = 5.0f;
    
    public void AddCurrentMonsterCount(int value) { _currentMonsterCount += value; }
    public void SetKeepMonsterCount(int value) { _keepMonsterCount += value; }

    void Start()
    {
        Managers.Game.OnSpawnEvent -= AddCurrentMonsterCount;
        Managers.Game.OnSpawnEvent += AddCurrentMonsterCount;


    }

    void Update()
    {
        while (_currentMonsterCount + _reserveMonsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }

    IEnumerator ReserveSpawn()
    {
        _reserveMonsterCount++;

        yield return new WaitForSeconds(Random.Range(0.0f, _spawnTime));

        GameObject obj = Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");

        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>();
        while (true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0.0f, _spawnRadius);
            randDir.y = 0;
            Vector3 randPos = _spawnPos + randDir;

            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randPos, path) == true)
            {
                obj.transform.position = randPos;
                _reserveMonsterCount--;
                break;
            }
        }
    }
}
