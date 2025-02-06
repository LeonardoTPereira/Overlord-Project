using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes; // lanes onde as notas aparecerão
    public float songDelayInSeconds; // delay antes de começar a musica 
    public double marginOfError; // in seconds

    public int inputDelayInMilliseconds;


    public string fileLocation; // caminho do arquivo MIDI

    // posicionamento das notas 
    public float noteTime;
    public float noteSpawnY;
    public float noteTapY;

    public Slider progressBar; // indicar a posição da musica 

    // posição onde as notas desaparecem apos passarem do ponto de acerto 
    public float noteDespawnY
    {
        get
        {
            return noteTapY - (noteSpawnY - noteTapY);
        }
    }

    public static MidiFile midiFile;

    void Start()
    {
        Instance = this;
        // verifica se o arquivo MIDI esta hospedado online ou localmente 
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadFromFile();
        }
    }

    void Update()
    {
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        // atualiza a barra de progresso com base no tempo atual da música
        if (progressBar != null && audioSource.clip != null)
        {
            progressBar.value = audioSource.time / audioSource.clip.length;
        }
    }

    private IEnumerator ReadFromWebsite()
    {
        // faz requisição para carregar o arquivo MIDI da web
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
        {
            yield return www.SendWebRequest();

            // verifica se houve erro ao baixar o arquivo
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                // lê os dados do arquivo MIDI recebido
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }

    private void ReadFromFile()
    {
        // caminho do arquivo MIDI salvo localmente
        string customPath = Application.dataPath + "/Rhythm Game/StreamingAssets/" + fileLocation;
        midiFile = MidiFile.Read(customPath);
        GetDataFromMidi();
    }
    public void GetDataFromMidi()
    {
        // obtem todas as notas do arquivo MIDI
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        // define os timestamps das notas para cada lane
        foreach (var lane in lanes) lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }
    public void StartSong()
    {
        audioSource.Play();
    }
    public static double GetAudioSourceTime()
    {
        // retorna o tempo atual da música em segundos
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

}
