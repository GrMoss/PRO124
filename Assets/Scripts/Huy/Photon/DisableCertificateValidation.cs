using UnityEngine;
using System.Net;

public class DisableCertificateValidation : MonoBehaviour
{
    void Awake()
    {
        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
    }
}
