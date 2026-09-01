using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCustomerDatabase", menuName = "HerbProject/Database/CustomerDatabase")]
public class CustomerDatabaseSO : ScriptableObject
{
    [SerializeField] private List<CustomerDataSO> _allCustomers = new List<CustomerDataSO>();

    // Untuk pencarian cepat menggunakan dictionary, mencari berdasarkan Tkey yg disini adalah id
    private Dictionary<string, CustomerDataSO> _custLookup;

    // Inisialisasi untuk mengkaitkan id ke data jika data tersebut belum dikaitkan
    public void Initialize()
    {
        _custLookup = new Dictionary<string, CustomerDataSO>(_allCustomers.Count); // Objek dictionary baru dengan kapasitas awal sesuai jumlah customer yang ada

        // Untuk setiap customer didalam semua customer
        foreach (var customer in _allCustomers)
        {
            // Lakukan pengecakan apakah ada? apakah sudah punya Tkey yang terdaftar di dictionary?
            if (customer != null && _custLookup.ContainsKey(customer.CustomerId))
            {
                // Jika benar, tambahkan id sesuai dalam data ke dictionary agar dapat dicari
                _custLookup.Add(customer.CustomerId, customer);
            }
        }
    }

    public CustomerDataSO GetCustomerById(string id)
    {
        if (_custLookup == null) Initialize();
        return _custLookup.TryGetValue(id, out CustomerDataSO customer) ? customer : null;
    }

    public CustomerDataSO GetRandomCustomer()
    {
        if (_allCustomers.Count == 0) return null;
        return _allCustomers[Random.Range(0, _allCustomers.Count)];
    }
}
