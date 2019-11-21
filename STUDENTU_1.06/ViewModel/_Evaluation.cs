
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
            Evaluation = new Evaluation();
            EvaluationRecord = new EvaluationRecord() { DeadLine = TMPStaticClass.CurrentOrder.Dates.AuthorDeadLine };                                   
            FinalEvaluationRecord = new EvaluationRecord()
            {
                DeadLine = EvaluationRecord.DeadLine,
                Price = 0,
                EvaluateDescription = ""
            };
            Order = new OrderLine(); 
           

           
          
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

        private OrderLine order;
        public OrderLine Order
        {
            get { return order; }
            set
            {
                if (order != value)
                {
                    order = value;
                    OnPropertyChanged(nameof(Order));
                }
            }
        }


        private RuleOrderLine _ruleOrderLine;
        public RuleOrderLine _RuleOrderLine
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

                        if (SetSelectEvaluation()!=null)
                            dialogService.ShowMessage("Оценка задана");
                        else
                            dialogService.ShowMessage("Оценка НЕ задана");
                    }
                    ));

        private EvaluationRecord SetSelectEvaluation()
        {
            
            foreach (EvaluationRecord item in _RuleOrderLine.AuthorsRecord.EvaluationRecords)
            {
                //проверка на наличие финальной оценки
                //check for existing final evaluation
                if (item.FinalEvaluation)
                {
                    if (dialogService.YesNoDialog("Уже заданиа финальная оценка. Переназначить?"))
                    {
                        //нашходим в оценках автора нужную и присвоили ей статус "финальной оценки" (EvaluationRecord.FinalEvaluation = true)
                        // found what was needed in the author’s evaluations and assigned her the status of “final evaluation” (EvaluationRecord.FinalEvaluation = true)

                        //также задали значение свойству ExecuteAuthor, в котором задаем информацию о победителе тендера на выполнение заказа
                        //also set the value to the ExecuteAuthor property, in which we set information about the winner of the tender

                        foreach (EvaluationRecord i in _RuleOrderLine.AuthorsRecord.EvaluationRecords)
                        {
                            if (i.DeadLine == EvaluationRecord.DeadLine &&
                                i.EvaluateDescription == EvaluationRecord.EvaluateDescription &&
                                i.Price == EvaluationRecord.Price)
                            {
                                EvaluationRecord.FinalEvaluation = true;
                                _RuleOrderLine.ExecuteAuthor = _RuleOrderLine.AuthorsRecord;
                                _RuleOrderLine.SelectedExecuteAuthor = _RuleOrderLine.AuthorsRecord.Author;
                                FinalEvaluationRecord = EvaluationRecord;
                                return FinalEvaluationRecord;
                            }
                        }

                    }
                    else
                        return null;
                    
                }
            }
            //нашходим в оценках автора нужную и присвоили ей статус "финальной оценки" (EvaluationRecord.FinalEvaluation = true)
            // found what was needed in the author’s evaluations and assigned her the status of “final evaluation” (EvaluationRecord.FinalEvaluation = true)

            //также задали значение свойству ExecuteAuthor, в котором задаем информацию о победителе тендера на выполнение заказа
            //also set the value to the ExecuteAuthor property, in which we set information about the winner of the tender

            foreach (EvaluationRecord i in _RuleOrderLine.AuthorsRecord.EvaluationRecords)
            {
                if (i.DeadLine == EvaluationRecord.DeadLine &&
                    i.EvaluateDescription == EvaluationRecord.EvaluateDescription &&
                    i.Price == EvaluationRecord.Price)
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

        //SetFinalEvaluationCommand

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

        


        //===================================== For call EDIT window  EditAvaluatWindow.xaml====================
        private RelayCommand calleditSelectedAvaluateRuleOrderWindowCommand;
        public RelayCommand CallEditSelectedAvaluateRuleOrderWindowCommand =>
                            calleditSelectedAvaluateRuleOrderWindowCommand ??
                            (calleditSelectedAvaluateRuleOrderWindowCommand = new RelayCommand(
                    (obj) =>
                    {
                        TmpEvaluationRecord = EvaluationRecord;
                        EditEvaluateWindow editAvaluatWindow = new EditEvaluateWindow(obj);                       
                        showWindow.ShowDialog(editAvaluatWindow);
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

       

        
        //==================================== COMMAND FOR ADD EVALUATION TO ORDER ====================================

        private RelayCommand addEvaluationToOrderCommand;
        public RelayCommand AddEvaluationToOrderCommand => addEvaluationToOrderCommand ?? (addEvaluationToOrderCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddEvaluationToOrder();
                    }
                    ));

        private void AddEvaluationToOrder()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    Order = db.Orderlines.Find(TMPStaticClass.CurrentOrder.OrderLineId);
                    //лагов тут еще пилить и пилить( (02/11/19)
                    //добавили авторов из AuthorsRecords в Evaluation
                    foreach (var item in _RuleOrderLine.SelectedAuthorsRecords)
                    {
                        Evaluation.Authors.Add(item.Author);//вот эта хрень уже под сомнение. Накой она теперь?...
                        Order.Author.Add(item.Author);
                    }

                    //добавили оценки Evaluation авторам  Order.Authors  из _Evaluation._RuleOrderLine.AuthorsRecord.EvaluationRecords
                    //т.е. получили полноценный список оценок авторов по текущему заказу
                    foreach (var i in Order.Author)
                    {
                        foreach (var item in _RuleOrderLine.AuthorsRecord.EvaluationRecords)
                        {
                            Evaluation.Moneys.Add(new Money() { Price = item.Price });
                            Evaluation.Description = item.EvaluateDescription;
                            Evaluation.Dates.Add(new Dates() { DeadLine = item.DeadLine });
                            Evaluation.Winner = item.FinalEvaluation;
                            i.Evaluation.Add(Evaluation);
                        }
                    }
                    //TMPStaticClass.CurrentOrder.Author = db.Authors.Find(new Author() { AuthorId = _Evaluation._RuleOrderLine.SelectedExecuteAuthor.AuthorId }.AuthorId);
                    //TMPStaticClass.CurrentOrder.Author.Evaluation.Add(Evaluation);



                    //Persone.Contacts = Contacts;
                    //Order.Direction = db.Directions.Find(_Dir.Dir.DirectionId);
                    //Order.Client = new Client() { Persone = Persone };
                    //Order.WorkType = db.WorkTypes.Find(_WorkType.WorkType.WorkTypeId);

                    //Order.Dates = Date;
                    //Order.Subject = db.Subjects.Find(_Subj.Subj.SubjectId); ;
                    //Order.Money = Price;
                    //if (_Status.Status.StatusName == "принимается")
                    //    // set a default entry to status field
                    //    Order.Status = db.Statuses.Find(new Status() { StatusId = 1 }.StatusId);
                    //else
                    //    //set realy selected status
                    //    Order.Status = _Status.Status;

                    //db.Orderlines.Add(Order);

                    //db.SaveChanges();
                    //dialogService.ShowMessage("Данные о заказе сохранены");


                }
                catch (ArgumentNullException ex)
                {
                    dialogService.ShowMessage(ex.Message);
                }
                catch (OverflowException ex)
                {
                    dialogService.ShowMessage(ex.Message);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    dialogService.ShowMessage(ex.Message);
                }
                catch (System.Data.Entity.Core.EntityCommandExecutionException ex)
                {
                    dialogService.ShowMessage(ex.Message);
                }
                catch (System.Data.Entity.Core.EntityException ex)
                {
                    dialogService.ShowMessage(ex.Message);
                }
            }

        }

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
