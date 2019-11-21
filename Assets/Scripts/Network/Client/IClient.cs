using Model;


public interface IClient
{
    GameModel Model { get; }
    Map Map { get; }
    long Id { get; }

    void Send(PlayerInput playerInput);
}
