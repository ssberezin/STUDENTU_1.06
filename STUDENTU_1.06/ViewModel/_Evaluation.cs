
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


        public _Evaluation(Dates date)
        {  
            Evaluation = new Evaluation();
            EvaluationRecord = new EvaluationRecord() { DeadLine = date.AuthorDeadLine };           

            FinalEvaluationRecord = new EvaluationRecord()
            {
                DeadLine = new DateTime(1900, 1, 1),
                Price = 0,
                EvaluateDescription = ""
            };
            _RuleOrderLine = new _RuleOrderLine();
            //LoadData();
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
        }

       
       


        private Evaluation evaluation;
        public Evaluation Evaluation
        {
            get { return evaluation; }
            set
            {
                if (evaluation != value)
                {
                    evaluation = value;
                    OnPropertyChanged(nameof(Evaluation));
                }
            }
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

        //нужна для того, чтоб отредактировать оценку автора, если запись еще не внесена в БД
        // needed in order to edit the author’s rating, if the record has not yet been entered into the database
        private EvaluationRecord tmpevaluationRecord;
        public EvaluationRecord TmpEvaluationRecord
        {
            get { return tmpevaluationRecord; }
            set
            {
                if (tmpevaluationRecord != value)
                {
                    tmpevaluationRecord = value;
                    OnPropertyChanged(nameof(TmpEvaluationRecord));
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

        

        private _RuleOrderLine _ruleOrderLine;
        public _RuleOrderLine _RuleOrderLine
        {
            get { return _ruleOrderLine; }
            set
            {
                if (_ruleOrderLine != value)
                {
                    _ruleOrderLine = value;
                    OnPropertyChanged(nameof(_RuleOrderLine));
                }
            }
        }
   


        //=======================================here we set final e value of evaluation ==================================================
        private RelayCommand setSelectEvaluationCommand;
        public RelayCommand SetSelectEvaluationCommand =>
            setSelectEvaluationCommand ?? (setSelectEvaluationCommand = new RelayCommand(
                    (obj) =>
                    {
                        SetSelectEvaluation();
                    }
                    ));

        private EvaluationRecord SetSelectEvaluation()
        {
            //нашли в оценках автора нужную и присвоили ей статус "финальной оценки" (EvaluationRecord.FinalEvaluation = true)
            // found what was needed in the author’s evaluations and assigned her the status of “final evaluation” (EvaluationRecord.FinalEvaluation = true)

            //также задали значение свойству ExecuteAuthor, в котором задаем информацию о победителе тендера на выполнение заказа
            //also set the value to the ExecuteAuthor property, in which we set information about the winner of the tender

            foreach (EvaluationRecord item in _RuleOrderLine.AuthorsRecord.EvaluationRecords)
            {
                if (item.DeadLine == EvaluationRecord.DeadLine &&
                    item.EvaluateDescription == EvaluationRecord.EvaluateDescription &&
                    item.Price == EvaluationRecord.Price)
                {
                    EvaluationRecord.FinalEvaluation = true;
                    _RuleOrderLine.ExecuteAuthor = _RuleOrderLine.AuthorsRecord;
                    _RuleOrderLine.SelectedExecuteAuthor = _RuleOrderLine.AuthorsRecord.Author;
                    FinalEvaluationRecord = EvaluationRecord;
                    return FinalEvaluationRecord; 
                }               
            }
            return null;

        }



        //call AddEvaluateWindow
        private RelayCommand addEvaluationCommand;
        public RelayCommand AddEvaluationCommand =>
            addEvaluationCommand ?? (addEvaluationCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddEvaluateWindow addEvaluateWindow = new AddEvaluateWindow(obj);
                        addEvaluateWindow.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(addEvaluateWindow);
                    }
                    ));

        //=====================Commands for call Editing wondow of Author evaluation ======================================
        private RelayCommand setAuthorAvaluationCommand;
        public RelayCommand SetAuthorAvaluationCommand => setAuthorAvaluationCommand ?? (setAuthorAvaluationCommand = new RelayCommand(
                    (obj) =>
                    {
                        //editAvaluationWindow
                        EditAvaluationWindow editAvaluationWindow = new EditAvaluationWindow(obj);
                        editAvaluationWindow.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(editAvaluationWindow);

                    }
                    ));

        //===================================== For delete evaluate order any author in EditAvaluationWindow.xaml====================
        private RelayCommand deleteSelectedAvaluateRuleOrderCommand;
        public RelayCommand DeleteSelectedAvaluateRuleOrderCommand =>
                            deleteSelectedAvaluateRuleOrderCommand ??
                            (deleteSelectedAvaluateRuleOrderCommand = new RelayCommand(
                    (obj) =>
                    {
                        DeleteSelectedAvaluateRuleOrder();
                    }
                    ));

        private void DeleteSelectedAvaluateRuleOrder()
        {
             _RuleOrderLine.AuthorsRecord.EvaluationRecords.Remove(EvaluationRecord);
        }


        //===================================== For call EDIT window  EditAvaluatWindow.xaml====================
        private RelayCommand calleditSelectedAvaluateRuleOrderWindowCommand;
        public RelayCommand CallEditSelectedAvaluateRuleOrderWindowCommand =>
                            calleditSelectedAvaluateRuleOrderWindowCommand ??
                            (calleditSelectedAvaluateRuleOrderWindowCommand = new RelayCommand(
                    (obj) =>
                    {
                        TmpEvaluationRecord = EvaluationRecord;
                        EditEvaluateWindow editAvaluatWindow = new EditEvaluateWindow(obj);
                        editAvaluatWindow.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(editAvaluatWindow);
                    }
                    ));

        //EditAuthorEvaluateAuthorRecordCommand
        private RelayCommand editAuthorEvaluateAuthorRecordCommand;
        public RelayCommand EditAuthorEvaluateAuthorRecordCommand =>
                            editAuthorEvaluateAuthorRecordCommand ??
                            (editAuthorEvaluateAuthorRecordCommand = new RelayCommand(
                    (obj) =>
                    {
                        EditAuthorEvaluateAuthorRecord();
                    }
                    ));

        //редактируем записи по оценке авторов
        // edit entries by authors evaluation
        private void EditAuthorEvaluateAuthorRecord()
        {
            //так как записи по оценкам авторов еще не сохранены в БД, то они не имеют ID.
            //потому производим поиск и замену старой оценки на новую, отредактированную
            // since the entries, according to the authors, are not yet stored in the 
            //, they do not have an ID.
            // therefore, we search and replace the old evaluation with the new, edited
            int i = 0;
            foreach (var item in _RuleOrderLine.AuthorsRecord.EvaluationRecords)
            {
                if (item.DeadLine == TmpEvaluationRecord.DeadLine &&
                   item.EvaluateDescription == TmpEvaluationRecord.EvaluateDescription &&
                   item.Price == TmpEvaluationRecord.Price)
                {
                    _RuleOrderLine.AuthorsRecord.EvaluationRecords[i] = EvaluationRecord;
                    break;
                }
                i++;
            }

        }

        //===================================== For save evaluate order any author in EditAvaluatonWindow.xaml====================
        private RelayCommand saveAuthorEvaluateAuthorRecordCommand;
        public RelayCommand SaveAuthorEvaluateAuthorRecordCommand =>
                            saveAuthorEvaluateAuthorRecordCommand ??
                            (saveAuthorEvaluateAuthorRecordCommand = new RelayCommand(
                    (obj) =>
                    {
                        SaveAuthorEvaluateAuthorRecord();
                    }
                    ));

        private void SaveAuthorEvaluateAuthorRecord()
        {
            //check for the entry before adding
            foreach (EvaluationRecord item in _RuleOrderLine.AuthorsRecord.EvaluationRecords)
                if (item.DeadLine != EvaluationRecord.DeadLine &&
                    item.Price != EvaluationRecord.Price &&
                    item.EvaluateDescription != EvaluationRecord.EvaluateDescription)
                {
                    _RuleOrderLine.AuthorsRecord.EvaluationRecords.Add(EvaluationRecord);
                    dialogService.ShowMessage("Данные сохранены");
                    EvaluationRecord.Price = 0;
                    EvaluationRecord.EvaluateDescription = null;
                }
                else
                    dialogService.ShowMessage("Уже есть запись с такой оценкой");
        }

        //===================================== For Cancel save evaluate order any author in EditAvaluatonWindow.xaml====================
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
            EvaluationRecord.Price = 0;
            EvaluationRecord.EvaluateDescription = null;
        }

    }

}
