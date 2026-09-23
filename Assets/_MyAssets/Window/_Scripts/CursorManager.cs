using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameCursor
{
    Arrow,
    Hand,
    OpenHand
}

[Serializable]
public struct GameCursorData
{
    [field: SerializeField] public GameCursor Type { get; private set; }
    [field: SerializeField] public Texture2D Texture { get; private set; } // TODO: ADD DARK MODE
}

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    [SerializeField] private List<GameCursorData> gameCursorsData;
    private Dictionary<GameCursor, Texture2D> gameCursorToTexture;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }
        Instance = this;


        gameCursorToTexture = gameCursorsData.ToDictionary(ct => ct.Type, ct => ct.Texture);
    }

    public void SetGameCursor(GameCursor cursor)
    {
        if (!gameCursorToTexture.TryGetValue(cursor, out var curText))
        {
            Debug.LogError($"Error setting game cursor: cursor '{cursor}' is not present on gameCursorToTexture dictionary!");
            return;
        }

        Cursor.SetCursor(curText, Vector2.zero, CursorMode.ForceSoftware);
    }
}