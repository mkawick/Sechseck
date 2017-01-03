
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

class PlayfieldInitializer
{
    protected List<Vector3> array = null;

    public virtual void Init()
    {
        array = new List<Vector3>();// { new Vector2( 1, 2 ), new Vector2( 3, 4 ), new Vector2(5, 6 ), new Vector2(7, 8 ) };
    }
    public virtual int GetNum()
    {
        if (array == null)
            return 0;
        return array.Count;
    }
    public Vector2 Get(int n)
    {
        if (array == null)
            return new Vector3() ;

        if (n > -1 && n < GetNum())
            return array[n];
        return new Vector3();
    }

}

class CirclePlayfieldInitializer: PlayfieldInitializer
{ 
    public override void Init()
    {
        array = new List<Vector3>();
        array.Add( new Vector3(0, 0, 0) );

        array.Add(new Vector3(0, 1, 0));
        array.Add(new Vector3(0, -1, 0));

        array.Add(new Vector3(1, 0, 0));
        array.Add(new Vector3(1, -1, 0));
        array.Add(new Vector3(-1, 0, 0));
        array.Add(new Vector3(-1, -1, 0));

        array.Add(new Vector3(1, 1, 0));
        array.Add(new Vector3(2, 1, 0));
        array.Add(new Vector3(2, 0, 0));
        array.Add(new Vector3(2, -1, 0));

        array.Add(new Vector3(1, -2, 0));
        array.Add(new Vector3(0, -2, 0));
        array.Add(new Vector3(-1, -2, 0));

        array.Add(new Vector3(-2, 1, 0));
        array.Add(new Vector3(-2, 0, 0));
        array.Add(new Vector3(-2, -1, 0));

        array.Add(new Vector3(-1, 1, 0));
        array.Add(new Vector3(0, 2, 0));
        //array.Add(new Vector3(2, 2));
        //array.Add(new Vector3(3, 3));
    }
}

class Level2PlayfieldInitializer : PlayfieldInitializer
{
    public override void Init()
    {
        array = new List<Vector3>();
        array.Add(new Vector3(0, 0, 0));

        array.Add(new Vector3(0, 1, 0));
        array.Add(new Vector3(0, -1, 0));

        array.Add(new Vector3(1, 0, 0));
        array.Add(new Vector3(1, -1, 0));
        array.Add(new Vector3(-1, 0, 0));
        array.Add(new Vector3(-1, -1, 0));
        array.Add(new Vector3(1, 1, 0));

        array.Add(new Vector3(2, 2, 0));
        array.Add(new Vector3(2, 1, 0));
        array.Add(new Vector3(2, 0, 0));
        array.Add(new Vector3(2, -1, 0));
        array.Add(new Vector3(2, -2, 0));

        array.Add(new Vector3(3, 1, 0));
        array.Add(new Vector3(3, 0, 0));
        array.Add(new Vector3(3, -1, 0));
        array.Add(new Vector3(3, -2, 0));

        array.Add(new Vector3(1, -2, 0));
        //array.Add(new Vector3(0, -2, 0));
        array.Add(new Vector3(-1, -2, 0));

        array.Add(new Vector3(-2, 2, 0));
        array.Add(new Vector3(-2, 1, 0));
        array.Add(new Vector3(-2, 0, 0));
        array.Add(new Vector3(-2, -1, 0));
        array.Add(new Vector3(-2, -2, 0));

        array.Add(new Vector3(-1, 1, 0));
        //array.Add(new Vector3(0, 2, 0));

        array.Add(new Vector3(-3, 1, 0));
        array.Add(new Vector3(-3, 0, 0));
        array.Add(new Vector3(-3, -1, 0));
        array.Add(new Vector3(-3, -2, 0));


        //array.Add(new Vector3(2, 2));
        //array.Add(new Vector3(3, 3));
    }
}

