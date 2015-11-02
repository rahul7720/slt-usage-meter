﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using SLTConsole.Library;

namespace SLTConsole
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>

    public partial class Widget : MetroWindow
    {
        private App app = ((App)Application.Current);
        LoginControl loginControl = new LoginControl();
        WidgetControl widgetControl = new WidgetControl();

        public Widget()
        {
            InitializeComponent();
            if (app.Key.GetValue("slt_user") != null || app.Key.GetValue("slt_password") != null)
            {                
                loginControl.txtUsername.Text = app.Key.GetValue("slt_user").ToString();
                loginControl.txtPassword.Password = app.Key.GetValue("slt_password").ToString();
                //app.Connection.Login(app.Key.GetValue("slt_user").ToString(), app.Key.GetValue("slt_pass").ToString());                
            }
            InitDisplay();
            loginControl.btnLogin.Click += btnLogin_Click;
            (this.FindResource("shakeEffect") as Storyboard).Begin();
        }

        private void InitDisplay()
        {
            display.Children.Clear();
            if (!app.Connection.IsLogged)
            {                
                display.Children.Add(loginControl);
            }
            else
            {
                ProfileManager profileManager = new ProfileManager(app.Connection);
                Profile profile = profileManager.GetProfile();
                widgetControl.lblPeakStatus.Content = profile.PeakStatus;
                widgetControl.lblTotalStatus.Content = profile.TotalStatus;
                display.Children.Add(widgetControl);
            }
        }

        void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            app.Key.SetValue("slt_user", loginControl.txtUsername.Text);
            app.Key.SetValue("slt_password", loginControl.txtPassword.Password);

            if (app.Connection.Login(loginControl.txtUsername.Text, loginControl.txtPassword.Password))
            {
                InitDisplay();
            }
            else
            {
                (this.FindResource("shakeEffect") as Storyboard).Begin();                                
            }
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width - 35;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }
    }
}
