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

	public static readonly BindableProperty HighScoreProperty = BindableProperty.Create(
		nameof(HighScore),
		typeof(int),
		typeof(HighScoreLabel),
		0,
		propertyChanged: HandleHighScoreChanged);

	public static readonly BindableProperty CelebrationTextColorProperty = BindableProperty.Create(
		nameof(CelebrationTextColor),
		typeof(Color),
		typeof(HighScoreLabel),
		Colors.DarkGreen);

	public int HighScore
	{
		get => (int)GetValue(HighScoreProperty);
		set => SetValue(HighScoreProperty, value);
	}

	public Color CelebrationTextColor
	{
		get => (Color)GetValue(CelebrationTextColorProperty);
		set => SetValue(CelebrationTextColorProperty, value);
	}

	static async void HandleHighScoreChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var highScoreLabel = (HighScoreLabel)bindable;

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