using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollows : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;
    public Vector3 offset; // Usar um Vector3 para definir a distância da câmera

    // Start is called before the first frame update
    void Start()
    {
        // Define o offset inicial com base na posição inicial do alvo e da câmera
        offset = transform.position - target.position;
    }

    // FixedUpdate é usado para atualizar a posição da câmera com base no físico
    void FixedUpdate()
    {
        // Calcula a nova posição da câmera com base no offset e na posição do alvo
        Vector3 targetCamPos = target.position + offset;
        // Interpola suavemente entre a posição atual da câmera e a nova posição calculada
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}