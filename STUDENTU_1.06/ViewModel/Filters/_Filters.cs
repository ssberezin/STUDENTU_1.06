using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Windows;
using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Model.DBModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;

using STUDENTU_1._06.Views.EditOrderWindows.ContactsWindows;
using STUDENTU_1._06.Views.EditOrderWindows.RuleOrderLineWindows;

namespace STUDENTU_1._06.ViewModel.Filters
{
    public class _Filters : Helpes.ObservableObject
    {

        //DirrectionFilter
        //source here http://www.jarloo.com/excel-like-autofilter-in-wpf/
        IDialogService dialogService;
        IShowWindowService showWindow;


        private bool dirrectionFilter;
        public bool DirrectionFilter
        {
            get { return dirrectionFilter; }
            set
            {
                if (dirrectionFilter != value)
                {
                    dirrectionFilter = value;
                    OnPropertyChanged(nameof(DirrectionFilter));
                }
            }
        }

        public _Filters()
        {
            this.DirrectionFilter = false;
        }


        //====================================COMMAND FOR SETF DIRECTION  FILTER===============================
        
        private RelayCommand setDirectionFilterCommand;
        public RelayCommand SetDirectionFilterCommand => setDirectionFilterCommand ?? (setDirectionFilterCommand = new RelayCommand(
                    (obj) =>
                    {
                        DirrectionFilter = true;
                    }
                    ));

        

        //======================================================================================================
        
        
        //====================================COMMAND FOR SET DIRECTION  FILTER===============================

        private RelayCommand unselectAllDirectionFilterCommand;
        public RelayCommand UnselectAllDirectionFilterCommand => unselectAllDirectionFilterCommand ?? (unselectAllDirectionFilterCommand = new RelayCommand(
                    (obj) =>
                    {
                        UnselectAllDirectionFilter();
                    }
                    ));

        private void UnselectAllDirectionFilter()
        {

        }
        //======================================================================================================
    }
}
