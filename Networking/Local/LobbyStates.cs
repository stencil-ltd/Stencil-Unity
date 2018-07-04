using Plugins.State;

namespace Plugins.Networking.Local
{
    public enum LobbyState
    {
        None, Server, Client, ClientReady, Playing
    }

    public class LobbyStates : StateVisibility<LobbyState>
    {}
}