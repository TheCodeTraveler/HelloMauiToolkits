namespace HelloMauiToolkits;

partial class TapGameViewModel : BaseViewModel
{
	readonly WeakEventManager _gameEndedWeakEventManager = new();
	readonly TapCountService _tapCountService;
	readonly IDispatcher _dispatcher;

	public TapGameViewModel(TapCountService tapCountService, IDispatcher dispatcher)
	{
		_dispatcher = dispatcher;
		_tapCountService = tapCountService;

		HighScore = tapCountService.TapCountHighScore;

		GameButtonTappedCommand = new Command<string?>(GameButtonTapped, _ => CanGameButtonTappedCommandExecute);
		UpdateHighScoreCommand = new Command<int>(UpdateHighScore);
	}

	public event EventHandler<GameEndedEventArgs> GameEnded
	{
		add => _gameEndedWeakEventManager.AddEventHandler(value);
		remove => _gameEndedWeakEventManager.RemoveEventHandler(value);
	}

	public Command GameButtonTappedCommand { get; }
	public Command UpdateHighScoreCommand { get; }

	public bool CanGameButtonTappedCommandExecute
	{
		get;
		set
		{
			if (SetProperty(ref field, value))
				GameButtonTappedCommand.ChangeCanExecute();
		}
	} = true;

	public string GameButtonText
	{
		get;
		set => SetProperty(ref field, value);
	} = GameConstants.GameButtonText_Start;

	public int TapCount
	{
		get;
		set => SetProperty(ref field, value);
	}

	public int HighScore
	{
		get;
		set => SetProperty(ref field, value);
	}

	public int TimerSecondsRemaining
	{
		get;
		set => SetProperty(ref field, value);
	} = GameConstants.GameDuration.Seconds;

	void GameButtonTapped(string? buttonText)
	{
		ArgumentNullException.ThrowIfNull(buttonText);

		if (buttonText is GameConstants.GameButtonText_Start)
		{
			GameButtonText = GameConstants.GameButtonText_Tap;
			StartGame();
		}
		else if (buttonText is GameConstants.GameButtonText_Tap)
		{
			TapCount++;
		}
		else
		{
			throw new NotSupportedException("Invalid Game State");
		}
	}

	void UpdateHighScore(int score)
	{
		_tapCountService.TapCountHighScore = HighScore = score;
	}

	void StartGame()
	{
		var timer = _dispatcher.CreateTimer();
		timer.Interval = TimeSpan.FromSeconds(1);

		timer.Tick += HandleTimerTicked;

		TapCount = 0;

		timer.Start();
	}

	async Task EndGame(int score)
	{
		try
		{
			CanGameButtonTappedCommandExecute = false;

			OnGameEnded(new GameEndedEventArgs(score));

			TimerSecondsRemaining = GameConstants.GameDuration.Seconds;
			GameButtonText = GameConstants.GameButtonText_Start;

			await Task.Delay(TimeSpan.FromSeconds(GameConstants.GameEndPopupDisplayTime.Seconds));
		}
		finally
		{
			CanGameButtonTappedCommandExecute = true;
		}
	}

	async void HandleTimerTicked(object? sender, EventArgs e)
	{
		TimerSecondsRemaining--;

		if (TimerSecondsRemaining is 0)
		{
			ArgumentNullException.ThrowIfNull(sender);

			var timer = (IDispatcherTimer)sender;

			timer.Stop();

			await EndGame(TapCount);
		}
	}

	void OnGameEnded(GameEndedEventArgs eventArgs) => _gameEndedWeakEventManager.HandleEvent(this, eventArgs, nameof(GameEnded));
}