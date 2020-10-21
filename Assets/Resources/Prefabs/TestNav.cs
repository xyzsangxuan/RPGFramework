using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNav : MonoBehaviour
{
    NavMeshAgent _agent;
    Animator _animator;

    float _stopDis = 5;
    public Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        _agent = gameObject.AddComponent<NavMeshAgent>();
        _agent.SetDestination(target.position);
        _agent.stoppingDistance = _stopDis;
        _animator = GetComponent<Animator>();
        _animator.SetInteger("MotionType", 1);
    }

    // Update is called once per frame
    void Update()
    {
        //如果是2D/2.5D则需要在平面内降维求距离
        if (Vector3.Distance(transform.position, target.transform.position) < _stopDis)
        {
            _animator.SetInteger("MotionType", 0);
        }
    }
}
