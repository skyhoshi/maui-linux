using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using AColor = Android.Graphics.Color;
using ARect = Android.Graphics.Rect;

namespace Microsoft.Maui
{
    public class BorderShapeDrawable : ShapeDrawable
    {
        readonly double _density;

        bool _invalidatePath;

        bool _disposed;

        int _width;
        int _height;

        Path? _clipPath;
        Path? _maskPath;
        Paint? _maskPaint;
        Paint? _borderPaint;

        CornerRadius _cornerRadius;

        AColor? _backgroundColor;
        AColor? _borderColor;

        float _borderWidth;

        public BorderShapeDrawable(Context? context, IView? view = null)
            : base(new RectShape())
        {
            _invalidatePath = true;

            _clipPath = new Path();

			_density = context?.Resources?.DisplayMetrics?.Density ?? 1.0f;

            if (view == null)
                return;

            UpdateBackgroundColor(view.BackgroundColor);
            UpdateBorderWidth(view.BorderWidth);
            UpdateBorderColor(view.BorderColor);
        }

        internal AColor? BackgroundColor => _backgroundColor;

        internal AColor? BorderColor => _borderColor;

        internal float? BorderWidth => _borderColor;

        internal CornerRadius CornerRadius => _cornerRadius;

        protected override void OnBoundsChange(ARect? bounds)
        {
            if (bounds != null)
            {
                var width = bounds.Width();
                var height = bounds.Height();

                if (_width == width && _height == height)
                    return;

                _invalidatePath = true;

                _width = width;
                _height = height;
            }

            base.OnBoundsChange(bounds);
        }

        protected override void OnDraw(Shape? shape, Canvas? canvas, Paint? paint)
        {
            if (_disposed)
                return;

            if (_borderPaint != null && HasBorder())
                _borderPaint.StrokeWidth = _borderWidth;

            if (Paint != null && _backgroundColor != null)
                Paint.Color = _backgroundColor.Value;

            if (_borderPaint != null && HasBorder() && _borderColor != null)
                _borderPaint.Color = _borderColor.Value;

            if (_invalidatePath)
            {
                _invalidatePath = false;

                var clipPath = GetRoundCornersPath(_width, _height, _borderWidth, _cornerRadius);

                if (clipPath == null)
                    return;

                if (_clipPath != null)
                {
                    _clipPath.Reset();
                    _clipPath.Set(clipPath);

                    if (_maskPath != null && HasBorder())
                    {
                        _maskPath.Reset();
                        _maskPath.AddRect(0, 0, _width, _height, Path.Direction.Cw!);
                        _maskPath.InvokeOp(_clipPath, Path.Op.Difference!);
                    }
                }
            }

            if (canvas == null)
                return;

            if (HasBorder())
            {
                var saveCount = canvas.SaveLayer(0, 0, _width, _height, null);

                if (_clipPath != null && Paint != null)
                    canvas.DrawPath(_clipPath, Paint);

                if (_clipPath != null && _borderPaint != null)
                    canvas.DrawPath(_clipPath, _borderPaint);

                if (_maskPath != null && _maskPaint != null)
                    canvas.DrawPath(_maskPath, _maskPaint);

                canvas.RestoreToCount(saveCount);
            }
            else
            {
                if (_clipPath != null && Paint != null)
                    canvas.DrawPath(_clipPath, Paint);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            if (disposing)
            {
                if (_clipPath != null)
                {
                    _clipPath.Dispose();
                    _clipPath = null;
                }
            }

            DisposeBorder(disposing);

            base.Dispose(disposing);
        }

        bool HasBorder()
        {
            InitializeBorderIfNeeded();

            return _borderWidth > 0;
        }

        void InitializeBorderIfNeeded()
        {
			if (_borderWidth == 0)
			{
				DisposeBorder(true);
				return;
			}

            if (_maskPath == null)
                _maskPath = new Path();

            if (_maskPaint == null)
            {
                _maskPaint = new Paint(PaintFlags.AntiAlias);
                _maskPaint.SetStyle(Paint.Style.FillAndStroke);

                PorterDuffXfermode porterDuffClearMode = new PorterDuffXfermode(PorterDuff.Mode.Clear);
                _maskPaint.SetXfermode(porterDuffClearMode);
            }

            if (_borderPaint == null)
            {
                _borderPaint = new Paint(PaintFlags.AntiAlias);
                _borderPaint.SetStyle(Paint.Style.Stroke);
            }
        }

        public void UpdateBackgroundColor(Color color)
        {
            _backgroundColor = color == Color.Default ? AColor.Transparent : color.ToNative();

            InvalidateSelf();
        }

        public void UpdateCornerRadius(CornerRadius cornerRadius)
        {
            _invalidatePath = true;

            _cornerRadius = cornerRadius;

            InitializeBorderIfNeeded();
            InvalidateSelf();
        }

        public void UpdateBorderColor(Color strokeColor)
        {
            _invalidatePath = true;

            _borderColor = strokeColor == Color.Default ? AColor.Transparent : strokeColor.ToNative();

            InitializeBorderIfNeeded();
            InvalidateSelf();
        }

        public void UpdateBorderWidth(double strokeWidth)
        {
            _invalidatePath = true;

            _borderWidth = (float)(strokeWidth * _density);

            InitializeBorderIfNeeded();
            InvalidateSelf();
        }

        protected virtual void DisposeBorder(bool disposing)
        {
            if (disposing)
            {
                if (_maskPath != null)
                {
                    _maskPath.Dispose();
                    _maskPath = null;
                }

                if (_maskPaint != null)
                {
                    _maskPaint.SetXfermode(null);
                    _maskPaint.Dispose();
                    _maskPaint = null;
                }

                if (_borderPaint != null)
                {
                    _borderPaint.Dispose();
                    _borderPaint = null;
                }
            }
        }

        Path GetRoundCornersPath(int width, int height, float borderWidth, CornerRadius cornerRadius)
        {
            var path = new Path();

            var cornerRadii = new[]
            {
                ToPixels(cornerRadius.TopLeft, _density),
                ToPixels(cornerRadius.TopLeft, _density),

                ToPixels(cornerRadius.TopRight, _density),
                ToPixels(cornerRadius.TopRight, _density),

                ToPixels(cornerRadius.BottomRight, _density),
                ToPixels(cornerRadius.BottomRight, _density),

                ToPixels(cornerRadius.BottomLeft, _density),
                ToPixels(cornerRadius.BottomLeft, _density)
            };


            var xPlatBorderWidth = (float)(borderWidth / _density / 2);

            path.AddRoundRect(xPlatBorderWidth, xPlatBorderWidth, width - xPlatBorderWidth, height - xPlatBorderWidth, cornerRadii, Path.Direction.Cw!);

            return path;
        }

        float ToPixels(double units, double density) =>
            (float)(units * density);
    }
}