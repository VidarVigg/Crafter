using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager GetInstance()
    {
        return instance;
    }

    public static GameManager INSTANCE;
    public List<IGameStateObserver> gameStateObservers = new List<IGameStateObserver>();

    [SerializeField]
    private GlobalInventoryController inventoryService;

    [SerializeField]
    private PopupManager popupService;

    [SerializeField]
    private PlayerManager player;

    public PlayerManager Player
    {
        get { return player; }
        set { player = value; }
    }

    private void Awake()
    {
        instance = this;
        ServiceLocator.Initialize();
        Initialize();
    }


    private void Initialize()
    {
        ServiceLocator.PopupService = popupService;
        ServiceLocator.InventoryService = inventoryService;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void HandleState(GameStates state)
    {
        for (int i = 0; i < gameStateObservers.Count; i++)
        {
            gameStateObservers[i].OnGameStateChanged(state);
        }
    }
}

public enum GameStates
{
    Game,
    Inventory,
}
