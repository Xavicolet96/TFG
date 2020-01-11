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
/// O open sink
/// C crease
/// D displace
/// 
/// B base state
/// L load
/// S save
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
        c.deselect();
    }

    public override void Execute(Controller c)
    {
        if (c.hasVertex1())
        {
            c.GetFSM().ChangeState(SelectedV.Instance);
        }

        if (c.hasEdge1())
        {
            c.GetFSM().ChangeState(SelectedE.Instance);
        }

        if (Input.GetKeyDown("u"))
        {
            c.GetFSM().ChangeState(SelectedU.Instance);
        }
        if (Input.GetKeyDown("r"))
        {
            c.GetFSM().ChangeState(SelectedR.Instance);
        }
        if (Input.GetKeyDown("b"))
        {
            c.GetFSM().ChangeState(BaseState.Instance);
        }
        if (Input.GetKeyDown("s"))
        {
            c.GetFSM().ChangeState(SelectedS.Instance);
        }
        if (Input.GetKeyDown("y"))
        {
            c.GetFSM().ChangeState(SelectedY.Instance);
        }

        if (Input.GetKeyDown("q"))
        {
            c.GetFSM().ChangeState(SelectedQ.Instance);
        }

        if (Input.GetKeyDown("l"))
        {
            c.GetFSM().ChangeState(SelectedL.Instance);
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

        if (!c.hasVertex2() && c.hasPoint1())
        {
            c.GetFSM().ChangeState(SelectedVP.Instance);
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
        if (Input.GetKeyDown("w"))
        {
            c.GetFSM().ChangeState(SelectedVVW.Instance);
        }
        if (Input.GetKeyDown("o"))
        {
            c.GetFSM().ChangeState(SelectedVVO.Instance);
        }
        if (Input.GetKeyDown("c"))
        {
            c.GetFSM().ChangeState(SelectedVVC.Instance);
        }
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat VV");
    }
}

public class SelectedVP : State<Controller>
{
    static readonly SelectedVP instance = new SelectedVP();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedVP()
    {
    }

    SelectedVP()
    {
    }
    //this is a singleton	
    public static SelectedVP Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat VP");
    }

    public override void Execute(Controller c)
    {
        if (Input.GetKeyDown("p"))
        {
            c.GetFSM().ChangeState(SelectedVPP.Instance);
        }
        if (Input.GetKeyDown("m"))
        {
            c.GetFSM().ChangeState(SelectedVPM.Instance);
        }
        if (Input.GetKeyDown("w"))
        {
            c.GetFSM().ChangeState(SelectedVPW.Instance);
        }
        if (Input.GetKeyDown("c"))
        {
            c.GetFSM().ChangeState(SelectedVPC.Instance);
        }
        if (Input.GetKeyDown("d"))
        {
            c.GetFSM().ChangeState(SelectedVPD.Instance);
        }
        if (Input.GetKeyDown("b"))
        {
            c.clearData();
            c.GetFSM().ChangeState(BaseState.Instance);
        }
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat VP");
    }
}

public class SelectedE : State<Controller>
{
    static readonly SelectedE instance = new SelectedE();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedE()
    {
    }

    SelectedE()
    {
    }
    //this is a singleton	
    public static SelectedE Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat E");
    }

    public override void Execute(Controller c)
    {
        if (c.hasEdge2())
        {
            c.GetFSM().ChangeState(SelectedEE.Instance);
        }
        if (Input.GetKeyDown("b"))
        {
            c.clearData();
            c.GetFSM().ChangeState(BaseState.Instance);
        }
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat E");
    }
}

public class SelectedEE : State<Controller>
{
    static readonly SelectedEE instance = new SelectedEE();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedEE()
    {
    }

    SelectedEE()
    {
    }
    //this is a singleton	
    public static SelectedEE Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat EE");
    }

    public override void Execute(Controller c)
    {
        if (Input.GetKeyDown("b"))
        {
            c.clearData();
            c.GetFSM().ChangeState(BaseState.Instance);
        }

        if (Input.GetKeyDown("m"))
        {
            c.GetFSM().ChangeState(SelectedEEM.Instance);
        }
        if (Input.GetKeyDown("w"))
        {
            c.GetFSM().ChangeState(SelectedEEW.Instance);
        }
        if (Input.GetKeyDown("o"))
        {
            c.GetFSM().ChangeState(SelectedEEO.Instance);
        }
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat EE");
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
        c.data.mountainFold(c.v1, c.v2);
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

public class SelectedEEM : State<Controller>
{
    static readonly SelectedEEM instance = new SelectedEEM();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedEEM()
    {
    }

    SelectedEEM()
    {
    }
    //this is a singleton	
    public static SelectedEEM Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat EEM");
    }

    public override void Execute(Controller c)
    {
        c.data.mountainFold(c.e1, c.e2);
        c.clearData();
        c.destroyStructure();
        c.plotStructure(c.data);
        c.GetFSM().ChangeState(BaseState.Instance);
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat EEM");
    }
}

public class SelectedEEW : State<Controller>
{
    static readonly SelectedEEW instance = new SelectedEEW();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedEEW()
    {
    }

    SelectedEEW()
    {
    }
    //this is a singleton	
    public static SelectedEEW Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat EEW");
    }

    public override void Execute(Controller c)
    {
        c.data.valleyFold(c.e1, c.e2);
        c.clearData();
        c.destroyStructure();
        c.plotStructure(c.data);
        c.GetFSM().ChangeState(BaseState.Instance);
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat EEW");
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
        if (c.data.unfoldCount == 0) {
            c.data.unfold();
            c.clearData();
            c.destroyStructure();
            c.plotStructure(c.data);
        }
        c.GetFSM().ChangeState(BaseState.Instance);
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat U");
    }
}

public class SelectedVVW : State<Controller>
{
    static readonly SelectedVVW instance = new SelectedVVW();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedVVW()
    {
    }

    SelectedVVW()
    {
    }
    //this is a singleton	
    public static SelectedVVW Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat VVW");
    }

    public override void Execute(Controller c)
    {
        c.data.valleyFold(c.v1, c.v2);
        c.clearData();
        c.destroyStructure();
        c.plotStructure(c.data);
        c.GetFSM().ChangeState(BaseState.Instance);

    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat VVW");
    }
}

public class SelectedVVC : State<Controller>
{
    static readonly SelectedVVC instance = new SelectedVVC();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedVVC()
    {
    }

    SelectedVVC()
    {
    }
    //this is a singleton	
    public static SelectedVVC Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat VVC");
    }

    public override void Execute(Controller c)
    {
        c.data.crease(c.v1, c.v2);
        c.clearData();
        c.destroyStructure();
        c.plotStructure(c.data);
        c.GetFSM().ChangeState(BaseState.Instance);
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat VVC");
    }
}

public class SelectedVPC : State<Controller>
{
    static readonly SelectedVPC instance = new SelectedVPC();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedVPC()
    {
    }

    SelectedVPC()
    {
    }
    //this is a singleton	
    public static SelectedVPC Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat VPC");
    }

    public override void Execute(Controller c)
    {
        c.data.crease(c.v1, c.p1);
        c.clearData();
        c.destroyStructure();
        c.plotStructure(c.data);
        c.GetFSM().ChangeState(BaseState.Instance);
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat VPC");
    }
}

public class SelectedVVO : State<Controller>
{
    static readonly SelectedVVO instance = new SelectedVVO();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedVVO()
    {
    }

    SelectedVVO()
    {
    }
    //this is a singleton	
    public static SelectedVVO Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat VVW");
    }

    public override void Execute(Controller c)
    {
        c.data.openSinkFold(c.v1, c.v2);
        c.clearData();

        c.destroyStructure();

        c.plotStructure(c.data);
        c.GetFSM().ChangeState(BaseState.Instance);
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat VVW");
    }
}

public class SelectedEEO : State<Controller>
{
    static readonly SelectedEEO instance = new SelectedEEO();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedEEO()
    {
    }

    SelectedEEO()
    {
    }
    //this is a singleton	
    public static SelectedEEO Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat EEO");
    }

    public override void Execute(Controller c)
    {
        c.data.openSinkFold(c.e1, c.e2);
        c.clearData();

        c.destroyStructure();

        c.plotStructure(c.data);
        c.GetFSM().ChangeState(BaseState.Instance);
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat EEO");
    }
}

public class SelectedVPP : State<Controller>
{
    static readonly SelectedVPP instance = new SelectedVPP();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedVPP()
    {
    }

    SelectedVPP()
    {
    }
    //this is a singleton	
    public static SelectedVPP Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat VPP");
    }

    public override void Execute(Controller c)
    {
        c.data.pull(c.v1, c.p1);
        c.clearData();
        c.destroyStructure();
        c.plotStructure(c.data);
        c.GetFSM().ChangeState(BaseState.Instance);
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat VPP");
    }
}

public class SelectedVPD : State<Controller>
{
    static readonly SelectedVPD instance = new SelectedVPD();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedVPD()
    {
    }

    SelectedVPD()
    {
    }
    //this is a singleton	
    public static SelectedVPD Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat VPD");
    }

    public override void Execute(Controller c)
    {
        c.data.displace(c.v1, c.p1);
        c.clearData();

        c.destroyStructure();

        c.plotStructure(c.data);
        c.GetFSM().ChangeState(BaseState.Instance);
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat VPD");
    }
}

public class SelectedVPM : State<Controller>
{
    static readonly SelectedVPM instance = new SelectedVPM();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedVPM()
    {
    }

    SelectedVPM()
    {
    }
    //this is a singleton	
    public static SelectedVPM Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat VPM");
    }

    public override void Execute(Controller c)
    {
        c.data.mountainFold(c.v1, c.p1);
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

public class SelectedVPW : State<Controller>
{
    static readonly SelectedVPW instance = new SelectedVPW();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedVPW()
    {
    }

    SelectedVPW()
    {
    }
    //this is a singleton	
    public static SelectedVPW Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat VPW");
    }

    public override void Execute(Controller c)
    {
        c.data.valleyFold(c.v1, c.p1);
        c.clearData();

        c.destroyStructure();


        c.plotStructure(c.data);
        c.GetFSM().ChangeState(BaseState.Instance);

    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat VPW");
    }
}

public class SelectedS : State<Controller>
{
    static readonly SelectedS instance = new SelectedS();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedS()
    {
    }

    SelectedS()
    {
    }
    //this is a singleton	
    public static SelectedS Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat S");
    }

    public override void Execute(Controller c)
    {
        c.save();
        c.GetFSM().ChangeState(BaseState.Instance);
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat S");
    }
}

public class SelectedL : State<Controller>
{
    static readonly SelectedL instance = new SelectedL();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedL()
    {
    }

    SelectedL()
    {
    }
    //this is a singleton	
    public static SelectedL Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat L");
    }

    public override void Execute(Controller c)
    {
        c.load();
        c.destroyStructure();
        c.plotStructure(c.data);
        c.GetFSM().ChangeState(BaseState.Instance);
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat L");
    }
}

public class SelectedY : State<Controller>
{
    static readonly SelectedY instance = new SelectedY();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedY()
    {
    }

    SelectedY()
    {
    }
    //this is a singleton	
    public static SelectedY Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat Play Y");
    }

    public override void Execute(Controller c)
    {
        c.play();
        c.GetFSM().ChangeState(BaseState.Instance);

    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat Y");
    }
}

public class SelectedQ : State<Controller>
{
    static readonly SelectedQ instance = new SelectedQ();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedQ()
    {
    }

    SelectedQ()
    {
    }
    //this is a singleton	
    public static SelectedQ Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat  Q");
    }

    public override void Execute(Controller c)
    {
        Application.Quit();
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat Q");
    }
}

public class SelectedR : State<Controller>
{
    static readonly SelectedR instance = new SelectedR();
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SelectedR()
    {
    }

    SelectedR()
    {
    }
    //this is a singleton	
    public static SelectedR Instance { get { return instance; } }

    public override void Enter(Controller c)
    {
        Debug.Log("Entro a l'estat  R");
    }

    public override void Execute(Controller c)
    {
        c.clearData();
        c.destroyStructure();
        c.resetModel();
        c.GetFSM().ChangeState(BaseState.Instance);
    }

    public override void Exit(Controller c)
    {
        Debug.Log("Surto de l'estat R");
    }
}