using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MngPts : MonoBehaviour
{
    public GameSettings gameSettings;
    public SceneLoader sceneLoader;
    public string creditsSceneName = "Credits";

    public float TiempEmpAnims = 2.5f;
    public float TiempEspReiniciar = 10f;
    public float TiempParpadeo = 0.7f;

    public float sceneDuration = 10f;

    private float tempo = 0;
    private float tempoParpadeo = 0;
    private bool primerImaParp = true;
    private bool activadoAnims = false;

    public TextMeshProUGUI dineroIzqText;  
    public TextMeshProUGUI dineroDerText;  
    public Image ganadorImage;  

    public Sprite ganadorIzqSprite;
    public Sprite ganadorDerSprite;

    private Visualizacion viz = new Visualizacion();

    void Start()
    {
        SetGanador();

        if (gameSettings.isSinglePlayerActive)
        {
            DisableTwoPlayerUI();
        }


        StartCoroutine(StartSceneDuration());
    }

    void Update()
    {
        // Reiniciar el juego
        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.Alpha0))
        {
            SceneManager.LoadScene(0);
        }

        // Cerrar la aplicación
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //}

        TiempEspReiniciar -= Time.deltaTime;
        if (TiempEspReiniciar <= 0)
        {
            SceneManager.LoadScene(0);
        }

        if (activadoAnims)
        {
            tempoParpadeo += Time.deltaTime;

            if (tempoParpadeo >= TiempParpadeo)
            {
                tempoParpadeo = 0;
                primerImaParp = !primerImaParp;
            }
        }

        if (!activadoAnims)
        {
            tempo += Time.deltaTime;
            if (tempo >= TiempEmpAnims)
            {
                tempo = 0;
                activadoAnims = true;
                SetDinero();
            }
        }
    }

    private IEnumerator StartSceneDuration()
    {
        yield return new WaitForSeconds(sceneDuration);
        sceneLoader.LoadLevel(creditsSceneName);
    }

    private void DisableTwoPlayerUI()
    {
        dineroDerText.gameObject.SetActive(false);
    }

    // Asigna la imagen del ganador
    void SetGanador()
    {
        if (!gameSettings.isSinglePlayerActive)
        {
            if (gameSettings.player1Money > gameSettings.player2Money)
                ganadorImage.sprite = ganadorIzqSprite;
            else
                ganadorImage.sprite = ganadorDerSprite;
        }
        else
        {
            ganadorImage.gameObject.SetActive(false);
        }
    }

    // Actualiza el dinero en pantalla
    void SetDinero()
    {
        if (!gameSettings.isSinglePlayerActive)
        {
            dineroIzqText.text = "$" + viz.PrepararNumeros(gameSettings.player1Money);
            dineroDerText.text = "$" + viz.PrepararNumeros(gameSettings.player2Money);
        }
        else
        {
            dineroIzqText.text = "$" + viz.PrepararNumeros(gameSettings.player1Money);
        }
    }

    // Opción para hacer desaparecer la UI si es necesario
    public void DesaparecerUI()
    {
        activadoAnims = false;
        tempo = -100;
        dineroIzqText.text = "";
        dineroDerText.text = "";
        ganadorImage.enabled = false;
    }
}
