
using System.Collections.Generic;
using UnityEngine;

class HexUtils
{
    public const double Pi = 3.14159;
    public const int InvalidPlayer = -1; // km per sec.

    public static Vector3 ConvertHexposition(int indexAcross, int indexDown)
    {
        float size = 2.1f;

        float largerDir = Mathf.Sqrt(3) / 2.0f;
        //float smallerDir = 1.0f / 2.0f;

        int odd = indexAcross & 1;

        float widthOfHex = size;
        float heightOfHex = widthOfHex * largerDir;



        float multiplierAcross = widthOfHex * 3 / 4;
        float x = (multiplierAcross * indexAcross);
        float y = (heightOfHex * indexDown) + odd * heightOfHex / 2;

        return new Vector3(x, Mathf.Abs(Random.insideUnitCircle.y * 20), y);
    }
    public static GameController GetGameController()
    {
        GameController gameController = null;
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
        return gameController;
    }
}

#if MICKEY_COMMENT
public class HexGridPosition
{ // Cube storage, axial constructor
    int q, r, s;

    public HexGridPosition(int q_, int r_)
    {
        q = q_;
        r = r_;
        s = (-q_ - r_);
    }
    public HexGridPosition(int q_, int r_, int s_)
    {
        q = q_;
        r = r_;
        s = s_;
    }
    /*   int q() { return q_; }
       int r() { return r_; }
       int s() { return -q_ - r_; }*/

    public static bool operator ==(HexGridPosition a, HexGridPosition b)
    {
        return a.q == b.q && a.r == b.r && a.s == b.s;
    }

    public static bool operator !=(HexGridPosition a, HexGridPosition b)
    {
        return !(a == b);
    }

    public static HexGridPosition hex_add(HexGridPosition a, HexGridPosition b)
    {
        return new HexGridPosition(a.q + b.q, a.r + b.r, a.s + b.s);
    }

    public static HexGridPosition hex_subtract(HexGridPosition a, HexGridPosition b)
    {
        return new HexGridPosition(a.q - b.q, a.r - b.r, a.s - b.s);
    }

    public static HexGridPosition hex_multiply(HexGridPosition a, int k)
    {
        return new HexGridPosition(a.q * k, a.r * k, a.s * k);
    }

    public static int hex_length(HexGridPosition hex)
    {
        return (int)((Mathf.Abs(hex.q) + Mathf.Abs(hex.r) + Mathf.Abs(hex.s)) / 2);
    }

    public static int hex_distance(HexGridPosition a, HexGridPosition b)
    {
        return hex_length(hex_subtract(a, b));
    }

    public static readonly List<HexGridPosition> hex_directions = new List<HexGridPosition>(){
        new HexGridPosition(1, 0, -1), new HexGridPosition(1, -1, 0), new HexGridPosition(0, -1, 1),
        new HexGridPosition(-1, 0, 1), new HexGridPosition(-1, 1, 0), new HexGridPosition(0, 1, -1)
    };

    HexGridPosition hex_direction(int direction /* 0 to 5 */)
    {
        Debug.Assert(0 <= direction && direction < 6);
        return hex_directions[direction];
    }

    HexGridPosition hex_neighbor(HexGridPosition hex, int direction)
    {
        return hex_add(hex, hex_direction(direction));
    }
};
#endif