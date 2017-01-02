using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class HexTile : MonoBehaviour {

    private int x, y;
    private int strategicValue;
    public GameObject tokenObj;
    GameObject createdToken;
    public GameObject floatAwayText;
    private GameObject createdFloatAwayText;

    Color myColor;
    Playfield playfield;

    public Color [] playerTokenColors;
    int playerOwnerId = HexUtils.InvalidPlayer;

    bool isSelected = false;

    // Use this for initialization
    void Start () {
        //iTween.RotateBy(gameObject, iTween.Hash("x", .25, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", .4));
        //iTween.MoveTo(gameObject,iTween.Hash("y",0,"time",4,"loopType","pingPong","delay",.4,"easeType","easeInOutQuad"));

        iTween.MoveTo(gameObject, iTween.Hash( "y", 0, "delay", 0.2, "time", 1.2, "easetype", iTween.EaseType.easeOutBounce ) );

        ShowStrategicEvaluationLabel(false);
        ClearPlayerOwnerId();
    }

    public void Init( int _x, int _y)
    {
        x = _x;
        y = _y;
        Transform LabelTransform = transform.Find("HexLabel");
        if (LabelTransform)
        {
            GameObject obj = LabelTransform.gameObject;
            CameraFacingLabel cfl = obj.GetComponent<CameraFacingLabel>();
            if(cfl)
                cfl.Init(x, y);
        }
        ShowLabel(false);
    }
    public Vector2 GetPosition()
    {
        return new Vector2(x, y);
    }
    public void ClearPlayerOwnerId()
    {
        playerOwnerId = HexUtils.InvalidPlayer;
    }
    public int GetPlayerOwnerId()
    {
        return playerOwnerId;
    }

    public bool DoesPositionMatch(int _x, int _y)
    {
        if (x == _x && y == _y)
            return true;
        return false;
    }

    public void SetColor(Color c )
    {
        myColor = c;
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.SetColor("_Color", c);
    }

    public void SetPlayfield(Playfield pf)
    {
        playfield = pf;
    }

    public void ShowLabel(bool show)
    {
        Transform LabelTransform = transform.Find("HexLabel");
        if (LabelTransform)
        {
            GameObject obj = LabelTransform.gameObject;
            obj.SetActive(show);
        }
    }
    public void ShowStrategicEvaluationLabel(bool show)
    {
        Transform LabelTransform = transform.Find("StrategicEvaluationLabel");
        if (LabelTransform)
        {
            GameObject obj = LabelTransform.gameObject;
            obj.SetActive(show);
        }
    }

    public void SetStrategicEvaluationValue(int value)
    {
        Transform LabelTransform = transform.Find("StrategicEvaluationLabel");
        if (LabelTransform)
        {
            GameObject obj = LabelTransform.gameObject;
            CameraFacingLabel cfl = obj.GetComponent<CameraFacingLabel>();
            if (cfl)
                cfl.Init(value);
        }
        strategicValue = value;
    }
    public int GetStrategicValue()
    {
        return strategicValue;
    }

    public int Compare(HexTile rhs)
    {
        if(strategicValue < rhs.GetStrategicValue())
        {
            return -1;
        }
        if (strategicValue > rhs.GetStrategicValue())
        {
            return 1;
        }
        return 0;
    }

    // Update is called once per frame
    void Update () {
		
	}

    bool IsUIBlockingMouseClicks()
    {
        return !EventSystem.current.IsPointerOverGameObject();
    }
    void OnMouseDown()
    {
        if (IsUIBlockingMouseClicks())
        {
            GameController gc = HexUtils.GetGameController();
            int playerId = gc.GetCurrentPlayer();

            if (playerId != (int)GameController.PlayerID.Player1)
                return;
            CreateTokenObj(Color.white, playerId);

            gc.TransitionPlayerTurn();

            int strategicValue = GetStrategicValue();
            CreateFloaterText(strategicValue);
            playfield.UpdateScore(strategicValue);
        }
    }

    void CreateFloaterText(int value)
    {
        Vector3 pos = gameObject.transform.position;
        pos.y -= 1.0f;
        createdFloatAwayText = Instantiate(floatAwayText, pos, Quaternion.identity) as GameObject;
        createdFloatAwayText.transform.SetParent(gameObject.transform);
        createdFloatAwayText.SetActive(true);
        createdFloatAwayText.GetComponent<FloaterText>().SetText(value.ToString());
    }
    public void CreateTokenObj(Color c, int playerId)
    {
        if (createdToken)
            Destroy(createdToken);

        playerOwnerId = playerId;

        Vector3 pos = gameObject.transform.position;
        pos.y += 5.0f;
        createdToken = Instantiate(tokenObj, pos, Quaternion.identity) as GameObject;
        createdToken.transform.SetParent( gameObject.transform );
        Transform model = createdToken.transform.Find("Sphere");

        Renderer rend = model.gameObject.GetComponent<Renderer>();
        if (playerTokenColors.Length <= playerOwnerId || playerOwnerId == 0)
        {
            rend.material.SetColor("_Color", c);
        }
        else
        {
            rend.material.SetColor("_Color", playerTokenColors[playerOwnerId]);
        }
        DropTokenIn();
    }

    void DropTokenIn()
    {
        Vector3 pos = gameObject.transform.position;
        pos.y += 0.2f;
        iTween.MoveTo(createdToken, iTween.Hash("y", pos.y, "time", 0.4, "easetype", iTween.EaseType.easeOutSine));
    }

    void OnMouseOver()
    {
        if (playfield.enableLinearMapFlag == false)
            return;
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.SetColor("_Color", Color.green);
        foreach (Playfield.Directions d in Enum.GetValues(typeof(Playfield.Directions)))
        {
            HexTile ht = playfield.GetNeighborTile(d, x, y);
            if (ht)
            {
                ht.SelectRowItem(true);
                ht.SelectNextItemInRow(d);
            }
        }
    }

    void SelectNextItemInRow(Playfield.Directions d)
    {
        HexTile ht = playfield.GetNeighborTile(d, x, y);
        if (ht)
        {
            ht.SelectRowItem(true);
            ht.SelectNextItemInRow(d);
        }
    }

    void OnMouseExit()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.SetColor("_Color", myColor);

        playfield.UnselectAllTiles();
    }

    public void SelectRowItem(bool selected)
    {
        isSelected = selected;
        Renderer rend = gameObject.GetComponent<Renderer>();
        if(isSelected)
            rend.material.shader = Shader.Find("Shader Forge/Outline");
        else
            rend.material.shader = Shader.Find("Standard");
    }

    public bool IsSelected()
    {
        return isSelected;
    }
}
