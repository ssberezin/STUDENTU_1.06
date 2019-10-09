using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using STUDENTU_1._06.Views.EditOrderWindows;
using System.Collections.ObjectModel;
using STUDENTU_1._06.Model.HelpModelClasses;
using STUDENTU_1._06.Views.EditOrderWindows.Evaluation;

namespace STUDENTU_1._06.ViewModel
{
    public partial class ForEditOrder : Helpes.ObservableObject
    {
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

        private Evaluation winnerEvaluation;
        public Evaluation WinnerEvaluation
        {
            get { return winnerEvaluation; }
            set
            {
                if (winnerEvaluation != value)
                {
                    winnerEvaluation = value;
                    OnPropertyChanged(nameof(WinnerEvaluation));
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
        //нужна для того, чтоб отредвактировать оценку автора, если запись еще не внесена в БД
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


        //here we set final e value of evaluation 
        private RelayCommand setSelectEvaluationCommand;
        public RelayCommand SetSelectEvaluationCommand =>
            setSelectEvaluationCommand ?? (setSelectEvaluationCommand = new RelayCommand(
                    (obj) =>
                    {
                        SetSelectEvaluation();                        
                    }
                    ));

        private void SetSelectEvaluation()
        {
            //нашли в оценках автора нужную и присвоили ей статус "финальной оценки" (EvaluationRecord.FinalEvaluation = true)
            // found what was needed in the author’s evaluations and assigned her the status of “final evaluation” (EvaluationRecord.FinalEvaluation = true)

            //также задали значение свойству ExecuteAuthor, в котором задаем информацию о победителе тендера навыполнение заказа
            //also set the value to the ExecuteAuthor property, in which we set information about the winner of the tender

            foreach (EvaluationRecord item in AuthorsRecord.EvaluationRecords)
            {
                if (item.DeadLine == EvaluationRecord.DeadLine &&
                    item.EvaluateDescription == EvaluationRecord.EvaluateDescription &&
                    item.Price == EvaluationRecord.Price)
                {
                    EvaluationRecord.FinalEvaluation = true;
                    ExecuteAuthor = AuthorsRecord;
                    FinalEvaluationRecord = EvaluationRecord;

                    SelectedExecuteAuthor.AuthorId = AuthorsRecord.AuthorRecordId;

                    Order.ExecuteAuthorComments = item.EvaluateDescription;
                    Order.ExecuteAuthorPrice = item.Price;
                    Order.ExecuteAuthorDeadLine = item.DeadLine;

                    break;
                }
            }
           
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
            AuthorsRecord.EvaluationRecords.Remove(EvaluationRecord);
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
            foreach (var item in AuthorsRecord.EvaluationRecords)
            {
                if (item.DeadLine == TmpEvaluationRecord.DeadLine &&
                   item.EvaluateDescription == TmpEvaluationRecord.EvaluateDescription &&
                   item.Price == TmpEvaluationRecord.Price)
                {
                    AuthorsRecord.EvaluationRecords[i] = EvaluationRecord;
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
            AuthorsRecord.EvaluationRecords.Add(EvaluationRecord);
        }

    }


}
