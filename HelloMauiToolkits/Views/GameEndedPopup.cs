using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;

namespace HelloMauiToolkits;

sealed partial class GameEndedPopup : Popup
{
	public GameEndedPopup(string title, int score, string scoreEmoji)
	{
		const int titleFontSize = 32;
		const int descriptionFontSize = 24;
		const int scoreEmojiFontSize = 64;
		const int combinedDescriptionLabelEmojiLabelHeight = 175;
		const int popupWidth = 250;
		const int popupHeight = 315;
		const int padding = 24;
		const int spacing = 12;

		var description = $"You scored {score} points!";

		Opened += HandleOpened;

		Padding = padding;
		WidthRequest = popupWidth;
		HeightRequest = popupHeight;
		BackgroundColor = ColorConstants.ButtonBackgroundColor;
		CanBeDismissedByTappingOutsideOfPopup = false;
		VerticalOptions = HorizontalOptions = LayoutOptions.Center;

		Content = new VerticalStackLayout
		{
			Spacing = spacing,
			Children =
			{
				new GamedEndedLabel(titleFontSize, title)
					.Margins(bottom: 8),

				new GamedEndedLabel(descriptionFontSize, description)
					.Assign(out Label descriptionLabel),

				new GamedEndedLabel(scoreEmojiFontSize, scoreEmoji)
					.Bind(Label.HeightRequestProperty,
							static (Label descriptionLabel) => descriptionLabel.Height,
							convert: (double descriptionLabelHeight) => combinedDescriptionLabelEmojiLabelHeight - descriptionLabelHeight,
							source: descriptionLabel)
			}
		};
	}

	public static PopupOptions PopupOptions { get; } = new()
	{
		Shape = new RoundRectangle
		{
			CornerRadius = new CornerRadius(40),
			StrokeThickness = 16,
			Stroke = ColorConstants.ButtonBackgroundColor
		}
	};

	async void HandleOpened(object? sender, EventArgs e)
	{
		await Task.Delay(GameConstants.GameEndPopupDisplayTime);
		await CloseAsync();
	}

	sealed partial class GamedEndedLabel : Label
	{
		public GamedEndedLabel(int fontSize, string text)
		{
			this.Text(text, Colors.White)
				.Center()
				.TextCenter()
				.Font(size: fontSize);
		}
	}
}