using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

static public class NetworkServerProcessing
{

    #region Send and Receive Data Functions
    static public void ReceivedMessageFromClient(string msg, int PlayerID, TransportPipeline pipeline)
    {
        Debug.Log("Network msg received =  " + msg + ", from connection id = " + PlayerID + ", from pipeline = " + pipeline);

        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);

        switch (signifier)
        {
            case ClientToServerSignifiers.OnInputChange:
                string[] Pos = csv[1].Split('_');
                string[] input = csv[2].Split('_');

                Vector2 InputVector = new Vector2(float.Parse(input[0]), float.Parse(input[1]));

                gameLogic.OnRecievedInput(PlayerID, Pos, InputVector);

                break;
            case ClientToServerSignifiers.SendingMainPlayer:
                int SendToID = int.Parse(csv[1]);
                string[] pos = csv[2].Split('_');
                string[] velocity = csv[3].Split('_');

                gameLogic.OnRecivedCharactherId(SendToID, PlayerID, pos, velocity);
                break;

        }
    }
    static public void SendMessageToClient(string msg, int clientConnectionID, TransportPipeline pipeline)
    {
        networkServer.SendMessageToClient(msg, clientConnectionID, pipeline);
    }



    #endregion


    #region Connection Events

    static public void ConnectionEvent(int clientConnectionID)
    {
        Debug.Log("Client connection, ID == " + clientConnectionID);
        gameLogic.OnConnectionEvent(clientConnectionID);
    }
    static public void DisconnectionEvent(int clientConnectionID)
    {
        Debug.Log("Client disconnection, ID == " + clientConnectionID);
        gameLogic.OnDisconnectionEvent(clientConnectionID);

    }

 

    #endregion

    #region Setup
    static NetworkServer networkServer;
    static GameLogic gameLogic;

    static public void SetNetworkServer(NetworkServer NetworkServer)
    {
        networkServer = NetworkServer;
    }
    static public NetworkServer GetNetworkServer()
    {
        return networkServer;
    }
    static public void SetGameLogic(GameLogic GameLogic)
    {
        gameLogic = GameLogic;
    }

    #endregion
}

#region Protocol Signifiers

static public class ClientToServerSignifiers
{
    public const int OnInputChange = 1;
    public const int SendingMainPlayer = 2;
}

static public class ServerToClientSignifiers
{
    public const int OnPlayerDisconnection = -1;
    public const int OnPlayerConnection = 0;
    public const int OnPlayerMovement = 1;
    public const int CreateOldPlayer = 2;
}
#endregion

