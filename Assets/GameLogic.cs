using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    int ballonID=1;
    void Start()
    {
        NetworkServerProcessing.SetGameLogic(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnConnectionEvent(ballonID);
            ballonID++;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ballonID--;
            OnDisconnectionEvent(ballonID);
        }
    }

    public void OnConnectionEvent(int IDRecieved)
    {

        foreach (int SendToID in NetworkServerProcessing.GetAllIDs())
        {
            SendMessageToClient(ServerToClientSignifiers.OnPlayerConnection, IDRecieved + "", SendToID);

        }
    }
    public void OnRecievedInput(int PlayerID, string[] input)
    {
        foreach (int SendToID in NetworkServerProcessing.GetAllIDs())
        {
            SendMessageToClient(ServerToClientSignifiers.OnPlayerMovement, PlayerID + "," + input[0] + "_" + input[1], SendToID);
        }
    }

    public void OnRecivedCharactherId(int SendToID, int PlayerID, string[] porcentage)
    {
        SendMessageToClient(ServerToClientSignifiers.CreateOldPlayer, PlayerID + "," + porcentage[0] + "_" + porcentage[1], SendToID);
    }

    public void OnDisconnectionEvent(int PlayerID)
    {
        foreach (int SendToID in NetworkServerProcessing.GetAllIDs())
        {
            SendMessageToClient(ServerToClientSignifiers.OnPlayerDisconnection, PlayerID + "", SendToID);
        }
    }

    public void SendMessageToClient(int signifier, string message, int ID)
    {
        NetworkServerProcessing.SendMessageToClient(signifier+ "," +message, ID, TransportPipeline.ReliableAndInOrder);
    }

}

#region Protocol Signifiers

static public class ServerToClientSignifiers
{
    public const int OnPlayerDisconnection = -1;
    public const int OnPlayerConnection = 0;
    public const int OnPlayerMovement = 1;
    public const int CreateOldPlayer = 2;




}

#endregion
