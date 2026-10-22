using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource audioSource;
    [Header("Clips")]
    [SerializeField] private AudioClip _onCustomerArrived;
    [SerializeField] private AudioClip _onCustomerAngry;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        EventBus<CustomerArrivedEvent>.Subscribe(OnCustomerArrived);
        EventBus<CustomerAngryEvent>.Subscribe(OnCustomerAngry);
    }

    private void OnDisable()
    {
        EventBus<CustomerArrivedEvent>.Unsubscribe(OnCustomerArrived);
        EventBus<CustomerAngryEvent>.Unsubscribe(OnCustomerAngry);
    }

    private void OnCustomerArrived(CustomerArrivedEvent e)
    {
        if (_onCustomerArrived == null)
        {
            Debug.LogWarning("Audio clip for customer arrival is not assigned.");
            return;
        }

        audioSource.PlayOneShot(_onCustomerArrived, .5f);
    }

    private void OnCustomerAngry(CustomerAngryEvent e)
    {
        if (_onCustomerAngry == null)
        {
            Debug.LogWarning("Audio clip for angry customer is not assigned");
            return;
        }

        audioSource.PlayOneShot(_onCustomerAngry);
    }
}