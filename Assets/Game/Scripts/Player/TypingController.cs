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
    public const byte TypingControllerEventCode = 1;

    private TMP_InputField tmpInput;
    private TextMeshProUGUI chatDisplay;
    private PlayerController player;

    [SerializeField] private int maxVisibleLines = 5;

    private List<string> chatLines = new List<string>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Init(TMP_InputField input, TextMeshProUGUI chat, PlayerController player)
    {
        tmpInput = input;
        chatDisplay = chat;
        this.player = player;

        tmpInput.text = "Press '1' to type.";
        tmpInput.onSubmit.AddListener(OnChatSubmit);
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            tmpInput.gameObject.SetActive(true);
            tmpInput.text = "";
            tmpInput.ActivateInputField();
        }
    }

    private void OnChatSubmit(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
            SendChatMessageEvent(message.Trim());

        tmpInput.text = "Press '1' to type.";
        tmpInput.DeactivateInputField();

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

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
