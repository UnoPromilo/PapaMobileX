using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace PapaMobileX.App;

public class VideoImageView : ImageView
{
    private Bitmap? _oldBitmap;
    
    protected VideoImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
    {
        InitView(null);
    }

    public VideoImageView(Context context) : base(context)
    {
        InitView(context);
    }

    public VideoImageView(Context context, IAttributeSet attrs) : base(context, attrs)
    {
        InitView(context);
    }

    public VideoImageView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
    {
        InitView(context);
    }

    public VideoImageView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) :
        base(context, attrs, defStyleAttr, defStyleRes)
    {
        InitView(context);
    }

    private void InitView(Context? context)
    {
    }

    public void SetFrame(Stream? frame)
    {
        Bitmap? bitmap = BitmapFactory.DecodeStream(frame);
        SetImageBitmap(bitmap);
        if(_oldBitmap is {IsRecycled:false})
            _oldBitmap.Recycle();
        _oldBitmap = bitmap;
    }
}