using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorInimigos : MonoBehaviour
{

    public GameObject[] cactoPrefabs;
    public GameObject dinossauroVoadorPrefab;
    public float dinossuroVoadorYMinimo = -1;
    public float dinossauroVoadorMaximo = 1;
    public float dinossauroVoadorPontuacaoMinima = 300;
    public float delayInicial;
    public float delayEntreCactos;
    public float distanciaMinima = 4;
    public float distanciaMaxima = 8;
    public Jogador jogadorScript;


    private void Start()
    {
        //InvokeRepeating("GerarInimigo", delayInicial, delayEntreCactos);
        StartCoroutine(GerarInimigo());
    }

    private IEnumerator GerarInimigo()
    {
        yield return new WaitForSeconds(delayInicial);

        GameObject ultimoInimigoGerado = null;

        var distanciNecessaria = 0f;

        while (true)
        {

            var geracaoObjetoLiberada =
                ultimoInimigoGerado == null
                 || Vector3.Distance(transform.position, ultimoInimigoGerado.transform.position) >= distanciNecessaria;

            if (geracaoObjetoLiberada)
            {
                var dado = Random.Range(1, 7);

                if (jogadorScript.pontos >= dinossauroVoadorPontuacaoMinima && dado <= 2)
                {
                    //gerar dinossauro voador
                    var posicaoYAleatoria = Random.Range(dinossuroVoadorYMinimo, dinossauroVoadorMaximo);

                    var posicao = new Vector3(
                        transform.position.x,
                        transform.position.y + posicaoYAleatoria,
                        transform.position.z
                    );

                    ultimoInimigoGerado = Instantiate(dinossauroVoadorPrefab, posicao, Quaternion.identity);
                }
                else
                {
                    //gerar cacto
                    var quantidadeCactos = cactoPrefabs.Length;
                    var indiceAleatorio = Random.Range(0, quantidadeCactos);
                    var cactoPrefab = cactoPrefabs[indiceAleatorio];
                    ultimoInimigoGerado = Instantiate(cactoPrefab, transform.position, Quaternion.identity);
                }

                distanciNecessaria = Random.Range(distanciaMinima, distanciaMaxima);
            }
            yield return null;
            //yield return new WaitForSeconds(delayEntreCactos);
        }
    }
}
