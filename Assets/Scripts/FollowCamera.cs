using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FollowCamera : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField]
    private bool _drawDebug = false;
    [SerializeField]
    private GameObject _target;
    
    [SerializeField]
    private Vector3 _offset;
    [SerializeField] private Material _originalMat;
    [SerializeField] private Material _transparentMat;
    [SerializeField]
    private LayerMask _objectsToHideOnBlock;
    private Camera _main;

    private List<GameObject> _viewBlockers;
    private float _targetDistance;
    // Start is called before the first frame update
    void Start()
    {
        _viewBlockers = new List<GameObject>();
        _main = GetComponent<Camera>();
        LateUpdate();
        _targetDistance = Vector3.Distance(_main.transform.position, _target.transform.position);
    }

    void Update()
    {
        IsViewBlocked();
        if(_drawDebug)
        {
            Vector3 forward = _main.transform.TransformDirection(Vector3.forward) * _targetDistance;
            Debug.DrawRay(_main.transform.position, forward, Color.green);
        }
    }

    // Camera transform should be in LateUpdate as it ensures _target's Update function is already processed
    void LateUpdate()
    {
        transform.localPosition = _target.transform.localPosition;
        transform.Translate(_offset);
    }

    private bool IsViewBlocked()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(_main.transform.position, _main.transform.TransformDirection(Vector3.forward), out hitInfo, _targetDistance, _objectsToHideOnBlock))
        {
            UnblockView(hitInfo.transform.gameObject);
        }
        else
        {
            ResetViewBlockers(null);
        }

        return false;
    }

    private void UnblockView(GameObject gameObject)
    {
        gameObject.GetComponent<MeshRenderer>().material = _transparentMat;

        if(!_viewBlockers.Contains(gameObject))
        {
            _viewBlockers.Insert(0, gameObject);
        }

        ResetViewBlockers(gameObject);
    }

    private void ResetViewBlockers(GameObject exceptGameObject)
    {
        for(int i = _viewBlockers.Count - 1; i >= 0; i--)
        {
            if(_viewBlockers[i] != exceptGameObject)
            {
                _viewBlockers[i].GetComponent<MeshRenderer>().material = _originalMat;
                _viewBlockers.Remove(gameObject);
            }
        }
    }
}
