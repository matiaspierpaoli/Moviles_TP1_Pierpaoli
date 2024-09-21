using System.Diagnostics.Contracts;
using UnityEngine;

public class ControlDireccion : MonoBehaviour 
{
	float Giro = 0;
	
	public bool Habilitado = true;
	CarController carController;

	public int playerId = -1;
    public string inputName = "Horizontal";

    private string axisKey;

    //---------------------------------------------------------//

    // Use this for initialization
    void Start () 
	{
		carController = GetComponent<CarController>();

        axisKey = inputName + playerId.ToString();
    }
	
	// Update is called once per frame
	void Update () 
	{
		Giro = InputManager.Instance.GetAxis(axisKey, "");

		carController.SetGiro(Giro);
	}

	public float GetGiro()
	{
		return Giro;
	}
	
}
