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


public class SendDataPhP : MonoBehaviour
{
    void OnEnable()
    {
        Simulator.OnNewPlayer += NewPlayerAction;
    }

    private void NewPlayerAction(string arg1, string arg2, int arg3, float arg4, DateTime arg5)
    {
        Debug.Log("Hola");
        StartCoroutine(SendDataToServer(arg1, arg2, arg3, arg4, arg5));
        CallbackEvents.OnAddPlayerCallback?.Invoke(99);
    }
     

    private IEnumerator SendDataToServer(string name, string country, int age, float gender, DateTime dateTime)
    {
        // Now create an instance of the UserInfo class
        UserInfo userInfo = new UserInfo
        {
            name = name,
            country = country,
            age = age,
            gender = gender,
            date = dateTime.ToString("o") // Format in ISO 8601
        };


        var json = JsonUtility.ToJson(userInfo);

        // Crear una solicitud POST
        using (UnityWebRequest request = new UnityWebRequest("https://citmalumnes.upc.es/~danielmc11/userInfo.php", UnityWebRequest.kHttpVerbPOST))
        {
            // Establecer el cuerpo de la solicitud
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Configurar el tipo de contenido
            request.SetRequestHeader("Content-Type", "application/json");

            // Enviar la solicitud y esperar la respuesta
            yield return request.SendWebRequest();

            // Manejar errores
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error al enviar datos: {request.error}");
            }
            else
            {
                Debug.Log("Datos enviados correctamente: " + request.downloadHandler.text);
                CallbackEvents.OnAddPlayerCallback?.Invoke(8);
            }
        }
    }

}
