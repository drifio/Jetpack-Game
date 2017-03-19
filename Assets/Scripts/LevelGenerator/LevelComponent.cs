using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent(typeof(LevelComponentMarker))]
public class LevelComponent : MonoBehaviour
{
    public LevelComponentMarker Bottom;
    public LevelComponentMarker Top;

    public void Awake()
    {

    }

    public void Update()
    {

    }
}
