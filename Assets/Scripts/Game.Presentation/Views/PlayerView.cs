using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private PlayerService _playerService;

    public void Initialize(PlayerService playerService)
    {
        _playerService = playerService;
    }

    private void Update()
    {
        if (_playerService == null)
        {
            return;
        }

        Vector2 position = _playerService.ShipModel.Position;
        transform.position = new Vector3(position.x, position.y, 0f);

        transform.rotation = Quaternion.Euler(0f, 0f, _playerService.ShipModel.Rotation);
    }
}