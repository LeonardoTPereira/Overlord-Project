using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX; // som de acerto
    public AudioSource missSFX; // som de erro
    public TMPro.TextMeshPro scoreText; // texto para exibir score
    public TMPro.TextMeshPro comboText; // texto para exibir a quant de combos
    public TMPro.TextMeshPro missText; // texto para exibir "MISS"

    static int totalScore; // armazenar a pontuação
    static int comboScore;  // armazena a contagem de combos 

    void Start()
    {
        Instance = this;
        totalScore = 0;
        comboScore = 0;
        if (missText != null)
            missText.gameObject.SetActive(false); // Esconde a mensagem "MISS" no início
    }

    public static void Hit()
    {
        comboScore += 1;
        totalScore += 5 * comboScore; // aumenta a pontuação com base na quantidade de combos
        Instance.hitSFX.Play();
    }

    public static void Miss()
    {
        comboScore = 0; // reseta o combo
        Instance.missSFX.Play();

        if (Instance.missText != null)
        {
            Instance.missText.gameObject.SetActive(true);
            Instance.StartCoroutine(Instance.HideMissText());
        }
    }

    private IEnumerator HideMissText()
    {
        yield return new WaitForSeconds(1.0f); // Exibe "MISS" por 1 segundo
        if (missText != null)
            missText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (scoreText != null)
            scoreText.text = "Score\n" + totalScore;

        if (comboText != null)
            comboText.text = comboScore > 1 ? "Combo " + comboScore : "";
    }
}
