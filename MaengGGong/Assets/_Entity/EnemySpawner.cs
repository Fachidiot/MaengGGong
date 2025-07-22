using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject _enemyPrefab;
    [SerializeField] private Transform[] _spawnPoints;

    private GameObject _player;
    float _nextSpawnTime = 0;
    bool _spawnDone = true;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (_spawnDone)
        {
            _spawnDone = false;
            StartCoroutine(RandomSpawn());
        }
    }

    IEnumerator RandomSpawn()
    {
        _nextSpawnTime = Random.Range(1, 2);
        yield return new WaitForSeconds(_nextSpawnTime);

        Spawn(_spawnPoints[Random.Range(0, _spawnPoints.Length)]);
        _spawnDone = true;
    }

    private void Spawn(Transform transform)
    {
        GameObject enemy = Instantiate(_enemyPrefab, transform.position, transform.rotation);
        Enemy setting = enemy.GetComponent<Enemy>();
        setting.SetPlayer(_player);
        Destroy(enemy, 15);
    }
}
