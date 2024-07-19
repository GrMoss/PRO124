
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
    public GameObject setATButton;
    public GameObject setATButton2;

    private bool allFieldsFilled;

    public static bool nextStep = true;

    private void Update()
    {
        Debug.Log(" allFieldsFilled: " + allFieldsFilled);
        allFieldsFilled = !string.IsNullOrEmpty(nameInputField.text) &&
                          !string.IsNullOrEmpty(emailInputField.text) &&
                          !string.IsNullOrEmpty(phoneInputField.text);

        if (allFieldsFilled)
        {
            setATButton2.SetActive(true);
            setATButton.SetActive(false);
        }
        else
        {
            setATButton2.SetActive(false);
            setATButton.SetActive(true);
        }
    }

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

        // Kiểm tra các giá trị đầu vào
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(location))
        {
            Debug.LogWarning("Một hoặc nhiều trường dữ liệu bị bỏ trống.");
            yield break;
        }

        APIData userData = new APIData(name, phone, email, location);

        string jsonStringRequest = JsonConvert.SerializeObject(userData);

        var request = new UnityWebRequest(apiUrlPost, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        switch (request.responseCode)
        {
            case 200:
                Debug.Log("Người dùng được thêm thành công.");
                break;
            case 400:
                Debug.Log("Yêu cầu không hợp lệ.");
                break;
            case 500:
                Debug.Log("Số điện thoại đã được sử dụng.");
                break;
            case 422:
                Debug.LogError("Yêu cầu gặp lỗi: Unprocessable Entity (422). Kiểm tra dữ liệu đầu vào.");
                if (request.downloadHandler != null)
                {
                    Debug.LogError("Lỗi từ API: " + request.downloadHandler.text);
                }
                break;
            default:
                Debug.LogError("Yêu cầu gặp lỗi: " + request.error);
                break;
        }

    }


    private bool IsValidEmail(string email)
    {
        // Kiểm tra xem email có chứa ký tự '@' và có ít nhất một ký tự trước và sau '@'
        int atIndex = email.IndexOf('@');
        if (atIndex <= 0 || atIndex >= email.Length - 1)
        {
            return false;
        }

        // Kiểm tra phần đuôi của email phải là ".com"
        string domain = email.Substring(atIndex + 1);
        if (!domain.EndsWith(".com"))
        {
            return false;
        }

        // Kiểm tra độ dài tối thiểu của phần domain (ít nhất là "a.com")
        if (domain.Length < 5)
        {
            return false;
        }

        return true;
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
            nameInputField.textComponent.color = Color.black;
        }

        // Kiểm tra định dạng số điện thoại
        if (!IsValidPhone(phoneInputField.text))
        {
            phoneInputField.textComponent.color = Color.red;
            hasError = true;
        }
        else
        {
            phoneInputField.textComponent.color = Color.black;
        }

        // Kiểm tra định dạng email
        if (!IsValidEmail(emailInputField.text))
        {
            emailInputField.textComponent.color = Color.red;
            hasError = true;
        }
        else
        {
            emailInputField.textComponent.color = Color.black;
        }

        // Nếu không có lỗi, thực hiện gửi dữ liệu
        if (!hasError)
        {
            nextStep = true;
        }
    }
}
