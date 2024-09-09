using UnityEngine;
using System.Collections;

public class Bolsa : MonoBehaviour
{
	public Pallet.Valores Monto;
	public string TagPlayer = "";
	public Texture2D ImagenInventario;
	Player Pj = null;
	
	public float maxSpawnTime = 12f;
    public string spawnManagerTag = "SpawnManagerTag";

    private float spawnTimer = 0f;
    private Transform spawnPoint;
    private SpawnManager moneyBagSpawnManager;

	void Start () 
	{
		Monto = Pallet.Valores.Valor2;
		moneyBagSpawnManager = GameObject.FindGameObjectWithTag(spawnManagerTag).GetComponent<SpawnManager>();
    }

    private void OnEnable()
    {
		spawnTimer = 0f;
    }

    void Update()
	{
		spawnTimer += Time.deltaTime;
		if (spawnTimer >= maxSpawnTime)
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
        spawnTimer = 0f;
        ObjectPool.Instance.ReturnObjectToPool(this.gameObject, moneyBagSpawnManager.objectTag);
    }

    public void SetSpawnPoint(Transform point, SpawnManager manager)
    {
        spawnPoint = point;
        moneyBagSpawnManager = manager;
    }
}
