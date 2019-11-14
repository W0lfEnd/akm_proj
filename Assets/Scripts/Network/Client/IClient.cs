using Model;


public interface IClient
{
    GameModel Model { get; }
    Map Map { get; }
    int Id { get; }

    void Send(PlayerInput playerInput);
}
