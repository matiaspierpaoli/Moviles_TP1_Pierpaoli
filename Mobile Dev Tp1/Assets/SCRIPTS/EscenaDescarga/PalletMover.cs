using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletMover : ManejoPallets {

    [SerializeField] private InputManager inputManager;
    public int playerID = -1;
    public string horizontalInputName = "Horizontal";
    public string verticalInputName = "Vertical";


    public ManejoPallets Desde, Hasta;
    bool segundoCompleto = false;

    private void Update() {

        if (!Tenencia() && Desde.Tenencia() && inputManager.GetAxis(horizontalInputName, playerID.ToString()) > inputManager.GetMinAxisValue()) 
        {
            PrimerPaso();
        }
        if (Tenencia() && inputManager.GetAxis(verticalInputName, playerID.ToString()) < inputManager.GetMinAxisValue()) 
        {
            SegundoPaso();
        }
        if (segundoCompleto && inputManager.GetAxis(horizontalInputName, playerID.ToString()) > inputManager.GetMinAxisValue()) 
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
