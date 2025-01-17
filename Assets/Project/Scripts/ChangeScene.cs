using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    GameObject canvas;

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject CSV;

    private void Awake()
    {
        Time.timeScale = 0.0f;
    }

    public void changeScene()
    {
        Time.timeScale = 0;

        CSV.GetComponent<CSV>().StopAllCoroutines();
        GameObject text = canvas.transform.Find("Text").gameObject;

        if (player.GetComponent<ObjectManager>().isEndTest())
        {
            text = canvas.transform.Find("Text").gameObject;
            text.GetComponent<TextMeshProUGUI>().text = "End of Test. Thank you!!";

            Debug.Log("End of test");
        }
        else
        {
            
            text.GetComponent<TextMeshProUGUI>().text = "Take off the headset and fill the questionnaire. <br>Press B or Y to continue";
            player.GetComponent<ObjectManager>().EnableSecondaryButtons();

            Debug.Log("Scene change");
        }

        canvas.SetActive(true);

        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        Transform locomotion = player.transform.Find("Locomotion");
        locomotion.gameObject.SetActive(false);

        Debug.Log("Locomotion OFF");
    }

   
}
