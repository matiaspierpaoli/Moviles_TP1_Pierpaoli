using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletMover : ManejoPallets {

    public int playerID = -1;
    public string horizontalInputName = "Horizontal";
    public string verticalInputName = "Vertical";


    public ManejoPallets Desde, Hasta;
    bool segundoCompleto = false;

    private string horizontalAxisKey;
    private string verticalAxisKey;

    private void Start()
    {
        horizontalAxisKey = horizontalInputName + playerID.ToString();
        verticalAxisKey = verticalInputName + playerID.ToString();
    }

    private void Update() {

        if (!Tenencia() && Desde.Tenencia() && InputManager.Instance.GetAxis(horizontalAxisKey, "") > InputManager.Instance.GetMinAxisValue()) 
        {
            PrimerPaso();
        }
        if (Tenencia() && InputManager.Instance.GetAxis(verticalAxisKey, "") < InputManager.Instance.GetMinAxisValue()) 
        {
            SegundoPaso();
        }
        if (segundoCompleto && InputManager.Instance.GetAxis(horizontalAxisKey, "") > InputManager.Instance.GetMinAxisValue()) 
        {
            TercerPaso();
        }
    }

    void PrimerPaso() {
        Desde.Dar(this);
        segundoCompleto = false;
    }
    void SegundoPaso() {
        base.Pallets[0].transform.position = transform.position;
        segundoCompleto = true;
    }
    void TercerPaso() {
        Dar(Hasta);
        segundoCompleto = false;
    }

    public override void Dar(ManejoPallets receptor) {
        if (Tenencia()) {
            if (receptor.Recibir(Pallets[0])) {
                Pallets.RemoveAt(0);
            }
        }
    }
    public override bool Recibir(Pallet pallet) {
        if (!Tenencia()) {
            pallet.Portador = this.gameObject;
            base.Recibir(pallet);
            return true;
        }
        else
            return false;
    }
}
