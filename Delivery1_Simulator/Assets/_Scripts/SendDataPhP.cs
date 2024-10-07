using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
        // Crear un objeto con la información a enviar
        var jsonData = new
        {
            name = name,
            country = country,
            age = age,
            gender = gender,
            date = dateTime.ToString("o") // Formato ISO 8601
        };

        string json = JsonUtility.ToJson(jsonData);

        // Crear una solicitud POST
        using (UnityWebRequest request = UnityWebRequest.Post("https://citmalumnes.upc.es/~danielmc11/userInfo.php", json))
        {
            request.method = UnityWebRequest.kHttpVerbPOST; // Configurar como POST
            request.SetRequestHeader("Content-Type", "application/json"); // Establecer tipo de contenido
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

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
            }
        }
    }

}
