//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.Text;
//using System.Drawing.Html;
//using bitgui;
//using UnityEngine;
//using Graphics = System.Drawing.Graphics;
//using Random = UnityEngine.Random;


//public class BitRichText : BitControl
//{
//    //otimizações...
//    //usar POT para otimizar espaço em memória.

//    public bool noClipAndVerticalResize = false;

//    private string lastText = "";
//    private Bitverse.Unity.Gui.Size lastSize;
//    private Bitverse.Unity.Gui.Size lastParentSize;

//    private bool textureChanged;

//    private float lastResizeUpdate;

//    protected override void DoDraw()
//    {

//        try
//        {
//            if (Event.current.type != EventType.Repaint)
//                return;
//            if (Application.isPlaying)
//            {
//                Texture2D texture = Stage.TextureCache.Get(Content.text);
//                if (texture == null)
//                {
//                    textureChanged = true;
//                    texture = UpdateImage();
//                }
//                else if (!lastSize.Equals(Size))
//                {
//                    if (Time.time > (lastResizeUpdate + 0.2f))
//                    {
//                        textureChanged = true;
//                        lastSize = Size;
//                        texture = UpdateImage();
//                        lastResizeUpdate = Time.time;
//                    }
//                }
//                else if (lastText != Content.text)
//                {
//                    textureChanged = true;
//                    texture = UpdateImage();
//                    lastText = Content.text;
//                }
//                else if ((noClipAndVerticalResize) && (!lastParentSize.Equals(Parent.Size)))
//                {
//                    if (Time.time > (lastResizeUpdate + 0.2f))
//                    {
//                        textureChanged = true;
//                        lastParentSize = Parent.Size;
//                        texture = UpdateImage();
//                        lastResizeUpdate = Time.time;
//                    }
//                }
//                if (texture != null)
//                {
//                    if (textureChanged)
//                    {
//                        Stage.TextureCache.Put(Content.text, texture, 1f);
//                        textureChanged = false;
//                    }
//                    GUI.DrawTexture(new Rect(Position.x, Position.y, width, height), texture);
//                }
//            }
//        }
//        catch (Exception e)
//        {
//            Debug.Log(e.ToString());
//        }
//    }

//    private int width;
//    private int height;

//    private Texture2D UpdateImage()
//    {
//        Texture2D texture = null;
//        try
//        {
//            Bitmap bmp = new Bitmap(10, 10);
//            Graphics g = Graphics.FromImage(bmp);
//            InitialContainer container = new InitialContainer(Content.text);
//            Region prevClip = g.Clip;
//            RectangleF area;
//            if (noClipAndVerticalResize)
//            {
//                area = new RectangleF(0, 0, Parent.Size.Width, Parent.Size.Height);
//                g.SetClip(area);
//            }
//            else
//            {
//                area = new RectangleF(0, 0, width, height);
//            }
//            container.SetBounds(area);
//            container.MeasureBounds(g);
//            SizeF maximum = container.MaximumSize;
//            bmp.Dispose();

//            if (noClipAndVerticalResize)
//            {
//                width = (int)Position.width;
//                height = (int)maximum.Height;
//                Size = new Bitverse.Unity.Gui.Size(width, height);
//            }
//            else
//            {
//                width = (int)Position.width;
//                height = (int)Position.height;
//            }

//            Bitmap bitmap = new Bitmap(width, height);
//            Graphics graphics = Graphics.FromImage(bitmap);

//            container.Paint(graphics);

//            if (noClipAndVerticalResize) graphics.SetClip(prevClip, System.Drawing.Drawing2D.CombineMode.Replace);

//            texture = new Texture2D(width, height);
//            //optimize using lockbits
//            for (int x = 0; x < bitmap.Width; x++)
//            {
//                for (int y = 0; y < bitmap.Height; y++)
//                {
//                    System.Drawing.Color color = bitmap.GetPixel(x, y);
//                    texture.SetPixel(x, (int)Position.height - 1 - y, new UnityEngine.Color(color.R, color.G, color.B, color.A));
//                }
//            }
//            texture.Apply();
//            bitmap.Dispose();
//        }
//        catch (Exception e)
//        {
//            Debug.Log(e.ToString());
//        }
//        return texture;
//    }


//}

