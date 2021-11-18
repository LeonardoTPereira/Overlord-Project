using Game.LevelManager;
using System;
using System.Collections.Generic;
using Game.Events;
using Game.GameManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : PlaceableRoomObject
{

    private static Player instance = null;
    public List<int> keys = new List<int>();
    public List<int> usedKeys = new List<int>();
    public int x { private set; get; }
    public int y { private set; get; }
    public Camera cam;
    public Camera minimap;
    private AudioSource audioSrc;
    private PlayerController playerController;
    public static event EnterRoomEvent EnterRoomEventHandler;
    public static event ExitRoomEvent ExitRoomEventHandler;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            cam = Camera.main;
            audioSrc = GetComponent<AudioSource>();
            playerController = GetComponent<PlayerController>();
        }
    }

    public static Player Instance { get { return instance; } }

    public void OnEnable()
    {
        GameManagerSingleton.NewLevelLoadedEventHandler += ResetValues;
        RoomBHV.StartRoomEventHandler += PlacePlayerInStartRoom;
        KeyBHV.KeyCollectEventHandler += GetKey;
        GameManagerSingleton.EnterRoomEventHandler += GetHealth;
        GameManagerSingleton.EnterRoomEventHandler += AdjustCamera;
        RoomBHV.EnterRoomEventHandler += GetHealth;
        RoomBHV.EnterRoomEventHandler += AdjustCamera;
        DoorBHV.ExitRoomEventHandler += ExitRoom;
    }

    public void OnDisable()
    {
        GameManagerSingleton.NewLevelLoadedEventHandler -= ResetValues;
        RoomBHV.StartRoomEventHandler -= PlacePlayerInStartRoom;
        KeyBHV.KeyCollectEventHandler -= GetKey;
        GameManagerSingleton.EnterRoomEventHandler -= GetHealth;
        GameManagerSingleton.EnterRoomEventHandler -= AdjustCamera;
        RoomBHV.EnterRoomEventHandler -= GetHealth;
        RoomBHV.EnterRoomEventHandler -= AdjustCamera;
        DoorBHV.ExitRoomEventHandler -= ExitRoom;
    }

    private void GetKey(object sender, KeyCollectEventArgs eventArgs)
    {
        audioSrc.PlayOneShot(audioSrc.clip, 0.6f);
        keys.Add(eventArgs.KeyIndex);
    }

    public void AdjustCamera(object sender, EnterRoomEventArgs eventArgs)
    {
        Debug.Log("Adjusting Camera: " + eventArgs.RoomPosition.x + " - " + eventArgs.RoomPosition.y);
        int roomWidth = eventArgs.RoomDimensions.Width;
        float cameraXPosition = eventArgs.RoomPosition.x + roomWidth / 3.5f;
        float cameraYPosition = eventArgs.RoomPosition.y;
        float cameraZPosition = -5f;
        cam.transform.position = new Vector3(cameraXPosition, cameraYPosition, cameraZPosition);
        //minimap.transform.position = new Vector3(roomTransf.position.x, roomTransf.position.y, -5f);
    }

    private void PlacePlayerInStartRoom(object sender, StartRoomEventArgs e)
    {
        instance.transform.position = e.position;
    }

    private void ExitRoom(object sender, ExitRoomEventArgs eventArgs)
    {
        Instance.transform.position = eventArgs.EntrancePosition;
        eventArgs.PlayerHealthWhenExiting = playerController.GetHealth();
        ExitRoomEventHandler?.Invoke(this, eventArgs);
    }

    private void ResetValues(object sender, EventArgs eventArgs)
    {
        keys.Clear();
        usedKeys.Clear();
        playerController.ResetHealth();
    }
    private void GetHealth(object sender, EnterRoomEventArgs eventArgs)
    {
        int health = playerController.GetHealth();
        eventArgs.PlayerHealthWhenEntering = health;
        EnterRoomEventHandler(this, eventArgs);
    }
}
