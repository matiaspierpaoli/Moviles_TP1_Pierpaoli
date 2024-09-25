public class PalletMover : ManejoPallets
{
    public int playerID = -1;
    public string horizontalInputName = "Horizontal";
    public string verticalInputName = "Vertical";

    public ManejoPallets Desde, Hasta;

    private enum EstadoPallet { Ninguno, PrimerPaso, SegundoPaso, TercerPaso }
    private EstadoPallet estadoActual = EstadoPallet.Ninguno;

    private void Update()
    {
        float horizontalInput = InputManager.Instance.GetAxis(horizontalInputName, playerID.ToString());
        float verticalInput = InputManager.Instance.GetAxis(verticalInputName, playerID.ToString());

        switch (estadoActual)
        {
            case EstadoPallet.Ninguno:
                if (!Tenencia() && Desde.Tenencia() && horizontalInput < -InputManager.Instance.GetMinAxisValue())
                {
                    PrimerPaso();
                }
                break;

            case EstadoPallet.PrimerPaso:
                if (Tenencia() && verticalInput > InputManager.Instance.GetMinAxisValue())
                {
                    SegundoPaso();
                }
                break;

            case EstadoPallet.SegundoPaso:
                if (horizontalInput > InputManager.Instance.GetMinAxisValue())
                {
                    TercerPaso();
                }
                break;
        }
    }

    void PrimerPaso()
    {
        Desde.Dar(this);
        estadoActual = EstadoPallet.PrimerPaso;
    }

    void SegundoPaso()
    {
        base.Pallets[0].transform.position = transform.position;
        estadoActual = EstadoPallet.SegundoPaso;
    }

    void TercerPaso()
    {
        Dar(Hasta);
        estadoActual = EstadoPallet.Ninguno; 
    }

    public override void Dar(ManejoPallets receptor)
    {
        if (Tenencia())
        {
            if (receptor.Recibir(Pallets[0]))
            {
                Pallets.RemoveAt(0);
            }
        }
    }

    public override bool Recibir(Pallet pallet)
    {
        if (!Tenencia())
        {
            pallet.Portador = this.gameObject;
            base.Recibir(pallet);
            return true;
        }
        else
        {
            return false;
        }
    }
}
