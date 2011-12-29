// THIS IS AN AUTO GENERATED FILE
// IT SIMPLIFIES THE USE OF THE GUI, ONLY USE IT DURING INITIALIZATION,
// DONT USE IT AT UPDATE(), OR YOU WILL HAVE PERFORMANCE LOSS, USE IT AT START()
using System;
using UnityEngine;
using Bitverse.Unity.Gui;
public class NewgameWindowGuiAcessor
{
    private BitWindow root;
    public NewgameWindowGuiAcessor(BitWindow root)
    {
               if (root==null)
                   throw new Exception("ROOT CANT BE NULL: NewgameWindowGuiAcessor");
        this.root=root;
        Refresh();
    }

    public void Refresh()
    {
           NewgameWindowValue = root;
           if (NewgameWindowValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME newgame_window , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           BackButtonValue = root.FindControl<BitButton>("back_button");
           if (BackButtonValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME back_button , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           StartButtonValue = root.FindControl<BitButton>("start_button");
           if (StartButtonValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME start_button , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           NameLabelValue = root.FindControl<BitLabel>("name_label");
           if (NameLabelValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME name_label , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           NameTextfieldValue = root.FindControl<BitTextField>("name_textfield");
           if (NameTextfieldValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME name_textfield , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");

    }

    public BitWindow NewgameWindow
    {
        get{
            return root;
        }
    }


   private BitWindow NewgameWindowValue;

   public BitWindow NewgameWindowProperty
   {
       get{
           return NewgameWindowValue;
       }
   }

   private BitButton BackButtonValue;

   public BitButton BackButton
   {
       get{
           return BackButtonValue;
       }
   }

   private BitButton StartButtonValue;

   public BitButton StartButton
   {
       get{
           return StartButtonValue;
       }
   }

   private BitLabel NameLabelValue;

   public BitLabel NameLabel
   {
       get{
           return NameLabelValue;
       }
   }

   private BitTextField NameTextfieldValue;

   public BitTextField NameTextfield
   {
       get{
           return NameTextfieldValue;
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
            NewgameWindowValue = null;
            BackButtonValue = null;
            StartButtonValue = null;
            NameLabelValue = null;
            NameTextfieldValue = null;

    }
}
