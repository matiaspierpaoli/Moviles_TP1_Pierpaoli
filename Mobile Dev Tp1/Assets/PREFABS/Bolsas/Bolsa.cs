using UnityEngine;
using System.Collections;

public class Bolsa : MonoBehaviour, ISpawnable
{
	public Pallet.Valores Monto;
	public string TagPlayer = "";
	public Texture2D ImagenInventario;
	Player Pj = null;
	
	public float maxSpawnTime = 12f;
    public string spawnManagerTag = "SpawnManagerTag";

    private Transform spawnPoint;
    private SpawnManager moneyBagSpawnManager;

	void Start () 
	{
		Monto = Pallet.Valores.Valor2;
		moneyBagSpawnManager = GameObject.FindGameObjectWithTag(spawnManagerTag).GetComponent<SpawnManager>();
    }

    void Update()
	{
		if (IsBehindLastCheckpoint())
		{
			Disappear();
        }
	}

	void OnTriggerEnter(Collider coll)
	{
		if(coll.tag == TagPlayer)
		{
			Pj = coll.GetComponent<Player>();
			if(Pj.AddBag(this))
				Disappear();
		}
	}
	
	public void Disappear()
	{
        moneyBagSpawnManager.ReleaseSpawnPoint(spawnPoint);
        ObjectPool.Instance.ReturnObjectToPool(this.gameObject, moneyBagSpawnManager.spawnSettings.objectTag);
    }

    public void SetSpawnPoint(Transform point, SpawnManager manager)
    {
        spawnPoint = point;
        moneyBagSpawnManager = manager;
    }

    private bool IsBehindLastCheckpoint()
    {
        int lastActiveCheckpoint = moneyBagSpawnManager.GetLastActiveCheckpoint();

        if (lastActiveCheckpoint >= 0)
        {
			if (spawnPoint.position.z < moneyBagSpawnManager.checkPointsHolder.checkPoints[lastActiveCheckpoint].gameObject.transform.position.z)
			{
                return true;
            }
        }
        return false;
    }
}
