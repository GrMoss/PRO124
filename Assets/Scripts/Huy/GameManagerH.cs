using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameManagerH : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static int globalScore;

    void Start()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == 0)
        {
            globalScore = (int)photonEvent.CustomData;
            // Cập nhật điểm số trên UI hoặc xử lý khác

        }
    }
}
