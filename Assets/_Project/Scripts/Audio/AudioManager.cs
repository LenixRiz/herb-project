using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource audioSource;
    [Header("Clips")]
    [SerializeField] private AudioClip _onCustomerArrived;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public void OnCustomerArrived()
    {
        if (_onCustomerArrived == null)
        {
            Debug.LogWarning("Audio clip for customer arrival is not assigned.");
            return;
        }

        audioSource.PlayOneShot(_onCustomerArrived, .5f);
    }
}