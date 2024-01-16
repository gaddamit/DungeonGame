using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 100.0f;
    [SerializeField]
    private GameObject _collectableObject;
    private AudioSource _collectSound;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _collectSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Rotate(0, Time.deltaTime * _rotationSpeed, 0, Space.World);
    }

    public virtual void OnCollect(PlayerController playerController)
    {
        _collectSound?.Play();
        _collectableObject?.SetActive(false);
        Invoke("HideCollectable", 0.5f);
    }

    public void Reset()
    {
        _collectableObject?.SetActive(true);
        gameObject.SetActive(true);
    }

    private void HideCollectable()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if(player)
        {
            OnCollect(player);
            player.OnCollect(this);
        }
    }
}
