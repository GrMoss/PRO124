
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Collections.Generic;
using TMPro;

public class ConnectToAPI : MonoBehaviour
{
    public string apiUrlPost = "https://chungnhanthamgia.io.vn/users/addUser";

    public TMP_InputField nameInputField;
    public TMP_InputField phoneInputField;
    public TMP_InputField emailInputField;

    private string urlEmailInput;
    public static bool nextStep = false;

    public void StartButton()
    {
        ValidateInputFields();

        if (nextStep)
        {
            StartCoroutine(SendUserData());
        }
    }

    public IEnumerator SendUserData()
    {
        string name = nameInputField.text;
        string phone = phoneInputField.text;
        string email = emailInputField.text;
        string location = "HCM";

        APIData userData = new APIData(name, phone, email, location);

        string jsonStringRequest = JsonConvert.SerializeObject(userData);

        var request = new UnityWebRequest(apiUrlPost, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.responseCode == 200)
        {
            Debug.Log("Người dùng được thêm thành công.");
            //request.Dispose();
        }
        else if (request.responseCode == 400)
        {
            Debug.Log("Yêu cầu không hợp lệ.");
        }
        else if (request.responseCode == 500)
        {
            Debug.Log("Số điện thoại đã được sử dụng.");
        }
        else
        {
            Debug.Log("Gửi dử liệu không thành công.");
        }
    }

    private bool IsValidEmail(string email)
    {
        // Kiểm tra xem email có chứa ký tự '@' không
        // và có ít nhất một ký tự trước và sau ký tự '@'
        return email.Contains("@") && email.IndexOf("@") > 0 && email.IndexOf("@") < email.Length - 1;
    }

    private bool IsValidPhone(string phone)
    {
        // Kiểm tra xem số điện thoại chỉ chứa số và có đúng 10 ký tự không
        return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^[0-9]{10}$");
    }

    public void ValidateInputFields()
    {
        bool hasError = false;

        // Kiểm tra độ dài của tên, ít nhất phải là 2 ký tự
        if (nameInputField.text.Length < 2)
        {
            nameInputField.textComponent.color = Color.red;
            hasError = true;
        }
        else
        {
            nameInputField.textComponent.color = Color.white;
        }

        // Kiểm tra định dạng email
        if (!IsValidEmail(emailInputField.text))
        {
            emailInputField.textComponent.color = Color.red;
            hasError = true;
        }
        else
        {
            emailInputField.textComponent.color = Color.white;
        }

        // Kiểm tra định dạng số điện thoại
        if (!IsValidPhone(phoneInputField.text))
        {
            phoneInputField.textComponent.color = Color.red;
            hasError = true;
        }
        else
        {
            phoneInputField.textComponent.color = Color.white;
        }

        // Nếu không có lỗi, thực hiện gửi dữ liệu
        if (!hasError)
        {
            nextStep = true;
        }
    }

}
