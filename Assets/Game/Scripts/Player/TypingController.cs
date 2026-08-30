using UnityEngine;
using Photon.Pun;
using TMPro;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TypingController : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] private TMP_InputField tmpTyper;
    [SerializeField] private TextMeshProUGUI chatDisplay;
    [SerializeField] private PlayerController player;
    [SerializeField] private int maxVisibleLines = 5;

    public const byte TypingControllerEventCode = 1;

    private List<string> chatLines = new List<string>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tmpTyper.text = "Press 'T' to type.";
        tmpTyper.onSubmit.AddListener(OnChatSubmit);
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            tmpTyper.gameObject.SetActive(true);
            tmpTyper.text = "";
            tmpTyper.ActivateInputField();
        }
    }

    private void OnChatSubmit(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
            SendChatMessageEvent(message.Trim());

        tmpTyper.text = "Press 'T' to type.";
        tmpTyper.DeactivateInputField();

        player.SetTypingState(false);
    }

    private void SendChatMessageEvent(string message)
    {
        double sendTime = PhotonNetwork.Time;

        object[] content = new object[] { PhotonNetwork.LocalPlayer.NickName, message, sendTime };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(TypingControllerEventCode, content, options, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == TypingControllerEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            string sender = (string)data[0];
            string message = (string)data[1];
            double timestamp = (double)data[2];

            AddMessageToDisplay(sender, message, timestamp);
        }
    }

    private void AddMessageToDisplay(string sender, string message, double photonTime)
    {
        string timeLabel = FormatTimestamp(photonTime);
        string line = $"<color=#AAAAAA>[{timeLabel}]</color> <b>{sender}:</b> {message}";

        chatLines.Add(line);

        if (chatLines.Count > maxVisibleLines)
            chatLines.RemoveAt(0);

        chatDisplay.text = string.Join("\n", chatLines);
    }

    private string FormatTimestamp(double photonTime)
    {
        TimeSpan t = TimeSpan.FromSeconds(photonTime);
        return t.ToString(@"hh\:mm\:ss");
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
