using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyGenerator;
using UnityEngine.Tilemaps;

public class RoomBHV : MonoBehaviour {

	public int x;
	public int y;
	public List<int> northDoor; 
	public List<int> southDoor;
	public List<int> eastDoor;
	public List<int> westDoor;
	public List<int> availableKeyID;
    public int treasureID = -1;
	public bool isStart = false;
	public bool isEnd = false;

    public bool hasEnemies;
    public int difficultyLevel;
    public List<int> enemiesIndex;
    private int enemiesDead;

    public DoorBHV doorNorth;
	public DoorBHV doorSouth;
	public DoorBHV doorEast;
	public DoorBHV doorWest;

	public KeyBHV keyPrefab;
    public TriforceBHV triPrefab;
    public TreasureController treasurePrefab;

    public Collider2D colNorth;
	public Collider2D colSouth;
	public Collider2D colEast;
	public Collider2D colWest;

	public TileBHV tilePrefab;
    public BlockBHV blockPrefab;

    public Sprite northWall, southWall, eastWall, westWall;
    public GameObject NWCollumn, NECollumn, SECollumn, SWCollumn;

    public GameObject minimapIcon;

    public List<Vector3> spawnPoints;

    private void Awake()
    {
        hasEnemies = true;
        enemiesIndex = new List<int>();
        //null for non-existant
        northDoor = new List<int>();
        southDoor = new List<int>();
        eastDoor = new List<int>();
        westDoor = new List<int>();
        availableKeyID = new List<int>();
        enemiesDead = 0;
    }

    // Use this for initialization
    void Start () {
		SetLayout ();
		if (availableKeyID.Count > 0) // instancia chave se existir
        {
            int offset = 0;
            foreach (int actualKey in availableKeyID)
            {
                Vector3 keyPosition = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
                KeyBHV key = Instantiate(keyPrefab, keyPosition, transform.rotation);
                key.keyID = actualKey;
                //Debug.Log ("KeyID: " + key.keyID);
                key.SetRoom(x, y);
                offset += 5;
            }
		}
        if(treasureID > -1)
        {
            //Debug.Log("This Room has a treasure. Instantiate!");
            TreasureController treasure = Instantiate(treasurePrefab, transform);
            treasure.Treasure = GameManager.instance.treasureSet.Items[treasureID];
            treasure.SetRoom(x, y);
        }
		if (isStart){
			//Algum efeito
			transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            minimapIcon.GetComponent<SpriteRenderer>().color = new Color(0.5433761f, 0.2772784f, 0.6320754f, 1.0f);
            hasEnemies = false;
        }
        else if (isEnd){
            TriforceBHV tri = Instantiate(triPrefab, transform);
            tri.SetRoom(x, y);
            //Algum efeito
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
            hasEnemies = false;
        }
        else
        {
            if (GameManager.instance.enemyMode)
                SelectEnemies();
            else
                hasEnemies = false;
        }
        minimapIcon.transform.localScale = new Vector3(Room.sizeX, Room.sizeY, 1);
    }

    // Update is called once per frame
    void Update () {
        
	}

	void SetLayout(){
		doorNorth.keyID = northDoor;
		doorSouth.keyID = southDoor;
		doorEast.keyID = eastDoor;
		doorWest.keyID = westDoor;

        float centerX = Room.sizeX / 2.0f - 0.5f;
		float centerY = Room.sizeY / 2.0f - 0.5f;
		const float delta = 0.0f; //para que os colisores das portas e das paredes não se sobreponham completamente
		//Posiciona as portas - são somados/subtraídos 1 para que as portas e colisores estejam periféricos à sala
		doorNorth.transform.localPosition = new Vector2 (0.0f, centerY + 1 - delta);
		doorSouth.transform.localPosition = new Vector2 (0.0f, -centerY -1 + delta);
		doorEast.transform.localPosition = new Vector2 (centerX + 1 - delta, 0.0f);
		doorWest.transform.localPosition = new Vector2 (-centerX -1 + delta, 0.0f);

		//Posiciona os colisores das paredes da sala
		colNorth.transform.localPosition = new Vector2 (0.0f, centerY + 1);
		colSouth.transform.localPosition = new Vector2 (0.0f, -centerY - 1);
		colEast.transform.localPosition = new Vector2 (centerX + 1, 0.0f);
		colWest.transform.localPosition = new Vector2 (-centerX -1, 0.0f);
		colNorth.GetComponent<BoxCollider2D> ().size = new Vector2(Room.sizeX + 2, 1);
		colSouth.GetComponent<BoxCollider2D> ().size = new Vector2(Room.sizeX + 2, 1);
		colEast.GetComponent<BoxCollider2D> ().size = new Vector2 (1, Room.sizeY + 2);
		colWest.GetComponent<BoxCollider2D> ().size = new Vector2 (1, Room.sizeY + 2);

		//Ajusta sprites das paredes
		colNorth.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(Room.sizeX + 2, 1);
		colSouth.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(Room.sizeX + 2, 1);
		colEast.gameObject.GetComponent<SpriteRenderer>().size = new Vector2 (1, Room.sizeY + 2);
		colWest.gameObject.GetComponent<SpriteRenderer>().size = new Vector2 (1, Room.sizeY + 2);
        
        GameObject auxObj;
		//Posiciona os tiles
		Room thisRoom = GameManager.instance.GetMap().rooms[x, y]; //TODO fazer de forma similar para tirar construção de salas do GameManager
		for (int ix = 0; ix < Room.sizeX; ix++){
			for (int iy = 0; iy < Room.sizeY; iy++){
				int tileID = thisRoom.tiles [ix, iy];
                TileBHV tileObj;
                if (tileID == 1)
                {
                    tileObj = Instantiate(blockPrefab);
                    if (ix == 0)
                    {
                        if (iy == 0)
                        {
                            auxObj = Instantiate(NWCollumn);
                            auxObj.transform.SetParent(transform);
                            auxObj.transform.localPosition = new Vector2(ix - centerX, Room.sizeY - 1 - iy - centerY);
                        }
                        if(iy == (Room.sizeY-1))
                        {
                            auxObj = Instantiate(SWCollumn);
                            auxObj.transform.SetParent(transform);
                            auxObj.transform.localPosition = new Vector2(ix - centerX, Room.sizeY - 1 - iy - centerY);
                        }
                        tileObj.GetComponent<SpriteRenderer>().sprite = westWall;
                    }
                    else if(iy==0)
                    {
                        tileObj.GetComponent<SpriteRenderer>().sprite = northWall;
                    }
                    else if(ix == (Room.sizeX - 1))
                    {
                        if (iy == 0)
                        {
                            auxObj = Instantiate(NECollumn);
                            auxObj.transform.SetParent(transform);
                            auxObj.transform.localPosition = new Vector2(ix - centerX, Room.sizeY - 1 - iy - centerY);
                        }
                        if (iy == (Room.sizeY - 1))
                        {
                            auxObj = Instantiate(SECollumn);
                            auxObj.transform.SetParent(transform);
                            auxObj.transform.localPosition = new Vector2(ix - centerX, Room.sizeY - 1 - iy - centerY);
                        }
                        tileObj.GetComponent<SpriteRenderer>().sprite = eastWall;
                    }
                    else
                    {
                        tileObj.GetComponent<SpriteRenderer>().sprite = southWall;
                    }
                }
                else
                    tileObj = Instantiate(tilePrefab);
				tileObj.transform.SetParent (transform);
				tileObj.transform.localPosition = new Vector2 (ix - centerX, Room.sizeY -1 - iy - centerY);
				tileObj.GetComponent<SpriteRenderer> (); //FIXME provisório para diferenciar sprites
				tileObj.id = tileID;
				tileObj.x = ix;
				tileObj.y = iy;
			}
		}

        auxObj = Instantiate(NWCollumn);
        auxObj.transform.SetParent(transform);
        auxObj.transform.localPosition = new Vector2(- 0.5f - centerX, Room.sizeY - 0.5f - centerY);
        auxObj = Instantiate(SECollumn);
        auxObj.transform.SetParent(transform);
        auxObj.transform.localPosition = new Vector2(Room.sizeX - 0.5f - centerX, - 0.5f - centerY);
        auxObj = Instantiate(NECollumn);
        auxObj.transform.SetParent(transform);
        auxObj.transform.localPosition = new Vector2(Room.sizeX - 0.5f - centerX, Room.sizeY - 0.5f - centerY); 
        auxObj = Instantiate(SWCollumn);
        auxObj.transform.SetParent(transform);
        auxObj.transform.localPosition = new Vector2(- 0.5f - centerX, - 0.5f - centerY);

        int margin = Util.distFromBorder;
        float xOffset = transform.position.x;
        float yOffset = transform.position.y;

        int lowerHalfVer = (Room.sizeY / Util.nSpawnPointsHor);
        int upperHalfVer = (3*Room.sizeY / Util.nSpawnPointsHor);
        int lowerHalfHor = (Room.sizeX / Util.nSpawnPointsVer);
        int upperHalfHor = (3*Room.sizeX / Util.nSpawnPointsVer);
        int topHor = (margin + (Room.sizeX * (Util.nSpawnPointsVer - 1) / Util.nSpawnPointsVer));
        int topVer = (margin + (Room.sizeY * (Util.nSpawnPointsHor - 1) / Util.nSpawnPointsHor));

        //Create spawn points avoiding the points close to doors.
        for (int ix = margin; ix < (Room.sizeX - margin); ix+= (Room.sizeX/Util.nSpawnPointsVer))
        {
            for (int iy = margin; iy < (Room.sizeY - margin); iy += (Room.sizeY / Util.nSpawnPointsHor))
            {
                if ((ix <= margin) || (ix >= topHor))
                {
                    if (iy < lowerHalfVer || iy > upperHalfVer)
                        spawnPoints.Add(new Vector3(ix - centerX + xOffset, Room.sizeY - 1 - iy - centerY + yOffset, 0));
                }
                else if ((iy <= margin) || (iy >= topVer))
                {
                    if (ix < lowerHalfHor || ix > upperHalfHor)
                        spawnPoints.Add(new Vector3(ix - centerX + xOffset, Room.sizeY - 1 - iy - centerY + yOffset, 0));
                }
                else
                    spawnPoints.Add(new Vector3(ix - centerX + xOffset, Room.sizeY - 1 - iy - centerY + yOffset, 0));
            }
        }
        
    }

    /*private void OnDrawGizmos()
    {
        foreach (Vector3 spawnPoint in spawnPoints)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(spawnPoint, 1);
        }
    }*/

    private void SelectEnemies()
    {
        difficultyLevel = GameManager.instance.GetMap().rooms[x, y].difficulty;
        if (difficultyLevel == 0)
            hasEnemies = false;
        else
        {
            float actualDifficulty = 0;
            int auxIndex;
            while (actualDifficulty < difficultyLevel)
            {
                auxIndex = Random.Range(0, EnemyUtil.nBestEnemies);
                enemiesIndex.Add(auxIndex);
                actualDifficulty += GameManager.instance.enemyLoader.bestEnemies[auxIndex].fitness;
            }
        }
    }

    public void SpawnEnemies()
    {
        GameObject enemy;
        List<int> selectedSpawnPoints = new List<int>();
        int actualSpawn;
        for (int i = 0; i < enemiesIndex.Count; ++i)
        {
            if (enemiesIndex.Count >= spawnPoints.Count)
            {
                actualSpawn = Random.Range(0, spawnPoints.Count);
            }
            else
            {
                do
                {
                    actualSpawn = Random.Range(0, spawnPoints.Count);
                } while (selectedSpawnPoints.Contains(actualSpawn));
            }
            enemy = GameManager.instance.enemyLoader.InstantiateEnemyWithIndex(enemiesIndex[i], new Vector3(spawnPoints[actualSpawn].x, spawnPoints[actualSpawn].y, 0f), transform.rotation);
            enemy.GetComponent<EnemyController>().SetRoom(this);
            selectedSpawnPoints.Add(actualSpawn);
        }
        /*if (enemiesIndex.Count != 4)
        {
            for (int i = 0; i < enemiesIndex.Count; ++i)
            {
                enemy = GameManager.instance.enemyLoader.InstantiateEnemyWithIndex(enemiesIndex[i], new Vector3(transform.position.x, transform.position.y, 0f), transform.rotation);
                enemy.GetComponent<EnemyController>().SetRoom(this);
            }
        }
        else
        {
            enemy = GameManager.instance.enemyLoader.InstantiateEnemyWithIndex(enemiesIndex[0], new Vector3(transform.position.x + 6, transform.position.y + 5.5f, 0f), transform.rotation);
            enemy.GetComponent<EnemyController>().SetRoom(this);
            enemy = GameManager.instance.enemyLoader.InstantiateEnemyWithIndex(enemiesIndex[1], new Vector3(transform.position.x + 6, transform.position.y - 6, 0f), transform.rotation);
            enemy.GetComponent<EnemyController>().SetRoom(this);
            enemy = GameManager.instance.enemyLoader.InstantiateEnemyWithIndex(enemiesIndex[2], new Vector3(transform.position.x - 6, transform.position.y - 6, 0f), transform.rotation);
            enemy.GetComponent<EnemyController>().SetRoom(this);
            enemy = GameManager.instance.enemyLoader.InstantiateEnemyWithIndex(enemiesIndex[3], new Vector3(transform.position.x - 6, transform.position.y + 5.5f, 0f), transform.rotation);
            enemy.GetComponent<EnemyController>().SetRoom(this);
        }*/
    }
    //TODO FIX THIS
    public void SpawnItems()
    {
        GameObject enemy;
        List<int> selectedSpawnPoints = new List<int>();
        int actualSpawn;
        for (int i = 0; i < enemiesIndex.Count; ++i)
        {
            if (enemiesIndex.Count >= spawnPoints.Count)
            {
                actualSpawn = Random.Range(0, spawnPoints.Count);
            }
            else
            {
                do
                {
                    actualSpawn = Random.Range(0, spawnPoints.Count);
                } while (selectedSpawnPoints.Contains(actualSpawn));
            }
            enemy = GameManager.instance.enemyLoader.InstantiateEnemyWithIndex(enemiesIndex[i], new Vector3(spawnPoints[actualSpawn].x, spawnPoints[actualSpawn].y, 0f), transform.rotation);
            enemy.GetComponent<EnemyController>().SetRoom(this);
            selectedSpawnPoints.Add(actualSpawn);
        }
    }

        public void OnRoomEnter()
    {
        if (hasEnemies)
            SpawnEnemies();
        minimapIcon.GetComponent<SpriteRenderer>().color = new Color(0.5433761f, 0.2772784f, 0.6320754f, 1.0f);
        PlayerProfile.instance.OnRoomEnter(x, y, hasEnemies, enemiesIndex, Player.instance.GetComponent<PlayerController>().GetHealth());
    }

    public void CheckIfAllEnemiesDead()
    {
        enemiesDead++;
        if(enemiesDead == enemiesIndex.Count)
        {
            hasEnemies = false;
            doorEast.OpenDoorAfterKilling();
            doorWest.OpenDoorAfterKilling();
            doorNorth.OpenDoorAfterKilling();
            doorSouth.OpenDoorAfterKilling();
        }
    }
}
