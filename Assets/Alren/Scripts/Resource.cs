using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Resources")]
public class Resource : ScriptableObject
{
    public int ID;
    public int WorkerCount;
    public int ResourceCount;
    public string Name;
    public string Description;
}