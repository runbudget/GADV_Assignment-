using UnityEngine;




[CreateAssetMenu(menuName = "Game/LevelRules", fileName = "LevelRules")]
public class LevelCriteriaSO : ScriptableObject
{
    public int starsToWin = 3;
    public int coinsPerCorrect = 5;
}
