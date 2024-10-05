using System;
using System.Collections;
using SchemaTest.InstanceSharingTypes;
using Unity.VisualScripting;
using UnityEngine;

public class GameboardManager : MonoBehaviour
{
  public static GameboardManager Instance { get; private set; } //Singleton instance

  //Properties
  public GameTile LastTouchedGameTile { get; set; } // The last touched game tile
  public bool CurrentlyTouchingGameBoard { get; private set; } // True if the player is currently touching the game board
  public GameTile[,] GameTiles { get; private set; } //2D array of GameTile objects

  [Header("Prefabs")]
  [SerializeField] private GameObject _gameTilePrefab; //Reference to the GameTile prefab

  //Private variables
  private NetworkManager _networkManager; //Reference to the NetworkManager
  private ResourceManager _resourceManager; //Reference to the ResourceManager
  private GamePieceManager _gamePieceManager; //Reference to the GamePieceManager

  //Awake is called when the script instance is being loaded
  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  //Start is called before the first frame update
  private void Start()
  {
    //Get references to the managers
    _networkManager = NetworkManager.Instance;
    _resourceManager = ResourceManager.Instance;
    _gamePieceManager = GamePieceManager.Instance;

    //Subscribe to events
    _networkManager.OnBoardCreated += CreateGameboard;
    _networkManager.OnBoardChanged += HandleChange;
  }

  private void Update()
  {
    IsPlayerTouchingGameBoard();
  }

  /*---------------------------------------------------------
  * Network event handlers
  ----------------------------------------------------------*/
  public void SelectTadpoleToEvolve()
  {
    Debug.Log("Choose piece to evolve");
    LastTouchedGameTile = null;

    StartCoroutine(SendEvolvingTadpole());
  }

  private IEnumerator SendEvolvingTadpole()
  {
    yield return new WaitUntil(() => LastTouchedGameTile != null);

    if (LastTouchedGameTile.CurrentlyHeldPiece.pieceType == "tadpole")
    {
      Debug.Log(LastTouchedGameTile.ArrayPosition.x + " " + LastTouchedGameTile.ArrayPosition.y);
      _networkManager.SendEvolvingTadpole(LastTouchedGameTile.ArrayPosition.x, LastTouchedGameTile.ArrayPosition.y);
    }
    else
    {
      Debug.Log("Not a tadpole");
      SelectTadpoleToEvolve();
    }
  }

  /*---------------------------------------------------------
  * Place a tadpole on the game board
  * @param state - The state of the game piece to place
  ----------------------------------------------------------*/
  private void PlaceTadpole(int tileX, int tileY, int playerId)
  {
    GameObject prefab;
    GamePieceType type;

    if (playerId == _networkManager.PlayerId)
    {
      type = _gamePieceManager.ClientTadpoleType;
    }
    else
    {
      type = _gamePieceManager.OpponentTadpoleType;
    }

    prefab = _resourceManager.GetPrefab(type);

    GameTile gameTile = GameTiles[tileX, tileY];

    GameObject tadpole = Instantiate(prefab, gameTile.transform.position, Quaternion.identity);
    PlacedPiece gamePiece = tadpole.AddComponent<PlacedPiece>();
    gamePiece.SetTilePlacement(gameTile, "tadpole");
  }

  /*---------------------------------------------------------
  * Place a frog on the game board
  * @param state - The state of the game piece to place
  ----------------------------------------------------------*/
  private void PlaceFrog(int tileX, int tileY, int playerId)
  {
    GameObject prefab;
    GamePieceType type;

    if (playerId == _networkManager.PlayerId)
    {
      type = _gamePieceManager.ClientFrogType;
    }
    else
    {
      type = _gamePieceManager.OpponentFrogType;
    }

    prefab = _resourceManager.GetPrefab(type);

    GameTile gameTile = GameTiles[tileX, tileY];

    GameObject frog = Instantiate(prefab, gameTile.transform.position, Quaternion.identity);
    PlacedPiece gamePiece = frog.AddComponent<PlacedPiece>();
    gamePiece.SetTilePlacement(gameTile, "frog");
  }

  /*---------------------------------------------------------
  * Create the game board from the server
  ----------------------------------------------------------*/
  private void CreateGameboard(BoardState state)
  {
    Debug.Log("Creating game board");
    float tileSize = 0.65f;
    GameTiles = new GameTile[state.width, state.height];

    float boardWidth = state.width * tileSize;
    float boardHeight = state.height * tileSize;

    float startX = -boardWidth / 2 + tileSize / 2;
    float startY = -boardHeight / 2 + tileSize / 2;

    state.tiles.ForEach((tile) =>
    {
      PositionState position = tile.position;
      GameTile gameTile = Instantiate(_gameTilePrefab, new Vector3(startX + position.x * tileSize, startY + position.y * tileSize, 0), Quaternion.identity).GetComponent<GameTile>();
      gameTile.ArrayPosition = new Vector2Int(position.x, position.y);

      GameTiles[position.x, position.y] = gameTile;
    });

    _networkManager.InitializeTileListener();
  }

  private void HandleChange(BoardState state)
  {
    // Place pieces on the board
    state.tiles.ForEach((tile) =>
    {
      PositionState position = tile.position;
      GameTile gameTile = GameTiles[position.x, position.y];

      if (tile.gamePiece != null && gameTile.CurrentlyHeldPiece == null)
      {
        if (tile.gamePiece.type == "tadpole")
        {
          PlaceTadpole(position.x, position.y, tile.gamePiece.playerId);
        }
        else if (tile.gamePiece.type == "frog")
        {
          PlaceFrog(position.x, position.y, tile.gamePiece.playerId);
        }
      }
      else if (tile.gamePiece == null && gameTile.CurrentlyHeldPiece != null)
      {
        Destroy(gameTile.CurrentlyHeldPiece.gameObject);
      }
    });
  }

  /*------------------------------------------------------------------------------------------
  * Check if the player is currently touching the game board
  * Set property CurrentlyTouchingGameBoard to true if the player is touching the game board
  -------------------------------------------------------------------------------------------*/
  private void IsPlayerTouchingGameBoard()
  {
    if (Input.touchCount > 0)
    {
      Touch touch = Input.GetTouch(0);
      Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
      touchPosition.z = 0;

      if (Physics2D.OverlapPoint(touchPosition) != null)
      {
        CurrentlyTouchingGameBoard = true;
      }
      else
      {
        CurrentlyTouchingGameBoard = false;
      }
    }
  }
}