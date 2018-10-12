using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using TMPro;
public class ClientLogin : MonoBehaviour {
    [SerializeField] private Button m_LoginButton;
    [SerializeField] private TMP_InputField m_InputField;

    [SerializeField] private MMONetworkManager m_MMOManager;

    public String Username;

    void Awake() {
        m_MMOManager = GameObject.Find("NetworkManager").GetComponent<MMONetworkManager>();
        m_LoginButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick() {
        Username = m_InputField.text;
        m_MMOManager.username = Username;
        m_MMOManager.StartClient();
    }

    void Login(string username) {
        Username = username;
        m_MMOManager.StartClient();
    }
}