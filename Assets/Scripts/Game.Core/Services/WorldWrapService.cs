using UnityEngine;

namespace Game.Core
{
public class WorldWrapService
{
    public void WrapPosition(ShipModel shipModel, WorldConfig worldConfig)
    {
        float halfWidth = worldConfig.Width * 0.5f;
        float halfHeight = worldConfig.Height * 0.5f;

        Vector2 position = shipModel.Position;

        if (position.x > halfWidth)
        {
            position.x = -halfWidth;
        }
        else if (position.x < -halfWidth)
        {
            position.x = halfWidth;
        }

        if (position.y > halfHeight)
        {
            position.y = -halfHeight;
        }
        else if (position.y < -halfHeight)
        {
            position.y = halfHeight;
        }

        shipModel.Position = position;
    }

    public void WrapPosition(EnemyModel enemyModel, WorldConfig worldConfig)
    {
        float halfWidth = worldConfig.Width * 0.5f;
        float halfHeight = worldConfig.Height * 0.5f;

        Vector2 position = enemyModel.Position;

        if (position.x > halfWidth)
        {
            position.x = -halfWidth;
        }
        else if (position.x < -halfWidth)
        {
            position.x = halfWidth;
        }

        if (position.y > halfHeight)
        {
            position.y = -halfHeight;
        }
        else if (position.y < -halfHeight)
        {
            position.y = halfHeight;
        }

        enemyModel.Position = position;
    }
}
}
