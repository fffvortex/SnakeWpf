﻿using SnakeWpf.ViewModels;
using System.Windows;

namespace SnakeWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindVM(this);
        }
    }
}