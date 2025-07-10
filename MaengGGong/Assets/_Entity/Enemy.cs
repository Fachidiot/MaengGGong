using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f; // 이동 속도
    [SerializeField] private float _rotationSpeed = 5f; // 회전 속도
    [SerializeField] private float _stoppingDistance = 0.5f; // 플레이어와의 최소 거리
    private Transform _player;
    private Rigidbody _rigidBody;
    private MeshCollider _collider;

    public void SetPlayer(GameObject player)
    {
        _player = player.transform;
    }

    void Start()
    {
        _collider = GetComponentInChildren<MeshCollider>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (null == _player)
        {
            Debug.LogError("Player not found.");
            return;
        }

        if (_stoppingDistance < Vector3.Distance(transform.position, _player.position))
        {   // 플레이어가 stoppingDistance 이상 거리에 있을 때만 이동
            Vector3 direction = (_player.position - transform.position).normalized;   // 플레이어를 향하는 방향 계산

            Vector3 velocity = direction * _moveSpeed;  // 이동 벡터 계산

            _rigidBody.velocity = new Vector3(velocity.x, _rigidBody.velocity.y, velocity.z);   // Rigidbody를 사용해 이동, y축은 중력 유지

            Quaternion targetRotation = Quaternion.LookRotation(direction); // 플레이어를 향해 부드럽게 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {   // 플레이어 근처에 도달하면 자동 소멸
            Destroy(gameObject);
        }
    }
}
