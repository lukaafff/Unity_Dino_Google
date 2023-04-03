using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Jogador : MonoBehaviour
{
    public Rigidbody2D rb;
    public float forcaPulo;
    public LayerMask layerChao;
    public float distanciaMinimaChao = 1;
    private bool estsNoChao;
    private float pontos;
    private float highscore;
    public float multiplicadorPontos = 1;
    public Text pontosText;
    public Text highscoreText;
    public Animator animatorComponent;
    public AudioSource pularAudioSource;
    public AudioSource cemPontosAudioSource;
    public AudioSource fimeJogoAudioSource;

    private void Start()
    {
        highscore = PlayerPrefs.GetFloat("HIGHSCORE");
        highscoreText.text = $"HI {Mathf.FloorToInt(highscore)}";
    }

    void Update()
    {
        pontos += Time.deltaTime * multiplicadorPontos;

        var pontosArrendodado = Mathf.FloorToInt(pontos);
        pontosText.text = $"Score: {pontosArrendodado}";

        if (pontosArrendodado > 0
            && pontosArrendodado % 100 == 0
            && !cemPontosAudioSource.isPlaying)
        {
            cemPontosAudioSource.Play();
        }


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Pular();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Abaixar();
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Levantar();
        }
    }

    void Pular()
    {
        if (estsNoChao)
        {
            rb.AddForce(Vector2.up * forcaPulo);
            pularAudioSource.Play();
        }
    }

    void Abaixar()
    {
        animatorComponent.SetBool("Abaixado", true);
    }

    void Levantar()
    {
        animatorComponent.SetBool("Abaixado", false);
    }

    private void FixedUpdate()
    {
        estsNoChao = Physics2D.Raycast(transform.position, Vector2.down, distanciaMinimaChao, layerChao);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Inimigo"))
        {
            if (pontos > highscore)
            {
                highscore = pontos;

                PlayerPrefs.SetFloat("HIGHSCORE", highscore);
            }

            fimeJogoAudioSource.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
