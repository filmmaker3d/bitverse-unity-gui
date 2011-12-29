// THIS IS AN AUTO GENERATED FILE
// IT SIMPLIFIES THE USE OF THE GUI, ONLY USE IT DURING INITIALIZATION,
// DONT USE IT AT UPDATE(), OR YOU WILL HAVE PERFORMANCE LOSS, USE IT AT START()
using System;
using UnityEngine;
using Bitverse.Unity.Gui;
public class OptionsWindowGuiAcessor
{
    private BitWindow root;
    public OptionsWindowGuiAcessor(BitWindow root)
    {
               if (root==null)
                   throw new Exception("ROOT CANT BE NULL: OptionsWindowGuiAcessor");
        this.root=root;
        Refresh();
    }

    public void Refresh()
    {
           OptionsWindowValue = root;
           if (OptionsWindowValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME options_window , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           BackButtonValue = root.FindControl<BitButton>("back_button");
           if (BackButtonValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME back_button , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           ResolutionLabelValue = root.FindControl<BitLabel>("resolution_label");
           if (ResolutionLabelValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME resolution_label , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           ResOpt1ToggleValue = root.FindControl<BitToggle>("res_opt1_toggle");
           if (ResOpt1ToggleValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME res_opt1_toggle , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           ResOpt2ToggleValue = root.FindControl<BitToggle>("res_opt2_toggle");
           if (ResOpt2ToggleValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME res_opt2_toggle , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           ResOpt3ToggleValue = root.FindControl<BitToggle>("res_opt3_toggle");
           if (ResOpt3ToggleValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME res_opt3_toggle , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");

    }

    public BitWindow OptionsWindow
    {
        get{
            return root;
        }
    }


   private BitWindow OptionsWindowValue;

   public BitWindow OptionsWindowProperty
   {
       get{
           return OptionsWindowValue;
       }
   }

   private BitButton BackButtonValue;

   public BitButton BackButton
   {
       get{
           return BackButtonValue;
       }
   }

   private BitLabel ResolutionLabelValue;

   public BitLabel ResolutionLabel
   {
       get{
           return ResolutionLabelValue;
       }
   }

   private BitToggle ResOpt1ToggleValue;

   public BitToggle ResOpt1Toggle
   {
       get{
           return ResOpt1ToggleValue;
       }
   }

   private BitToggle ResOpt2ToggleValue;

   public BitToggle ResOpt2Toggle
   {
       get{
           return ResOpt2ToggleValue;
       }
   }

   private BitToggle ResOpt3ToggleValue;

   public BitToggle ResOpt3Toggle
   {
       get{
           return ResOpt3ToggleValue;
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
            OptionsWindowValue = null;
            BackButtonValue = null;
            ResolutionLabelValue = null;
            ResOpt1ToggleValue = null;
            ResOpt2ToggleValue = null;
            ResOpt3ToggleValue = null;

    }
}
