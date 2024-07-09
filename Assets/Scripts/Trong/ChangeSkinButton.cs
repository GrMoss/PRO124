
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChangeSkinButton : MonoBehaviour
{
    public Button changeBodyPartButton;

    private void Start()
    {
        changeBodyPartButton = GameObject.Find("ConfirmButton").GetComponent<Button>();
        changeBodyPartButton.onClick.AddListener(OnChangeBodyPartButtonClicked);

        changeBodyPartButton.onClick.AddListener(TurnOffPanel);
    }
    private void Update()
    {
    }

    void TurnOffPanel()
    {
        Debug.Log("OFF");
        GameObject skinsPanel = GameObject.Find("SkinsPanel");
        skinsPanel.SetActive(false);
    }
    private void OnChangeBodyPartButtonClicked()
    {
        // Find the local player's SkinManager
        SkinManager localSkinManager = FindLocalPlayerSkinManager();
        if (localSkinManager != null)
        {
            // Update the body part on the local player's SkinManager
            localSkinManager.UpdateBodyParts();

            Debug.Log("Found!");
        }
        else
        {
            Debug.Log("Skinmanager Not Found");
        }
        Debug.Log("ChangeSkin");
    }

    private SkinManager FindLocalPlayerSkinManager()
    {
        // Find all SkinManager instances in the scene
        SkinManager[] skinManagers = FindObjectsOfType<SkinManager>();
        foreach (var skinManager in skinManagers)
        {
            // Check if the SkinManager belongs to the local player
            if (skinManager.photonView.IsMine)
            {
                return skinManager;
            }
        }
        return null;
    }
}
