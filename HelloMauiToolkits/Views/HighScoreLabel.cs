using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;

namespace HelloMauiToolkits;

sealed partial class HighScoreLabel : Label
{
	public HighScoreLabel()
	{
		this.Center()
			.TextCenter()
			.TextColor(ColorConstants.TapGameLabelTextColor)
			.Font(size: 36, bold: true);
	}

	[BindableProperty(PropertyChangedMethodName = nameof(HandleHighScoreChanged))]
	public partial int HighScore { get; set; }

	[BindableProperty]
	public partial Color CelebrationTextColor { get; set; } = Colors.DarkGreen;

	static async void HandleHighScoreChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var highScoreLabel = (HighScoreLabel)bindable;

		// Don't celebrate the saved High Score that is applied when TapGamePage is first created
		if (!highScoreLabel.IsLoaded || (int)newValue <= (int)oldValue)
			return;

		await highScoreLabel.AnimateNewHighScore();
	}

	async Task AnimateNewHighScore()
	{
		var originalTextColor = TextColor;

		var changeTextColorTask = this.TextColorTo(CelebrationTextColor, length: 50);
		var scaleTask = this.ScaleToAsync(1.15, 110);
		var minimumAnimationTimeTask = Task.Delay(GameConstants.GameEndPopupDisplayTime);

		await Task.WhenAll(changeTextColorTask, scaleTask);

		scaleTask = this.ScaleToAsync(1.0, 100);

		await Task.WhenAll(scaleTask, minimumAnimationTimeTask);

		await this.TextColorTo(originalTextColor, length: 500);
	}
}