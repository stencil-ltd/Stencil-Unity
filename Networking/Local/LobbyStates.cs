using Plugins.State;

namespace Plugins.Networking.Local
{
    public enum LobbyState
    {
        None, Server, Client, Playing
    }

    public class LobbyStates : StateVisibility<LobbyState>
    {}
}