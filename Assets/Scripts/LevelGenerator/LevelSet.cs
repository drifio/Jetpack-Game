using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "Level/Level Set", order = 1)]
public class LevelSet : ScriptableObject
{
    public string Name = "";

    public List<LevelComponent> SpawnComponent = new List<LevelComponent>();
    public List<LevelComponent> CommonComponents = new List<LevelComponent>();
    public List<LevelComponent> EndComponents = new List<LevelComponent>();
}
