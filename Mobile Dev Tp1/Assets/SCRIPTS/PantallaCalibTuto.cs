using UnityEngine;
using System.Collections;

public class PantallaCalibTuto : MonoBehaviour 
{
	public Texture2D[] ImagenesDelTuto;
	public float Intervalo = 1.2f;//tiempo de cada cuanto cambia de imagen
	float TempoIntTuto = 0;
	int EnCursoTuto = 0;

	public bool isPlayer1 = false;
	
	public Texture2D PCCalibImage;
	public Texture2D MobileCalibImage;
	int EnCursoCalib = 0;
	float TempoIntCalib = 0;
	
	public Texture2D ImaReady;
	
	public ContrCalibracion ContrCalib;
	
	// Update is called once per frame
	void Update () 
	{
		switch(ContrCalib.EstAct)
		{
		case ContrCalibracion.Estados.Calibrando:
			if (GameManager.Instancia.gameSettings.isSinglePlayerActive)
            {
                if (GameManager.Instancia.IsPlatformPC())
                    GetComponent<Renderer>().material.mainTexture = PCCalibImage;
                else
                    GetComponent<Renderer>().material.mainTexture = MobileCalibImage;
            }
			else
			{
                if (GameManager.Instancia.IsPlatformPC())
				{
                    if (isPlayer1) 
						GetComponent<Renderer>().material.mainTexture = PCCalibImage;
					else
                        GetComponent<Renderer>().material.mainTexture = MobileCalibImage;
                }
                else
                    GetComponent<Renderer>().material.mainTexture = MobileCalibImage;
            }

            break;
			
		case ContrCalibracion.Estados.Tutorial:
			//tome la bolsa y depositela en el estante
			TempoIntTuto += T.GetDT();
			if(TempoIntTuto >= Intervalo)
			{
				TempoIntTuto = 0;
				if(EnCursoTuto + 1 < ImagenesDelTuto.Length)
					EnCursoTuto++;
				else
					EnCursoTuto = 0;
			}
			GetComponent<Renderer>().material.mainTexture = ImagenesDelTuto[EnCursoTuto];
			
			break;
			
		case ContrCalibracion.Estados.Finalizado:
			//esperando al otro jugador		
			GetComponent<Renderer>().material.mainTexture = ImaReady;
			
			break;
		}
			
			
	}
}
