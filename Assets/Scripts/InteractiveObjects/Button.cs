using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Button : ActivatableObject
{
    [SerializeField]
    private float _thresholdCollisionVelocity = -5.0f;
    
    [SerializeField]
    private ActivatableObject _linkedObject;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(_state == State.DEFAULT)
        {
            // Activate by jumping on the button
            if(collision.relativeVelocity.y <= _thresholdCollisionVelocity)
            {
                Activate();
                ActivateLinkedObject();
            }
        }
    }

    private void ActivateLinkedObject()
    {
        if(_linkedObject)
        {
            _linkedObject.Activate();
        }
    }
}
