using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated; // tempo em que a nota foi instanciada 
    public float assignedTime; // tempo determinado para a nota ser tocada 
    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
    }

    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated; // calcula o tempo decorrido desde a criação da nota
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2)); // normaliza o tempo para definir a posição da nota na tela

        // se a nota ultrapassou o tempo de vida, destroi o objeto
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.right * SongManager.Instance.noteSpawnY, Vector3.right * SongManager.Instance.noteDespawnY, t);
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
