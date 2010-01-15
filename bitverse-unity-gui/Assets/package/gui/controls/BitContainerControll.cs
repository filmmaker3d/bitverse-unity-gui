using System;
using UnityEngine;
using System.Collections.Generic;


    public abstract class BitContainerControl : BitControl
    {
        //private List<Control> _controlList;

        //public List<Control> ControlList
        //{
        //    get { return _controlList; }
        //}

        //public Control AddControl(Control control)
        //{
        //    if (this.ParentForm == null)
        //    {
        //        throw new BadStructureException("debe perteneces a un formulario");
        //    }

        //    control.Parent = (Control)this;

        //    _controlList.Add(control);

        //    try
        //    {
        //        ParentForm.AddDictionaryControl(control);
        //    }
        //    catch (Exception err)
        //    {
        //        Debug.LogError(err.ToString());
        //    }

        //    return control;
        //}

        //public ContainerControl()
        //{
        //    _controlList = new List<Control>();
        //}

        protected virtual void DrawChildren()
        {
            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                Transform ch = transform.GetChild(i);
                BitControl c = (BitControl)ch.GetComponent(typeof(BitControl));
                if (c != null) c.Draw();
            }
        }

    }

