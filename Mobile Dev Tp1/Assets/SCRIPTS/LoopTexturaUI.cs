using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con el componente Image

public class LoopTexturaUI : MonoBehaviour
{
    public float Intervalo = 1f;
    private float Tempo = 0f;

    public Sprite[] Imagenes; // Cambiado a Sprite para UI Image
    private int Contador = 0;

    private Image imageComponent; // Referencia al componente Image

    // Use this for initialization
    void Start()
    {
        // Obtén la referencia al componente Image en el mismo GameObject
        imageComponent = GetComponent<Image>();

        if (Imagenes.Length > 0 && imageComponent != null)
            imageComponent.sprite = Imagenes[0];
    }

    // Update is called once per frame
    void Update()
    {
        Tempo += Time.deltaTime;

        if (Tempo >= Intervalo)
        {
            Tempo = 0f;
            Contador++;
            if (Contador >= Imagenes.Length)
            {
                Contador = 0;
            }
            if (imageComponent != null)
                imageComponent.sprite = Imagenes[Contador];
        }
    }
}
