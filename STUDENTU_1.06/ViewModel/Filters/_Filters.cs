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
        public ObservableCollection<Direction> SelectedDirections { get; set; }
        public ObservableCollection<Direction> AllDirections { get; set; }
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
           SelectedDirections = new ObservableCollection<Direction>();
           AllDirections = new ObservableCollection<Direction>();
        }


        //====================================COMMAND FOR SETF DIRECTION  FILTER===============================
        
        private RelayCommand setDirectionFilterCommand;
        public RelayCommand SetDirectionFilterCommand => setDirectionFilterCommand ?? (setDirectionFilterCommand = new RelayCommand(
                    (obj) =>
                    {
                        DirrectionFilter = true;
                        SetDirectionFilter(AllDirections);
                    }
                    ));
        private void SetDirectionFilter(ObservableCollection<Direction> OrdersDirections)
        {
            bool existFlag = false;
            int countOrders = OrdersDirections.Count();
            SelectedDirections.Add(OrdersDirections[0]);
            for (int i = 0; i < countOrders; i++)

            {
                foreach (var item in OrdersDirections)
                {
                    if (item.DirectionId == SelectedDirections[i].DirectionId)
                    {
                        existFlag = true;
                        break;
                    }
                }
                if (!existFlag)
                {
                    SelectedDirections.Add(OrdersDirections[i]);
                    existFlag=false;
                }
            }
                  
        }


        //======================================================================================================


        //====================================COMMAND FOR SET DIRECTION  FILTER===============================
        //SelectAlltDirectionFilterCommand
        private RelayCommand selectAlltDirectionFilterCommand;
        public RelayCommand SelectAlltDirectionFilterCommand => selectAlltDirectionFilterCommand ?? (selectAlltDirectionFilterCommand = new RelayCommand(
                    (obj) =>
                    {
                        UnselectAllDirectionFilter();
                    }
                    ));

        private void SelectAllDirectionFilter()
        {

        }


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
