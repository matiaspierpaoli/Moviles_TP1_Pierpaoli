using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

// Interfaz para los estados del juego
public interface IGameState
{
    void EnterState(GameManager gameManager);
    void UpdateState(GameManager gameManager);
    void ExitState(GameManager gameManager);
}

// Estado de calibraci�n
public class CalibratingState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        gameManager.IniciarTutorial();
    }

    public void UpdateState(GameManager gameManager)
    {
        if (gameManager.inputManager.IsUpPressed(gameManager.verticalInputName, "1"))
        {
            gameManager.Player1.Seleccionado = true;
        }

        if (!gameManager.gameSettings.isSinglePlayerActive && gameManager.inputManager.IsUpPressed(gameManager.verticalInputName, "2"))
        {
            gameManager.Player2.Seleccionado = true;
        }

        //if (gameManager.Player1.Seleccionado && (gameManager.singlePlayer || gameManager.Player2.Seleccionado))
        //{
        //    gameManager.ChangeState(new PlayingState());
        //}
    }

    public void ExitState(GameManager gameManager)
    {
        // Limpiar o resetear lo necesario al salir del estado
    }
}

// Estado de juego
public class PlayingState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        gameManager.ConteoRedresivo = true;
        gameManager.ConteoParaInicion = 3;
        gameManager.CambiarACarrera();
    }

    public void UpdateState(GameManager gameManager)
    {
        if (gameManager.ConteoRedresivo)
        {
            gameManager.ConteoParaInicion -= Time.deltaTime;
            if (gameManager.ConteoParaInicion < 0)
            {
                gameManager.EmpezarCarrera();
                gameManager.ConteoRedresivo = false;
            }
        }
        else
        {
            gameManager.TiempoDeJuego -= Time.deltaTime;
            if (gameManager.TiempoDeJuego <= 0)
            {
                gameManager.ChangeState(new GameOverState());
            }
        }

        gameManager.ActualizarUI();
    }

    public void ExitState(GameManager gameManager)
    {
        // Limpiar o resetear lo necesario al salir del estado
    }
}

// Estado de finalizaci�n
public class GameOverState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        gameManager.FinalizarCarrera();
    }

    public void UpdateState(GameManager gameManager)
    {
        gameManager.TiempEspMuestraPts -= Time.deltaTime;
        if (gameManager.TiempEspMuestraPts <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void ExitState(GameManager gameManager)
    {
        // Limpiar o resetear lo necesario al salir del estado
    }
}

// GameManager reorganizado
public class GameManager : MonoBehaviour
{
    [Header("Config")]
    public static GameManager Instancia;
    public GameSettings gameSettings;

    public InputManager inputManager;
    public string verticalInputName = "Vertical";
    public float TiempoDeJuego = 60;
    public Text ConteoInicio;
    public Text TiempoDeJuegoText;

    public float ConteoParaInicion = 3;
    public bool ConteoRedresivo = true;
    public float TiempEspMuestraPts = 3;

    public GameObject[] ObjsCarrera;

    [Header("Player 1")]
    public Player Player1;
    public GameObject[] ObjsCalibracion1;

    [Header("Player 2")]
    public Player Player2;
    public GameObject[] ObjsCalibracion2;
    public GameObject player2Camera;
    public GameObject virtualJoystick2;
    public GameObject unloadScene2GO;
    public GameObject unloadController2GO;
    public GameObject player2UI;


    public Vector3 PosCamionesCarreraSinglePlayer;
    public Vector3[] PosCamionesCarreraMultiplayer = new Vector3[2];
    public Vector3 PosCamion1Tuto = Vector3.zero;
    public Vector3 PosCamion2Tuto = Vector3.zero;

    private IGameState currentState;

    void Awake()
    {
        GameManager.Instancia = this;
    }

    private void OnDestroy()
    {
        GameManager.Instancia = null;
    }

    IEnumerator Start()
    {
        yield return null;

        if (gameSettings.isSinglePlayerActive)
        {
            TurnOffTwoPlayerObjects();
        }

        ChangeState(new CalibratingState());
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        currentState?.UpdateState(this);
    }

    public void ChangeState(IGameState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState?.EnterState(this);
    }

    private void TurnOffTwoPlayerObjects()
    {
        for (int i = 0; i < ObjsCalibracion2.Length; i++)
        {
            ObjsCalibracion2[i].SetActive(false);
        }

        virtualJoystick2.SetActive(false);
        Player2.gameObject.SetActive(false);
        player2Camera.SetActive(false);
        unloadScene2GO.SetActive(false);
        unloadController2GO.SetActive(false);
        player2UI.SetActive(false);
    }

    public void IniciarTutorial()
    {
        for (int i = 0; i < ObjsCalibracion1.Length; i++)
        {
            ObjsCalibracion1[i].SetActive(true);
        }

        for (int i = 0; i < ObjsCarrera.Length; i++)
        {
            ObjsCarrera[i].SetActive(false);
        }

        Player1.CambiarATutorial();
        if (!gameSettings.isSinglePlayerActive) Player2.CambiarATutorial();

        TiempoDeJuegoText.transform.parent.gameObject.SetActive(false);
        ConteoInicio.gameObject.SetActive(false);
    }

    public void CambiarACarrera()
    {
        for (int i = 0; i < ObjsCarrera.Length; i++)
        {
            ObjsCarrera[i].SetActive(true);
        }

        Player1.FinCalibrado = true;
        if (!gameSettings.isSinglePlayerActive) Player2.FinCalibrado = true;

        for (int i = 0; i < ObjsCalibracion1.Length; i++)
        {
            ObjsCalibracion1[i].SetActive(false);
        }

        if (Player1.LadoActual == Visualizacion.Lado.Izq)
        {
            if (!gameSettings.isSinglePlayerActive)
            {
                Player1.gameObject.transform.position = PosCamionesCarreraMultiplayer[0];
                Player2.gameObject.transform.position = PosCamionesCarreraMultiplayer[1];
            }
            else
            {
                Player1.gameObject.transform.position = PosCamionesCarreraSinglePlayer;
            }
        }
        else
        {
            if (!gameSettings.isSinglePlayerActive)
            {
                Player1.gameObject.transform.position = PosCamionesCarreraMultiplayer[1];
                Player2.gameObject.transform.position = PosCamionesCarreraMultiplayer[0];
            }
            else
            {
                Player1.gameObject.transform.position = PosCamionesCarreraSinglePlayer;
            }
        }

        Player1.transform.forward = Vector3.forward;
        Player1.GetComponent<Frenado>().Frenar();
        Player1.CambiarAConduccion();

        if (!gameSettings.isSinglePlayerActive)
        {
            Player2.transform.forward = Vector3.forward;
            Player2.GetComponent<Frenado>().Frenar();
            Player2.CambiarAConduccion();
        }
    }

    public void EmpezarCarrera()
    {
        Player1.GetComponent<Frenado>().RestaurarVel();
        Player1.GetComponent<ControlDireccion>().Habilitado = true;

        if (!gameSettings.isSinglePlayerActive)
        {
            Player2.GetComponent<Frenado>().RestaurarVel();
            Player2.GetComponent<ControlDireccion>().Habilitado = true;
        }
    }

    public void FinalizarCarrera()
    {
        TiempoDeJuego = 0;
        ChangeState(new GameOverState());

        // L�gica para determinar el ganador y los puntajes

        Player1.GetComponent<Frenado>().Frenar();
        Player1.ContrDesc.FinDelJuego();

        if (!gameSettings.isSinglePlayerActive)
        {
            Player2.GetComponent<Frenado>().Frenar();
            Player2.ContrDesc.FinDelJuego();
        }
    }

    public void FinCalibracion(int playerId)
    {
        if (playerId == 1)
        {
            Player1.Seleccionado = true;
        }
        else if (playerId == 2 && !gameSettings.isSinglePlayerActive)
        {
            Player2.Seleccionado = true;
        }

        // Si ambos jugadores han finalizado la calibraci�n o si es un solo jugador,
        // se cambia al estado de juego.
        if (Player1.Seleccionado && (gameSettings.isSinglePlayerActive || Player2.Seleccionado))
        {
            ChangeState(new PlayingState());
        }
    }

    public void ActualizarUI()
    {
        if (ConteoRedresivo)
        {
            if (ConteoParaInicion > 1)
            {
                ConteoInicio.text = ConteoParaInicion.ToString("0");
            }
            else
            {
                ConteoInicio.text = "GO";
            }
        }

        ConteoInicio.gameObject.SetActive(ConteoRedresivo);
        TiempoDeJuegoText.text = TiempoDeJuego.ToString("00");
        TiempoDeJuegoText.transform.parent.gameObject.SetActive(!ConteoRedresivo);
    }
}
