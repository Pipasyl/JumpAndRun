using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Breakout/Level", order = 0)]
public class Level : ScriptableObject
{

    #region Public Fields

    [TextArea]
    public string levelString;

    #endregion

}
