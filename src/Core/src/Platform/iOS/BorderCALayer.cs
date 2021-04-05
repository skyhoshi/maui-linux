using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Microsoft.Maui
{
    public class BorderCALayer : CALayer
    {
        CGRect _bounds;

        CornerRadius _cornerRadius;

        UIColor? _backgroundColor;

        float _borderWidth;
        UIColor? _borderColor;

        public BorderCALayer()
        {
            _bounds = new CGRect();

            ContentsScale = UIScreen.MainScreen.Scale;
        }

        public override void LayoutSublayers()
        {
            base.LayoutSublayers();

            if (Bounds.Equals(_bounds))
                return;

            _bounds = new CGRect(Bounds.Location, Bounds.Size);
        }

        public override void DrawInContext(CGContext ctx)
        {
            base.DrawInContext(ctx);

            ctx.AddPath(GetClipPath());
            ctx.Clip();

            DrawBackground(ctx);
            DrawBorder(ctx);
        }

        public void UpdateCornerRadius(CornerRadius cornerRadius)
        {
            _cornerRadius = cornerRadius;

            SetNeedsDisplay();
        }

        public void UpdateColor(Color color)
        {
            _backgroundColor = color.ToNative();

            SetNeedsDisplay();
        }

        public void UpdateBorderColor(Color borderColor)
        {
            _borderColor = borderColor.ToNative();

            SetNeedsDisplay();
        }

        public void UpdateBorderWidth(double borderWidth)
        {
            _borderWidth = (float)borderWidth;

            SetNeedsDisplay();
        }

        CGPath? GetClipPath() =>
            GetRoundCornersPath(Bounds, _cornerRadius, _borderWidth).CGPath;

        void DrawBackground(CGContext ctx)
        {
            if (_backgroundColor != null)
            {
                ctx.SetFillColor(_backgroundColor.CGColor);
                ctx.AddPath(GetClipPath());
                ctx.DrawPath(CGPathDrawingMode.Fill);
            }
        }

        void DrawBorder(CGContext ctx)
        {
            if (_borderWidth == 0)
                return;

            ctx.SetLineWidth(2 * _borderWidth);
            ctx.AddPath(GetClipPath());

            if (_borderColor != null)
            {
                ctx.SetStrokeColor(_borderColor.CGColor);
                ctx.DrawPath(CGPathDrawingMode.Stroke);
            }
        }

        UIBezierPath GetRoundCornersPath(CGRect bounds, CornerRadius cornerRadius, float borderWidth = 0f)
        {
            if (cornerRadius == new CornerRadius(0d))
            {
                return UIBezierPath.FromRect(bounds);
            }

            var topLeft = ValidateCornerRadius(cornerRadius.TopLeft, borderWidth);
            var topRight = ValidateCornerRadius(cornerRadius.TopRight, borderWidth);
            var bottomLeft = ValidateCornerRadius(cornerRadius.BottomLeft, borderWidth);
            var bottomRight = ValidateCornerRadius(cornerRadius.BottomRight, borderWidth);

            var bezierPath = new UIBezierPath();
            bezierPath.AddArc(new CGPoint((float)bounds.X + bounds.Width - topRight, (float)bounds.Y + topRight), topRight, (float)(Math.PI * 1.5), (float)Math.PI * 2, true);
            bezierPath.AddArc(new CGPoint((float)bounds.X + bounds.Width - bottomRight, (float)bounds.Y + bounds.Height - bottomRight), bottomRight, 0, (float)(Math.PI * .5), true);
            bezierPath.AddArc(new CGPoint((float)bounds.X + bottomLeft, (float)bounds.Y + bounds.Height - bottomLeft), bottomLeft, (float)(Math.PI * .5), (float)Math.PI, true);
            bezierPath.AddArc(new CGPoint((float)bounds.X + topLeft, (float)bounds.Y + topLeft), topLeft, (float)Math.PI, (float)(Math.PI * 1.5), true);
            bezierPath.ClosePath();

            return bezierPath;
        }

        float ValidateCornerRadius(double corner, float borderWidth)
        {
            var cornerRadius = corner - borderWidth;
            return cornerRadius > 0 ? (float)cornerRadius : 0;
        }
    }
}