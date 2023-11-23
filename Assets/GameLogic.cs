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

    public void OnRecievedInput(int IDRecieved, string[] input)
    {
        foreach(int IDtoSend in NetworkServerProcessing.GetAllIDs())
        {
            SendMessageToClient(ServerToClientSignifiers.OnPlayerMovement, IDRecieved + "," + input[0] + "_" + input[1], IDtoSend);
        }
    }

    public void OnNewPlayer(int IDRecieved, string[] Porcentage)
    {
        foreach (int IDtoSend in NetworkServerProcessing.GetAllIDs())
        {
            if(IDRecieved!=IDtoSend)
            {
                SendMessageToClient(ServerToClientSignifiers.OnPlayerMovement, IDRecieved + "," + Porcentage[0] + "_" + Porcentage[1], IDtoSend);
            }
        }
    }

    public void OnConnectionEvent(int IDRecieved)
    {
        foreach (int IDtoSend in NetworkServerProcessing.GetAllIDs())
        {
            SendMessageToClient(ServerToClientSignifiers.OnPlayerConnection, IDRecieved+"", IDtoSend);
        }
    }

    public void OnDisconnectionEvent(int IDRecieved)
    {
        foreach (int IDtoSend in NetworkServerProcessing.GetAllIDs())
        {
            SendMessageToClient(ServerToClientSignifiers.OnPlayerDisconnection, IDRecieved + "", IDtoSend);
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
    public const int SetAdditionalPlayers = 2;



}

#endregion
