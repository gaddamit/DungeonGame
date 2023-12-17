using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AIState
{
    IDLE,
    PATROL,
    CHASE,
    ATTACK,
    DIE,
    DEAD
};

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private GameObject _targetObject;
    [SerializeField]
    private float _distanceToChase = 5.0f;
    [SerializeField]
    private float _distanceToAttack = 0.5f;
    [SerializeField]
    private float _deathAnimationTime = 2.0f;
    [SerializeField]
    private string[] _animations;
    [SerializeField]
    private GameObject[] _patrolPoints;

    private Transform _targetTransform;
    private UnityEngine.AI.NavMeshAgent _agent;

    private bool _beginDeath = false;

    private AIState _state;
    private AIState _prevState;
    private Animator _animator;

    private AudioSource _audioDeath;

    private Vector3 _startingPosition;
    private Quaternion _startingRotation;

    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
        _startingRotation = transform.rotation;

        if(_targetObject)
        {
            _targetTransform = _targetObject.transform;
        }

        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        _animator = GetComponent<Animator>();
        _state = AIState.IDLE;
        _prevState = AIState.IDLE;

        _audioDeath = GetComponent<AudioSource>();

        PerformAIAction();
    }

    private void PerformAIAction()
    {
        switch(_state)
        {
            case AIState.IDLE:
                Idle();
                break;
            case AIState.PATROL:
                Patrol();
                break;
            case AIState.CHASE:
                Chase();
                break;
            case AIState.ATTACK:
                Attack();
                break;
            case AIState.DIE:
                Die();
                break;
            default:
                break;
        }

        if(_state != _prevState)
        {
            string animation = _animations[(int)_state];
            if(!string.IsNullOrEmpty(animation))
            {
                _animator?.Play(animation);
                _prevState = _state;
            }
        }
    }

    private bool IsWithinDistance(GameObject gameObject, float distanceCheck)
    {
        float distance = Vector3.Distance(transform.position, gameObject.transform.position);
        return distance < distanceCheck; 
    }

    private void Idle()
    {
        SetDestination(this.gameObject);
    }

    private void Chase()
    {
        SetDestination(_targetObject);
    }

    private void Patrol()
    {

    }

    private void Attack()
    {

    }

    private void Die()
    {
        SetDestination(this.gameObject);
        Invoke("HideEnemy", _deathAnimationTime);
    }

    private void SetDestination(GameObject destinationObject)
    {
        _agent.destination = destinationObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        if(_state == AIState.DEAD)
        {
            return;
        }

        if(_beginDeath)
        {
            _state = AIState.DIE;
        }
        else
        {
            _state = AIState.IDLE;

            if(_patrolPoints.Count > 0)
            {
                _state = AIState.PATROL;
            }
            if(IsWithinDistance(_targetObject, _distanceToChase))
            {
                _state = AIState.CHASE;
            }
            if(IsWithinDistance(_targetObject, _distanceToAttack))
            {
                _state = AIState.ATTACK;
            }
        }

        PerformAIAction();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Projectile")
        {
            _beginDeath = true;
            _audioDeath?.Play();
            SetColliderEnabled(false);
        }
    }

    public void Reset()
    {
        // Reset original properties
        transform.position = _startingPosition;
        transform.rotation = _startingRotation;

        gameObject.SetActive(true);
        SetColliderEnabled(true);

        _state = AIState.IDLE;
        _beginDeath = false;
    }

    private void HideEnemy()
    {
        gameObject.SetActive(false);
    }

    private void SetColliderEnabled(bool enabled)
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = enabled;
    }
}
