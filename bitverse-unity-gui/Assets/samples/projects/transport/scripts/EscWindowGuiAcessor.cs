// THIS IS AN AUTO GENERATED FILE
// IT SIMPLIFIES THE USE OF THE GUI, ONLY USE IT DURING INITIALIZATION,
// DONT USE IT AT UPDATE(), OR YOU WILL HAVE PERFORMANCE LOSS, USE IT AT START()
using System;
using UnityEngine;
using Bitverse.Unity.Gui;
public class EscWindowGuiAcessor
{
    private BitWindow root;
    public EscWindowGuiAcessor(BitWindow root)
    {
               if (root==null)
                   throw new Exception("ROOT CANT BE NULL: EscWindowGuiAcessor");
        this.root=root;
        Refresh();
    }

    public void Refresh()
    {
           EscWindowValue = root;
           if (EscWindowValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME esc_window , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           BacktogameButtonValue = root.FindControl<BitButton>("backtogame_button");
           if (BacktogameButtonValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME backtogame_button , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           ExitgameButtonValue = root.FindControl<BitButton>("exitgame_button");
           if (ExitgameButtonValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME exitgame_button , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");

    }

    public BitWindow EscWindow
    {
        get{
            return root;
        }
    }


   private BitWindow EscWindowValue;

   public BitWindow EscWindowProperty
   {
       get{
           return EscWindowValue;
       }
   }

   private BitButton BacktogameButtonValue;

   public BitButton BacktogameButton
   {
       get{
           return BacktogameButtonValue;
       }
   }

   private BitButton ExitgameButtonValue;

   public BitButton ExitgameButton
   {
       get{
           return ExitgameButtonValue;
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
            EscWindowValue = null;
            BacktogameButtonValue = null;
            ExitgameButtonValue = null;

    }
}
