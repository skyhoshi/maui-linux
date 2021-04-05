using Android.Content;
using Android.Content.Res;
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
        Paint? _strokePaint;

        CornerRadius _cornerRadius;

        AColor? _color;
        AColor? _strokeColor;

        float _strokeWidth;

        public BorderShapeDrawable(IView? view = null)
        {
            _invalidatePath = true;

            _clipPath = new Path();
            _density = Resources?.DisplayMetrics?.Density ?? 1.0f;

            if (view == null)
                return;

            UpdateBackgroundColor(view.BackgroundColor);
            UpdateBorderWidth(view.BorderWidth);
            UpdateBorderColor(view.BorderColor);
        }

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

            if (CanDrawBorder())
                _strokePaint?.StrokeWidth = _strokeWidth;

            if (_color != null)
                Paint?.Color = _color.Value;

            if (CanDrawBorder() && _strokeColor != null)
                _strokePaint?.Color = _strokeColor.Value;

            if (_invalidatePath)
            {
                _invalidatePath = false;

                var clipPath = GetRoundCornersPath(_width, _height, _strokeWidth, _cornerRadius);

                if (clipPath == null)
                    return;

                _clipPath.Reset();
                _clipPath.Set(clipPath);

                if (_maskPath != null && CanDrawBorder())
                {
                    _maskPath.Reset();
                    _maskPath.AddRect(0, 0, _width, _height, Path.Direction.Cw!);
                    _maskPath.InvokeOp(_clipPath, Path.Op.Difference!);
                }
            }

            if (CanDrawBorder())
            {
                var saveCount = canvas.SaveLayer(0, 0, _width, _height, null);
                canvas.DrawPath(_clipPath, Paint);
                canvas.DrawPath(_clipPath, _strokePaint);
                canvas.DrawPath(_maskPath, _maskPaint);
                canvas.RestoreToCount(saveCount);
            }
            else
                canvas.DrawPath(_clipPath, Paint);
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
            return _strokeWidth > 0;
        }

        bool CanDrawBorder()
        {
            InitializeBorderIfNeeded();

            return _strokeWidth > 0;
        }

        void InitializeBorderIfNeeded()
        {
            if (!HasBorder())
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

            if (_strokePaint == null)
            {
                _strokePaint = new Paint(PaintFlags.AntiAlias);
                _strokePaint.SetStyle(Paint.Style.Stroke);
            }
        }

        public void UpdateBackgroundColor(Color color)
        {
            _color = color == Color.Default
                ? null : (AColor?)color.ToNative();

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

            _strokeColor = strokeColor == Color.Default
                ? null : (AColor?)strokeColor.ToNative();

            InitializeBorderIfNeeded();
            InvalidateSelf();
        }

        public void UpdateBorderWidth(double strokeWidth)
        {
            _invalidatePath = true;

            _strokeWidth = (float)(strokeWidth * _density);

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

                if (_strokePaint != null)
                {
                    _strokePaint.Dispose();
                    _strokePaint = null;
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