using System;
using UnityEngine;
using Bitverse.Unity.Gui;


public class BitScrollView : BitContainerControl
    {
        [SerializeField]
        private Vector2 _scrollPosition;
        [SerializeField]
        private Rect _viewRect;

        public Vector2 ScrollPosition
        {
            get { return _scrollPosition; }
            set { _scrollPosition = value; }
        }

        public Rect ViewRect
        {
            get { return _viewRect; }
            set { _viewRect = value; }
        }

        public override void DoDraw()
        {

            if (Style != null)
            {
                ScrollPosition = UnityEngine.GUI.BeginScrollView(Position, ScrollPosition, _viewRect, Style, Style);
            }
            else
            {
                ScrollPosition = UnityEngine.GUI.BeginScrollView(Position, ScrollPosition, _viewRect);
            }

            DrawChildren();

            UnityEngine.GUI.EndScrollView();

        }
    }
