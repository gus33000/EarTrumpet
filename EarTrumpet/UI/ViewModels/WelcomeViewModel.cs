﻿using EarTrumpet.Interop.Helpers;
using EarTrumpet.UI.Helpers;
using System.Windows;
using System.Windows.Input;

namespace EarTrumpet.UI.ViewModels
{
    class WelcomeViewModel
    {
        public string VisibleTitle => ""; // We have a header instead
        public string Title { get; }
        public ICommand Close { get; set; }
        public ICommand LearnMore { get; }
        public ICommand DisplaySettingsChanged { get; }

        private WindowViewModelState _state;

        public WelcomeViewModel()
        {
            // Used for the window title.
            Title = Properties.Resources.WelcomeDialogHeaderText;
            LearnMore = new RelayCommand(() =>
            {
                ProcessHelper.StartNoThrow("https://github.com/File-New-Project/EarTrumpet");
            });
        }

        public void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            switch (_state)
            {
                case WindowViewModelState.Open:
                    _state = WindowViewModelState.Closing;
                    e.Cancel = true;

                    var window = (Window)sender;
                    WindowAnimationLibrary.BeginWindowExitAnimation(window, () =>
                    {
                        _state = WindowViewModelState.CloseReady;
                        window.Close();
                    });
                    break;
                case WindowViewModelState.Closing:
                    // Ignore any requests while playing the close animation.
                    e.Cancel = true;
                    break;
                case WindowViewModelState.CloseReady:
                    // Accept the close.
                    break;
            }
        }
    }
}