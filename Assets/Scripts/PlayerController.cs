using TMPro;
using UnityEngine;

enum PlayerState
{
    IDLE,
    WALK,
    RUN,
    ATTACK,
    DIE,
    JUMP
};

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _jumpForce = 200.0f;
    [SerializeField]
    private float _origSpeed = 500.0f;
    public float OrigSpeed
    {
        get
        {
            return _origSpeed;
        }
        set
        {
            _origSpeed = value;
        }
    }

    [SerializeField]
    private float _speed = 500.0f;
    public float Speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }

    [SerializeField]
    private Material _origMaterial;
    [SerializeField]
    private LayerMask _groundMask;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private ParticleSystem _trailParticles;
    [SerializeField]
    private string[] _animations;

    private Camera _camera;
    private Rigidbody _rb;
    private float _inputH, _inputV;
    private bool _isJumpPressed = false;
    private bool _isControlEnabled = true;
    private float _colliderOffest;

    private Vector3 _startingPosition;
    private Quaternion _startingRotation;

    private PlayerState _state;
    private PlayerState _prevState;
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
        _startingRotation = transform.rotation;

        _rb = GetComponent<Rigidbody>();
        
        // Cache Camera.main
        _camera = Camera.main;

        // Pre-calculate ray cast max distance using collider height plus a small offset
        _colliderOffest = GetComponent<CapsuleCollider>().height / 2 + 0.1f;
        
        _state = PlayerState.IDLE;
        _prevState = PlayerState.IDLE;

        _animator = GetComponent<Animator>();
        UpdateAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isControlEnabled)
        {
            return;
        }

        if(Input.GetButtonDown("Jump"))
        {
            _isJumpPressed = true;
        }

        // Save the input values to be used in FixedUpdate()
        _inputH = Input.GetAxis("Horizontal");
        _inputV = Input.GetAxis("Vertical");

        Vector3 mouseAimPosition = AimAtMousePosition();
        if(mouseAimPosition != Vector3.zero)
        {
            Vector3 direction = mouseAimPosition - transform.position;
            // Since the character can't look up and down
            direction.y = 0;
            transform.forward = direction;
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if(_inputH == 0 && _inputV == 0)
        {
            _state = PlayerState.IDLE;
        }
        else
        {
            _state = PlayerState.RUN;
        }

        if(_state != _prevState)
        {
            _animator.Play((_animations[(int)_state]));
            _prevState = _state;
        }
    }

    // Check if object's feet touches an object
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _colliderOffest);
    }

    // Cast a ray from mouse to ground then return then position if there's a hit
    private Vector3 AimAtMousePosition()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, _groundMask.value))
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }

    // Physics calculations sould be done on FixedUpdate to avoid rough movement
    void FixedUpdate()
    {
        if(!_isControlEnabled)
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        if(IsGrounded())
        {
            if(_isJumpPressed)
            {
                _rb.AddForce(Vector3.up * _jumpForce);
            }
        }
       
        _isJumpPressed = false;

        float speedPerFrame = _speed * Time.fixedDeltaTime;
        _rb.velocity = new Vector3(_inputH * speedPerFrame, _rb.velocity.y, _inputV * speedPerFrame);
    }

    private void OnTriggerEnter(Collider other)
    {
        Collectable collectable = other.GetComponent<Collectable>();
        if(collectable)
        {
            _gameManager?.CollectItem(collectable);
            collectable.OnCollect(this);
        }
    }

    public void Reset()
    {
        // Reset original properties
        transform.position = _startingPosition;
        transform.rotation = _startingRotation;
        
        _speed = _origSpeed;

        // Re-enable controls
        SetControlsActive(true);
    }

    public void SetControlsActive(bool active)
    {
        _isControlEnabled = active;
    }

    public void StartTrailParticles()
    {
        _trailParticles?.Play();
    }
}