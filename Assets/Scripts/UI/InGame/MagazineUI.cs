using UnityEngine;
using TMPro;

public class MagazineUI : MonoBehaviour
{
    private TextMeshProUGUI _bulletsText;

    private PlayerGunData _playerGunData;

    void Start()
    {
        _playerGunData = DataManager.ReadData<PlayerGunData>();

        _bulletsText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _bulletsText.text = $"Bullets: {_playerGunData.Magazine.bullets.Count}";
    }
}
