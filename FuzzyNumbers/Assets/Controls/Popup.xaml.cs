﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FuzzyNumbers.Assets.Controls
{
    /// <summary>
    /// Interaction logic for Popup.xaml
    /// </summary>
    public partial class Popup : UserControl
    {
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(Popup));
        public string Message
        {
            get
            {
                return this.GetValue(MessageProperty) as string;
            }
            set
            {
                this.SetValue(MessageProperty, value);
            }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(Popup));
        public string Header
        {
            get
            {
                return this.GetValue(HeaderProperty) as string;
            }
            set
            {
                this.SetValue(HeaderProperty, value);
            }
        }

        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register("Enabled", typeof(bool), typeof(Popup));
        public bool? Enabled
        {
            get
            {
                return this.GetValue(EnabledProperty) as bool?;
            }
            set
            {
                this.SetValue(EnabledProperty, value);

                if (value == true)
                {
                    this.Visibility = Visibility.Visible;
                    this._status = true;
                }
                else
                {
                    this.Visibility = Visibility.Hidden;
                    this._status = false;
                }
            }
        }

        private int? _timeout;
        public int? Timeout
        {
            get
            {
                return _timeout as int?;
            }
            set
            {
                _timeout = value;
            }
        }

        private bool _status = false;
        public Popup()
        {
            InitializeComponent();

            if (Enabled == true)
            {
                this.Visibility = Visibility.Visible;
                this._status = true;
            }
            else
            {
                this.Visibility = Visibility.Hidden;
                this._status = false;
            }
        }

        public void Show()
        {
            if (this._status)
                return;

            DoubleAnimation db = new DoubleAnimation();
            db.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };
            db.To = 0;
            db.From = this.ActualHeight;
            db.Duration = TimeSpan.FromSeconds(0.5);

            this.Visibility = Visibility.Visible;
            slideTransform.BeginAnimation(TranslateTransform.YProperty, db);

            this._status = true;

            if (_timeout != null)
            {
                if (_timeout > 0)
                {
                    this.HideTimeout();
                }
            }
        }

        private async void HideTimeout()
        {
            await Task.Run(() =>
            {
                Thread.Sleep((int)this._timeout);

                App.Current.Dispatcher.Invoke(() =>
                {
                    this.Hide();
                });
            });
        }

        private void Hide()
        {
            if (!this._status)
                return;

            DoubleAnimation db = new DoubleAnimation();
            db.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn };
            db.From = 0;
            db.To = this.ActualHeight;
            db.Duration = TimeSpan.FromSeconds(0.5);

            slideTransform.BeginAnimation(TranslateTransform.YProperty, db);

            this._status = false;
        }

        private void Button_Collapse(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
