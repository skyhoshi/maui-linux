using System;
using System.Graphics;
using CoreAnimation;
using CoreGraphics;
using Microsoft.Maui.Graphics;
using UIKit;

namespace Microsoft.Maui
{
	public class MauiShapeView : UIView
    {
        public MauiShapeView()
        {
            BackgroundColor = UIColor.Clear;

            ShapeLayer = new ShapeLayer();
            Layer.AddSublayer(ShapeLayer);
            Layer.MasksToBounds = false;
        }

        public ShapeLayer ShapeLayer
        {
            private set;
            get;
        }
    }

    public class ShapeLayer : CALayer
    {
        CGPath? _path;
        CGRect _pathFillBounds;
        CGRect _pathStrokeBounds;

        Shape? _shape;

        CGPath? _renderPath;

        bool _fillMode;

        Color _stroke;
        Color _fill;

        nfloat _strokeWidth;
        nfloat[]? _strokeDash;
        nfloat _dashOffset;

        Stretch _stretch;

        CGLineCap _strokeLineCap;
        CGLineJoin _strokeLineJoin;
        nfloat _strokeMiterLimit;

        public ShapeLayer()
        {
            ContentsScale = UIScreen.MainScreen.Scale;

            _fillMode = false;
            _stretch = Stretch.None;
            _strokeLineCap = CGLineCap.Butt;
            _strokeLineJoin = CGLineJoin.Miter;
            _strokeMiterLimit = 10;
        }

        public override void DrawInContext(CGContext ctx)
        {
            base.DrawInContext(ctx);
            RenderShape(ctx);
        }

        public SizeRequest GetDesiredSize()
        {
            return new SizeRequest(new Size(
                Math.Max(0, nfloat.IsNaN(_pathStrokeBounds.Right) ? 0 : _pathStrokeBounds.Right),
                Math.Max(0, nfloat.IsNaN(_pathStrokeBounds.Bottom) ? 0 : _pathStrokeBounds.Bottom)));
        }

        public void UpdateShape(Shape shape)
        {
            _shape = shape;
        }

        public void UpdateFillMode(bool fillMode)
        {
            _fillMode = fillMode;
            SetNeedsDisplay();
        }

        public void UpdateSize(CGSize size)
        {
            Bounds = new CGRect(new CGPoint(), size);

            if (_shape != null)
            {
                var rect = new RectangleF(
                    (float)Bounds.X, (float)Bounds.Y,
                    (float)Bounds.Width, (float)Bounds.Height);

                var path = _shape.CreatePath(rect);

                _path = path.ToNative();

                if (_path != null)
                    _pathFillBounds = _path.PathBoundingBox;
                else
                    _pathFillBounds = new CGRect();

                UpdatePathStrokeBounds();
                BuildRenderPath();
            }
        }

        public void UpdateStretch(Stretch stretch)
        {
            _stretch = stretch;
            BuildRenderPath();
        }

        public void UpdateFill(Color fill)
        {
            _fill = fill;
            SetNeedsDisplay();
        }

        public void UpdateStroke(Color stroke)
        {
            _stroke = stroke;
            SetNeedsDisplay();
        }

        public void UpdateStrokeThickness(double strokeWidth)
        {
            _strokeWidth = new nfloat(strokeWidth);
            BuildRenderPath();
        }

        public void UpdateStrokeDashArray(nfloat[] dash)
        {
            _strokeDash = dash;
            SetNeedsDisplay();
        }

        public void UpdateStrokeDashOffset(nfloat dashOffset)
        {
            _dashOffset = dashOffset;
            SetNeedsDisplay();
        }

        public void UpdateStrokeLineCap(CGLineCap strokeLineCap)
        {
            _strokeLineCap = strokeLineCap;
            UpdatePathStrokeBounds();
            SetNeedsDisplay();
        }

        public void UpdateStrokeLineJoin(CGLineJoin strokeLineJoin)
        {
            _strokeLineJoin = strokeLineJoin;
            UpdatePathStrokeBounds();
            SetNeedsDisplay();
        }

        public void UpdateStrokeMiterLimit(nfloat strokeMiterLimit)
        {
            _strokeMiterLimit = strokeMiterLimit;
            UpdatePathStrokeBounds();
            SetNeedsDisplay();
        }

        void BuildRenderPath()
        {
            if (_path == null)
            {
                _renderPath = null;
                return;
            }

            CATransaction.Begin();
            CATransaction.DisableActions = true;

            if (_stretch != Stretch.None)
            {
                CGRect viewBounds = Bounds;
                viewBounds.X += _strokeWidth / 2;
                viewBounds.Y += _strokeWidth / 2;
                viewBounds.Width -= _strokeWidth;
                viewBounds.Height -= _strokeWidth;

                nfloat widthScale = viewBounds.Width / _pathFillBounds.Width;
                nfloat heightScale = viewBounds.Height / _pathFillBounds.Height;
                var stretchTransform = CGAffineTransform.MakeIdentity();

                switch (_stretch)
                {
                    case Stretch.None:
                        break;

                    case Stretch.Fill:
                        stretchTransform.Scale(widthScale, heightScale);

                        stretchTransform.Translate(
                            viewBounds.Left - widthScale * _pathFillBounds.Left,
                            viewBounds.Top - heightScale * _pathFillBounds.Top);
                        break;

                    case Stretch.Uniform:
                        nfloat minScale = NMath.Min(widthScale, heightScale);

                        stretchTransform.Scale(minScale, minScale);

                        stretchTransform.Translate(
                            viewBounds.Left - minScale * _pathFillBounds.Left +
                            (viewBounds.Width - minScale * _pathFillBounds.Width) / 2,
                            viewBounds.Top - minScale * _pathFillBounds.Top +
                            (viewBounds.Height - minScale * _pathFillBounds.Height) / 2);
                        break;

                    case Stretch.UniformToFill:
                        nfloat maxScale = NMath.Max(widthScale, heightScale);

                        stretchTransform.Scale(maxScale, maxScale);

                        stretchTransform.Translate(
                            viewBounds.Left - maxScale * _pathFillBounds.Left,
                            viewBounds.Top - maxScale * _pathFillBounds.Top);
                        break;
                }

                Frame = Bounds;
                _renderPath = _path.CopyByTransformingPath(stretchTransform);
            }
            else
            {
                nfloat adjustX = NMath.Min(0, _pathStrokeBounds.X);
                nfloat adjustY = NMath.Min(0, _pathStrokeBounds.Y);

                if (adjustX < 0 || adjustY < 0)
                {
                    nfloat width = Bounds.Width;
                    nfloat height = Bounds.Height;

                    if (_pathStrokeBounds.Width > Bounds.Width)
                        width = Bounds.Width - adjustX;
                    if (_pathStrokeBounds.Height > Bounds.Height)
                        height = Bounds.Height - adjustY;

                    Frame = new CGRect(adjustX, adjustY, width, height);
                    var transform = new CGAffineTransform(Bounds.Width / width, 0, 0, Bounds.Height / height, -adjustX, -adjustY);
                    _renderPath = _path.CopyByTransformingPath(transform);
                }
                else
                {
                    Frame = Bounds;
                    _renderPath = _path.CopyByTransformingPath(CGAffineTransform.MakeIdentity());
                }
            }

            CATransaction.Commit();

            SetNeedsDisplay();
        }

        void RenderShape(CGContext graphics)
        {
            if (_path == null)
                return;

            if (_stroke == null && _fill == null)
                return;

            CATransaction.Begin();
            CATransaction.DisableActions = true;

            graphics.SetLineWidth(_strokeWidth);
            graphics.SetLineDash(_dashOffset * _strokeWidth, _strokeDash);
            graphics.SetLineCap(_strokeLineCap);
            graphics.SetLineJoin(_strokeLineJoin);
            graphics.SetMiterLimit(_strokeMiterLimit * _strokeWidth / 4);

            CGColor fillColor = UIColor.Clear.CGColor;

            if (_fill != Color.Default)
                fillColor = _fill.ToCGColor();

            graphics.AddPath(_renderPath);
            graphics.SetFillColor(fillColor);
            graphics.DrawPath(_fillMode ? CGPathDrawingMode.FillStroke : CGPathDrawingMode.EOFillStroke);

            CGColor strokeColor = UIColor.Clear.CGColor;

            if (_stroke != Color.Default)
                strokeColor = _stroke.ToCGColor();

            graphics.AddPath(_renderPath);
            graphics.SetStrokeColor(strokeColor);
            graphics.DrawPath(CGPathDrawingMode.Stroke);

            CATransaction.Commit();
        }

        void UpdatePathStrokeBounds()
        {
            if (_path != null)
                _pathStrokeBounds = _path.CopyByStrokingPath(_strokeWidth, _strokeLineCap, _strokeLineJoin, _strokeMiterLimit).PathBoundingBox;
            else
                _pathStrokeBounds = new CGRect();

            BuildRenderPath();
        }
    }
}