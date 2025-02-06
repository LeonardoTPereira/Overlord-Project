using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction; // restrição de nota para essa lane 
    public KeyCode input; // tecla associada para ativar essa lane
    public GameObject notePrefab; 
    List<Note> notes = new List<Note>(); // lista de notas ativas na lane 
    public List<double> timeStamps = new List<double>();

    int spawnIndex = 0;
    int inputIndex = 0;

    void Start()
    {
    }

    // configura os timestamps das notas filtrando apenas as que pertencem a esta lane
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }

    void Update()
    {
        // verifica se ainda há notas para spawnar e se é o momento correto
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                // instancia uma nova nota e armazena na lista
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }

        // verifica se ainda há notas para capturar input e processar acertos/erros
        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            // verifica se o jogador pressionou a tecla correspondente
            if (Input.GetKeyDown(input))
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    // nota acertada dentro da margem de erro
                    Hit();
                    print($"Hit on {inputIndex} note");
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    // nota pressionada fora da margem de erro
                    print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
            }
            // verifica se a nota foi perdida
            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                print($"Missed {inputIndex} note");
                inputIndex++;
            }
        }

    }
    // jogador acertou uma nota
    private void Hit()
    {
        ScoreManager.Hit();
    }

    // jogador errou uma nota
    private void Miss()
    {
        ScoreManager.Miss();
    }
}
