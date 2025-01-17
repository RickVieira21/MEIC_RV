using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ElasticGrab : MonoBehaviour
{
    private CubeDeformation deformation; // Referência ao script de deformação
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    private Transform leftHand;
    private Transform rightHand;

    private void Awake()
    {
        // Obtém o XRGrabInteractable e o CubeDeformation
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        deformation = GetComponent<CubeDeformation>();

        // Vincula os eventos de agarrar e soltar
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Identifica qual mão está interagindo
        if (args.interactorObject.transform.name.ToLower().Contains("left"))
        {
            leftHand = args.interactorObject.transform;
        }
        else if (args.interactorObject.transform.name.ToLower().Contains("right"))
        {
            rightHand = args.interactorObject.transform;
        }

        if (leftHand != null && rightHand != null)
        {
            // Estica o cubo ao agarrar com ambas as mãos
            StretchCube();
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Limpa a referência à mão que soltou o objeto
        if (args.interactorObject.transform == leftHand)
        {
            leftHand = null;
        }
        else if (args.interactorObject.transform == rightHand)
        {
            rightHand = null;
        }

        // Restaura a forma se uma das mãos soltar
        if (leftHand == null || rightHand == null)
        {
            deformation.StartCoroutine("RestoreShape");
        }
    }

    private void StretchCube()
    {
        if (leftHand == null || rightHand == null || deformation == null) return;

        // Calcula o ponto médio entre as mãos
        Vector3 midpoint = (leftHand.position + rightHand.position) / 2;

        // Calcula a direção e intensidade do alongamento
        Vector3 stretchDirection = rightHand.position - leftHand.position;
        float stretchMagnitude = stretchDirection.magnitude;

        // Aplica a deformação no cubo
        deformation.Stretch(midpoint, stretchDirection.normalized, stretchMagnitude);
    }

    private void Update()
    {
        // Continuamente atualiza a deformação enquanto ambas as mãos seguram o cubo
        if (leftHand != null && rightHand != null)
        {
            StretchCube();
        }
    }

    private void OnDestroy()
    {
        // Remove os listeners para evitar erros ao destruir o objeto
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}
