using UnityEngine;

public interface IEnemyAI
{
    public void SetPlayer(GameObject newPlayer, bool detected);

    public void TurnOff();
}
