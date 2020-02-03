
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using System;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Model.HelpModelClasses;
using STUDENTU_1._06.Model.DBModelClasses;
using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Views.EditOrderWindows;
using STUDENTU_1._06.Views.EditOrderWindows.Evaluation;

namespace STUDENTU_1._06.ViewModel
{
    //Class for operations with Direction table
    public class _Evaluation : Helpes.ObservableObject
    {
      

        IDialogService dialogService;
        IShowWindowService showWindow;


        public _Evaluation()
        {  
            //Evaluation = new Evaluation();
            EvaluationRecord = new EvaluationRecord() { DeadLine = TMPStaticClass.CurrentOrder.Dates.AuthorDeadLine };                                   
            FinalEvaluationRecord = new EvaluationRecord()
            {
                DeadLine = EvaluationRecord.DeadLine,
                Price = 0,
                EvaluateDescription = ""
            };
            //Order = new OrderLine(); 
            //LoadData();
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();

        }

      

        private EvaluationRecord evaluationRecord;
        public EvaluationRecord EvaluationRecord
        {
            get { return evaluationRecord; }
            set
            {
                if (evaluationRecord != value)
                {
                    evaluationRecord = value;
                    OnPropertyChanged(nameof(EvaluationRecord));
                }
            }
        }

        //for store final evaluation value selected author
        //can find in in SetFinalAvaluationWindow.xaml
        private EvaluationRecord finalevaluationRecord;
        public EvaluationRecord FinalEvaluationRecord
        {
            get { return finalevaluationRecord; ; }
            set
            {
                if (finalevaluationRecord != value)
                {
                    finalevaluationRecord = value;
                    OnPropertyChanged(nameof(FinalEvaluationRecord));
                }
            }
        }

        



        //===============================================call SetFinalAvaluationWindow==========================================
        private RelayCommand setFinalEvaluationCommand;
        public RelayCommand SetFinalEvaluationCommand =>
            setFinalEvaluationCommand ?? (setFinalEvaluationCommand = new RelayCommand(
                    (obj) =>
                    {
                        SetFinalAvaluationWindow finalEvaluateWindow = new SetFinalAvaluationWindow(obj);
                        showWindow.ShowDialog(finalEvaluateWindow);
                    }
                    ));


        //===============================================call AddEvaluateWindow==========================================
        private RelayCommand addEvaluationCommand;
        public RelayCommand AddEvaluationCommand =>
            addEvaluationCommand ?? (addEvaluationCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddEvaluateWindow addEvaluateWindow = new AddEvaluateWindow(obj);                        
                        showWindow.ShowDialog(addEvaluateWindow);
                    }
                    ));

        //=====================Commands for call Editing wondow of Author evaluation ======================================
        private RelayCommand setAuthorAvaluationCommand;
        public RelayCommand SetAuthorAvaluationCommand => setAuthorAvaluationCommand ?? (setAuthorAvaluationCommand = new RelayCommand(
                    (obj) =>
                    {
                        //editAvaluationWindow
                        EditAvaluationWindow editAvaluationWindow = new EditAvaluationWindow(obj);                        
                        showWindow.ShowDialog(editAvaluationWindow);
                    }
                    ));

        

        //===================================== For Cancel  evaluate order any author in AddAvaluatonWindow.xaml====================
        private RelayCommand cancelAuthorEvaluateAuthorRecordCommand;
        public RelayCommand CancelAuthorEvaluateAuthorRecordCommand =>
            cancelAuthorEvaluateAuthorRecordCommand ?? (cancelAuthorEvaluateAuthorRecordCommand = new RelayCommand(
                    (obj) =>
                    {
                        CancelAuthorEvaluateAuthorRecord();
                        Window window = obj as Window;
                        window.Close();
                    }
                    ));

        private void CancelAuthorEvaluateAuthorRecord()
        {           
           EvaluationRecord = new EvaluationRecord() { DeadLine = DateTime.Now };
        }

        //===================================== For close any window ===========================================
        private RelayCommand closeWindowCommand;
        public RelayCommand CloseWindowCommand =>
            closeWindowCommand ?? (closeWindowCommand = new RelayCommand(
                    (obj) =>
                    {
                        Window window = obj as Window;
                        window.Close();
                    }
                    ));

    }

}
