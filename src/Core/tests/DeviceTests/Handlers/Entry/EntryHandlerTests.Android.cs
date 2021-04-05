using System.Linq;
using System.Threading.Tasks;
using Android.Text;
using Android.Views.InputMethods;
using AndroidX.AppCompat.Widget;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Handlers;
using Xunit;
using AColor = Android.Graphics.Color;

namespace Microsoft.Maui.DeviceTests
{
	public partial class EntryHandlerTests
	{
		[Theory(DisplayName = "Font Family Initializes Correctly")]
		[InlineData(null)]
		[InlineData("monospace")]
		[InlineData("Dokdo")]
		public async Task FontFamilyInitializesCorrectly(string family)
		{
			var entry = new EntryStub
			{
				Text = "Test",
				Font = Font.OfSize(family, 10)
			};

			var handler = await CreateHandlerAsync(entry);
			var nativeEntry = GetNativeEntry(handler);

			var fontManager = handler.Services.GetRequiredService<IFontManager>();

			var nativeFont = fontManager.GetTypeface(Font.OfSize(family, 0.0));

			Assert.Equal(nativeFont, nativeEntry.Typeface);

			if (string.IsNullOrEmpty(family))
				Assert.Equal(fontManager.DefaultTypeface, nativeEntry.Typeface);
			else
				Assert.NotEqual(fontManager.DefaultTypeface, nativeEntry.Typeface);
		}

		[Fact(DisplayName = "ReturnType Initializes Correctly")]
		public async Task ReturnTypeInitializesCorrectly()
		{
			var xplatReturnType = ReturnType.Next;
			var entry = new EntryStub()
			{
				Text = "Test",
				ReturnType = xplatReturnType
			};

			ImeAction expectedValue = ImeAction.Next;

			var values = await GetValueAsync(entry, (handler) =>
			{
				return new
				{
					ViewValue = entry.ReturnType,
					NativeViewValue = GetNativeReturnType(handler)
				};
			});

			Assert.Equal(xplatReturnType, values.ViewValue);
			Assert.Equal(expectedValue, values.NativeViewValue);
		}

		[Fact(DisplayName = "Horizontal TextAlignment Initializes Correctly")]
		public async Task HorizontalTextAlignmentInitializesCorrectly()
		{
			var xplatHorizontalTextAlignment = TextAlignment.End;

			var entry = new EntryStub()
			{
				Text = "Test",
				HorizontalTextAlignment = xplatHorizontalTextAlignment
			};

			Android.Views.TextAlignment expectedValue = Android.Views.TextAlignment.ViewEnd;

			var values = await GetValueAsync(entry, (handler) =>
			{
				return new
				{
					ViewValue = entry.HorizontalTextAlignment,
					NativeViewValue = GetNativeTextAlignment(handler)
				};
			});

			Assert.Equal(xplatHorizontalTextAlignment, values.ViewValue);
			values.NativeViewValue.AssertHasFlag(expectedValue);
		}

		[Fact(DisplayName = "CharacterSpacing Initializes Correctly")]
		public async Task CharacterSpacingInitializesCorrectly()
		{
			var xplatCharacterSpacing = 4;

			var entry = new EntryStub()
			{
				CharacterSpacing = xplatCharacterSpacing,
				Text = "Some Test Text"
			};

			float expectedValue = entry.CharacterSpacing.ToEm();

			var values = await GetValueAsync(entry, (handler) =>
			{
				return new
				{
					ViewValue = entry.CharacterSpacing,
					NativeViewValue = GetNativeCharacterSpacing(handler)
				};
			});

			Assert.Equal(xplatCharacterSpacing, values.ViewValue);
			Assert.Equal(expectedValue, values.NativeViewValue, EmCoefficientPrecision);
		}

		[Fact(DisplayName = "CornerRadius Initializes Correctly")]
		public async Task CornerRadiusInitializesCorrectly()
		{
			var expected = new CornerRadius(12, 0, 0, 24);

			var entry = new EntryStub()
			{
				CornerRadius = expected,
				BorderColor = Color.OrangeRed,
				BorderWidth = 4,
				Text = "Test"
			};

			var values = await GetValueAsync(entry, (handler) =>
			{
				return new
				{
					ViewValue = entry.CornerRadius,
					NativeViewValue = GetNativeCornerRadius(handler)
				};
			});

			Assert.Equal(expected, values.ViewValue);
			Assert.Equal(expected, values.NativeViewValue);
		}

		[Fact(DisplayName = "Border Color Initializes Correctly")]
		public async Task BorderColorInitializesCorrectly()
		{
			var xplatBorderColor = Color.Red;

			var entry = new EntryStub()
			{
				BorderColor = xplatBorderColor,
				BorderWidth = 4,
				Text = "Test"
			};

			AColor? expectedValue = xplatBorderColor.ToNative();

			var values = await GetValueAsync(entry, (handler) =>
			{
				return new
				{
					ViewValue = entry.BorderColor,
					NativeViewValue = GetNativeBorderColor(handler)
				};
			});

			Assert.Equal(xplatBorderColor, values.ViewValue);
			Assert.Equal(expectedValue, values.NativeViewValue);
		}

		[Theory(DisplayName = "Border Width Initializes Correctly")]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		public async Task BorderWidthInitializesCorrectly(double borderWidth)
		{
			var xplatBorderWidth = borderWidth;

			var entry = new EntryStub()
			{
				BorderColor = Color.Black,
				BorderWidth = xplatBorderWidth,
				Text = "Test"
			};

			var values = await GetValueAsync(entry, (handler) =>
			{
				return new
				{
					ViewValue = entry.BorderWidth,
					NativeViewValue = GetNativeBorderWidth(handler)
				};
			});

			Assert.Equal(xplatBorderWidth, values.ViewValue);
			Assert.NotEqual(0, values.NativeViewValue);
		}

		AppCompatEditText GetNativeEntry(EntryHandler entryHandler) =>
			(AppCompatEditText)entryHandler.View;

		string GetNativeText(EntryHandler entryHandler) =>
			GetNativeEntry(entryHandler).Text;

		void SetNativeText(EntryHandler entryHandler, string text) =>
			GetNativeEntry(entryHandler).Text = text;

		Color GetNativeTextColor(EntryHandler entryHandler)
		{
			int currentTextColorInt = GetNativeEntry(entryHandler).CurrentTextColor;
			AColor currentTextColor = new AColor(currentTextColorInt);
			return currentTextColor.ToColor();
		}

		bool GetNativeIsPassword(EntryHandler entryHandler)
		{
			var inputType = GetNativeEntry(entryHandler).InputType;
			return inputType.HasFlag(InputTypes.TextVariationPassword) || inputType.HasFlag(InputTypes.NumberVariationPassword);
		}

		bool GetNativeIsTextPredictionEnabled(EntryHandler entryHandler) =>
			!GetNativeEntry(entryHandler).InputType.HasFlag(InputTypes.TextFlagNoSuggestions);

		string GetNativePlaceholder(EntryHandler entryHandler) =>
			GetNativeEntry(entryHandler).Hint;

		bool GetNativeIsReadOnly(EntryHandler entryHandler)
		{
			var editText = GetNativeEntry(entryHandler);

			return !editText.Focusable && !editText.FocusableInTouchMode;
		}

		double GetNativeUnscaledFontSize(EntryHandler entryHandler)
		{
			var textView = GetNativeEntry(entryHandler);
			return textView.TextSize / textView.Resources.DisplayMetrics.Density;
		}

		bool GetNativeIsBold(EntryHandler entryHandler) =>
			GetNativeEntry(entryHandler).Typeface.IsBold;

		bool GetNativeIsItalic(EntryHandler entryHandler) =>
			GetNativeEntry(entryHandler).Typeface.IsItalic;

		Android.Views.TextAlignment GetNativeTextAlignment(EntryHandler entryHandler) =>
			GetNativeEntry(entryHandler).TextAlignment;

		ImeAction GetNativeReturnType(EntryHandler entryHandler) =>
			GetNativeEntry(entryHandler).ImeOptions;

		bool GetNativeClearButtonVisibility(EntryHandler entryHandler)
		{
			var nativeEntry = GetNativeEntry(entryHandler);
			var unfocusedDrawables = nativeEntry.GetCompoundDrawables();

			bool compoundsValidWhenUnfocused = !unfocusedDrawables.Any(a => a != null);

			// This will display 'X' drawable.
			nativeEntry.RequestFocus();

			var focusedDrawables = nativeEntry.GetCompoundDrawables();

			// Index 2 for FlowDirection.LeftToRight.
			bool compoundsValidWhenFocused = focusedDrawables.Length == 4 && focusedDrawables[2] != null;

			return compoundsValidWhenFocused && compoundsValidWhenUnfocused;
		}

		double GetNativeCharacterSpacing(EntryHandler entryHandler)
		{
			var editText = GetNativeEntry(entryHandler);

			if (editText != null)
			{
				return editText.LetterSpacing;
			}

			return -1;
		}

		CornerRadius GetNativeCornerRadius(EntryHandler entryHandler)
		{
			var entry = GetNativeEntry(entryHandler);
			var borderDrawable = entry.GetBorderDrawable();

			if (borderDrawable != null)
				return borderDrawable.CornerRadius;

			return new CornerRadius();
		}

		AColor? GetNativeBorderColor(EntryHandler entryHandler)
		{
			var entry = GetNativeEntry(entryHandler);
			var borderDrawable = entry.GetBorderDrawable();

			if (borderDrawable != null)
				return borderDrawable.BorderColor;

			return null;
		}

		float? GetNativeBorderWidth(EntryHandler entryHandler)
		{
			var entry = GetNativeEntry(entryHandler);
			var borderDrawable = entry.GetBorderDrawable();

			if (borderDrawable != null)
				return borderDrawable.BorderWidth;

			return null;
		}
	}
}