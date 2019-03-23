﻿using System;
using GalaSoft.MvvmLight.CommandWpf;
using Xky.UI.Controls;

namespace MyDemo.ViewModel
{
    public class ImageBrowserDemoViewModel
    {
        public RelayCommand OpenImgCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() =>
                new ImageBrowser(new Uri("pack://application:,,,/Resources/Img/1.jpg")).Show())).Value;
    }
}