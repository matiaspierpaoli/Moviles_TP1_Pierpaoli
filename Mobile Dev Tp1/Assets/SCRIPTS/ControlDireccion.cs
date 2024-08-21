using System.Diagnostics.Contracts;
using UnityEngine;

public class ControlDireccion : MonoBehaviour 
{
	[SerializeField] private InputManager inputManager;

	float Giro = 0;
	
	public bool Habilitado = true;
	CarController carController;

	public int playerId = -1;
    public string inputName = "Horizontal";
		
	//---------------------------------------------------------//
	
	// Use this for initialization
	void Start () 
	{
		carController = GetComponent<CarController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Giro = inputManager.GetAxis(inputName + playerId);

		carController.SetGiro(Giro);
	}

	public float GetGiro()
	{
		return Giro;
	}
	
}
