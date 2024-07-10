using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private Slider healthSlider;
    private PlayerController player;

    private void Start()
    {
        healthSlider = GetComponent<Slider>();
        player = GetComponentInParent<PlayerController>();
    }

    [PunRPC]
    public void UpdateHealthSlider()
    {
        healthSlider.value = player.health;
    }
}
