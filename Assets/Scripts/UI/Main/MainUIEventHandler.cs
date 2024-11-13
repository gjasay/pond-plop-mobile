using UnityEngine;
using UnityEngine.UIElements;

public class MainUIEventHandler : MonoBehaviour
{
    public bool IsDragging { get; set; }
    
    private UIDocument _document;
    private VisualElement _root;
    private Image _tadpole;
    private Image _frog;
    private Label _numOfTadpoles;
    private Label _numOfFrogs;
    private ResourceManager _resourceManager;
    private GamePieceManager _gamePieceManager;
    private NetworkManager _networkManager;
    private GameObject _prefab;

    private void Start()
    {
        _resourceManager = ResourceManager.Instance;
        _gamePieceManager = GamePieceManager.Instance;
        _networkManager = NetworkManager.Instance;

        _networkManager.OnHandChanged += UpdateHandCount;
    }

    private void OnEnable()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;

        _tadpole = _root.Q<Image>("tadpole-image");
        _frog = _root.Q<Image>("frog-image");
        _numOfTadpoles = _root.Q<Label>("tadpole-count");
        _numOfFrogs = _root.Q<Label>("frog-count");

        //Will using click events cause issues for mobile??
        _tadpole.RegisterCallback((MouseEnterEvent evt, Image root) => HandleTadpoleClick(evt, root), _tadpole);
        _frog.RegisterCallback((MouseEnterEvent evt, Image root) => HandleFrogClick(evt, root), _frog);
    }

    public void SetRoomCode(string code)
    {
        var title = _root.Q<Label>("title");
        title.text += $"\nRoom Code: {code}";
    }

    public void SetUIGamePieces()
    {
        _tadpole.sprite = _resourceManager.GetSprite(_gamePieceManager.ClientTadpoleType);
        _frog.sprite = _resourceManager.GetSprite(_gamePieceManager.ClientFrogType);
    }

    // check if the player has greater than 0 of the piece
    public bool CanPlacePiece(string type)
    {
        if (type == "tadpole")
        {
            if (_numOfTadpoles.text == "0") return false;
        }
        else if (type == "frog")
        {
            if (_numOfFrogs.text == "0") return false;
        }

        return true;
    }

    private void UpdateHandCount(HandState hand)
    {
        _numOfTadpoles.text = hand.tadpoles.ToString();
        _numOfFrogs.text = hand.frogs.ToString();
    }

    private void HandleTadpoleClick(MouseEnterEvent evt, Image root)
    {
        if (!_networkManager.IsPlayerTurn() || IsDragging) return;
        Debug.Log("Tadpole clicked");
        _prefab = _resourceManager.GetPrefab(_gamePieceManager.ClientTadpoleType);
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject gamePiece = Instantiate(_prefab, position, Quaternion.identity);
        gamePiece.AddComponent<DragHandler>();

        gamePiece.GetComponent<DragHandler>().TypeOfPiece = _gamePieceManager.ClientTadpoleType;
        gamePiece.GetComponent<DragHandler>().PieceType = "tadpole";
        IsDragging = true;
        root.sprite = null;
    }

    private void HandleFrogClick(MouseEnterEvent evt, Image root)
    {
        if (!_networkManager.IsPlayerTurn() || IsDragging) return;
        Debug.Log("Frog clicked");
        _prefab = _resourceManager.GetPrefab(_gamePieceManager.ClientFrogType);
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject gamePiece = Instantiate(_prefab, position, Quaternion.identity);
        gamePiece.AddComponent<DragHandler>();

        gamePiece.GetComponent<DragHandler>().TypeOfPiece = _gamePieceManager.ClientFrogType;
        gamePiece.GetComponent<DragHandler>().PieceType = "frog";
        IsDragging = true;
        root.sprite = null;
    }
}