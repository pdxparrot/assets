﻿#pragma warning disable 0618    // disable obsolete warning for now

using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;

using UnityEngine;

#if USE_NETWORKING
using Unity.Netcode;
#endif

namespace pdxpartyparrot.Core.Network
{
#if USE_NETWORKING
    // https://github.com/Unity-Technologies/multiplayer-community-contributions/tree/main/com.community.netcode.extensions has a replacement for this HUD
    //[RequireComponent(typeof(NetworkManagerHUD))]
    [RequireComponent(typeof(NetworkDiscovery))]
    [RequireComponent(typeof(Unity.Netcode.NetworkManager))]
#else
#endif
    public sealed class NetworkManager : SingletonBehavior<NetworkManager>
    {
        #region Events

#if USE_NETWORKING
        public event EventHandler<EventArgs> ServerStartEvent;
        public event EventHandler<EventArgs> ServerStopEvent;
        public event EventHandler<EventArgs> ServerConnectEvent;
        public event EventHandler<EventArgs> ServerDisconnectEvent;
#endif

        public event EventHandler<ApprovalCheckSuccessEventArgs> ApprovalCheckSuccessEvent;
        public event EventHandler<EventArgs> ServerChangeSceneEvent;
        public event EventHandler<EventArgs> ServerChangedSceneEvent;

#if USE_NETWORKING
        public event EventHandler<EventArgs> ClientConnectEvent;
        public event EventHandler<EventArgs> ClientDisconnectEvent;
        public event EventHandler<ClientSceneEventArgs> ClientSceneChangeEvent;
        public event EventHandler<ClientSceneEventArgs> ClientSceneChangedEvent;
#endif

        #endregion

        #region Messages

#if USE_NETWORKING
        public class CustomMsgType
        {
            public const short SceneChange = MsgType.Highest + 1;
            public const short SceneChanged = MsgType.Highest + 2;

            // NOTE: always last, always highest
            public const short Highest = MsgType.Highest + 3;
        }
#endif

        #endregion

        [SerializeField]
        private bool _enableCallbackLogging = true;

#if USE_NETWORKING
        public Unity.Netcode.NetworkManager Manager { get; private set; }

        //private NetworkManagerHUD _hud;

        public NetworkDiscovery Discovery { get; private set; }
#else
        [SerializeField]
        [ReadOnly]
        private GameObject m_PlayerPrefab;

        public GameObject playerPrefab
        {
            get => m_PlayerPrefab;
            set => m_PlayerPrefab = value;
        }
#endif

        #region Unity Lifecycle

        private void Awake()
        {
            Initialize();
        }

        #endregion

        private void Initialize()
        {
#if USE_NETWORKING
            Manager = GetComponent<Unity.Netcode.NetworkManager>();
            Manager.ConnectionApprovalCallback += ApprovalCheckEventHandler;

            /*_hud = GetComponent<NetworkManagerHUD>();
            _hud.showGUI = false;*/

            Discovery = GetComponent<NetworkDiscovery>();
            Discovery.useNetworkManager = true;
            Discovery.showGUI = false;
            Discovery.enabled = PartyParrotManager.Instance.Config.Network.Discovery.Enable;
#endif

            InitDebugMenu();
        }

        public bool IsServerActive()
        {
#if USE_NETWORKING
            return NetworkServer.active;
#else
            return true;
#endif
        }

        public bool IsClientActive()
        {
#if USE_NETWORKING
            return NetworkClient.active;
#else
            return true;
#endif
        }

        #region Network Prefabs

        public void RegisterNetworkPrefab<T>(T networkPrefab) where T : NetworkBehaviour
        {
            Debug.Log($"[NetworkManager]: Registering network prefab '{networkPrefab.name}'");
#if USE_NETWORKING
            ClientScene.RegisterPrefab(networkPrefab.gameObject);
#else
            Debug.LogWarning($"[NetworkManager]: Not registering network prefab {networkPrefab.name}");
#endif
        }

        public void UnregisterNetworkPrefab<T>(T networkPrefab) where T : NetworkBehaviour
        {
            Debug.Log($"[NetworkManager]: Unregistering network prefab '{networkPrefab.name}'");
#if USE_NETWORKING
            ClientScene.UnregisterPrefab(networkPrefab.gameObject);
#else
            Debug.LogWarning($"[NetworkManager]: Not unregistering network prefab {networkPrefab.name}");
#endif
        }

        [CanBeNull]
        public T SpawnNetworkPrefab<T>(T networkPrefab) where T : NetworkBehaviour
        {
            if(!IsServerActive()) {
                Debug.LogWarning("[NetworkManager]: Cannot spawn network prefab without an active server!");
                return null;
            }

            Debug.Log($"[NetworkManager]: Spawning network prefab '{networkPrefab.name}'");

            T obj = Instantiate(networkPrefab);
            if(null == obj) {
                return null;
            }

            SpawnNetworkObject(obj);
            return obj;
        }

        [CanBeNull]
        public T SpawnNetworkPrefab<T>(T networkPrefab, Transform parent) where T : NetworkBehaviour
        {
            T obj = SpawnNetworkPrefab(networkPrefab);
            if(null == obj) {
                return null;
            }
            obj.transform.SetParent(parent, true);
            return obj;
        }

        public void SpawnNetworkObject<T>([NotNull] T networkObject) where T : NetworkBehaviour
        {
#if USE_NETWORKING
            networkObject.NetworkObject.Spawn();
#else
            Debug.LogWarning($"[NetworkManager]: Not spawning network object {networkObject.name}");
#endif
        }

        public void DeSpawnNetworkObject<T>([NotNull] T networkObject) where T : NetworkBehaviour
        {
#if USE_NETWORKING
            NetworkServer.UnSpawn(networkObject.gameObject);
#else
            Debug.LogWarning($"[NetworkManager]: Not despawning network object {networkObject.name}");
#endif
        }

        public void DestroyNetworkObject<T>([CanBeNull] T networkObject) where T : NetworkBehaviour
        {
            if(null == networkObject) {
                return;
            }

            if(!IsServerActive()) {
                Debug.LogWarning("[NetworkManager]: Cannot destroy network object without an active server!");
                return;
            }

#if USE_NETWORKING
            Debug.Log($"[NetworkManager]: Destroying network object '{networkObject.name}'");
            NetworkServer.Destroy(networkObject.gameObject);
#else
            Debug.LogWarning($"[NetworkManager]: Not destroying network object {networkObject.name}");
#endif
        }

        #endregion

        #region Player Prefab

        public void RegisterPlayerPrefab<T>(T prefab) where T : NetworkActor
        {
            Debug.Log($"[NetworkManager]: Registering player prefab '{prefab.name}'");

            // TODO: warn if already set?
            playerPrefab = prefab.gameObject;
#if USE_NETWORKING
            RegisterNetworkPrefab(prefab);
#endif
        }

        public void UnregisterPlayerPrefab()
        {
            if(null == playerPrefab) {
                Debug.LogWarning("[NetworkManager]: No player prefab to unregister!");
                return;
            }

            Debug.Log($"[NetworkManager]: Unregistering player prefab '{playerPrefab.name}'");

#if USE_NETWORKING
            UnregisterNetworkPrefab(playerPrefab.GetComponent<NetworkBehaviour>());
#endif

            playerPrefab = null;
        }

        public T SpawnPlayer<T>(ulong clientId) where T : NetworkActor
        {
            if(!IsServerActive()) {
                Debug.LogWarning("[NetworkManager]: Cannot spawn player prefab without an active server!");
                return null;
            }

            if(null == playerPrefab) {
                Debug.LogWarning("[NetworkManager]: Player prefab not registered!");
                return null;
            }

            GameObject player = Instantiate(playerPrefab);
            if(null == player) {
                Debug.LogError("Failed to spawn player!");
                return null;
            }
            player.name = $"Player {clientId}";

            // call this instead of NetworkServer.Spawn()
#if USE_NETWORKING
            NetworkServer.AddPlayerForConnection(conn, player.gameObject, controllerId);
#endif
            return player.GetComponent<T>();
        }

        public T SpawnPlayer<T>(ulong clientId, Transform parent) where T : NetworkActor
        {
            T player = SpawnPlayer<T>(clientId);
            if(null == player) {
                return null;
            }
            player.transform.SetParent(parent, true);
            return player;
        }

        public void DestroyPlayer(NetworkConnection conn)
        {
            if(!IsServerActive()) {
                Debug.LogWarning("Cannot despawn players without an active server!");
                return;
            }

#if USE_NETWORKING
            NetworkServer.DestroyPlayersForConnection(conn);
#endif
        }

        #endregion

        #region Discovery

#if USE_NETWORKING
        private bool InitDiscovery()
        {
            Discovery.broadcastPort = PartyParrotManager.Instance.Config.Network.Discovery.Port;
            return Discovery.Initialize();
        }

        public bool DiscoverServer()
        {
            if(!Discovery.enabled) {
                return true;
            }

            Debug.Log("[NetworkManager]: Starting server discovery");

            if(!InitDiscovery()) {
                return false;
            }

            return Discovery.StartAsServer();
        }

        public bool DiscoverClient()
        {
            if(!Discovery.enabled) {
                return true;
            }

            Debug.Log("[NetworkManager]: Starting client discovery");

            if(!InitDiscovery()) {
                return false;
            }

            return Discovery.StartAsClient();
        }

        public void DiscoverStop()
        {
            if(Discovery.running) {
                Debug.Log("[NetworkManager]: Stopping discovery");
                Discovery.StopBroadcast();
            }

            // TODO: see if this stops the "host id out of bound" error
            // on the client without breaking anything else
            /*if(NetworkTransport.IsBroadcastDiscoveryRunning()) {
                Debug.Log("[NetworkManager]: Removing broadcast host");
                NetworkTransport.StopBroadcastDiscovery();
                NetworkTransport.RemoveHost(0);
            }*/
        }
#endif

        #endregion

#if USE_NETWORKING
        public override NetworkClient StartHost()
        {
            Debug.Log("[NetworkManager]: Starting LAN host");

            maxConnections = PartyParrotManager.Instance.Config.Network.Server.MaxConnections;
            NetworkClient networkClient = Manager.StartHost();
            if(null == networkClient) {
                return null;
            }

            InitClient(networkClient);
            return networkClient;
        }

        public new bool StartServer()
        {
            maxConnections = PartyParrotManager.Instance.Config.Network.Server.MaxConnections;
            networkAddress = PartyParrotManager.Instance.Config.Network.Server.NetworkAddress;
            networkPort = PartyParrotManager.Instance.Config.Network.Server.Port;

            if(PartyParrotManager.Instance.Config.Network.Server.BindIp()) {
                serverBindAddress = PartyParrotManager.Instance.Config.Network.Server.NetworkAddress;
                serverBindToIP = true;

                Debug.Log($"[NetworkManager]: Binding to address {serverBindAddress}");
            }

            Debug.Log($"[NetworkManager]: Listening for clients on {networkAddress}:{networkPort}");
            return Manager.StartServer();
        }

        public new NetworkClient StartClient()
        {
            Debug.Log($"[NetworkManager]: Connecting client to {networkAddress}:{networkPort}");
            NetworkClient networkClient = Manager.StartClient();
            if(null == networkClient) {
                return null;
            }

            InitClient(networkClient);
            return networkClient;
        }

        private void InitClient(NetworkClient networkClient)
        {
            networkClient.RegisterHandler(CustomMsgType.SceneChange, OnClientCustomSceneChange);
            networkClient.RegisterHandler(CustomMsgType.SceneChanged, OnClientCustomSceneChanged);
        }
#else
        public NetworkClient StartHost()
        {
            return new NetworkClient();
        }

        public void StopHost()
        {
        }

        public bool StartServer()
        {
            return true;
        }

        public void StopServer()
        {
        }

        public NetworkClient StartClient()
        {
            return new NetworkClient();
        }

        public void StopClient()
        {
        }
#endif

        public void Stop()
        {
            if(IsServerActive() && IsClientActive()) {
                StopHost();
            } else if(IsServerActive()) {
                StopServer();
            } else if(IsClientActive()) {
                StopClient();
            }
        }

        public void LocalClientReady(NetworkConnection conn)
        {
#if USE_NETWORKING
            if(null == conn || conn.isReady) {
                return;
            }

            Debug.Log($"[NetworkManager]: Local client ready!");

            ClientScene.Ready(conn);
#endif
        }

        public void AddLocalPlayer(ulong clientId)
        {
            Debug.Log($"[NetworkManager]: Adding local player {clientId}!");

#if USE_NETWORKING
            ClientScene.AddPlayer(clientId);
#else
            ApprovalCheckSuccessEvent?.Invoke(this, new ApprovalCheckSuccessEventArgs(clientId));
#endif
        }

        public void ServerChangeScene(string sceneName)
        {
#if USE_NETWORKING
            Debug.Log($"[NetworkManager]: Server changing to scene '{sceneName}'...");

            NetworkServer.SetAllClientsNotReady();
            networkSceneName = sceneName;
#endif

            ServerChangeSceneEvent?.Invoke(this, EventArgs.Empty);

#if USE_NETWORKING
            StringMessage msg = new StringMessage(networkSceneName);
            NetworkServer.SendToAll(CustomMsgType.SceneChange, msg);

            // TDOO: call NetworkSceneManager.LoadScene() ?
#endif
        }

        public void ServerChangedScene()
        {
            if(!IsServerActive()) {
                return;
            }

            ServerChangedSceneEvent?.Invoke(this, EventArgs.Empty);

#if USE_NETWORKING
            NetworkServer.SpawnObjects();
            OnServerSceneChanged(networkSceneName);
#endif
        }

#if USE_NETWORKING

        #region Server Event Handlers

        private void ApprovalCheckEventHandler(byte[] connectionData, ulong clientId, Unity.Netcode.NetworkManager.ConnectionApprovedDelegate callback)
        {
            CallbackLog($"ApprovalCheck({clientId})");

            // TODO: validate the connection data

            bool createPlayerObject = false;
            bool approve = true;
            callback(createPlayerObject, null, approve, null, null);

            ApprovalCheckSuccessEvent?.Invoke(this, new ApprovalCheckSuccessEventArgs(clientId));
        }

        #endregion


        #region Server Callbacks

        public override void OnStartHost()
        {
            CallbackLog("OnStartHost()");

            Manager.OnStartHost();
        }

        public override void OnStopHost()
        {
            CallbackLog("OnStopHost()");

            Manager.OnStopHost();
        }

        public override void OnStartServer()
        {
            CallbackLog("OnStartServer()");

            Manager.OnStartServer();

            ServerStartEvent?.Invoke(this, EventArgs.Empty);
        }

        public override void OnStopServer()
        {
            CallbackLog("OnStopServer()");

            ServerStopEvent?.Invoke(this, EventArgs.Empty);

            Manager.OnStopServer();
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            CallbackLog($"OnServerConnect({conn})");

            Manager.OnServerConnect(conn);

            ServerConnectEvent?.Invoke(this, EventArgs.Empty);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            CallbackLog($"OnServerDisconnect({conn})");

            ServerDisconnectEvent?.Invoke(this, EventArgs.Empty);

            Manager.OnServerDisconnect(conn);
        }

        public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
        {
            CallbackLog($"OnServerRemovePlayer({conn}, {player})");

            Manager.OnServerRemovePlayer(conn, player);
        }

        public override void OnServerReady(NetworkConnection conn)
        {
            CallbackLog($"OnServerReady({conn})");

            Manager.OnServerReady(conn);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            CallbackLog($"OnServerSceneChanged({sceneName})");

            StringMessage msg = new StringMessage(networkSceneName);
            NetworkServer.SendToAll(CustomMsgType.SceneChanged, msg);
        }

        #endregion

        #region Client Callbacks

        public override void OnStartClient(NetworkClient networkClient)
        {
            CallbackLog($"OnStartClient({networkClient})");

            Manager.OnStartClient(networkClient);
        }

        public override void OnStopClient()
        {
            CallbackLog("OnStopClient()");

            Manager.OnStopClient();
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            CallbackLog($"OnClientConnect({conn})");

            ClientConnectEvent?.Invoke(this, EventArgs.Empty);

            // NOTE: do not call the base method
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            CallbackLog($"OnClientDisconnect({conn})");

            Manager.OnClientDisconnect(conn);

            ClientDisconnectEvent?.Invoke(this, EventArgs.Empty);
        }

        public override void OnClientSceneChanged(NetworkConnection conn)
        {
            CallbackLog($"OnClientSceneChanged({conn})");

            Manager.OnClientSceneChanged(conn);
        }

        public void OnClientCustomSceneChange(NetworkMessage netMsg)
        {
            CallbackLog($"OnClientCustomSceneChange({netMsg})");

            string sceneName = netMsg.reader.ReadString();
            ClientSceneChangeEvent?.Invoke(this, new ClientSceneEventArgs(sceneName));
        }

        public void OnClientCustomSceneChanged(NetworkMessage netMsg)
        {
            CallbackLog($"OnClientCustomSceneChanged({netMsg})");

            string sceneName = netMsg.reader.ReadString();
            ClientSceneChangedEvent?.Invoke(this, new ClientSceneEventArgs(sceneName));
        }

        #endregion

#endif

        private void CallbackLog(string message)
        {
            if(!_enableCallbackLogging) {
                return;
            }
            Debug.Log($"[NetworkManager]: {message}");
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Core.NetworkManager");
            debugMenuNode.RenderContentsAction = () => {
#if USE_NETWORKING
                /*if(_hud.enabled) {
                    _hud.showGUI = GUILayout.Toggle(_hud.showGUI, "Show Network HUD GUI");
                }*/

                if(Discovery.enabled) {
                    Discovery.showGUI = GUILayout.Toggle(Discovery.showGUI, "Show Network Discovery GUI");
                }
#endif

                _enableCallbackLogging = GUILayout.Toggle(_enableCallbackLogging, "Callback Logging");
            };
        }
    }
}
