using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyGenerator;
using UnityEngine.Tilemaps;

public class StartRoomEventArgs : System.EventArgs
{
    public Vector3 position;
    public StartRoomEventArgs(Vector3 position)
    {
        this.position = position;
    }
}

public delegate void StartRoomEventHandler(Object sender, StartRoomEventArgs e);
public class RoomBHV : MonoBehaviour {

    public static event StartRoomEventHandler StartRoomInstantiated;

    public DungeonRoom roomData;
	public List<int> northDoor; 
	public List<int> southDoor;
	public List<int> eastDoor;
	public List<int> westDoor;

    public bool hasEnemies;
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

    protected Vector3 availablePosition;

    private void Awake()
    {
        hasEnemies = true;
        enemiesIndex = new List<int>();
        enemiesDead = 0;
    }

    // Use this for initialization
    void Start () {
		SetLayout ();
        SetCenterPosition();
#if UNITY_EDITOR
        Debug.Log($"The current room is positionioded at {transform.position}");
#endif
        if (RoomHasKey())
        {
#if UNITY_EDITOR
            Debug.Log($"The current room: X {roomData.coordinates.X}, Y {roomData.coordinates.Y} has the keys with ");
            foreach (int keyID in roomData.keyIDs)
            {
                Debug.Log($"Key ID: {keyID}");
            }
#endif
            PlaceKeysInRoom();
        }
        if(RoomHasTreasure())
        {
            PlaceTreasureInRoom();
        }
		if (roomData.IsStartRoom()){
			transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            minimapIcon.GetComponent<SpriteRenderer>().color = new Color(0.5433761f, 0.2772784f, 0.6320754f, 1.0f);
            StartRoomInstantiated?.Invoke(this, new StartRoomEventArgs(GetAvailablePosition()));
        }
        else if (roomData.IsFinalRoom())
        {
            PlaceTriforceInRoom();
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (roomData.Difficulty == 0)
            hasEnemies = false;
        else
            if (GameManager.instance.enemyMode)
                SelectEnemies();
        minimapIcon.transform.localScale = new Vector3(roomData.Dimensions.Width, roomData.Dimensions.Height, 1); ;
    }

    // Update is called once per frame
    void Update () {
        
	}

	void SetLayout(){
        SetKeysToDoors();

        float centerX = roomData.Dimensions.Width / 2.0f - 0.5f;
		float centerY = roomData.Dimensions.Height / 2.0f - 0.5f;
        const float delta = 0.0f; //para que os colisores das portas e das paredes não se sobreponham completamente
		//Posiciona as portas - são somados/subtraídos 1 para que as portas e colisores estejam periféricos à sala
		doorNorth.transform.localPosition = new Vector2 (0.0f, centerY + 1 - delta);
		doorSouth.transform.localPosition = new Vector2 (0.0f, -centerY - 1 + delta);
		doorEast.transform.localPosition = new Vector2 (centerX + 1 - delta, 0.0f);
		doorWest.transform.localPosition = new Vector2 (-centerX -1 + delta, 0.0f);

		//Posiciona os colisores das paredes da sala
		colNorth.transform.localPosition = new Vector2 (0.0f, centerY + 1);
		colSouth.transform.localPosition = new Vector2 (0.0f, -centerY - 1);
		colEast.transform.localPosition = new Vector2 (centerX + 1, 0.0f);
		colWest.transform.localPosition = new Vector2 (-centerX -1, 0.0f);
		colNorth.GetComponent<BoxCollider2D> ().size = new Vector2(roomData.Dimensions.Width + 2, 1);
		colSouth.GetComponent<BoxCollider2D> ().size = new Vector2(roomData.Dimensions.Width + 2, 1);
		colEast.GetComponent<BoxCollider2D> ().size = new Vector2 (1, roomData.Dimensions.Height + 2);
		colWest.GetComponent<BoxCollider2D> ().size = new Vector2 (1, roomData.Dimensions.Height + 2);

		//Ajusta sprites das paredes
		colNorth.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
		colSouth.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
		colEast.gameObject.GetComponent<SpriteRenderer>().size = new Vector2 (1, roomData.Dimensions.Height + 2);
		colWest.gameObject.GetComponent<SpriteRenderer>().size = new Vector2 (1, roomData.Dimensions.Height + 2);
        
        GameObject auxObj;
		//Posiciona os tiles
		for (int ix = 0; ix < roomData.Dimensions.Width; ix++){
			for (int iy = 0; iy < roomData.Dimensions.Height; iy++){
				int tileID = roomData.tiles [ix, iy];
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
                            auxObj.transform.localPosition = new Vector2(ix - centerX, roomData.Dimensions.Height - 1 - iy - centerY);
                        }
                        if(iy == (roomData.Dimensions.Height-1))
                        {
                            auxObj = Instantiate(SWCollumn);
                            auxObj.transform.SetParent(transform);
                            auxObj.transform.localPosition = new Vector2(ix - centerX, roomData.Dimensions.Height - 1 - iy - centerY);
                        }
                        tileObj.GetComponent<SpriteRenderer>().sprite = westWall;
                    }
                    else if(iy==0)
                    {
                        tileObj.GetComponent<SpriteRenderer>().sprite = northWall;
                    }
                    else if(ix == (roomData.Dimensions.Width - 1))
                    {
                        if (iy == 0)
                        {
                            auxObj = Instantiate(NECollumn);
                            auxObj.transform.SetParent(transform);
                            auxObj.transform.localPosition = new Vector2(ix - centerX, roomData.Dimensions.Height - 1 - iy - centerY);
                        }
                        if (iy == (roomData.Dimensions.Height - 1))
                        {
                            auxObj = Instantiate(SECollumn);
                            auxObj.transform.SetParent(transform);
                            auxObj.transform.localPosition = new Vector2(ix - centerX, roomData.Dimensions.Height - 1 - iy - centerY);
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
				tileObj.transform.localPosition = new Vector2 (ix - centerX, roomData.Dimensions.Height -1 - iy - centerY);
				tileObj.GetComponent<SpriteRenderer> (); //FIXME provisório para diferenciar sprites
				tileObj.id = tileID;
				tileObj.x = ix;
				tileObj.y = iy;
			}
		}

        auxObj = Instantiate(NWCollumn);
        auxObj.transform.SetParent(transform);
        auxObj.transform.localPosition = new Vector2(- 0.5f - centerX, roomData.Dimensions.Height - 0.5f - centerY);
        auxObj = Instantiate(SECollumn);
        auxObj.transform.SetParent(transform);
        auxObj.transform.localPosition = new Vector2(roomData.Dimensions.Width - 0.5f - centerX, - 0.5f - centerY);
        auxObj = Instantiate(NECollumn);
        auxObj.transform.SetParent(transform);
        auxObj.transform.localPosition = new Vector2(roomData.Dimensions.Width - 0.5f - centerX, roomData.Dimensions.Height - 0.5f - centerY); 
        auxObj = Instantiate(SWCollumn);
        auxObj.transform.SetParent(transform);
        auxObj.transform.localPosition = new Vector2(- 0.5f - centerX, - 0.5f - centerY);

        int margin = Util.distFromBorder;
        float xOffset = transform.position.x;
        float yOffset = transform.position.y;

        int lowerHalfVer = (roomData.Dimensions.Height / Util.nSpawnPointsHor);
        int upperHalfVer = (3*roomData.Dimensions.Height / Util.nSpawnPointsHor);
        int lowerHalfHor = (roomData.Dimensions.Width / Util.nSpawnPointsVer);
        int upperHalfHor = (3*roomData.Dimensions.Width / Util.nSpawnPointsVer);
        int topHor = (margin + (roomData.Dimensions.Width * (Util.nSpawnPointsVer - 1) / Util.nSpawnPointsVer));
        int topVer = (margin + (roomData.Dimensions.Height * (Util.nSpawnPointsHor - 1) / Util.nSpawnPointsHor));

        //Create spawn points avoiding the points close to doors.
        for (int ix = margin; ix < (roomData.Dimensions.Width - margin); ix+= (roomData.Dimensions.Width/Util.nSpawnPointsVer))
        {
            for (int iy = margin; iy < (roomData.Dimensions.Height - margin); iy += (roomData.Dimensions.Height / Util.nSpawnPointsHor))
            {
                if ((ix <= margin) || (ix >= topHor))
                {
                    if (iy < lowerHalfVer || iy > upperHalfVer)
                        spawnPoints.Add(new Vector3(ix - centerX + xOffset, roomData.Dimensions.Height - 1 - iy - centerY + yOffset, 0));
                }
                else if ((iy <= margin) || (iy >= topVer))
                {
                    if (ix < lowerHalfHor || ix > upperHalfHor)
                        spawnPoints.Add(new Vector3(ix - centerX + xOffset, roomData.Dimensions.Height - 1 - iy - centerY + yOffset, 0));
                }
                else
                    spawnPoints.Add(new Vector3(ix - centerX + xOffset, roomData.Dimensions.Height - 1 - iy - centerY + yOffset, 0));
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
        roomData.Difficulty = roomData.Difficulty;
        if (roomData.Difficulty == 0)
            hasEnemies = false;
        else
        {
            float actualDifficulty = 0;
            int auxIndex;
            while (actualDifficulty < roomData.Difficulty)
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
    }


    public void OnRoomEnter()
    {
        if (hasEnemies)
            SpawnEnemies();
        minimapIcon.GetComponent<SpriteRenderer>().color = new Color(0.5433761f, 0.2772784f, 0.6320754f, 1.0f);
        PlayerProfile.instance.OnRoomEnter(roomData.coordinates, hasEnemies, enemiesIndex, Player.instance.GetComponent<PlayerController>().GetHealth());
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

    public void SetKeysToDoors()
    {
        Debug.Log($"This room has the keys: North {northDoor?.Count}, South {southDoor?.Count}, East {eastDoor?.Count}, West {westDoor?.Count}");
        doorNorth.keyID = northDoor;
        doorSouth.keyID = southDoor;
        doorEast.keyID = eastDoor;
        doorWest.keyID = westDoor;
    }

    public bool RoomHasKey()
    {
        return roomData.keyIDs.Count > 0;
    }
    public void PlaceKeysInRoom()
    {
        foreach (int actualKey in roomData.keyIDs)
        {
#if UNITY_EDITOR
            Debug.Log($"Key position: {availablePosition}");
#endif
            PlaceKeyInRoom(actualKey);
            availablePosition.x += 1;
        }
    }

    public void PlaceKeyInRoom(int keyId)
    {
        KeyBHV key = Instantiate(keyPrefab, availablePosition, transform.rotation);
        key.transform.position = availablePosition;
        key.keyID = keyId;
    }

    public bool RoomHasTreasure()
    {
        return roomData.Treasure > 0;
    }

    public void PlaceTreasureInRoom()
    {
        TreasureController treasure = Instantiate(treasurePrefab, transform);
        treasure.Treasure = GameManager.instance.treasureSet.Items[roomData.Treasure - 1];
        treasure.transform.position = availablePosition;
#if UNITY_EDITOR
        Debug.Log($"Treasure position: {availablePosition}");
#endif
        availablePosition.x += 1;
    }

    public void PlaceTriforceInRoom()
    {
        TriforceBHV tri = Instantiate(triPrefab, transform);
        tri.transform.position = availablePosition;
#if UNITY_EDITOR
        Debug.Log($"Key position: {availablePosition}");
#endif
        availablePosition.x += 1;
    }

    public Vector3 GetAvailablePosition()
    {
        return availablePosition;
    }

    public void SetCenterPosition()
    {
        availablePosition = roomData.GetCenterMostFreeTilePosition() + transform.position;
    }
}
