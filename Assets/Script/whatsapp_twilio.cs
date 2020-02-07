using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class whatsapp_twilio : MonoBehaviour
{
    public TMP_InputField textInput;

    string url = "api.twilio.com/2010-04-01/Accounts/";
    string service = "/Messages.json";

    [Space(20)]
    public string from;
    public string to;
    public string account_sid;
    public string auth;

    string body;
    string ws = "whatsapp:";

    private void Start()
    {
        //save();

        load();
    }

    void save()
    {
        PlayerPrefs.SetString("from_ws_phonenumber", from);
        PlayerPrefs.SetString("to_ws_phonenumber", to);
        PlayerPrefs.SetString("account_sid", account_sid);
        PlayerPrefs.SetString("account_auth", auth);

        Debug.Log("saved 'from' ws phone number : " + PlayerPrefs.GetString("from_ws_phonenumber"));
        Debug.Log("saved 'to' ws phone number : " + PlayerPrefs.GetString("to_ws_phonenumber"));
        Debug.Log("saved account sid : " + PlayerPrefs.GetString("account_sid"));
        Debug.Log("saved account auth : " + PlayerPrefs.GetString("account_auth"));
    }

    void load()
    {
        if (PlayerPrefs.HasKey("from_ws_phonenumber")){ from = PlayerPrefs.GetString("from_ws_phonenumber"); } else { Debug.LogWarning("There's no WhatsApp 'from' number found"); }
        if (PlayerPrefs.HasKey("to_ws_phonenumber")){ to = PlayerPrefs.GetString("to_ws_phonenumber"); } else { Debug.LogWarning("There's no WhatsApp 'to' number found"); }
        if (PlayerPrefs.HasKey("account_sid")) { account_sid = PlayerPrefs.GetString("account_sid"); } else { Debug.LogWarning("There's no account sid found"); }
        if (PlayerPrefs.HasKey("account_auth")) { auth = PlayerPrefs.GetString("account_auth"); } else { Debug.LogWarning("There's no account auth found"); }
    }

    public void SendRequest()
    {
        body = textInput.text;

        to = ws + to;
        from = ws + from;

        StartCoroutine(SendWSRequest());
    }

    IEnumerator SendWSRequest()
    {
        WWWForm wForm = new WWWForm();
        wForm.AddField("To", to);
        wForm.AddField("From", from);
        wForm.AddField("Body", body);

        string completeurl = "https://" + account_sid + ":" + auth + "@" + url + account_sid + service;

        using (UnityWebRequest wr = UnityWebRequest.Post(completeurl, wForm))
        {
            yield return wr.SendWebRequest();

            if (wr.isHttpError || wr.isNetworkError)
            {
                Debug.Log("wr error" + wr.error);
                Debug.Log(wr.downloadHandler.text);
            }
            else
            {
                Debug.Log("wr OK! SMS sent through Web API: " + wr.downloadHandler.text);
            }
        }
    }
}
