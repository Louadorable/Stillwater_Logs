using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Character", menuName = "Dialogue/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;

    public Sprite defaultPortrait;

    public List<ExpressionSprite> expressions;
}

[System.Serializable]
public class ExpressionSprite
{
    public string expressionName;
    public Sprite sprite;
}