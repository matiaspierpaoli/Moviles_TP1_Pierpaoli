using UnityEngine;
using System.Collections;

public class CheakPoint : MonoBehaviour
{
	[SerializeField] private GameSettings gameSettings;
	public string PlayerTag = "Player";
	bool HabilitadoResp = true;
	public float TiempPermanencia = 0.7f;//tiempo que no deja respaunear a un pj desp que el otro lo hizo.
	float Tempo = 0;

	public int player1ID;
	public int player2ID;

    private bool player1Passed = false;
    private bool player2Passed = false;

    // Use this for initialization
    void Start ()
	{
		GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!HabilitadoResp)
		{
			Tempo += T.GetDT();
			if(Tempo >= TiempPermanencia)
			{
				Tempo = 0;
				HabilitadoResp = true;
			}
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == PlayerTag)
		{
			other.GetComponent<Respawn>().AgregarCP(this);

			if (!gameSettings.isSinglePlayerActive)
			{
				if (other.GetComponent<Player>().IdPlayer == player1ID)
				{
					player1Passed = true;

                }
				else if (other.GetComponent<Player>().IdPlayer == player2ID)
				{
                    player2Passed = true;
                }
			}
		}	
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.tag == PlayerTag)
		{
			HabilitadoResp = true;
		}
	}
	
	//---------------------------------------------------//
	
	public bool Habilitado()
	{
		if(HabilitadoResp)
		{
			HabilitadoResp = false;
			Tempo = 0;
			return true;
		}
		else
		{
			return HabilitadoResp;
		}
	}

    public bool BothPlayersPassed()
    {
        return player1Passed && player2Passed;
    }
}
