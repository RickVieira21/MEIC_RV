using UnityEngine;
using System.Collections;

public class CubeDeformation : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] currentVertices;

    [SerializeField]
    private float deformRadius = 0.1f; // Raio de deformação
    [SerializeField]
    private float deformAmount = 0.1f; // Intensidade da deformação
    [SerializeField]
    private float restoreDuration = 1f; // Tempo para restaurar a forma

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Obtém a malha do objeto
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        currentVertices = mesh.vertices;
    }

    public void Deform(Vector3 impactPoint)
    {
        // Converte o ponto de impacto para o espaço local da malha
        Vector3 localImpactPoint = transform.InverseTransformPoint(impactPoint);

        for (int i = 0; i < currentVertices.Length; i++)
        {
            // Calcula a distância do vértice ao ponto de impacto
            float distance = Vector3.Distance(currentVertices[i], localImpactPoint);

            if (distance < deformRadius)
            {
                // Deforma o vértice baseado na intensidade
                currentVertices[i] += (localImpactPoint - currentVertices[i]).normalized * deformAmount * (1 - distance / deformRadius);
            }
        }

        // Atualiza a malha
        mesh.vertices = currentVertices;
        mesh.RecalculateNormals();

        // Inicia a restauração da forma
        StopAllCoroutines();
        StartCoroutine(RestoreShape());
    }

    private IEnumerator RestoreShape()
    {
        float elapsed = 0f;

        while (elapsed < restoreDuration)
        {
            elapsed += Time.deltaTime;
            for (int i = 0; i < currentVertices.Length; i++)
            {
                currentVertices[i] = Vector3.Lerp(currentVertices[i], originalVertices[i], elapsed / restoreDuration);
            }

            // Atualiza a malha a cada frame
            mesh.vertices = currentVertices;
            mesh.RecalculateNormals();
            yield return null;
        }

        // Garante que a forma original seja totalmente restaurada
        mesh.vertices = originalVertices;
        mesh.RecalculateNormals();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Obtém o ponto de contato para deformação
        foreach (ContactPoint contact in collision.contacts)
        {
            Deform(contact.point);
        }
    }

    public void Stretch(Vector3 midpoint, Vector3 direction, float magnitude)
    {
        // Converte o ponto médio para o espaço local da malha
        Vector3 localMidpoint = transform.InverseTransformPoint(midpoint);
        Vector3 localDirection = transform.InverseTransformDirection(direction);

        for (int i = 0; i < currentVertices.Length; i++)
        {
            // Calcula a projeção do vértice na direção do alongamento
            Vector3 projection = Vector3.Project(currentVertices[i] - localMidpoint, localDirection);

            // Ajusta a posição do vértice com base na magnitude do alongamento
            currentVertices[i] += projection.normalized * magnitude * 0.1f;
        }

        // Atualiza a malha
        mesh.vertices = currentVertices;
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
