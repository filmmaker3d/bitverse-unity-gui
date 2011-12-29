// THIS IS AN AUTO GENERATED FILE
// IT SIMPLIFIES THE USE OF THE GUI, ONLY USE IT DURING INITIALIZATION,
// DONT USE IT AT UPDATE(), OR YOU WILL HAVE PERFORMANCE LOSS, USE IT AT START()
using System;
using UnityEngine;
using Bitverse.Unity.Gui;
public class MainmenuWindowGuiAcessor
{
    private BitWindow root;
    public MainmenuWindowGuiAcessor(BitWindow root)
    {
               if (root==null)
                   throw new Exception("ROOT CANT BE NULL: MainmenuWindowGuiAcessor");
        this.root=root;
        Refresh();
    }

    public void Refresh()
    {
           MainmenuWindowValue = root;
           if (MainmenuWindowValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME mainmenu_window , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           _1MainPictureValue = root.FindControl<BitPicture>("1_main_picture");
           if (_1MainPictureValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME 1_main_picture , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           NewgameButtonValue = root.FindControl<BitButton>("newgame_button");
           if (NewgameButtonValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME newgame_button , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           OptionsButtonValue = root.FindControl<BitButton>("options_button");
           if (OptionsButtonValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME options_button , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           ExitButtonValue = root.FindControl<BitButton>("exit_button");
           if (ExitButtonValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME exit_button , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");

    }

    public BitWindow MainmenuWindow
    {
        get{
            return root;
        }
    }


   private BitWindow MainmenuWindowValue;

   public BitWindow MainmenuWindowProperty
   {
       get{
           return MainmenuWindowValue;
       }
   }

   private BitPicture _1MainPictureValue;

   public BitPicture _1MainPicture
   {
       get{
           return _1MainPictureValue;
       }
   }

   private BitButton NewgameButtonValue;

   public BitButton NewgameButton
   {
       get{
           return NewgameButtonValue;
       }
   }

   private BitButton OptionsButtonValue;

   public BitButton OptionsButton
   {
       get{
           return OptionsButtonValue;
       }
   }

   private BitButton ExitButtonValue;

   public BitButton ExitButton
   {
       get{
           return ExitButtonValue;
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
            MainmenuWindowValue = null;
            _1MainPictureValue = null;
            NewgameButtonValue = null;
            OptionsButtonValue = null;
            ExitButtonValue = null;

    }
}
