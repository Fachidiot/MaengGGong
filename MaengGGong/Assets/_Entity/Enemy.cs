using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f; // 이동 속도
    [SerializeField] private float _rotationSpeed = 5f; // 회전 속도
    [SerializeField] private float _stoppingDistance = 0.2f; // 플레이어와의 최소 거리
    [SerializeField] private Transform _player;
    private NavMeshAgent _navMeshAgent;

    public void SetPlayer(GameObject player)
    {
        _player = player.transform;
    }

    public void Die()
    {
        _navMeshAgent.isStopped = true;
        // Die Animation & SFX 재생
        Debug.Log($"{name} dead");
    }

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _moveSpeed; // NavMeshAgent의 속도 설정
        _navMeshAgent.stoppingDistance = _stoppingDistance; // NavMeshAgent의 정지 거리 설정
    }

    void Update()
    {
        if (null == _player)
        {
            Debug.LogError("Player not found.");
            return;
        }

        // 플레이어 위치로 목적지 설정
        _navMeshAgent.SetDestination(_player.position);

        // 플레이어와의 거리가 stoppingDistance 이하일 때 소멸
        if (Vector3.Distance(transform.position, _player.position) <= _stoppingDistance)
        {
            Destroy(gameObject);
        }
        else
        {
            // 플레이어를 향해 부드럽게 회전
            Vector3 direction = (_player.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}
