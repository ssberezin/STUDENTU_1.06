﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace STUDENTU_1._06.Views
{
    /// <summary>
    /// Логика взаимодействия для EditSourceWindow.xaml
    /// </summary>
    public partial class EditSourceWindow : Window
    {
        public EditSourceWindow(object DataContext)
        {
            InitializeComponent();
            this.DataContext = DataContext;
        }
      
    }
}
