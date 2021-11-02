﻿using Game.LevelManager;
using EnemyGenerator;
using System.Collections.Generic;
using Game.GameManager;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomBHV : MonoBehaviour
{

    public static event StartRoomEvent StartRoomEventHandler;

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
    public NPC[] npcPrefab;

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

    public static event EnterRoomEvent EnterRoomEventHandler;

    private void Awake()
    {
        hasEnemies = false;
        enemiesIndex = new List<int>();
        enemiesDead = 0;
    }

    // Use this for initialization
    void Start()
    {
        SetLayout();
        SetCenterPosition();
        if (RoomHasKey())
        {
            PlaceKeysInRoom();
        }
        if (RoomHasTreasure())
        {
            PlaceTreasureInRoom();
        }
        if (RoomHasNpc())
        {
            PlaceNpcInRoom();
        }
        if (roomData.IsStartRoom())
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            minimapIcon.GetComponent<SpriteRenderer>().color = new Color(0.5433761f, 0.2772784f, 0.6320754f, 1.0f);
            StartRoomEventHandler?.Invoke(this, new StartRoomEventArgs(GetAvailablePosition()));
        }
        else if (roomData.IsFinalRoom())
        {
            PlaceTriforceInRoom();
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (GameManagerSingleton.instance.enemyMode)
        {
            SelectEnemies();
        }

        minimapIcon.transform.localScale = new Vector3(roomData.Dimensions.Width, roomData.Dimensions.Height, 1);
    }

    private void DebugRoomData()
    {
        Debug.Log($"The current room: X {roomData.Coordinates.X}, Y {roomData.Coordinates.Y} has the keys with ");
        foreach (int keyID in roomData.KeyIDs)
        {
            Debug.Log($"Key ID: {keyID}");
        }
    }

    void SetLayout()
    {
        SetKeysToDoors();

        float centerX = roomData.Dimensions.Width / 2.0f - 0.5f;
        float centerY = roomData.Dimensions.Height / 2.0f - 0.5f;
        const float delta = 0.0f; //para que os colisores das portas e das paredes não se sobreponham completamente
                                  //Posiciona as portas - são somados/subtraídos 1 para que as portas e colisores estejam periféricos à sala
        SetDoorsTransform(centerX, centerY, delta);

        SetCollidersOnRoom(centerX, centerY);

        GameObject auxObj;
        //Posiciona os tiles
        for (int ix = 0; ix < roomData.Dimensions.Width; ix++)
        {
            for (int iy = 0; iy < roomData.Dimensions.Height; iy++)
            {
                int tileID = roomData.Tiles[ix, iy];
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
                        if (iy == (roomData.Dimensions.Height - 1))
                        {
                            auxObj = Instantiate(SWCollumn);
                            auxObj.transform.SetParent(transform);
                            auxObj.transform.localPosition = new Vector2(ix - centerX, roomData.Dimensions.Height - 1 - iy - centerY);
                        }
                        tileObj.GetComponent<SpriteRenderer>().sprite = westWall;
                    }
                    else if (iy == 0)
                    {
                        tileObj.GetComponent<SpriteRenderer>().sprite = northWall;
                    }
                    else if (ix == (roomData.Dimensions.Width - 1))
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
                {
                    tileObj = Instantiate(tilePrefab);
                }
                tileObj.transform.SetParent(transform);
                tileObj.transform.localPosition = new Vector2(ix - centerX, roomData.Dimensions.Height - 1 - iy - centerY);
                tileObj.GetComponent<SpriteRenderer>(); //FIXME provisório para diferenciar sprites
                tileObj.Id = tileID;
                tileObj.X = ix;
                tileObj.Y = iy;
            }
        }

        auxObj = Instantiate(NWCollumn);
        auxObj.transform.SetParent(transform);
        auxObj.transform.localPosition = new Vector2(-0.5f - centerX, roomData.Dimensions.Height - 0.5f - centerY);
        auxObj = Instantiate(SECollumn);
        auxObj.transform.SetParent(transform);
        auxObj.transform.localPosition = new Vector2(roomData.Dimensions.Width - 0.5f - centerX, -0.5f - centerY);
        auxObj = Instantiate(NECollumn);
        auxObj.transform.SetParent(transform);
        auxObj.transform.localPosition = new Vector2(roomData.Dimensions.Width - 0.5f - centerX, roomData.Dimensions.Height - 0.5f - centerY);
        auxObj = Instantiate(SWCollumn);
        auxObj.transform.SetParent(transform);
        auxObj.transform.localPosition = new Vector2(-0.5f - centerX, -0.5f - centerY);

        int margin = Util.Constants.distFromBorder;
        float xOffset = transform.position.x;
        float yOffset = transform.position.y;

        int lowerHalfVer = (roomData.Dimensions.Height / Util.Constants.nSpawnPointsHor);
        int upperHalfVer = (3 * roomData.Dimensions.Height / Util.Constants.nSpawnPointsHor);
        int lowerHalfHor = (roomData.Dimensions.Width / Util.Constants.nSpawnPointsVer);
        int upperHalfHor = (3 * roomData.Dimensions.Width / Util.Constants.nSpawnPointsVer);
        int topHor = (margin + (roomData.Dimensions.Width * (Util.Constants.nSpawnPointsVer - 1) / Util.Constants.nSpawnPointsVer));
        int topVer = (margin + (roomData.Dimensions.Height * (Util.Constants.nSpawnPointsHor - 1) / Util.Constants.nSpawnPointsHor));

        //Create spawn points avoiding the points close to doors.
        for (int ix = margin; ix < (roomData.Dimensions.Width - margin); ix += (roomData.Dimensions.Width / Util.Constants.nSpawnPointsVer))
        {
            for (int iy = margin; iy < (roomData.Dimensions.Height - margin); iy += (roomData.Dimensions.Height / Util.Constants.nSpawnPointsHor))
            {
                if ((ix <= margin) || (ix >= topHor))
                {
                    if (iy < lowerHalfVer || iy > upperHalfVer)
                    {
                        spawnPoints.Add(new Vector3(ix - centerX + xOffset, roomData.Dimensions.Height - 1 - iy - centerY + yOffset, 0));
                    }
                }
                else if ((iy <= margin) || (iy >= topVer))
                {
                    if (ix < lowerHalfHor || ix > upperHalfHor)
                    {
                        spawnPoints.Add(new Vector3(ix - centerX + xOffset, roomData.Dimensions.Height - 1 - iy - centerY + yOffset, 0));
                    }
                }
                else
                    spawnPoints.Add(new Vector3(ix - centerX + xOffset, roomData.Dimensions.Height - 1 - iy - centerY + yOffset, 0));
            }
        }

    }

    private void SetCollidersOnRoom(float centerX, float centerY)
    {
        //Posiciona os colisores das paredes da sala
        colNorth.transform.localPosition = new Vector2(0.0f, centerY + 1);
        colSouth.transform.localPosition = new Vector2(0.0f, -centerY - 1);
        colEast.transform.localPosition = new Vector2(centerX + 1, 0.0f);
        colWest.transform.localPosition = new Vector2(-centerX - 1, 0.0f);
        colNorth.GetComponent<BoxCollider2D>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
        colSouth.GetComponent<BoxCollider2D>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
        colEast.GetComponent<BoxCollider2D>().size = new Vector2(1, roomData.Dimensions.Height + 2);
        colWest.GetComponent<BoxCollider2D>().size = new Vector2(1, roomData.Dimensions.Height + 2);

        //Ajusta sprites das paredes
        colNorth.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
        colSouth.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
        colEast.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(1, roomData.Dimensions.Height + 2);
        colWest.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(1, roomData.Dimensions.Height + 2);
    }

    private void SetDoorsTransform(float centerX, float centerY, float delta)
    {
        doorNorth.transform.localPosition = new Vector2(0.0f, centerY + 1 - delta);
        doorSouth.transform.localPosition = new Vector2(0.0f, -centerY - 1 + delta);
        doorEast.transform.localPosition = new Vector2(centerX + 1 - delta, 0.0f);
        doorWest.transform.localPosition = new Vector2(-centerX - 1 + delta, 0.0f);
    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 spawnPoint in spawnPoints)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(spawnPoint, 1);
        }
    }

    private void SelectEnemies()
    {
        if (roomData.Difficulty > 0)
        {
            hasEnemies = true;
            GameManagerSingleton.instance.enemyLoader.LoadEnemies(roomData.EnemyType);
            for (int i = 0; i < roomData.Difficulty; ++i)
            {
                enemiesIndex.Add(GameManagerSingleton.instance.enemyLoader.GetRandomEnemyIndex(roomData.EnemyType));
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
            enemy = GameManagerSingleton.instance.enemyLoader.InstantiateEnemyWithIndex(enemiesIndex[i], new Vector3(spawnPoints[actualSpawn].x, spawnPoints[actualSpawn].y, 0f), transform.rotation, roomData.EnemyType);
            enemy.GetComponent<EnemyController>().SetRoom(this);
            selectedSpawnPoints.Add(actualSpawn);
        }
    }


    public void OnRoomEnter()
    {
        if (hasEnemies)
        {
            SpawnEnemies();
        }
        minimapIcon.GetComponent<SpriteRenderer>().color = new Color(0.5433761f, 0.2772784f, 0.6320754f, 1.0f);
        EnterRoomEventHandler(this, new EnterRoomEventArgs(roomData.Coordinates, hasEnemies, enemiesIndex, -1, gameObject.transform.position, roomData.Dimensions));
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
        doorNorth.keyID = northDoor;
        doorSouth.keyID = southDoor;
        doorEast.keyID = eastDoor;
        doorWest.keyID = westDoor;
    }

    public bool RoomHasKey()
    {
        return roomData.KeyIDs.Count > 0;
    }
    public void PlaceKeysInRoom()
    {
        foreach (int actualKey in roomData.KeyIDs)
        {
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
        treasure.Treasure = GameManagerSingleton.instance.treasureSet.Items[roomData.Treasure-1];
        treasure.transform.position = availablePosition;
        availablePosition.x += 1;
    }

    public void PlaceTriforceInRoom()
    {
        TriforceBHV tri = Instantiate(triPrefab, transform);
        tri.transform.position = availablePosition;
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

    public bool RoomHasNpc(){
        return roomData.NpcID > 0;
    }

    public void PlaceNpcInRoom(){
        NPC npc = Instantiate(npcPrefab[(roomData.NpcID-1)%3], transform);
        npc.transform.position = availablePosition;
        availablePosition.x += 1;
    }
}
