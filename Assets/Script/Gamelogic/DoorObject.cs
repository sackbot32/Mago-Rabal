using UnityEngine;

[CreateAssetMenu(fileName = "DoorObject", menuName = "Scriptable Objects/DoorObject")]
public class DoorObject : ScriptableObject
{
    public string doorName;
    public int sceneIndex;
}
