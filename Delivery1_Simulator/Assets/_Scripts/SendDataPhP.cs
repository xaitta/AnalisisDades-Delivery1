using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class UserInfo
{
    public string name;
    public string country;
    public int age;
    public float gender;
    public string date;
}
[System.Serializable]
public class Start_SessionInfo
{
    public uint player_id;
    public string start_date;
    
}
[System.Serializable]
public class End_SessionInfo
{
    public uint player_id;
    public string end_date;
    public int session_id;

}


[System.Serializable]
public class ItemBuyData
{
    public int itemId;
    public string date;
    public uint playerId;
}

public class SendDataPhP : MonoBehaviour
{
    int count = 0;
    int countSession = 0;

    private const string serverUrl = "https://citmalumnes.upc.es/~danielmc11/";

    void OnEnable()
    {
        Simulator.OnNewPlayer += SendNewPlayerData;
        Simulator.OnBuyItem += SendItemBuyData;
        Simulator.OnEndSession += SendEndSessionData;
        Simulator.OnNewSession += SendNewSessionData;
    }

    private void OnDisable()
    {
        Simulator.OnNewPlayer -= SendNewPlayerData;
        Simulator.OnBuyItem -= SendItemBuyData;
        Simulator.OnEndSession -= SendEndSessionData;
        Simulator.OnNewSession -= SendNewSessionData;
    }

    public void SendNewPlayerData(string name, string country, int age, float gender, DateTime date)
    {
        UserInfo data = new UserInfo
        {
            name = name,
            country = country,
            age = age,
            gender = gender,
            date = date.ToString("o")
        };
        StartCoroutine(SendDataToServer(data, "userInfo.php"));
        count++;
        CallbackEvents.OnAddPlayerCallback?.Invoke((uint)count);
    }
    public void SendNewSessionData( DateTime startSessionDate, uint playerId)
    {
        Start_SessionInfo data = new Start_SessionInfo
        {
            player_id = playerId,
            start_date = startSessionDate.ToString("o")
        };
        StartCoroutine(SendDataToServer(data, "start_sessionInfo.php"));
        countSession++;
        CallbackEvents.OnNewSessionCallback?.Invoke((uint)count);
    }

   
    public void SendEndSessionData(DateTime endSessionDate, uint playerId)
    {
        End_SessionInfo data = new End_SessionInfo
        {
            player_id = playerId,
            end_date = endSessionDate.ToString("o"),
            session_id = countSession
        };
        StartCoroutine(SendDataToServer(data, "end_sessionInfo.php"));
        CallbackEvents.OnEndSessionCallback?.Invoke((uint)count);
    }

   
    public void SendItemBuyData(int itemId, DateTime date, uint playerId)
    {
        ItemBuyData data = new ItemBuyData
        {
            playerId = playerId,
            itemId = itemId,
            date = date.ToString("o")
        };

        StartCoroutine(SendDataToServer(data, "buyItemsInfo.php", OnItemBuySuccessCallback));
    }

    // Callback que se invoca al confirmar que la compra fue enviada correctamente
    private void OnItemBuySuccessCallback()
    {
        CallbackEvents.OnItemBuyCallback?.Invoke((uint)count);
    }


    private IEnumerator SendDataToServer<T>(T data, string scriptName, Action successCallback = null)
    {
        var json = JsonUtility.ToJson(data);

        using (UnityWebRequest request = new UnityWebRequest(serverUrl + scriptName, UnityWebRequest.kHttpVerbPOST))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error al enviar datos a {scriptName}: {request.error}");
            }
            else
            {
                Debug.Log($"Datos enviados correctamente a {scriptName}: {request.downloadHandler.text}");
                successCallback?.Invoke();  // Llama al callback si se envió con éxito
            }
        }
    }


}
