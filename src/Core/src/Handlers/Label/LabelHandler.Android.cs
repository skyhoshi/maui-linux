using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Java.Lang;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Maui.Handlers
{
	public class MyTextView : TextView 
	{
		public MyTextView(Context? context) : base(context)
		{
		}

		public MyTextView(Context? context, IAttributeSet? attrs) : base(context, attrs)
		{
		}

		public MyTextView(Context? context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
		}

		public MyTextView(Context? context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
		}

		protected MyTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public override void SetText(ICharSequence? text, BufferType? type)
		{
			base.SetText(text, type);
			//RequestLayout();
		}
	}

	public partial class LabelHandler : AbstractViewHandler<ILabel, TextView>
	{
		static Color DefaultTextColor { get; set; }
		static float LineSpacingAddDefault { get; set; }
		static float LineSpacingMultDefault { get; set; }

		protected override TextView CreateNativeView() => new MyTextView(Context);

		protected override void SetupDefaults(TextView nativeView)
		{
			if (nativeView.TextColors == null)
			{
				DefaultTextColor = Color.Default;
			}
			else
			{
				DefaultTextColor = Color.FromUint((uint)nativeView.TextColors.DefaultColor);
			}
			LineSpacingAddDefault = nativeView.LineSpacingExtra;
			LineSpacingMultDefault = nativeView.LineSpacingMultiplier;
		}

		public static void MapText(LabelHandler handler, ILabel label)
		{
			handler.TypedNativeView?.UpdateText(label);
		}

		public static void MapTextColor(LabelHandler handler, ILabel label)
		{
			handler.TypedNativeView?.UpdateTextColor(label, DefaultTextColor);
		}

		public static void MapCharacterSpacing(LabelHandler handler, ILabel label)
		{
			handler.TypedNativeView?.UpdateCharacterSpacing(label);
		}

		public static void MapHorizontalTextAlignment(LabelHandler handler, ILabel label)
		{
			handler.TypedNativeView?.UpdateHorizontalTextAlignment(label);
		}

		public static void MapLineBreakMode(LabelHandler handler, ILabel label)
		{
			handler.TypedNativeView?.UpdateLineBreakMode(label);
		}

		public static void MapMaxLines(LabelHandler handler, ILabel label)
		{
			handler.TypedNativeView?.UpdateMaxLines(label);
		}

		public static void MapPadding(LabelHandler handler, ILabel label)
		{
			handler.TypedNativeView?.UpdatePadding(label);
		}

		public static void MapTextDecorations(LabelHandler handler, ILabel label)
		{
			handler.TypedNativeView?.UpdateTextDecorations(label);
		}

		public static void MapFont(LabelHandler handler, ILabel label)
		{
			_ = handler.Services ?? throw new InvalidOperationException($"{nameof(Services)} should have been set by base class.");

			var fontManager = handler.Services.GetRequiredService<IFontManager>();

			handler.TypedNativeView?.UpdateFont(label, fontManager);
		}

		public static void MapLineHeight(LabelHandler handler, ILabel label)
		{
			 handler.TypedNativeView?.UpdateLineHeight(label, LineSpacingAddDefault, LineSpacingMultDefault);
		}
	}
}