using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatableObject : MonoBehaviour
{
    [SerializeField]
    protected Vector3 _targetPosition;
    protected Vector3 _startingPosition;
    [SerializeField]
    protected Material _matActive;
    [SerializeField]
    protected Material _matInactive;
    protected enum State
    {
        DEFAULT,
        ACTIVATING,
        DEACTIVATED,
    }

    protected State _state = State.DEFAULT;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _startingPosition = transform.localPosition;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(_state == State.ACTIVATING)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _targetPosition, Time.deltaTime);
            if(Vector3.Distance(transform.localPosition, _targetPosition) <= 0.01f)
            {
                _state = State.DEACTIVATED;
            }
        }
    }

    public virtual void Activate()
    {
        _state = State.ACTIVATING;
        GetComponent<Renderer>().material = _matInactive;
    }

    public virtual void Reset()
    {
        transform.localPosition = _startingPosition;
        GetComponent<Renderer>().material = _matActive;

        _state = State.DEFAULT;
    }
}
