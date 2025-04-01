using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;
using LayoutAlignment = Microsoft.Maui.Primitives.LayoutAlignment;

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
		const int padding = 24;
		const int spacing = 12;

		var description = $"You scored {score} points!";

		VerticalOptions = HorizontalOptions = LayoutAlignment.Center;

		Opened += HandleOpened;
		Color = Colors.Transparent;
		CanBeDismissedByTappingOutsideOfPopup = false;

		Content = new Border
		{
			BackgroundColor = ColorConstants.ButtonBackgroundColor,
			StrokeThickness = 16,
			Stroke = ColorConstants.ButtonBackgroundColor,
#if Android
			StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(40) },
#endif

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
			}.Size(popupWidth, 315)
			 .Padding(padding)
		};
	}

	async void HandleOpened(object? sender, PopupOpenedEventArgs e)
	{
		await Task.Delay(TimeSpan.FromSeconds(3));
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