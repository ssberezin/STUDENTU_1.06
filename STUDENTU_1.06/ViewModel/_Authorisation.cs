using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using System;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using System.ComponentModel;

namespace STUDENTU_1._06.ViewModel
{
    class _Authorisation : Helpes.ObservableObject
    {
        IDialogService dialogService;
        IShowWindowService showWindow;

        private bool trueAuthorisation;//добро на авторизацию
        public bool TrueAuthorisation
        {
            get { return trueAuthorisation; }
            set
            {
                if (value != trueAuthorisation)
                {
                    trueAuthorisation = value;
                    OnPropertyChanged(nameof(TrueAuthorisation));
                }
            }
        }

        private bool falseAuthorisation;//отбой на авторизацию
        public bool FalseAuthorisation
        {
            get { return falseAuthorisation; }
            set
            {
                if (value != falseAuthorisation)
                {
                    falseAuthorisation = value;
                    OnPropertyChanged(nameof(FalseAuthorisation));
                }
            }
        }

        public _Authorisation()
        {
            FalseAuthorisation = false;
            TrueAuthorisation = false;
        }
       
        private RelayCommand setTrueAuthorisationCommand;
        public RelayCommand SetTrueAuthorisationCommand => setTrueAuthorisationCommand ?? (setTrueAuthorisationCommand = new RelayCommand(
                    (obj) =>
                    {                        
                        TrueAuthorisation = true;
                    }
                    ));

        private RelayCommand setFalseAuthorisationCommand;
        public RelayCommand SetFalseAuthorisationCommand => setFalseAuthorisationCommand ?? (setFalseAuthorisationCommand = new RelayCommand(
                    (obj) =>
                    {
                        FalseAuthorisation = true;
                    }
                    ));
    }
}
