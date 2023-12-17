using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStaff : MonoBehaviour
{
    [SerializeField]
    private GameObject _projectile;
    [SerializeField]
    private Transform _launchOrigin;
    [SerializeField]
    private float _projectileVelocity = 100f;
    [SerializeField]
    private LayerMask _groundMask;
    private Camera _camera;
    private bool _fireButtonDown = false;

    private AudioSource _audioAttack;

    // Start is called before the first frame update
    void Start()
    {
        // Cache Camera.main
        _camera = Camera.main;

        _audioAttack = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && Time.timeScale > 0)
        {
            _fireButtonDown = true;
            _audioAttack?.Play();
        }
    }

    private void FixedUpdate()
    {
        if(_fireButtonDown)
        {
            // Instantiate projectile
            Vector3 mouseAimPosition = AimAtMousePosition();

            GameObject projectile = Instantiate(_projectile, _launchOrigin.position, _launchOrigin.rotation);
            projectile.name = "Projectile";
            if(mouseAimPosition != Vector3.zero)
            {
                Vector3 direction = mouseAimPosition - transform.position;
                direction = Vector3.Normalize(direction);
                projectile.GetComponent<Rigidbody>().AddForce(direction * _projectileVelocity);
            }

            _fireButtonDown = false;
        }
    }

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
}
