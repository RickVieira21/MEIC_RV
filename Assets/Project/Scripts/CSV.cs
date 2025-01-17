using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class CSV : MonoBehaviour
{
    // Variável para armazenar o nome do ficheiro
    private string filename = "";
    private string filePath = "";
    private string timestamp = "";

    [SerializeField]
    float recordInterval;

    [SerializeField] 
    GameObject rightHand;

    [SerializeField]
    GameObject leftHand;

    [SerializeField] 
    GameObject rightController;

    [SerializeField] 
    GameObject leftController;

    [SerializeField] 
    GameObject[] interactableObjects;

    // Start é chamado antes da primeira execução do Update
    void Awake()
    {
        // Gerar um nome único para o ficheiro com base na data e hora
        filename = $"Log_{DateTime.Now:yyyy-MM-dd_HH-mm}.csv";
        filePath = Path.Combine(Application.persistentDataPath, filename);
        Debug.Log(filePath);

        // Criar o cabeçalho do ficheiro CSV
        string header = "Timestamp;Name;Position;Rotation;Grabbed;Hand";
        File.WriteAllText(filePath, header + Environment.NewLine);
        Debug.Log($"Ficheiro criado: {filePath}");

        StartCoroutine(WrtieCSV());
    }

    public string GetTimestamp()
    {
        return Time.time.ToString();
    }

    public IEnumerator WrtieCSV()
    {
        // Obter timestamp
        timestamp = GetTimestamp();

        // Guardar dados do player e dos objetos
        SaveObjectData(rightHand, 1);
        SaveObjectData(leftHand, 1);
        SaveObjectData(rightController, 1);
        SaveObjectData(leftController, 1);

        foreach (GameObject obj in interactableObjects)
        {
            if (obj.activeSelf)
                SaveObjectData(obj, 2);
        }
        yield return new WaitForSeconds(recordInterval);
        StartCoroutine(WrtieCSV());
    }

    public void SaveObjectData(GameObject obj, int objectType)
    {
        if (obj == null) return;

        timestamp = GetTimestamp();

        // Obter posição e rotação do objeto
        string position = obj.transform.position.ToString();
        string rotation = obj.transform.rotation.eulerAngles.ToString();

        bool objIsGrabbed;
        string hand;
        string line;

        if (objectType == 2)
        {
            objIsGrabbed = obj.GetComponent<InteractableObject>().isGrabbed;
            hand = obj.GetComponent<InteractableObject>().GetGrabbingHand();

            line = $"{timestamp};{obj.name};{position};{rotation};{objIsGrabbed};{hand}";
        }
        else
        {
            // Formatar a linha para o CSV
            line = $"{timestamp};{obj.name};{position};{rotation}";
        }
        File.AppendAllText(filePath, line + Environment.NewLine);
    }
}
