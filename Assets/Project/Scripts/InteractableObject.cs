using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    GameObject CSV;

    [HideInInspector]
    public bool isGrabbed = false;
    [HideInInspector]
    public string hand = "";

    public string GetGrabbingHand()
    {
        return hand;
    }

    public void SetGrabbingHand(string handName)
    {
        hand = handName;
    }

    public void IsSelected()
    {
        isGrabbed = true;
        if (CSV.activeSelf)
        {
            CSV.GetComponent<CSV>().SaveObjectData(transform.gameObject, 2);
        }
    }
    public void IsNotSelected()
    {
        isGrabbed = false;
        hand = "";

        if (CSV.activeSelf)
        {
            CSV.GetComponent<CSV>().SaveObjectData(transform.gameObject, 2);
        }
    }
}
