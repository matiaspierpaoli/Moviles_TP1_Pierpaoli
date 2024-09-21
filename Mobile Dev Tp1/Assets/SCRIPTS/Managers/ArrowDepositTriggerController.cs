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
        public Transform designatedDepositTriggerTransform;  
        public GameObject indicatingArrow;
    }

    [SerializeField] private List<DepositData> depositDataList;
    [SerializeField] private string[] depositTriggerPoolNames;
    private ObjectPool objectPool;

    void Start()
    {
        objectPool = ObjectPool.Instance;

        foreach (var depositData in depositDataList)
        {
            depositData.indicatingArrow.SetActive(false);
        }

        StartCoroutine(CheckPoolsAfterDelay(1f));
    }

    private IEnumerator CheckPoolsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        CheckPoolsAndUpdateArrows();
    }

    private void CheckPoolsAndUpdateArrows()
    {
        foreach (var depositData in depositDataList)
        {
            bool isArrowActive = false;

            foreach (string poolName in depositTriggerPoolNames)
            {
                Pool depositTriggersPool = objectPool.GetPoolByTag(poolName);
                List<GameObject> objectPoolList = objectPool.GetAllObjectsFromPool(depositTriggersPool.tag);

                foreach (GameObject obj in objectPoolList)
                {
                    if (obj.transform.position == depositData.designatedDepositTriggerTransform.position && obj.activeSelf)
                    {
                        isArrowActive = true;
                        break; 
                    }
                }

                if (isArrowActive) break;
            }

            depositData.indicatingArrow.SetActive(isArrowActive);
        }
    }
}
