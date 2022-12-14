using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour

{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;

    [SyncVar(hook =nameof(HandleDisplayNameUpdated))]
    [SerializeField]
    private string displayName = "Missing Name";

    [SyncVar(hook= nameof(HandleDisplayColorUpdated))]
    [SerializeField]
    private Color displayColor = Color.black;

    #region Server

    [Server]
    public void SetDisplayName (string newDisplayName)
    {
        displayName = newDisplayName;
    }
    [Server]
    public void SetDisplayColor (Color newDisplayColor)
    {
        displayColor = newDisplayColor;
    }

    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        if (newDisplayName.Length < 5)
        {
            Debug.Log("Display name is not valid.");
            return;
        }
        RpcLogNewName(newDisplayName);
        SetDisplayName(newDisplayName);
    }

    #endregion

    #region Client
    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.color = newColor;
    }

    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        displayNameText.text = newName;
    }

    [ContextMenu("Set My Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("toot");
    }

    [ClientRpc]
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }

    #endregion
}
