using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    int ballonID=1;

    const float CharacterSpeed = 0.25f;
    float DiagonalCharacterSpeed;

    void Start()
    {
        NetworkServerProcessing.SetGameLogic(this);
        DiagonalCharacterSpeed = Mathf.Sqrt(CharacterSpeed * CharacterSpeed + CharacterSpeed * CharacterSpeed) / 2f;

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

        foreach (int SendToID in NetworkServerProcessing.GetAllConnectedIDs())
        {
            SendMessageToClient(ServerToClientSignifiers.OnPlayerConnection, IDRecieved + "", SendToID);

        }
    }
    public void OnRecievedInput(int PlayerID, Vector2 InputVector)
    {
        Vector2 PlayerVelocity = Vector2.zero;
        if (Mathf.Abs(InputVector.x * InputVector.y)==1)
        {
            PlayerVelocity.x = DiagonalCharacterSpeed * InputVector.x;
            PlayerVelocity.y = DiagonalCharacterSpeed * InputVector.y;
        }
        else
        {
            PlayerVelocity.x = CharacterSpeed * InputVector.x;
            PlayerVelocity.y = CharacterSpeed * InputVector.y;
        }

        foreach (int SendToID in NetworkServerProcessing.GetAllConnectedIDs())
        {
            SendMessageToClient(ServerToClientSignifiers.OnPlayerMovement, PlayerID + "," + PlayerVelocity.x + "_" + PlayerVelocity.y, SendToID);
        }
    }

    public void OnRecivedCharactherId(int SendToID, int PlayerID, string[] porcentage)
    {
        SendMessageToClient(ServerToClientSignifiers.CreateOldPlayer, PlayerID + "," + porcentage[0] + "_" + porcentage[1], SendToID);
    }

    public void OnDisconnectionEvent(int PlayerID)
    {
        foreach (int SendToID in NetworkServerProcessing.GetAllConnectedIDs())
        {
            SendMessageToClient(ServerToClientSignifiers.OnPlayerDisconnection, PlayerID + "", SendToID);
        }
    }

    public void SendMessageToClient(int signifier, string message, int ID)
    {
        NetworkServerProcessing.SendMessageToClient(signifier+ "," +message, ID, TransportPipeline.ReliableAndInOrder);
    }

}

