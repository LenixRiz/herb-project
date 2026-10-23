using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource audioSource;
    [Header("Clips")]
    [SerializeField] private AudioClip _onCustomerArrived;
    [SerializeField] private AudioClip _onCustomerAngry;
    [SerializeField] private AudioClip _onCustomerServed;
    [SerializeField] private AudioClip _onGoldChange;

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
        EventBus<CustomerServedEvent>.Subscribe(OnCustomerServed);
        EventBus<GoldChangeEvent>.Subscribe(OnGoldChange);
    }

    private void OnDisable()
    {
        EventBus<CustomerArrivedEvent>.Unsubscribe(OnCustomerArrived);
        EventBus<CustomerAngryEvent>.Unsubscribe(OnCustomerAngry);
        EventBus<CustomerServedEvent>.Unsubscribe(OnCustomerServed);
        EventBus<GoldChangeEvent>.Unsubscribe(OnGoldChange);
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

    private void OnCustomerServed(CustomerServedEvent e)
    {
        if (_onCustomerServed == null)
        {
            Debug.LogWarning("Audio clip for customer served is not assigned");
            return;
        }

        audioSource.PlayOneShot(_onCustomerServed);
    }

    private void OnGoldChange(GoldChangeEvent e)
    {
        if (_onCustomerServed == null)
        {
            Debug.LogWarning("Audio clip for customer served is not assigned");
            return;
        }

        audioSource.PlayOneShot(_onGoldChange);
    }
}