using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;
    
    [SerializeField]
    private Vector3 _offset;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Camera transform should be in LateUpdate as it ensures _target's Update function is already processed
    void LateUpdate()
    {
        transform.localPosition = _target.transform.localPosition;
        transform.Translate(_offset);
    }
}
