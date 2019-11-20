using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The follwing states represent all the flows available in the simulation
/// there is the base state, and then the abreviations are as follow
/// V vertex
/// E edge
/// F face
/// P point
/// M mountain
/// W valley
/// U unfold
/// R reverse fold
/// I inside reverse fold
/// E rabbit ear
/// S open sink
/// 
/// B base state
/// </summary>

public class BaseState : State<Controller>
{
    static readonly BaseState instance = new BaseState();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static BaseState()
    {
    }

    BaseState()
    {
    }
    //this is a singleton	
    public static BaseState Instance { get { return instance; } }

    public override void Enter(Controller  c)
    {
        Debug.Log("Entro a l'estat base");
    }

    public override void Execute(Controller c)
    {
        if (c.hasVertex1())
        {
            c.GetFSM().ChangeState(SelectedV.Instance);
        }
        if (Input.GetKeyDown("u"))
        {
            c.GetFSM().ChangeState(SelectedU.Instance);
        }
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat base");
    }

}

public class SelectedV : State<Controller>
{
    static readonly SelectedV instance = new SelectedV();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedV()
    {
    }

    SelectedV()
    {
    }
    //this is a singleton	
    public static SelectedV Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat V");
    }

    public override void Execute(Controller c)
    {
        if (c.hasVertex2())
        {
            c.GetFSM().ChangeState(SelectedVV.Instance);
        }
        if (Input.GetKeyDown("b"))
        {
            c.clearData();
            c.GetFSM().ChangeState(BaseState.Instance);
        }
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat V");
    }
}

public class SelectedVV : State<Controller>
{
    static readonly SelectedVV instance = new SelectedVV();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedVV()
    {
    }

    SelectedVV()
    {
    }
    //this is a singleton	
    public static SelectedVV Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat VV");
    }

    public override void Execute(Controller c)
    {
        if (Input.GetKeyDown("m"))
        {
            c.GetFSM().ChangeState(SelectedVVM.Instance);
        }

        if (Input.GetKeyDown("b"))
        {
            c.clearData();
            c.GetFSM().ChangeState(BaseState.Instance);
        }
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat VV");
    }
}

public class SelectedVVM : State<Controller>
{
    static readonly SelectedVVM instance = new SelectedVVM();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedVVM()
    {
    }

    SelectedVVM()
    {
    }
    //this is a singleton	
    public static SelectedVVM Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat VVM");
    }

    public override void Execute(Controller c)
    {
        Debug.Log("estic execute");
        c.data.mountainFoldVtoV(c.v1, c.v2);
        c.clearData();

        c.destroyStructure();
        

        c.plotStructure(c.data);
        c.GetFSM().ChangeState(BaseState.Instance);

    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat VVM");
    }
}

public class SelectedU : State<Controller>
{
    static readonly SelectedU instance = new SelectedU();
    static SelectedU(){    }
    SelectedU()    {    }
    public static SelectedU Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat U");
    }

    public override void Execute(Controller c)
    {
        c.data.unfold();
        c.clearData();
        c.destroyStructure();
        c.plotStructure(c.data);
        c.GetFSM().ChangeState(BaseState.Instance);
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat U");
    }
}