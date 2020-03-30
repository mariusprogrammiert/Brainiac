using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Brainiac
{
    public partial class MainWindow : Window
    {
        private BrainiacController brainiacController;
        private MediaPlayer mpGreen, mpRed, mpYellow, mpBlue, mpGameOver;
        private readonly int pauseAfterFieldPlayed;
        public MainWindow()
        {
            InitializeComponent();
            brainiacController = new BrainiacController();
            brainiacController.PlayField += PlayField;
            brainiacController.UpdatePoints += UpdatePoints;
            brainiacController.LockButton += LockButton;
            brainiacController.UnlockButton += UnlockButton;
            brainiacController.PlayGameOver += PlayGameOver;

            pauseAfterFieldPlayed = 160;

            mpGreen = new MediaPlayer();
            mpRed = new MediaPlayer();
            mpYellow = new MediaPlayer();
            mpBlue = new MediaPlayer();
            mpGameOver = new MediaPlayer();

            mpGreen.Open(new Uri("Resources/550.wav", UriKind.Relative));
            mpGreen.MediaEnded += playedGreen;
            mpRed.Open(new Uri("Resources/620.wav", UriKind.Relative));
            mpRed.MediaEnded += playedRed;
            mpYellow.Open(new Uri("Resources/480.wav", UriKind.Relative));
            mpYellow.MediaEnded += playedYellow;
            mpBlue.Open(new Uri("Resources/690.wav", UriKind.Relative));
            mpBlue.MediaEnded += playedBlue;
            mpGameOver.Open(new Uri("Resources/gameover.wav", UriKind.Relative));
            mpGameOver.MediaEnded += playedGameOver;
        }
        private void PlayField(FieldColors field)
        {
            switch (field)
            {
                case FieldColors.blue:
                    playBlue();
                    break;
                case FieldColors.green:
                    playGreen();
                    break;
                case FieldColors.red:
                    playRed();
                    break;
                case FieldColors.yellow:
                    playYellow();
                    break;
            }
        }
        private void UpdatePoints(int points)
        {
            labelPoints.Content = "Punkte: " + points;
        }
        private void LockButton()
        {
            buttonGame.IsEnabled = false;
        }
        private void UnlockButton()
        {
            buttonGame.IsEnabled = true;
        }
        private void buttonGame_Click(object sender, RoutedEventArgs e)
        {
            brainiacController.startNewGame();
        }
        private void playGreen()
        {
            fieldGreen.Fill = Brushes.Green;
            mpGreen.Position = new TimeSpan(0L);
            mpGreen.Play();
        }
        private void playRed()
        {
            fieldRed.Fill = Brushes.Red;
            mpRed.Position = new TimeSpan(0L);
            mpRed.Play();
        }
        private void playYellow()
        {
            fieldYellow.Fill = Brushes.Yellow;
            mpYellow.Position = new TimeSpan(0L);
            mpYellow.Play();
        }
        private void playBlue()
        {
            fieldBlue.Fill = Brushes.Blue;
            mpBlue.Position = new TimeSpan(0L);
            mpBlue.Play();
        }
        private void PlayGameOver()
        {
            fieldGreen.Fill = Brushes.Green;
            fieldRed.Fill = Brushes.Red;
            fieldYellow.Fill = Brushes.Yellow;
            fieldBlue.Fill = Brushes.Blue;
            mpGameOver.Position = new TimeSpan(0L);
            mpGameOver.Play();
        }
        private void fieldGreen_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (brainiacController.isGameRunning)
            {
                playGreen();
            }
        }
        private void fieldRed_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (brainiacController.isGameRunning)
            {
                playRed();
            }
        }
        private void fieldYellow_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (brainiacController.isGameRunning)
            {
                playYellow();
            }
        }
        private void fieldBlue_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (brainiacController.isGameRunning)
            {
                playBlue();
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (brainiacController.isGameRunning)
            {
                if (MessageBox.Show("Wirklich beenden?", "Das Spiel läuft noch", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
        private void playedGreen(object sender, EventArgs e)
        {
            fieldGreen.Fill = Brushes.DarkGreen;
            performFieldUpdate();
            Thread.Sleep(pauseAfterFieldPlayed);
            brainiacController.handlePlayedColor(FieldColors.green);
        }
        private void playedRed(object sender, EventArgs e)
        {
            fieldRed.Fill = Brushes.DarkRed;
            performFieldUpdate();
            Thread.Sleep(pauseAfterFieldPlayed);
            brainiacController.handlePlayedColor(FieldColors.red);
        }
        private void playedYellow(object sender, EventArgs e)
        {
            fieldYellow.Fill = Brushes.Orange;
            performFieldUpdate();
            Thread.Sleep(pauseAfterFieldPlayed);
            brainiacController.handlePlayedColor(FieldColors.yellow);
        }
        private void playedBlue(object sender, EventArgs e)
        {
            fieldBlue.Fill = Brushes.DarkBlue;
            performFieldUpdate();
            Thread.Sleep(pauseAfterFieldPlayed);
            brainiacController.handlePlayedColor(FieldColors.blue);
        }
        private void playedGameOver(object sender, EventArgs e)
        {
            fieldBlue.Fill = Brushes.DarkBlue;
            fieldYellow.Fill = Brushes.Orange;
            fieldRed.Fill = Brushes.DarkRed;
            fieldGreen.Fill = Brushes.DarkGreen;
        }
        private void performFieldUpdate()
        {
            DispatcherFrame dispatcherFrame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(delegate (object parameter)
            {
                dispatcherFrame.Continue = false;
                return null;
            }), null);
            Dispatcher.PushFrame(dispatcherFrame);
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
        }
    }
}
