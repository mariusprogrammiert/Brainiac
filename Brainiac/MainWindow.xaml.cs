using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Brainiac
{
    public partial class MainWindow : Window
    {
        private BrainiacController brainiacController;
        private MediaPlayer mpGreen, mpRed, mpYellow, mpBlue, mpGameOver;
        private DoubleAnimation doubleAnimation;
        private RotateTransform rotateTransform;
        private FieldColors nextFieldColor;
        private readonly int pauseAfterFieldPlayed;
        private int rotateCounter;
        private bool isFirstTurn;
        public MainWindow()
        {
            InitializeComponent();
            brainiacController = new BrainiacController();
            brainiacController.PlayField += PlayField;
            brainiacController.UpdatePoints += UpdatePoints;
            brainiacController.LockElements += LockElements;
            brainiacController.UnlockElements += UnlockElements;
            brainiacController.PlayGameOver += PlayGameOver;

            pauseAfterFieldPlayed = 160;
            rotateCounter = 0;
            isFirstTurn = true;

            doubleAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(1)));
            rotateTransform = new RotateTransform();
            doubleAnimation.Completed += RotationCompleted;
            gameField.RenderTransform = rotateTransform;
            gameField.RenderTransformOrigin = new Point(0.5, 0.5);

            mpGreen = new MediaPlayer();
            mpRed = new MediaPlayer();
            mpYellow = new MediaPlayer();
            mpBlue = new MediaPlayer();
            mpGameOver = new MediaPlayer();

            mpGreen.Open(new Uri("Resources/550.wav", UriKind.Relative));
            mpGreen.MediaEnded += playedGreen;
            mpGreen.MediaFailed += loadingFailed;
            mpRed.Open(new Uri("Resources/620.wav", UriKind.Relative));
            mpRed.MediaEnded += playedRed;
            mpRed.MediaFailed += loadingFailed;
            mpYellow.Open(new Uri("Resources/480.wav", UriKind.Relative));
            mpYellow.MediaEnded += playedYellow;
            mpYellow.MediaFailed += loadingFailed;
            mpBlue.Open(new Uri("Resources/690.wav", UriKind.Relative));
            mpBlue.MediaEnded += playedBlue;
            mpBlue.MediaFailed += loadingFailed;
            mpGameOver.Open(new Uri("Resources/gameover.wav", UriKind.Relative));
            mpGameOver.MediaEnded += playedGameOver;
            mpGameOver.MediaFailed += loadingFailed;
        }

        private void loadingFailed(object sender, ExceptionEventArgs e)
        {
            MessageBox.Show("Es konnten nicht alle Sounddateien geladen werden!\nDas Spiel wird beendet...",
                "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }

        private void PlayField(FieldColors field, bool rotate)
        {
            nextFieldColor = field;
            if (rotate)
            {
                if (isFirstTurn)
                {
                    isFirstTurn = false;
                    playNextFieldColor();
                }
                else
                {
                    rotateGameField();
                }
            }
            else
            {
                playNextFieldColor();
            }
        }

        private void UpdatePoints(int points)
        {
            labelPoints.Content = "Punkte: " + points;
        }

        private void LockElements()
        {
            buttonGame.IsEnabled = false;
            cbHardcoreMode.IsEnabled = false;
        }

        private void UnlockElements()
        {
            buttonGame.IsEnabled = true;
            cbHardcoreMode.IsEnabled = true;
        }

        private void buttonGame_Click(object sender, RoutedEventArgs e)
        {
            brainiacController.startNewGame(cbHardcoreMode.IsChecked.Value);
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

            isFirstTurn = true;

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

        private void rotateGameField()
        {
            rotateCounter += 90;
            doubleAnimation.To = rotateCounter;
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
        }

        private void RotationCompleted(object sender, EventArgs e)
        {
            playNextFieldColor();
        }

        private void playNextFieldColor()
        {
            switch (nextFieldColor)
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
    }
}
