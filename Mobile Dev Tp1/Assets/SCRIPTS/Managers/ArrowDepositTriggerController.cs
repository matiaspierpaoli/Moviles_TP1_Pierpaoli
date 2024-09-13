//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Pool;
//using static ObjectPool;

//public class ArrowDepositTriggerController : MonoBehaviour
//{
//    [SerializeField] private Transform designatedDepositTriggerTransform;
//    [SerializeField] private GameObject indicatingArrow;
//    [SerializeField] private string leftDepositTriggersPoolName;
//    [SerializeField] private string rightDepositTriggersPoolName;
//    private float coroutineDuration = 1f;

//    private ObjectPool objectPool;

//    void Start()
//    {
//        objectPool = ObjectPool.Instance;
//        indicatingArrow.SetActive(false);

//        StartCoroutine(LateStart(coroutineDuration));
//    }

//    private IEnumerator LateStart(float duration)
//    {
//        yield return new WaitForSeconds(duration);

//        DefineArrowState(objectPool.GetPoolByTag(leftDepositTriggersPoolName));
//        DefineArrowState(objectPool.GetPoolByTag(rightDepositTriggersPoolName));
//    }

//    private void DefineArrowState(Pool depositTriggersPool)
//    {
//        List<GameObject> newOjectPool = objectPool.GetAllObjectsFromPool(depositTriggersPool.tag);

//        for (int i = 0; i < depositTriggersPool.size; i++)
//        {
//            if (newOjectPool[i].transform.position == designatedDepositTriggerTransform.position)
//            {
//                if (objectPool.GetAllObjectsFromPool(depositTriggersPool.tag)[i].gameObject.activeSelf)
//                {
//                    indicatingArrow.SetActive(true);
//                    return;
//                }
//            }
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static ObjectPool;

public class ArrowDepositManager : MonoBehaviour
{
    [System.Serializable]
    public class DepositData
    {
        public Transform designatedDepositTriggerTransform;  // El transform del trigger a verificar
        public GameObject indicatingArrow;                  // Flecha que será activada/desactivada
    }

    [SerializeField] private List<DepositData> depositDataList;  // Lista de depósitos y sus flechas
    [SerializeField] private string[] depositTriggerPoolNames;   // Array con los nombres de las pools a verificar
    private ObjectPool objectPool;

    void Start()
    {
        objectPool = ObjectPool.Instance;

        // Inicialmente, apagar todas las flechas
        foreach (var depositData in depositDataList)
        {
            depositData.indicatingArrow.SetActive(false);
        }

        // Iniciar la corrutina para hacer el chequeo después de un breve delay
        StartCoroutine(CheckPoolsAfterDelay(1f));  // 1 segundo de delay
    }

    // Corrutina que espera un tiempo antes de ejecutar el chequeo
    private IEnumerator CheckPoolsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        CheckPoolsAndUpdateArrows();
    }

    // Método que revisa todas las pools y actualiza todas las flechas
    private void CheckPoolsAndUpdateArrows()
    {
        // Iterar por cada depósito
        foreach (var depositData in depositDataList)
        {
            bool isArrowActive = false;

            // Iterar por cada pool
            foreach (string poolName in depositTriggerPoolNames)
            {
                Pool depositTriggersPool = objectPool.GetPoolByTag(poolName);
                List<GameObject> objectPoolList = objectPool.GetAllObjectsFromPool(depositTriggersPool.tag);

                // Verificar si algún objeto activo coincide con la posición del depósito
                foreach (GameObject obj in objectPoolList)
                {
                    if (obj.transform.position == depositData.designatedDepositTriggerTransform.position && obj.activeSelf)
                    {
                        isArrowActive = true;
                        break;  // Salir del loop si ya se encontró una coincidencia
                    }
                }

                if (isArrowActive) break;  // Si la flecha ya debe activarse, no seguir chequeando más pools
            }

            // Activar o desactivar la flecha según el resultado del chequeo
            depositData.indicatingArrow.SetActive(isArrowActive);
        }
    }
}


