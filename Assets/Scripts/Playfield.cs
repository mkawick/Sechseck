using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Playfield : MonoBehaviour {

    public GameObject hex;
    private List<GameObject> hexField = new List<GameObject>();
    public bool showLabelsFlag;
    public bool showStrategicEvaluationLabelsFlag = false;
    public bool enableLinearMapFlag;
    public PlayerScore playerStats;

    public enum Directions
    {
        North,
        NorthEast,
        SouthEast,
        South,
        SouthWest,
        NorthWest,
        DirectionsCount
    }

    void Start()
    {
        
    }
    public void Init(int boardIndex, bool resetPlayerScore)
    {
        SetToCircle();
        playerStats.SetScore(0);
    }

    public void ResetToTestDefault()
    {
        ClearField();

        for (int y = -10; y <= 10; y++)
        {
            for (int x = -10; x <= 10; x++)
            {
                Color c = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
                CreateHex(x, y, 0.0f, c);
            }
        }
    }

    public void SetToCircle()
    {
        ClearField();
        CirclePlayfieldInitializer cpfi = new CirclePlayfieldInitializer();
        cpfi.Init();
        int num = cpfi.GetNum();
        for (int i = 0; i < num; i++)
        {
            Vector3 v = cpfi.Get(i);
            if (i == 0)
                CreateHex((int)v.x, (int)v.y, v.z, Color.red);
            else if (i < 7)
                CreateHex((int)v.x, (int)v.y, v.z, Color.blue);
            else
                CreateHex((int)v.x, (int)v.y, v.z, Color.black);
        }
    }

    void CreateHex(int x, int y, float z, Color c)
    {
        Vector3 pos = HexUtils.ConvertHexposition(x, y);
        GameObject obj = Instantiate(hex, pos, Quaternion.identity) as GameObject;

        Transform TileChildTransform = obj.transform.Find("HexTileChild");
        if (TileChildTransform != null)
        {
            HexTile child = TileChildTransform.GetComponent<HexTile>();
            child.Init(x, y);
            child.SetColor(c);
            child.SetPlayfield(this);
        }
        obj.transform.parent = gameObject.transform;
        hexField.Add(obj);
    }

    public void ShowLabels()
    {
        showLabelsFlag = !showLabelsFlag;
        foreach (GameObject obj in hexField)
        {
            Transform TileChildTransform = obj.transform.Find("HexTileChild");
            if (TileChildTransform != null)
            {
                HexTile child = TileChildTransform.GetComponent<HexTile>();
                child.ShowLabel(showLabelsFlag);
            }
        }
    }

    public void ToggleLinearMap()
    {
        enableLinearMapFlag = !enableLinearMapFlag;
    }

    public void UnselectAllTiles()
    {
        foreach (GameObject obj in hexField)
        {
            Transform TileChildTransform = obj.transform.Find("HexTileChild");
            if (TileChildTransform != null)
            {
                HexTile child = TileChildTransform.GetComponent<HexTile>();
                child.SelectRowItem(false);
            }
        }
    }

    public HexTile GetNeighborTile(Directions d, int x, int y)
    {
        if (d == Directions.North)
            return Find(x, y + 1);
        if (d == Directions.South)
            return Find(x, y - 1);
        if ((x & 1) == 0)//even
        {
            switch (d)
            {
                case Directions.NorthEast:
                    return Find(x + 1, y);
                case Directions.SouthEast:
                    return Find(x + 1, y - 1);
                case Directions.NorthWest:
                    return Find(x - 1, y);
                case Directions.SouthWest:
                    return Find(x - 1, y - 1);
            }
        }
        else // odd
        {
            switch (d)
            {
                case Directions.NorthEast:
                    return Find(x + 1, y + 1);
                case Directions.SouthEast:
                    return Find(x + 1, y);
                case Directions.NorthWest:
                    return Find(x - 1, y + 1);
                case Directions.SouthWest:
                    return Find(x - 1, y);
            }
        }
        return null;
    }

    HexTile Find(int x, int y)
    {
        foreach (GameObject obj in hexField)
        {
            Transform TileChildTransform = obj.transform.Find("HexTileChild");
            if (TileChildTransform != null)
            {
                HexTile child = TileChildTransform.GetComponent<HexTile>();
                if (child.DoesPositionMatch(x, y))
                    return child;
            }
        }
        return null;
    }

    void ClearField()
    {
        enableLinearMapFlag = false;
        showLabelsFlag = false;
        showStrategicEvaluationLabelsFlag = false;
        foreach (GameObject obj in hexField)
        {
            Destroy(obj);
        }
        hexField.Clear();
    }

    public void AiPickATile(int playerId)
    {
        var possibleHexes = new List<HexTile>();
        foreach (GameObject obj in hexField)
        {
            Transform TileChildTransform = obj.transform.Find("HexTileChild");
            if (TileChildTransform != null)
            {
                HexTile tile = TileChildTransform.GetComponent<HexTile>();
                int ownerOfCurrentTile = tile.GetPlayerOwnerId();

                if (ownerOfCurrentTile == HexUtils.InvalidPlayer)
                {
                    possibleHexes.Add(tile);
                }
            }
        }
        possibleHexes.Sort((x, y) => { if (x.GetStrategicValue() > y.GetStrategicValue()) return -1; else return 0; } );

        //return possibleHexes
        int num = possibleHexes.Count;
        if(num>0)
            possibleHexes[0].CreateTokenObj(Color.red, playerId);
    }

    public void EvaluateStrategicValue()
    {
        GameController gc = HexUtils.GetGameController();
        int currentPlayerId = gc.GetCurrentPlayer();

        showStrategicEvaluationLabelsFlag = true;
        foreach (GameObject obj in hexField)
        {
            Transform TileChildTransform = obj.transform.Find("HexTileChild");
            if (TileChildTransform != null)
            {
                HexTile tile = TileChildTransform.GetComponent<HexTile>();
                int ownerOfCurrentTile = tile.GetPlayerOwnerId();

                if (ownerOfCurrentTile == HexUtils.InvalidPlayer)
                    EvaluateTileValueForAI(tile, currentPlayerId);
                else
                    tile.ShowStrategicEvaluationLabel(false);
                /* if (child.DoesPositionMatch(x, y))
                     return child;*/
            }
        }
    }

    public bool IsGameBoardFull()
    {
        foreach (GameObject obj in hexField)
        {
            Transform TileChildTransform = obj.transform.Find("HexTileChild");
            if (TileChildTransform != null)
            {
                HexTile tile = TileChildTransform.GetComponent<HexTile>();
                int ownerOfCurrentTile = tile.GetPlayerOwnerId();

                if (ownerOfCurrentTile == HexUtils.InvalidPlayer)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void EvaluateTileValueForAI(HexTile tile, int currentPlayerId)
    {
        int[] neighbors = { HexUtils.InvalidPlayer, HexUtils.InvalidPlayer, HexUtils.InvalidPlayer, HexUtils.InvalidPlayer, HexUtils.InvalidPlayer, HexUtils.InvalidPlayer };
        int numNeighborTiles = 0;
        int numEnemyTiles = 0;
        int numFriendlyTiles = 0;
        int index = 0;
        int continuousRows = 0;
        foreach (Playfield.Directions d in Enum.GetValues(typeof(Playfield.Directions)))
        {
            Vector2 pos = tile.GetPosition();
            HexTile ht = GetNeighborTile(d, (int)pos.x, (int)pos.y);

            if (ht)
            {
                numNeighborTiles++;
                int owner = ht.GetPlayerOwnerId();
                neighbors[index] = owner;
                if (owner > HexUtils.InvalidPlayer)
                {
                    if (owner == currentPlayerId)
                        numFriendlyTiles++;
                    else
                        numEnemyTiles++;

                    Vector2 neighborPos = ht.GetPosition();
                    HexTile htExtended = GetNeighborTile(d, (int)neighborPos.x, (int)neighborPos.y);
                    if (htExtended != null && owner == htExtended.GetPlayerOwnerId())// test for those in a row
                    {
                        continuousRows++;
                    }
                }

            }
            index++;
        }

        int value = 0;
        if (numNeighborTiles == 6)
        {
            value++;
        }
        if (numEnemyTiles > 2)
        {
            value += 2;
        }
        else if (numEnemyTiles > 1)
        {
            value++;
        }
        if (numFriendlyTiles != 0)
            value++;
        if (continuousRows > 0)
        {
            if (numEnemyTiles > 1)
                value += 1;
            value += 3;
        }

        // look for opposite directions.. 
        for (int i = 0; i < 3; i++)
            if (neighbors[i] != HexUtils.InvalidPlayer && neighbors[i] == neighbors[i + 3])
                value += 2;

        tile.SetStrategicEvaluationValue(value);

        if (false) // global lookup
        {
            tile.ShowStrategicEvaluationLabel(true);
        }
    }

    public bool TestForFourInARow(int currentPlayerId)
    {
        bool didFindFourInARow = false;
        Clear4InARow();
        foreach (GameObject obj in hexField)
        {
            Transform TileChildTransform = obj.transform.Find("HexTileChild");
            if (TileChildTransform != null)
            {
                HexTile child = TileChildTransform.GetComponent<HexTile>();
                if (child.IsSelected())
                    continue;
                int owner = child.GetPlayerOwnerId();
                if (owner != currentPlayerId)
                    continue;
                // now we know that we have a tile that could be part of four in a row.
                //child.SelectRowItem(false);
                didFindFourInARow |= WalkLinearlyToTryForFourInARow(child);
            }
        }
        return didFindFourInARow;
    }

    void RecursiveCountInDirection(HexTile tile, Playfield.Directions d, List<HexTile> lineOfTiles)
    {
        int owner = tile.GetPlayerOwnerId();
        Vector2 pos = tile.GetPosition();
        HexTile ht = GetNeighborTile(d, (int)pos.x, (int)pos.y);
        if (ht && ht.GetPlayerOwnerId() == owner)
        {
            lineOfTiles.Add(ht);
            RecursiveCountInDirection(ht, d, lineOfTiles);
        }
    }

    bool WalkLinearlyOppositeDirections(HexTile tile, Playfield.Directions d1, Playfield.Directions d2)
    {
        bool FourInARowFound = false;
        List<HexTile> lineOfTiles = new List<HexTile>();
        lineOfTiles.Add(tile);
        RecursiveCountInDirection(tile, d1, lineOfTiles);
        RecursiveCountInDirection(tile, d2, lineOfTiles);

        if (lineOfTiles.Count >= 4)
        {
            foreach (HexTile ht in lineOfTiles)
            {
                ht.SelectRowItem(true);
            }
            FourInARowFound = true;
        }
        return FourInARowFound;
    }
    bool WalkLinearlyToTryForFourInARow(HexTile tile)
    {
        bool FourInARowFound;
        FourInARowFound = WalkLinearlyOppositeDirections(tile, Playfield.Directions.North, Playfield.Directions.South);
        FourInARowFound |= WalkLinearlyOppositeDirections(tile, Playfield.Directions.NorthEast, Playfield.Directions.SouthWest);
        FourInARowFound |= WalkLinearlyOppositeDirections(tile, Playfield.Directions.NorthWest, Playfield.Directions.SouthEast);
        return FourInARowFound;
    }

    public void Clear4InARow()
    {
        foreach (GameObject obj in hexField)
        {
            Transform TileChildTransform = obj.transform.Find("HexTileChild");
            if (TileChildTransform != null)
            {
                HexTile child = TileChildTransform.GetComponent<HexTile>();
                child.SelectRowItem(false);
            }
        }
    }

    public void UpdateScore( int value )
    {
        playerStats.AddScore(value);
    }
    // Update is called once per frame
    void Update () {
	
	}
}
