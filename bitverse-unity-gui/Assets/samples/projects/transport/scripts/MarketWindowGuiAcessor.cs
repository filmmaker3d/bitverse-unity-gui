// THIS IS AN AUTO GENERATED FILE
// IT SIMPLIFIES THE USE OF THE GUI, ONLY USE IT DURING INITIALIZATION,
// DONT USE IT AT UPDATE(), OR YOU WILL HAVE PERFORMANCE LOSS, USE IT AT START()
using System;
using UnityEngine;
using Bitverse.Unity.Gui;
public class MarketWindowGuiAcessor
{
    private BitWindow root;
    public MarketWindowGuiAcessor(BitWindow root)
    {
               if (root==null)
                   throw new Exception("ROOT CANT BE NULL: MarketWindowGuiAcessor");
        this.root=root;
        Refresh();
    }

    public void Refresh()
    {
           MarketWindowValue = root;
           if (MarketWindowValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME market_window , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           _1ItemlistLabelValue = root.FindControl<BitLabel>("1_itemlist_label");
           if (_1ItemlistLabelValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME 1_itemlist_label , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           CloseButtonValue = root.FindControl<BitButton>("close_button");
           if (CloseButtonValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME close_button , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           BuyButtonValue = root.FindControl<BitButton>("buy_button");
           if (BuyButtonValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME buy_button , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           ItemlistGroupValue = root.FindControl<BitGroup>("itemlist_group");
           if (ItemlistGroupValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME itemlist_group , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           ItemListValue = ItemlistGroupValue.FindControl<BitList>("item_list");
           if (ItemListValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME item_list , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           ItemGroupValue = ItemListValue.FindControl<BitGroup>("item_group");
           if (ItemGroupValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME item_group , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           ItemnameLabelValue = ItemGroupValue.FindControl<BitLabel>("itemname_label");
           if (ItemnameLabelValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME itemname_label , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           ItempriceLabelValue = ItemGroupValue.FindControl<BitLabel>("itemprice_label");
           if (ItempriceLabelValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME itemprice_label , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           ItemimagePictureValue = ItemGroupValue.FindControl<BitPicture>("itemimage_picture");
           if (ItemimagePictureValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME itemimage_picture , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           DescriptionGroupValue = root.FindControl<BitGroup>("description_group");
           if (DescriptionGroupValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME description_group , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           DescriptionTextareaValue = DescriptionGroupValue.FindControl<BitTextArea>("description_textarea");
           if (DescriptionTextareaValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME description_textarea , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           DescriptionLabelValue = DescriptionGroupValue.FindControl<BitLabel>("description_label");
           if (DescriptionLabelValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME description_label , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           PriceLabelValue = root.FindControl<BitLabel>("price_label");
           if (PriceLabelValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME price_label , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");
           NameLabelValue = root.FindControl<BitLabel>("name_label");
           if (NameLabelValue==null)
               throw new Exception("COULD NOT FIND BITCONTROL WITH NAME name_label , PLEASE CHECK THE PREFAB AND REGENERATE GUI ACESSOR.");

    }

    public BitWindow MarketWindow
    {
        get{
            return root;
        }
    }


   private BitWindow MarketWindowValue;

   public BitWindow MarketWindowProperty
   {
       get{
           return MarketWindowValue;
       }
   }

   private BitLabel _1ItemlistLabelValue;

   public BitLabel _1ItemlistLabel
   {
       get{
           return _1ItemlistLabelValue;
       }
   }

   private BitButton CloseButtonValue;

   public BitButton CloseButton
   {
       get{
           return CloseButtonValue;
       }
   }

   private BitButton BuyButtonValue;

   public BitButton BuyButton
   {
       get{
           return BuyButtonValue;
       }
   }

   private BitGroup ItemlistGroupValue;

   public BitGroup ItemlistGroup
   {
       get{
           return ItemlistGroupValue;
       }
   }

   private BitList ItemListValue;

   public BitList ItemList
   {
       get{
           return ItemListValue;
       }
   }

   private BitGroup ItemGroupValue;

   public BitGroup ItemGroup
   {
       get{
           return ItemGroupValue;
       }
   }

   private BitLabel ItemnameLabelValue;

   public BitLabel ItemnameLabel
   {
       get{
           return ItemnameLabelValue;
       }
   }

   private BitLabel ItempriceLabelValue;

   public BitLabel ItempriceLabel
   {
       get{
           return ItempriceLabelValue;
       }
   }

   private BitPicture ItemimagePictureValue;

   public BitPicture ItemimagePicture
   {
       get{
           return ItemimagePictureValue;
       }
   }

   private BitGroup DescriptionGroupValue;

   public BitGroup DescriptionGroup
   {
       get{
           return DescriptionGroupValue;
       }
   }

   private BitTextArea DescriptionTextareaValue;

   public BitTextArea DescriptionTextarea
   {
       get{
           return DescriptionTextareaValue;
       }
   }

   private BitLabel DescriptionLabelValue;

   public BitLabel DescriptionLabel
   {
       get{
           return DescriptionLabelValue;
       }
   }

   private BitLabel PriceLabelValue;

   public BitLabel PriceLabel
   {
       get{
           return PriceLabelValue;
       }
   }

   private BitLabel NameLabelValue;

   public BitLabel NameLabel
   {
       get{
           return NameLabelValue;
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
            MarketWindowValue = null;
            _1ItemlistLabelValue = null;
            CloseButtonValue = null;
            BuyButtonValue = null;
            ItemlistGroupValue = null;
            ItemListValue = null;
            ItemGroupValue = null;
            ItemnameLabelValue = null;
            ItempriceLabelValue = null;
            ItemimagePictureValue = null;
            DescriptionGroupValue = null;
            DescriptionTextareaValue = null;
            DescriptionLabelValue = null;
            PriceLabelValue = null;
            NameLabelValue = null;

    }
}
