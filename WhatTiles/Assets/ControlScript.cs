using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;

public class ControlScript : MonoBehaviour, RealTimeMultiplayerListener {
    #region multiplayer initialisation
    public bool authenticate = false;
    public GameObject[] playerColor; 
    public GameObject opponent;

    private string _myParticipantId;
    float timer = 0f;
    #endregion

    #region player initialisation
    private float movementSpeed = 3f;

    private float tileSize = 1.732f;

    private enum Orientation { Horizontal, Vertical };

    private Orientation gridOrientation = Orientation.Horizontal;

    private bool moving = false;

    private bool allowDiagonals = false;

    private Vector2 movementInput;

    private Vector3 startPosition;

    private Vector3 endPosition;

    private float t;

    private float factor;

   // public List<HexMap.Node> currentPath = null;



    private Vector3 destinationPos;
    private float destinationDist;
    private Transform myTransform;

    private float moveSpeed;
    public bool isMoving;

    #endregion

    void Start()
    {
        Authenticate();
    }

    void Update()
    {
        if (authenticate)
        {
            #region On Player Touch
            destinationDist = Vector3.Distance(destinationPos, myTransform.position);

            if (destinationDist < .5f)  //prevent shaking behvaior when approaching destination
            {
                moveSpeed = 0;
                isMoving = false;
            }
            else
            {
                moveSpeed = 3;
                isMoving = true;
            }

            if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 targetPoint = ray.GetPoint(hit.distance);
                    destinationPos = ray.GetPoint(hit.distance);
                    Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

                    myTransform.rotation = targetRotation;
                }
            }

            if (destinationDist > .5f)
            {
                myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPos, moveSpeed * Time.deltaTime);
            }
            #endregion

            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }

            else
            {
                timer = 0.035f;
                bool reliability = true;
                string data = "Position:" + myTransform.position.x + ":" + myTransform.position.y + ":" + myTransform.position.z;
                byte[] bytedata = System.Text.ASCIIEncoding.Default.GetBytes(data);
                PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliability, bytedata);
            }

        }
    }

    void setup()
    {
        // getParticipantId
        _myParticipantId = GetMyParticipantId();
        // getListofParticipant
        List<Participant> allPlayers = GetAllPlayers();
        for (int i = 0; i < allPlayers.Count; i++)
        {
            string nextParticipantId = allPlayers[i].ParticipantId;
            Debug.Log("Setting up for " + nextParticipantId);
            // initialising
            Vector3 StartPoint = new Vector3(0f, 4f, 0f);
            if (nextParticipantId == _myParticipantId)
            {
                // if is self
                GameObject self = Instantiate(playerColor[i], StartPoint, Quaternion.identity) as GameObject;
              
                myTransform = self.transform;
                myTransform.position = StartPoint;
            }
            else
            {
                // init opponent
               opponent = (Instantiate(playerColor[i], StartPoint, Quaternion.identity) as GameObject);
         
            }
        }
         authenticate = true;
    }

    #region movement
    private IEnumerator move(Transform transform)
    {
        moving = true;
        startPosition = transform.position;
        t = 0;

        if (gridOrientation == Orientation.Horizontal)
        {
            if (movementInput.x != 0)
            {
                endPosition = new Vector3(
                startPosition.x + System.Math.Sign(movementInput.x) * tileSize,
                startPosition.y,
                startPosition.z);
            }
            if (movementInput.y != 0)
            {
                endPosition = new Vector3(
                startPosition.x + (System.Math.Sign(movementInput.x) * tileSize / 2.0f),
                startPosition.y,
                startPosition.z + System.Math.Sign(movementInput.y) * tileSize);
            }

        }
        else
        {
            endPosition = new Vector3(
                startPosition.x + System.Math.Sign(movementInput.x) * tileSize,
                startPosition.y + System.Math.Sign(movementInput.y) * 2f,
                startPosition.z);
        }

        factor = 1f;

        while (t < 1f)
        {
            t += Time.deltaTime * (movementSpeed / tileSize) * factor;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            yield return null;
        }


        moving = false;
        yield return 0;


    }

    public void clickMove(Vector3 finalPosition)
    {
        startPosition = transform.position;
        transform.Translate(endPosition * Time.deltaTime);

    }

    #endregion

    #region multiplayer
    void Authenticate()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Authentication succeeded");
                CreateQuickGame();
            }

            else
            {
                Debug.Log("Authentication failed");
            }
        });
    }

    void CreateQuickGame()
    {
        const int MinOpponents = 1, MaxOpponents = 1;
        const int GameVariant = 0;
        PlayGamesPlatform.Instance.RealTime.CreateQuickGame(MinOpponents, MaxOpponents,
            GameVariant, this);
    }

    #region RealTimeMultiplayerListener implementation

    private bool isRoomSetuped = false;
    public void OnRoomSetupProgress(float percent)
    {
        if (percent >= 20f)
        {
            isRoomSetuped = true;
            Debug.Log("Currently finding a macth...");
            PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
        }
    }

    private bool connected = false;
    public void OnRoomConnected(bool success)
    {
        if (success)
        {
            //Instantiate your player on the network.
            connected = true;
            authenticate = true;
            setup();
        }

        else
        {
            connected = false;
            CreateQuickGame();
        }
    }

    public void OnLeftRoom()
    {
        connected = false;
        authenticate = false;
    }

    public void OnParticipantLeft(Participant participant)
    {
        connected = false;
        authenticate = false;
    }

    public void OnPeersConnected(string[] participantIds)
    {

    }

    public void OnPeersDisconnected(string[] participantIds)
    {
        connected = false;
        authenticate = false;
    }
    public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
    {
        if (!PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId.Equals(senderId))
        {
            string rawdata = System.Text.ASCIIEncoding.Default.GetString(data);
            string[] sliced = rawdata.Split(new string[] { ":" }, System.StringSplitOptions.RemoveEmptyEntries);

            if (sliced[0].Contains("Position"))
            {
                //Transform target = GameObject.Find(senderId).transform;

                //if (target == null)
                //{
                //    return;
                //}

                Vector3 newpos = new Vector3
                (
                    System.Convert.ToSingle(sliced[1]),
                    System.Convert.ToSingle(sliced[2]),
                    System.Convert.ToSingle(sliced[3])
                );

                opponent.transform.position = newpos;
            }
        }
    }
    #endregion

    #endregion

    #region participant information
    public List<Participant> GetAllPlayers()
    {
        return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
    }

    public string GetMyParticipantId()
    {
        return PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId;
    }
    #endregion
}