// THIS IS AN AUTO GENERATED FILE
// IT SIMPLIFIES THE USE OF THE GUI, ONLY USE IT DURING INITIALIZATION,
// DONT USE IT AT UPDATE(), OR YOU WILL HAVE PERFORMANCE LOSS, USE IT AT START()
using System;
using UnityEngine;
using Bitverse.Unity.Gui;
public class TopWindowGuiAcessor
{
    private BitWindow root;
    public TopWindowGuiAcessor(BitWindow root)
    {
               if (root==null)
                   throw new Exception("ROOT CANT BE NULL: TopWindowGuiAcessor");
        this.root=root;
        Refresh();
    }

    public void Refresh()
    {
           TopWindowValue = root;
           if (TopWindowValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME top_window , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           MarketButtonValue = root.FindControl<BitButton>("market_button");
           if (MarketButtonValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME market_button , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           PlayernameLabelValue = root.FindControl<BitLabel>("playername_label");
           if (PlayernameLabelValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME playername_label , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           DepotButtonValue = root.FindControl<BitButton>("depot_button");
           if (DepotButtonValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME depot_button , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           PlayerbalanceLabelValue = root.FindControl<BitLabel>("playerbalance_label");
           if (PlayerbalanceLabelValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME playerbalance_label , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");

    }

    public BitWindow TopWindow
    {
        get{
            return root;
        }
    }


   private BitWindow TopWindowValue;

   public BitWindow TopWindowProperty
   {
       get{
           return TopWindowValue;
       }
   }

   private BitButton MarketButtonValue;

   public BitButton MarketButton
   {
       get{
           return MarketButtonValue;
       }
   }

   private BitLabel PlayernameLabelValue;

   public BitLabel PlayernameLabel
   {
       get{
           return PlayernameLabelValue;
       }
   }

   private BitButton DepotButtonValue;

   public BitButton DepotButton
   {
       get{
           return DepotButtonValue;
       }
   }

   private BitLabel PlayerbalanceLabelValue;

   public BitLabel PlayerbalanceLabel
   {
       get{
           return PlayerbalanceLabelValue;
       }
   }


    public bool IsLoaded
    {
        get
        {
            return root!=null;
        }
    }

    public void Dispose()
    {
            TopWindowValue = null;
            MarketButtonValue = null;
            PlayernameLabelValue = null;
            DepotButtonValue = null;
            PlayerbalanceLabelValue = null;

    }
}
