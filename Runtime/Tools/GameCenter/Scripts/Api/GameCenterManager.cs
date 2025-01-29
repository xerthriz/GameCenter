using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Apple.GameKit;

public class AppleGameCenterManager: Singleton<AppleGameCenterManager>
{
    private string Signature;
    private string TeamPlayerID;
    private string Salt;
    private string PublicKeyUrl;
    private string Timestamp;

    public IEnumerator  Start()
    {
        yield return new WaitForSeconds(1); 
        Login();
    }

    public async Task Login()
    {
        Debug.Log("USERAUTENTICATED: "+!GKLocalPlayer.Local.IsAuthenticated);
        if (!GKLocalPlayer.Local.IsAuthenticated)
        {
            // Perform the authentication.
            var player = await GKLocalPlayer.Authenticate();
            Debug.Log($"GameKit Authentication: player {player}");

            // Grab the display name.
            var localPlayer = GKLocalPlayer.Local;
            Debug.Log($"Local Player: {localPlayer.DisplayName}");

            // Fetch the items.
            var fetchItemsResponse =  await GKLocalPlayer.Local.FetchItems();

            Signature = Convert.ToBase64String(fetchItemsResponse.GetSignature());
            TeamPlayerID = localPlayer.TeamPlayerId;
            Debug.Log($"Team Player ID: {TeamPlayerID}");

            Salt = Convert.ToBase64String(fetchItemsResponse.GetSalt());
            PublicKeyUrl = fetchItemsResponse.PublicKeyUrl;
            Timestamp = fetchItemsResponse.Timestamp.ToString();

            Debug.Log($"GameKit Authentication: signature => {Signature}");
            Debug.Log($"GameKit Authentication: publickeyurl => {PublicKeyUrl}");
            Debug.Log($"GameKit Authentication: salt => {Salt}");
            Debug.Log($"GameKit Authentication: Timestamp => {Timestamp}");
        }
        else
        {
            Debug.Log("AppleGameCenter player already logged in.");
        }
    }
}