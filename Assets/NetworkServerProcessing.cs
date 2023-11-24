using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

static public class NetworkServerProcessing
{

    #region Send and Receive Data Functions
    static public void ReceivedMessageFromClient(string msg, int PlayerID, TransportPipeline pipeline)
    {

        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);

        switch (signifier)
        {
            case ClientToServerSignifiers.OnInputChange:
                string[] Input = csv[1].Split('_');
                gameLogic.OnRecievedInput(PlayerID, Input);
                break;
            case ClientToServerSignifiers.SendingMainPlayer:
                Debug.Log("Network msg received =  " + msg + ", from connection id = " + PlayerID + ", from pipeline = " + pipeline);
                int SendToID = int.Parse(csv[1]);
                string[] Porcentage = csv[2].Split('_');
                gameLogic.OnRecivedCharactherId(SendToID, PlayerID, Porcentage);
                break;

        }
    }
    static public void SendMessageToClient(string msg, int clientConnectionID, TransportPipeline pipeline)
    {
        networkServer.SendMessageToClient(msg, clientConnectionID, pipeline);
    }

    static public List<int> GetAllIDs()
    {
        return networkServer.GetAllIDs();
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

static public class ClientToServerInputs
{
    public const int negative = -1;
    public const int none = 0;
    public const int positive = 1;
}

#endregion

