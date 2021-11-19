﻿using System;
using Game.LevelManager;
using System.Collections.Generic;
using Game.EnemyGenerator;
using Game.Events;
using Game.GameManager;
using ScriptableObjects;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

public class RoomBhv : MonoBehaviour
{

    public static event StartRoomEvent StartRoomEventHandler;

    public DungeonRoom roomData;
    public List<int> northDoor;
    public List<int> southDoor;
    public List<int> eastDoor;
    public List<int> westDoor;

    public bool hasEnemies;
    public Dictionary<EnemySO, int> enemiesDictionary;
    private int enemiesDead;

    public DoorBhv doorNorth;
    public DoorBhv doorSouth;
    public DoorBhv doorEast;
    public DoorBhv doorWest;

    public KeyBHV keyPrefab;
    public TriforceBHV triPrefab;
    public TreasureController treasurePrefab;
    public NpcController[] npcPrefab;

    public Collider2D colNorth;
    public Collider2D colSouth;
    public Collider2D colEast;
    public Collider2D colWest;

    public TileBhv tilePrefab;
    public BlockBhv blockPrefab;

    public Sprite northWall, southWall, eastWall, westWall;
    public GameObject NWCollumn, NECollumn, SECollumn, SWCollumn;

    public GameObject minimapIcon;

    public List<Vector3> spawnPoints;

    protected Vector3 availablePosition;

    public static event EnterRoomEvent EnterRoomEventHandler;

    /// If true, the room is an Arena and do not have neighbors.
    private bool isArena = false;

    private void Awake()
    {
        hasEnemies = false;
        enemiesDictionary = new Dictionary<EnemySO, int>();
        enemiesDead = 0;
        isArena = GameManagerSingleton.instance.arenaMode;
    }

    // Use this for initialization
    void Start()
    {
        // If the Arena Mode is on, then set up the Arena
        if (roomData.IsStartRoom() && isArena)
        {
            roomData.TotalEnemies = GameManagerSingleton.instance.enemyLoader.arena.Length;
            hasEnemies = true;
        }

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

        // If the Arena Mode is on, then spawn enemies in the Arena
        if (roomData.IsStartRoom() && isArena)
        {
            SpawnEnemies();
        }
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
                TileBhv tileObj;
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
                    else if(iy == (roomData.Dimensions.Height - 1))
                    {
                        tileObj.GetComponent<SpriteRenderer>().sprite = southWall;
                    }
                }
                else
                {
                    tileObj = Instantiate(tilePrefab);
                }
                tileObj.SetPosition(ix - centerX, roomData.Dimensions.Height - 1 - iy - centerY, transform);
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

        int margin = Constants.distFromBorder;
        float xOffset = transform.position.x;
        float yOffset = transform.position.y;

        int lowerHalfVer = (roomData.Dimensions.Height / Constants.nSpawnPointsHor);
        int upperHalfVer = (3 * roomData.Dimensions.Height / Constants.nSpawnPointsHor);
        int lowerHalfHor = (roomData.Dimensions.Width / Constants.nSpawnPointsVer);
        int upperHalfHor = (3 * roomData.Dimensions.Width / Constants.nSpawnPointsVer);
        int topHor = (margin + (roomData.Dimensions.Width * (Constants.nSpawnPointsVer - 1) / Constants.nSpawnPointsVer));
        int topVer = (margin + (roomData.Dimensions.Height * (Constants.nSpawnPointsHor - 1) / Constants.nSpawnPointsHor));

        //Create spawn points avoiding the points close to doors.
        for (int ix = margin; ix < (roomData.Dimensions.Width - margin); ix += (roomData.Dimensions.Width / Constants.nSpawnPointsVer))
        {
            for (int iy = margin; iy < (roomData.Dimensions.Height - margin); iy += (roomData.Dimensions.Height / Constants.nSpawnPointsHor))
            {
                // Calculate the spwan point 2D position (spx, spy)
                float spx = ix - centerX + xOffset;
                float rh = roomData.Dimensions.Height - 1;
                float spy = rh - iy - centerY + yOffset;
                Vector3 point = new Vector3(spx, spy, 0);

                // If the room is an Arena, then ignore the room center
                if (isArena && Math.Abs(spx - 0.5f) < 0.001f && spy == 0.0f)
                {
                    continue;
                }

                // Add the calculated point to spawn point list
                if (ix <= margin || ix >= topHor)
                {
                    if (iy >= lowerHalfVer && iy <= upperHalfVer) continue;
                    if (isArena || roomData.Tiles[ix, iy] == (int) Enums.TileTypes.Block) continue;
                    spawnPoints.Add(point);
                }
                else if (iy <= margin || iy >= topVer)
                {
                    if (ix >= lowerHalfHor && ix <= upperHalfHor) continue;
                    if (isArena || roomData.Tiles[ix, iy] == (int) Enums.TileTypes.Block) continue;
                    spawnPoints.Add(point);
                }
                else
                {
                    spawnPoints.Add(point);
                }
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
        if (roomData.EnemiesByType == null && !isArena) return;
        hasEnemies = true;
        if (isArena)
        {
            // Load all the enemies from the folder
            EnemySO[] arena = GameManagerSingleton.instance.enemyLoader.arena;
            foreach (var enemy in arena)
            {
                enemiesDictionary.Add(enemy, 1);
            }
        }
        else
        {
            enemiesDictionary = EnemyDispenser.GetEnemiesForRoom(this);
        }
    }

    public void SpawnEnemies()
    {
        var selectedSpawnPoints = new List<int>();
        foreach (var enemiesFromType in enemiesDictionary)
        {
            for (var i = 0; i < enemiesFromType.Value; i++)
            {
                int actualSpawn;
                if (selectedSpawnPoints.Count >= spawnPoints.Count)
                {
                    selectedSpawnPoints.Clear();
                }
                do
                {
                    actualSpawn = Random.Range(0, spawnPoints.Count);
                } while (selectedSpawnPoints.Contains(actualSpawn));
                var enemy = GameManagerSingleton.instance.enemyLoader.InstantiateEnemyFromScriptableObject(
                    new Vector3(spawnPoints[actualSpawn].x, spawnPoints[actualSpawn].y, 0f), 
                    transform.rotation, enemiesFromType.Key);
                enemy.GetComponent<EnemyController>().SetRoom(this);
                selectedSpawnPoints.Add(actualSpawn);
            }
        }
    }


    public void OnRoomEnter()
    {
        if (hasEnemies)
        {
            SpawnEnemies();
        }
        minimapIcon.GetComponent<SpriteRenderer>().color = new Color(0.5433761f, 0.2772784f, 0.6320754f, 1.0f);
        EnterRoomEventHandler(this, new EnterRoomEventArgs(roomData.Coordinates, hasEnemies, enemiesDictionary, -1, gameObject.transform.position, roomData.Dimensions));
    }

    public void CheckIfAllEnemiesDead()
    {
        enemiesDead++;
        if(enemiesDead == roomData.TotalEnemies)
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
        NpcController npcController = Instantiate(npcPrefab[(roomData.NpcID-1)%3], transform);
        npcController.transform.position = availablePosition;
        availablePosition.x += 1;
    }
    
    
}