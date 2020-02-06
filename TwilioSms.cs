using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TwilioSms : MonoBehaviour{
    string url = "api.twilio.com/2010-04-01/Accounts/";
	string service = "/Messages.json";
	public string from;
	public string to;
	public string account_sid;
	public string auth;
	public string body;

    public void SendSms(){
        StartCoroutine(SendRequest());
    }

    IEnumerator SendRequest(){
        WWWForm wForm = new WWWForm();
		wForm.AddField("To", to);
		wForm.AddField("From", from);
		wForm.AddField("Body", body);

		string completeurl = "https://" + account_sid + ":" + auth + "@" + url + account_sid + service;

        using(UnityWebRequest wr = UnityWebRequest.Post(completeurl, wForm)){
			yield return wr.SendWebRequest();

            if(wr.isHttpError || wr.isNetworkError)
            {
				Debug.Log("wr error" + wr.error);
            }
            else
            {
				Debug.Log("SMS sent through Web API: " + wr.downloadHandler.text);
            }
        }
    }
}