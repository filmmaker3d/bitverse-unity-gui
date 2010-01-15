using UnityEngine;


    public class BitDrawTexture : BitControl
    {

        [SerializeField]
        private Texture _image;
        [SerializeField]
        private ScaleMode _scaleMode = ScaleMode.StretchToFill;
        [SerializeField]
        private bool _alphaBlend = true;
        [SerializeField]
        private float _imageAspect = 0;
      
        public Texture Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public ScaleMode ScaleMode
        {
            get { return _scaleMode; }
            set { _scaleMode = value; }
        }

        public bool AlphaBlend
        {
            get { return _alphaBlend; }
            set { _alphaBlend = value; }
        }

        public float ImageAspect
        {
            get { return _imageAspect; }
            set { _imageAspect = value; }
        }

        public override void DoDraw()
        {
            if(Image != null)
                UnityEngine.GUI.DrawTexture(Position, Image, ScaleMode, AlphaBlend, ImageAspect);
        }

       
    }
