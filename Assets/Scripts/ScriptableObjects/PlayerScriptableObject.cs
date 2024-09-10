using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPropteries",menuName = "Player",order = 0)]
public class PlayerScriptableObject : ScriptableObject
{
    [SerializeField] private Sprite[] playerIconSprites;
    public Sprite[] PlayerIconSprites { get { return playerIconSprites; } }
    [SerializeField] private Color[] playerColors;
    public Color[] PlayerColors { get { return playerColors;}}
}
