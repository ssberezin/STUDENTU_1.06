using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Linq;
using System.Windows;
using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Model.HelpModelClasses;
using STUDENTU_1._06.Model.DBModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Views.PersoneOperations.AuthorOperationsWindows;

namespace STUDENTU_1._06.ViewModel
{
    class MainViewModel : Helpes.ObservableObject
    {
        public ObservableCollection<Records> Records { get; set; }
        //public DateFormatConverter DateFormatConverter { get; set; }
      
        private Persone selectedPersone;
        private Window mainWindow;

        
        IDialogService dialogService;//for show messages in mvvm pattern order
        IShowWindowService showWindow;//for show messages in mvvm pattern order


        
        

        public MainViewModel(Window mainWindow, DefaultShowWindowService showWindow)
        {
            Records = new ObservableCollection<Records>();            
            this.mainWindow = mainWindow;            
            mainWindow.Loaded += MainWindow_Loaded;
            this.showWindow = showWindow;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
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

        private void LoadData()
        {
            LoadPreviosData();//setting default values in DB ​​on first start
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {                    
                    var COrders = db.Orderlines.Include("Client")                                              
                                               .Include("Dates")
                                               .Include("Subject")
                                               .Include("Author")
                                               .Include("Status")
                                               .Include("Money")
                                               .Include("WorkType")
                        .ToList<OrderLine>();                   

                    //creat a orderlist in datagrid (mainwindow)
                    foreach (var item in COrders)
                    {
                        
                        Records record = new Records
                        {
                            RecordId= item.OrderLineId,
                            OrderCount=item.OrderCount,
                            OrderNumber = item.OrderNumber,
                            DateOfReception = item.Dates.DateOfReception,
                            DeadLine = item.Dates.DeadLine,
                            DateDone = item.Dates.DateDone,
                            Price = item.Money.Price,
                            Prepayment = item.Money.Prepayment,
                            Status = item.Status.StatusName,
                            TypeOfWork = item.WorkType.TypeOfWork,
                            //GetExecuteAuthor()
                            //AuthorNickName = item.ExecuteAuthor==null?"---": item.ExecuteAuthor.Persone.NickName,
                            AuthorNickName = item.GetExecuteAuthor() == null ? "---" : item.GetExecuteAuthor().Persone.NickName,
                            ClientName = item.Client.Persone.Name+' '+ item.Client.Persone.Patronimic,
                            SubName = item.Direction.DirectionName
                        };
                        Records.Add(record);
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
        }


        private void LoadPreviosData()//setting default values in DB ​​on first start
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var COrders = db.Orderlines.Include("Client")
                                               .Include("Dates")
                                               .Include("Subject")
                                               .Include("Author")
                                               .Include("Status")
                                               .Include("Money")
                                               .Include("WorkType")
                                               .ToList<OrderLine>();
                    //add default orders statuses
                    if (db.Statuses.Count() == 0)
                    {                       
                        db.Statuses.Add(new Status() { StatusName = "---" });
                        db.Statuses.Add(new Status() { StatusName = "принят" });
                        db.Statuses.Add(new Status() { StatusName = "готов" });
                        db.Statuses.Add(new Status() { StatusName = "выполняется" });
                        db.Statuses.Add(new Status() { StatusName = "отдан зказчику" });
                        db.Statuses.Add(new Status() { StatusName = "на оценке" });
                        db.Statuses.Add(new Status() { StatusName = "принят на доработку" });
                        db.Statuses.Add(new Status() { StatusName = "дорабатывается" });
                        db.Statuses.Add(new Status() { StatusName = "отказ от выполнения" });
                        db.Statuses.Add(new Status() { StatusName = "ждем новостей от заказчика" });
                        db.SaveChanges();
                    }
                    //add default authorstatuses
                    if (db.AuthorStatuses.Count() == 0)
                    {                        
                        db.AuthorStatuses.Add(new AuthorStatus() { AuthorStatusName = "---" });
                        db.AuthorStatuses.Add(new AuthorStatus() { AuthorStatusName = "работает" });
                        db.AuthorStatuses.Add(new AuthorStatus() { AuthorStatusName = "уволен" });
                        db.AuthorStatuses.Add(new AuthorStatus() { AuthorStatusName = "не проверен" });
                        db.AuthorStatuses.Add(new AuthorStatus() { AuthorStatusName = "проверяется" });
                        db.AuthorStatuses.Add(new AuthorStatus() { AuthorStatusName = "на паузе" });
                    }
                    if (db.Directions.Count() == 0)
                    {
                        db.Directions.Add(new Direction() { DirectionName = "---" });                        
                        db.SaveChanges();
                    }
                    //add default worktype
                    if (db.WorkTypes.Count() == 0)
                    {
                        db.WorkTypes.Add(new WorkType() { TypeOfWork = "---" });
                        db.WorkTypes.Add(new WorkType() { TypeOfWork = "контрольная" });
                        db.WorkTypes.Add(new WorkType() { TypeOfWork = "курсовая" });
                        db.WorkTypes.Add(new WorkType() { TypeOfWork = "дипломная" });
                        db.WorkTypes.Add(new WorkType() { TypeOfWork = "реферат" });
                        db.WorkTypes.Add(new WorkType() { TypeOfWork = "отчет по практике" });
                        db.WorkTypes.Add(new WorkType() { TypeOfWork = "распечатка" });
                        db.WorkTypes.Add(new WorkType() { TypeOfWork = "ксерокопия" });
                        db.WorkTypes.Add(new WorkType() { TypeOfWork = "сканкопия" });
                        db.WorkTypes.Add(new WorkType() { TypeOfWork = "набор текста" });
                        db.WorkTypes.Add(new WorkType() { TypeOfWork = "чертежи" });
                        db.WorkTypes.Add(new WorkType() { TypeOfWork = "тезисы" });
                        db.WorkTypes.Add(new WorkType() { TypeOfWork = "статья" });
                        db.SaveChanges();
                    }                   
                    //add default subjects
                    if (db.Subjects.Count() == 0)
                    {
                        db.Subjects.Add(new Subject() { SubName = "---" });
                        db.SaveChanges();
                    }
                   //add default author
                    if (db.Authors.Count() == 0)
                    {

                        //Persone = new Persone() { Name = "---", NickName = "---" };

                        //Contacts Contacts = new Contacts() { Phone1 = "+380000000000" };
                        //Persone.Contacts = Contacts;

                        //PersoneDescription PersoneDescription = new PersoneDescription();
                        //Persone.PersoneDescription = PersoneDescription;


                        //Persone.Dates.Add(new Dates());

                        //Author.AuthorStatus = db.AuthorStatuses.Find(new AuthorStatus() { AuthorStatusId = 1 }.AuthorStatusId);                         
                        //Author.Direction.Add(db.Directions.Find(new Direction() { DirectionId = 1 }.DirectionId));
                        //Author.Subject.Add(db.Subjects.Find(new Subject() { SubjectId = 1 }.SubjectId));                        
                        //db.Authors.Add(Author);
                        //db.SaveChanges();
                        
                        Persone = new Persone() { NickName = "---" };
                        Contacts Contacts = new Contacts() { Phone1 = "---" };
                        Persone.Contacts = Contacts;
                        Author = new Author();
                        Persone.Author.Add(Author);
                        db.Persones.Add(Persone);
                        db.SaveChanges();
                    }
                    //add default sources
                    if (db.Sources.Count() == 0)
                    {
                        db.Sources.Add(new Source() {SourceName = "---" });
                        db.Sources.Add(new Source() { SourceName = "Вайбер" });
                        db.Sources.Add(new Source() { SourceName = "Соцсети" });
                        db.Sources.Add(new Source() { SourceName = "Сайт" });
                        db.Sources.Add(new Source() { SourceName = "Оформлен лично в офисе" });
                        db.SaveChanges();
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
        }

        private RelayCommand updateOrederList;

        public RelayCommand UpdateOrederList => updateOrederList ?? (updateOrederList = new RelayCommand(
                    (obj) =>
                    {
                        Records.Clear();
                        LoadData();
                    }
                    ));


        private RelayCommand newOrder;
        public RelayCommand NewOrder => newOrder ?? (newOrder = new RelayCommand(
                    (obj) =>
                    {
                        EditOrder editOrder = new EditOrder();
                        editOrder.Owner = Application.Current.MainWindow;                        
                        showWindow.ShowWindow(editOrder);
                    }
                    ));

        private RelayCommand closeMainWindow;

        public RelayCommand CloseMainWindow => closeMainWindow ?? (closeMainWindow = new RelayCommand(
                    (obj) =>
                    {                        
                        mainWindow.Close();
                    }
                    ));


        //AddAuthorCommand
        private RelayCommand addAuthorCommand;
        public RelayCommand AddAuthorCommand => addAuthorCommand ?? (addAuthorCommand = new RelayCommand(
                    (obj) =>
                    {
                        AuthorWindow authorWindow = new AuthorWindow();
                        authorWindow.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(authorWindow);
                    }
                    ));

    }
}
