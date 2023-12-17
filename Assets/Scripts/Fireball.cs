using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    private float _lifeSpan = 2.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObject", _lifeSpan);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        DestroyObject();
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
