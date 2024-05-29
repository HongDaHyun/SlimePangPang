using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

public class NetworkRepository: MonoBehaviour
{
    public static NetworkRepository instance;

    static readonly string BASE_URL = "https://ol98wq7cs0.execute-api.ap-southeast-1.amazonaws.com";

    private int userId;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [Button]
    public void Test()
    {
        userId = 3;
        SetScore(70);
    }

    public void SetUserId(int id)
    {
        Debug.Log(id);
        userId = id;
    }

    public void SetScore(int score)
    {
        StartCoroutine(SetScoreRoutine(score));
    }

    IEnumerator SetScoreRoutine(int score)
    {
        string url = $"{BASE_URL}/score/{userId}";
        Debug.Log(url);
        /*WWWForm form = new WWWForm();

        form.AddField("score", score);*/

        using(UnityWebRequest www = UnityWebRequest.Post(url, ""))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes("{ \"score\": " + score + " }");
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);

            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();  // 응답 대기

            if (www.error == null)
            {
                Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
                yield break;
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }
}

