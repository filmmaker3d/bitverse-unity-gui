using System;
using UnityEngine;
using System.Collections.Generic;


    public static class BitFormsManager
    {
        private static Dictionary<Guid, BitForm> _formList = new Dictionary<Guid, BitForm>();
        private static Stack<Guid> _formStack = new Stack<Guid>();

        internal static void PushModal(BitForm source)
        {
            if (_formStack.Count == 0)
            {
                foreach (KeyValuePair <Guid, BitForm> item in _formList)
                {
                    if (source != item.Value)
                    {
                        item.Value.Disabled = true;
                    }
                }
            }
            else
            {
                _formList[_formStack.Peek()].Disabled = true;
            }

            _formStack.Push(source.ID);

            //Debug.Log(string.Format("Push({0}) '{1}'", _formStack.Count, source.ID));
        }

        internal static void PopModal()
        {

            if (_formStack.Count == 1)
            {
                _formStack.Pop();

                foreach (KeyValuePair <Guid, BitForm> item in _formList)
                {
                    item.Value.Disabled = false;
                }
            }
            else
            {
                _formStack.Pop();
                _formList[_formStack.Peek()].Disabled = false;
            }

            //Debug.Log(string.Format("Pop({0})", _formStack.Count));

        }

        public static BitForm LoadForm(GameObject gameObject, Type formType, GUISkin defaultSkin)
        {
            if (gameObject == null)
            {
                throw new ApplicationException("GameObject cannot be null");
            }

            BitForm form = (BitForm) gameObject.AddComponent(formType);

            _formList.Add(form.ID, form);

            form.DefaultSkin = defaultSkin;

            form.Initialize();

            form.OnLoad();

            return form;
        }

        public static void CloseForm(BitForm form)
        {
            form.OnClose();
            _formList.Remove(form.ID);
        }

      
    }
