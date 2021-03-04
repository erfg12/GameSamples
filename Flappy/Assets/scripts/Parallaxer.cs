using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour {

	class PoolObject
	{
		public Transform transform;
		public bool inUse;
		public PoolObject(Transform t) { transform = t; }
		public void Use()
		{
			inUse = true;
		}

		public void Dispose()
		{
			inUse = false;
		}
	}

	[System.Serializable]//to have a struct lookup in the inspector
	public struct YSpawnRange
	{//Since the Y is of the woods will change, they must have min and max Y values.
		public float min;
		public float max;
	}

	public GameObject Prefab;//what type of prefab will be born
	public int poolSize;//How many prefabs need to be born or enough. (How many wood, how many stars, how many clouds)
	public float shiftSpeed;//speed of objects
	public float spawnRate;//how often objects are born

	public YSpawnRange ySpawnRange;
	public Vector3 defaultSpawnPos;//default spawn position of objects
	public bool spawnImmediate;//necessary for the initial adjustment of our objects to spawn ??????
	public Vector3 immediateSpawnPos;
	public Vector2 targetAspectsRatio;//Aspect Ratio for large screen devices (ipad).
									  //necessary for objects not to appear on the screen when spawning or disappearing.Accordingly, it is associated with our default and instant spawn positions.

	float spawnTimer;
	float targetAspect;
	PoolObject[] poolObjects;
	GameManager game;

	void Awake(){
		Configure ();
	}

	void Start(){
		game = GameManager.Instance;
	}

	void OnEnable(){
		GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
	}

	void OnDisable(){
		GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
	}

	void OnGameOverConfirmed(){
		for (int i = 0; i < poolObjects.Length; i++) {
			poolObjects [i].Dispose ();
			poolObjects[i].transform.position=Vector3.one*1000;
		}		
		if (spawnImmediate) {
			SpawnImmediate ();
		}
	}

	void Update(){
		if (game.GameOver)
			return;
		
		Shift ();
		spawnTimer += Time.deltaTime;

		if (spawnTimer > spawnRate) {
			Spawn ();
			spawnTimer = 0;
		}
	}

	void Configure(){
		targetAspect = targetAspectsRatio.x / targetAspectsRatio.y;
		poolObjects = new PoolObject [poolSize];
		for (int i = 0; i < poolObjects.Length; i++) {
			GameObject go = Instantiate (Prefab) as GameObject;//Method used to clone prefab
			Transform t=go.transform;
			t.SetParent (transform);//script takes the transform of whatever object it is attached to
			t.position=Vector3.one*1000;
			poolObjects [i] = new PoolObject (t);
		}
		if (spawnImmediate) {
			SpawnImmediate ();
		}
	}

	void Spawn(){
		Transform t = GetPoolObject ();
		if (t == null)//could not find any suitable object. poolSize is too small.
			return;
		Vector3 pos = Vector3.zero;
		pos.x = (defaultSpawnPos.x*Camera.main.aspect)/targetAspect;
		pos.y = Random.Range (ySpawnRange.min,ySpawnRange.max);
		t.position = pos;
	}

	void SpawnImmediate(){
		Transform t = GetPoolObject ();
		if (t == null)//could not find any suitable object. poolSize is too small.
			return;
		Vector3 pos = Vector3.zero;
		pos.x = (immediateSpawnPos.x*Camera.main.aspect)/targetAspect;
		pos.y = Random.Range (ySpawnRange.min,ySpawnRange.max);
		t.position = pos;
		Spawn ();
	}

	void Shift(){
		for (int i = 0; i < poolObjects.Length ; i++) {
			poolObjects [i].transform.position+= -Vector3.right * shiftSpeed * Time.deltaTime;
			CheckDisposeObject (poolObjects [i]);
		}
	}

	void CheckDisposeObject(PoolObject poolObject)
	{//is our object out of the screen
		if (poolObject.transform.position.x<(-defaultSpawnPos.x*Camera.main.aspect)/targetAspect){
			poolObject.Dispose ();
			poolObject.transform.position = Vector3.one * 1000;//the object is sent too far because it is no longer used.
		}
	}

	Transform GetPoolObject()
	{//In order to get the object in the appropriate state
		for (int i=0;i<poolObjects.Length;i++){
			if (!poolObjects [i].inUse) {
				poolObjects [i].Use ();
				return poolObjects [i].transform;
			}
		}
		return null;
	}

}
