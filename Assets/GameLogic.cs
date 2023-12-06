using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    const float CharacterSpeed = 0.25f;
    float DiagonalCharacterSpeed;

    int ballonID = 1;

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
        foreach (int SentToID in GetPlayerIds())
        {
            SendMessageToClient(ServerToClientSignifiers.OnPlayerConnection, IDRecieved + "", SentToID);
        }
    }

    public void OnRecievedInput(int PlayerID,  string[] PosVector, Vector2 InputVector)
    {
        Vector2 PlayerVelocity;

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

        foreach (int SendToID in GetPlayerIds())
        { 
            SendMessageToClient(ServerToClientSignifiers.OnPlayerMovement, PlayerID +
                "," + GetString(PosVector) + "," + GetString(InputVector), SendToID);
        }
    }

    public void OnRecivedCharactherId(int SendToID, int PlayerID, string[] pos, string[] Velocity)
    {
        SendMessageToClient(ServerToClientSignifiers.CreateOldPlayer, PlayerID + "," 
            + GetString(pos) + "," + GetString(Velocity), SendToID);
    }


    public void OnDisconnectionEvent(int PlayerID)
    {
        foreach (int SendToID in GetPlayerIds())
        {
            SendMessageToClient(ServerToClientSignifiers.OnPlayerDisconnection, PlayerID + "", SendToID);
        }

    }

    public void SendMessageToClient(int signifier, string message, int ID)
    {
        NetworkServerProcessing.SendMessageToClient(signifier+ "," +message, ID, TransportPipeline.ReliableAndInOrder);
    }


    //Getters
    public string GetString(Vector2 vector)
    {
        return vector.x+"_"+vector.y;
    }

    public string GetString(string[] array)
    {
        return array[0] + "_" + array[1];
    }

    public List<int> GetPlayerIds()
    {
        return NetworkServerProcessing.GetNetworkServer().GetPlayerIDs();
    }
}

