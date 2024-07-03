using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIData
{
    public string name { get; set; }
    public string phone { get; set; }
    public string email { get; set; }
    public string location { get; set; }

    public APIData(string name, string phone, string email, string location)
    {
        this.name = name;
        this.phone = phone;
        this.email = email;
        this.location = location;
    }
}
