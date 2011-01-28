using UnityEngine;


public class BitSprite : BitPicture
{
    [SerializeField]
    private Texture2D[] _images = { };

    [SerializeField]
    private float _timeBetweenFrames = 1.0f;

    public float TimeBetweenFrames
    {
        get { return _timeBetweenFrames; }
        set { _timeBetweenFrames = value; }
    }

    private float _lastFrameTime = 0.0f;
    private int _frameIndex = 0;

    #region Draw

    protected override void DoDraw()
    {
        if (_images.Length > 0)
        {
            float currentTime = Time.time;

            if ((currentTime-_lastFrameTime) > TimeBetweenFrames)
            {
                _frameIndex++;
                if (_frameIndex > (_images.Length - 1))
                    _frameIndex = 0;
                Image = _images[_frameIndex];
                _lastFrameTime = currentTime;
            }
            base.DoDraw();
        }
    }

    #endregion
}