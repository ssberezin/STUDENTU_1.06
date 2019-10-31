
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Model.DBModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Views;

namespace STUDENTU_1._06.ViewModel
{
  public partial class ForEditOrder : Helpes.ObservableObject
    {
        public ObservableCollection<Contacts> ContactsRecords { get; set; }       

        private Window editWindow;
        private Window editDirection;

       
         IDialogService dialogService;
        IShowWindowService showWindow;

        //==========================================PROPERTIES============================================
        //for set value of order number. Need to work more
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

        private Persone persone;
        public Persone Persone
        {
            get { return persone; }
            set
            {
                if (persone != value)
                {
                    persone = value;
                    OnPropertyChanged(nameof(Persone));
                }
            }
        }

        private Author author;
        public Author Author
        {
            get { return author; }
            set
            {
                if (author != value)
                {
                    author = value;
                    OnPropertyChanged(nameof(Author));
                }
            }
        }

        private Money price;
        public Money Price
        {
            get { return price; }
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        private _Direction _dir;
        public _Direction _Dir
        {
            get { return _dir; }
            set
            {
                if (_dir != value)
                {
                    _dir = value;
                    OnPropertyChanged(nameof(_Dir));
                }
            }
        }

        private _Subject _subj;
        public _Subject _Subj
        {
            get { return _subj; }
            set
            {
                if (_subj != value)
                {
                    _subj = value;
                    OnPropertyChanged(nameof(_Subj));
                }
            }
        }

        private _Status _status;
        public _Status _Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(_Status));
                }
            }
        }

        private _Source _source;
        public _Source _Source
        {
            get { return _source; }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    OnPropertyChanged(nameof(_Source));
                }
            }
        }

        private _WorkType _workType;
        public _WorkType _WorkType
        {
            get { return _workType; }
            set
            {
                if (_workType != value)
                {
                    _workType = value;
                    OnPropertyChanged(nameof(_WorkType));
                }
            }
        }

        private Contacts contacts;
        public Contacts Contacts
        {
            get { return contacts; }
            set
            {
                if (contacts != value)
                {
                    contacts = value;
                    OnPropertyChanged(nameof(contacts));
                }
            }
        }

        private Dates date;
        public Dates Date
        {
            get { return date; }
            set
            {
                if (date!= value)
                {
                    date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }
        

        private PersoneDescription personeDescription;
        public PersoneDescription PersoneDescription
        {
            get { return personeDescription; }
            set
            {
                if (personeDescription != value)
                {
                    personeDescription = value;
                    OnPropertyChanged(nameof(PersoneDescription));
                }
            }
        }

        public ForEditOrder(Window editWindow, DefaultShowWindowService showWindow,
           IDialogService dialogService)
        {
            AuthorsRecords = new ObservableCollection<AuthorsRecord>();           
            SelectedAuthorsRecords = new ObservableCollection<AuthorsRecord> ();            
            ContactsRecords = new ObservableCollection<Contacts>();
            BlackListRecords = new ObservableCollection<BlackListHelpModel>();
            editWindow.Loaded += EditWindow_Loaded;
            this.showWindow = showWindow;
            this.dialogService = dialogService;
        }



        private void EditWindow_Loaded(object sender, RoutedEventArgs e)
        {

            Author = new Author();
            AuthorsRecord = new AuthorsRecord();
            Contacts = new Contacts();
            Date = new Dates();           
            _Dir = new _Direction();            
            Evaluation = new Evaluation();
            EvaluationRecord = new EvaluationRecord();
            ExecuteAuthor = new AuthorsRecord();
            ExecuteAuthor.Persone.NickName = "не задан";
            FinalEvaluationRecord = new EvaluationRecord()
            {
                DeadLine = new DateTime(1900, 1, 1),
                Price = 0,
                EvaluateDescription = ""
            };
            Order = new OrderLine { OrderNumber = GetOrderNumber() };
            Persone = new Persone ();
            PersoneDescription = new PersoneDescription();
            Price = new Money();            
            SelectetdAuthorContacts = new Contacts();
            SelectedExecuteAuthor = new Author();            
            _Status = new _Status();
            _Subj = new _Subject();
            _Source = new _Source();            
            WinnerEvaluation = new Evaluation();
            _WorkType = new _WorkType();
        }

        //=================================METHODS FOR PREVIOS LOAD TO CONTROLS OF EditOrder.xaml =====

        //create new oreder number (get max namber and add 1)
        private int GetOrderNumber()
        {
            int result = 0;

            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    //int name = (from m in db.Таблица select m.Нужное_поле).ToList().Last();
                    if (db.Orderlines.Count() == 0)
                        return 98000;
                    else
                    {
                        //нужно допилить , когда придет время
                        var res = db.Orderlines.OrderBy(p => p.OrderNumber).ToList().Last();

                        //result = (from m in db.Orderlines.OrderBy(p => p.OrderNumber) select m.OrderNumber).ToList().Last();
                        result = res.OrderNumber;
                        result += 1;
                    }
                    
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
            return result;

        }

        
        //==================================== COMMAND FOR SAVE NEW ORDER ====================================

        private RelayCommand createNewOrderLine;
        public RelayCommand CreateNewOrderLine => createNewOrderLine ?? (createNewOrderLine = new RelayCommand(
                    (obj) =>
                    {
                        SaveNewOrder();
                    }
                    ));

        private void SaveNewOrder()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    //Order.ExecuteAuthor = db.Authors.Find(new Author() { AuthorId = SelectedExecuteAuthor.AuthorId }.AuthorId);

                    if (SelectedExecuteAuthor.AuthorId==0)
                    {
                        // set a default entry to author field
                        Order.ExecuteAuthor = db.Authors.Find(new Author() { AuthorId = 1 }.AuthorId);
                        Order.Author = Order.ExecuteAuthor;
                       // Order.Author.Evaluation.Add(Evaluation);
                    }
                    else
                    {
                        //добавили авторов из AuthorsRecords в Evaluation
                        foreach (var item in AuthorsRecords)
                        {
                            Evaluation.Authors.Add(Author);
                            //Evaluation.Authors.Add(new Author()
                            //{
                            //    AuthorId = item.AuthorRecordId,
                            //    Source = item.Source
                                
                            //});
                        }
                        //добавили оценки авторам  Evaluation из AuthorsRecord.EvaluationRecords
                        foreach (var i in Evaluation.Authors)
                        {
                            foreach (var item in AuthorsRecord.EvaluationRecords)
                            {

                                Evaluation.Moneys.Add(new Money() { Price = item.Price });
                                Evaluation.Description = item.EvaluateDescription;
                                Evaluation.Dates.Add(new Dates() { DeadLine = item.DeadLine });
                                i.Evaluation.Add(Evaluation);
                            }
                        }

                        Order.ExecuteAuthor = db.Authors.Find(new Author() { AuthorId = SelectedExecuteAuthor.AuthorId }.AuthorId);
                        Order.Author = db.Authors.Find(new Author() { AuthorId = SelectedExecuteAuthor.AuthorId }.AuthorId);
                        Order.Author.Evaluation.Add(Evaluation);
                    }

                    //in SetSelectEvaluation(), whot in EvaluationClass.cs, we get :
                    //Order.ExecuteAuthorComments = item.EvaluateDescription;
                    //Order.ExecuteAuthorPrice = item.Price;
                    //Order.ExecuteAuthorDeadLine = item.DeadLine;

                    //эта ветка уже нафиг не нужна, по идее
                    //foreach (var item in AuthorsRecord.EvaluationRecords)
                    //{
                    //    if (item.FinalEvaluation==true)
                    //    {
                    //        WinnerEvaluation.Moneys.Add(new Money() { Price = item.Price });
                    //        WinnerEvaluation.Description = item.EvaluateDescription;
                    //        WinnerEvaluation.Dates.Add(new Dates() { DeadLine = item.DeadLine });
                    //        SelectedExecuteAuthor.Evaluation.Add(WinnerEvaluation);
                    //        break;
                    //    }
                    //}
                    
                    Persone.Contacts=Contacts;
                    Order.Direction = db.Directions.Find(_Dir.Dir.DirectionId);                    
                    Order.Client=new Client() { Persone=Persone};
                    Order.WorkType = db.WorkTypes.Find(_WorkType.WorkType.WorkTypeId);
                    
                    Order.Dates = Date;
                    Order.Subject = db.Subjects.Find(_Subj.Subj.SubjectId); ;
                    Order.Money = Price;
                    //if (SelectedExecuteAuthor is null)
                    //    // set a default entry to author field
                    //    Order.Author = db.Authors.Find(new Author() { AuthorId = 1 }.AuthorId);
                    //else
                    //    //set realy selected author
                    //    Order.Author = SelectedExecuteAuthor;
                    if (_Status.Status.StatusName=="принимается")
                        // set a default entry to status field
                        Order.Status = db.Statuses.Find(new Status() { StatusId = 1 }.StatusId); 
                    else
                        //set realy selected status
                        Order.Status = _Status.Status;

                    db.Orderlines.Add(Order);
                    
                    db.SaveChanges();
                    dialogService.ShowMessage("Данные о заказе сохранены");

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

        //==================================COMMAND FOR CLOSE WINDOW ==========================
        private RelayCommand closeWindowCommand;

        public RelayCommand CloseWindowCommand => closeWindowCommand ?? (closeWindowCommand = new RelayCommand(
                    (obj) =>
                    {
                        Window window = obj as Window;
                        window.Close();                       
                    }
                    ));
        //========================================================================================================================


    }
}
