﻿using STUDENTU_1._06.Model;
using STUDENTU_1._06.ViewModel.PersoneOperations.AuthorsOperationsVM;
using System;
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

namespace STUDENTU_1._06.Views.PersoneOperations.AuthorOperationsWindows
{
    /// <summary>
    /// Логика взаимодействия для AuthorInfo.xaml
    /// </summary>
    public partial class AuthorInfo : Window
    {
        public AuthorInfo()
        {
            InitializeComponent();
            this.DataContext = new AuthorsVMClass();
        }
        public AuthorInfo (Author author)
        {
            InitializeComponent();
            this.DataContext = new AuthorsVMClass(author);
        }


    }
}
