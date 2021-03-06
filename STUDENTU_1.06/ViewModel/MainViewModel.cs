﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Linq;
using System.Windows;
using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.ViewModel.Filters;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Model.HelpModelClasses;
using STUDENTU_1._06.Model.DBModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Views.PersoneOperations.AuthorOperationsWindows;
using STUDENTU_1._06.Views.EditOrderWindows.EditOrderLine;
using STUDENTU_1._06.Views;
using STUDENTU_1._06.Views.PersoneOperations.UserOperations;
using STUDENTU_1._06.ViewModel.PersoneOperations.PersoneOperations;

namespace STUDENTU_1._06.ViewModel
{
    class MainViewModel : Helpes.ObservableObject
    {
        public ObservableCollection<Records> Records { get; set; }
        //public DateFormatConverter DateFormatConverter { get; set; }
      
        private Persone selectedPersone;
        private Window mainWindow;

       // _Authorisation AuthorisationTry;//to attempt authorization
       


        IDialogService dialogService;//for show messages in mvvm pattern order
        IShowWindowService showWindow;//for show messages in mvvm pattern order

        public MainViewModel(Window mainWindow, DefaultShowWindowService showWindow, int userId)
        {
            Records = new ObservableCollection<Records>();
            EndDateReception = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                //DateTime.Now;//.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute).AddMilliseconds(-DateTime.Now.Millisecond);
            StartDateReception = DateTime.Now.AddDays(-10).AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute).AddMilliseconds(-DateTime.Now.Millisecond);
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
            Usver = FindeUser(userId);
            if (Usver == null)
            {
                dialogService.ShowMessage("Проблема с авторизацией подвязкой пользователя к оформлению заказа");
                return;
            }
            UsrOps = new UserOps(userId,0);
            
            LoadData();
            this.mainWindow = mainWindow;            
            this.showWindow = showWindow;
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

        
        private Records selectedRecord;
        public Records SelectedRecord
        {
            get { return selectedRecord; }
            set
            {
                if (selectedRecord != value)
                {
                    selectedRecord = value;
                    OnPropertyChanged(nameof(SelectedRecord));
                }
            }
        }

        private _Filters _filters;
        public _Filters _Filters
        {
            get { return _filters; }
            set
            {
                if (_filters != value)
                {
                    _filters = value;
                    OnPropertyChanged(nameof(Filters));
                }
            }
        }

        //для фильтрации заказов по дате, начальная дата 
        //for order filtration by date, start date
        private DateTime startDateReception;
        public DateTime StartDateReception
        {
            get { return startDateReception; }
            set
            {
                if (startDateReception != value)
                {
                    startDateReception = value;
                    OnPropertyChanged(nameof(StartDateReception));
                }
            }
        }
        //для фильтрации заказов по дате, конечная дата 
        //for order filtration by date, end date
        private DateTime endDateReception;
        public DateTime EndDateReception
        {
            get { return endDateReception; }
            set
            {
                if (endDateReception != value)
                {
                    endDateReception = value;
                    OnPropertyChanged(nameof(EndDateReception));
                }
            }
        }

        private User usver;
        public User Usver
        {
            get { return usver; }
            set
            {
                if (usver != value)
                {
                    usver = value;
                    OnPropertyChanged(nameof(Usver));
                }
            }
        }

        private UserOps usrOps;
        public UserOps UsrOps
        {
            get { return usrOps; }
            set
            {
                if (usrOps != value)
                {
                    usrOps = value;
                    OnPropertyChanged(nameof(UsrOps));
                }
            }
        }


        private  User FindeUser(int usverId)
        {
            
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {                  
                    User usver = db.Users.Where(o => o.UserId == usverId).FirstOrDefault();                    
                    return usver;
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
                return null;
            }
        }


        private void LoadData()
        {
            LoadPreviosData();//setting default values in DB ​​on first start
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {                    
                    _Filters = new _Filters();
        
                    EndDateReception = DateTime.Now;                   
                    var COrders = db.Orderlines.Where(o => o.Dates.DateOfReception <= EndDateReception && o.Dates.DateOfReception >= StartDateReception).OrderBy(o => o.OrderNumber);
                    string authorNickName;
                    //creat a orderlist in datagrid (mainwindow)
                    foreach (var item in COrders)
                    {

                        if (item.GetExecuteAuthor(item.Author) == null)
                            authorNickName = "---";
                        else
                            authorNickName = item.GetExecuteAuthor(item.Author).Persone.NickName;


                        Records record = new Records
                        {
                            RecordId = item.OrderLineId,
                            OrderCount = item.OrderCount,
                            OrderNumber = item.OrderNumber,
                            DateOfReception = item.Dates.DateOfReception,
                            DeadLine = item.Dates.DeadLine,
                            DateDone = item.Dates.DateDone,
                            Price = item.Money.Price,
                            Prepayment = item.Money.Prepayment,
                            Status = item.Status.StatusName,
                            TypeOfWork = item.WorkType.TypeOfWork,                            
                            AuthorNickName=authorNickName,
                            ClientName = item.Client.Persone.Name+' '+ item.Client.Persone.Patronimic,
                            SubName = item.Direction.DirectionName
                        };
                        Records.Add(record);
                        _Filters.AllDirections.Add(item.Direction);
                    }
                    TMPStaticClass.TMPFilters = (_Filters)_Filters.Clone();                    
                   


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

        //от этого метода желательно уйти в дальнейшем, но пока лень переделывать
        //он не столь ресурсозатратен...
        private void LoadPreviosData()//setting default values in DB ​​on first start
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    if (db.Universities.Count() == 0)
                    {
                        db.Universities.Add(new University() { UniversityName = "---", City="---" });
                        db.Universities.Add(new University() { UniversityName = "(КНУ) Криворожский национальный университет", City = "Кривой Рог" });
                        db.Universities.Add(new University() { UniversityName = "(КГПУ) Криворожский государственный педагогический университет", City = "Кривой Рог" });
                        db.Universities.Add(new University() { UniversityName = "(КЭИ) Киевский экономический", City = "Кривой Рог" });
                        db.Universities.Add(new University() { UniversityName = "Одесская юридическая академия", City = "Кривой Рог" });
                        db.Universities.Add(new University() { UniversityName = "(НМетАУ) Национальная Металлургическая академия Украины", City = "Кривой Рог" });
                        db.Universities.Add(new University() { UniversityName = "(ДонНТУ) Донецкий национальный технический университет", City = "Кривой Рог" });
                        db.SaveChanges();
                    }

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
                        db.Directions.Add(new Direction() { DirectionName = "физика" });
                        db.Directions.Add(new Direction() { DirectionName = "математика" });
                        db.Directions.Add(new Direction() { DirectionName = "химия" });
                        db.Directions.Add(new Direction() { DirectionName = "экономика" });
                        db.Directions.Add(new Direction() { DirectionName = "бухгалтерский учет" });
                        db.Directions.Add(new Direction() { DirectionName = "история" });
                        db.Directions.Add(new Direction() { DirectionName = "педагогика" });
                        db.Directions.Add(new Direction() { DirectionName = "психология" });
                        db.Directions.Add(new Direction() { DirectionName = "программирование" });
                        db.Directions.Add(new Direction() { DirectionName = "информатика" });
                        db.Directions.Add(new Direction() { DirectionName = "лингвистика" });
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
                        db.Subjects.Add(new Subject() { SubName = "высшая математика" });                        
                        db.Subjects.Add(new Subject() { SubName = "теория вероятностей" });
                        db.Subjects.Add(new Subject() { SubName = "математическая статистика" });
                        db.Subjects.Add(new Subject() { SubName = "ТОЭ(теоретические основы электротехники)" });
                        db.Subjects.Add(new Subject() { SubName = "дошкольная педагогика" });
                        db.Subjects.Add(new Subject() { SubName = "украинский язык" });
                        db.Subjects.Add(new Subject() { SubName = "английский язык" });
                        db.SaveChanges();
                    }
                   //add default author
                    if (db.Authors.Count() == 0)
                    {
                        Persone = new Persone() { NickName = "---" };
                        Contacts Contacts = new Contacts() { Phone1 = "---" };
                        Persone.Contacts = Contacts;
                        Author = new Author();
                        Author.AuthorStatus= db.AuthorStatuses.Find(new AuthorStatus() { AuthorStatusId = 1 }.AuthorStatusId);
                        Author.Subject.Add(db.Subjects.Find(new Subject() { SubjectId=1}.SubjectId));
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
                        EditOrderRedactionCall(0, Usver.UserId);
                    }
                    ));


        private void EditOrderRedactionCall(int Orderid, int UserId)
        {
            EditOrderRedaction editOrderRedaction;
            if (Orderid!=0 )
                 editOrderRedaction = new EditOrderRedaction(Orderid, UserId );//for edit order
            else
                editOrderRedaction = new EditOrderRedaction(UserId);//for recept new order
            editOrderRedaction.Owner = Application.Current.MainWindow;
            showWindow.ShowWindow(editOrderRedaction);
        }

        private RelayCommand editOrderCommand;
        public RelayCommand EditOrderCommand => editOrderCommand ?? (editOrderCommand = new RelayCommand(
                    (obj) =>
                    {                        
                        EditOrderRedactionCall(SelectedRecord.RecordId, Usver.UserId);
                    }
                    ));

        private RelayCommand deleteOrderCommand;
        public RelayCommand DeleteOrderCommand => deleteOrderCommand ?? (deleteOrderCommand = new RelayCommand(
                    (obj) =>
                    {
                        string msg = "Точно удалять?";
                        DeleteRecord(obj as Records, msg);
                    }
                    ));

        private void DeleteRecord(Records i, string msg)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {    
                    
                    //хз , как эта движуха работае в реале. Не удаляется ли чего нужного... надо тестить
                    if (!dialogService.YesNoDialog(msg))
                        return;
                    OrderLine tmpOrder = db.Orderlines.Where(c => c.OrderLineId == i.RecordId).FirstOrDefault();
                    if (tmpOrder != null)
                    {
                        db.Orderlines.Remove(tmpOrder);
                        db.SaveChanges();
                    }
                    Records.Clear();
                    LoadData();
                    
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

        //for edit author data
        private RelayCommand callEditAuthorWindow;
        public RelayCommand CallEditAuthorWindow => callEditAuthorWindow ?? (callEditAuthorWindow = new RelayCommand(
                    (obj) =>
                    {
                        AuthorEditWindow authorEditWindow = new AuthorEditWindow();
                        authorEditWindow.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(authorEditWindow);
                    }
                    ));

        //AddTestInfoCommand не работает нихера, падло. Дета лаги,надо шукать,а лень
        //======================================COMMAND FOR ADD TEST INFO TO DB==========================
        private RelayCommand addTestInfoCommand;
        public RelayCommand AddTestInfoCommand => addTestInfoCommand ?? (addTestInfoCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddTestInfo();
                    }
                    ));
        private void AddTestInfo()
        {
            AddAuthor("Ольга", "Кравченко", "о1", "+3809625148765",1,3, 4,2, 1, 9);
            AddAuthor("Ольга", "", "о2", "+3809725118764", 1,6, 8, 5, 2, 9);
            AddAuthor("Ирина", "Петрусь", "и_укр", "+3809625218785", 1,5, 6, 4, 5, 7.5);
            SaveNewOrder("Иван", "+380962514258", 3, 2,4, 350, 350, 2);
            SaveNewOrder("Егор", "+380972514456", 5, 6, 3, 950, 950, 2);
            SaveNewOrder("Виктор", "+380662514214", 6, 3,5, 450, 450, 2);
        }
        private void AddAuthor(string name,string surname, string nickName,
                               string phone1, int authorstatus, int subj1, int subj2, 
                               int dir1, int dir2, double authorrating)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {   
                        Persone = new Persone(){
                            Name =name,
                            Surname=surname,
                            NickName = nickName
                        };
                        Contacts Contacts = new Contacts() { Phone1 = phone1 };
                        Persone.Contacts = Contacts;
                        Author = new Author();
                        Author.AuthorStatus = db.AuthorStatuses.Find(new AuthorStatus() {
                        AuthorStatusId =authorstatus}.AuthorStatusId);
                        Author.Subject.Add(db.Subjects.Find(new Subject() { SubjectId=subj1 }.SubjectId));
                        Author.Subject.Add(db.Subjects.Find(new Subject() { SubjectId = subj2 }.SubjectId));
                        Author.Direction.Add(db.Directions.Find(new Direction() { DirectionId = dir1 }.DirectionId));
                        Author.Direction.Add(db.Directions.Find(new Direction() { DirectionId = dir2 }.DirectionId));
                        Author.Rating = authorrating;
                        Persone.Author.Add(Author);                        
                        db.Persones.Add(Persone);
                        db.SaveChanges();
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

        private void SaveNewOrder(string name,string phone1, int dir, int workType,
                                int subj,decimal price, decimal prepayment, int source)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    OrderLine Order = new OrderLine();
                    Persone = new Persone()
                    {
                        Name = name                                            
                    };
                    Contacts Contacts = new Contacts() { Phone1 = phone1 };
                    Order.Direction = db.Directions.Find(new Direction() {DirectionId=dir}.DirectionId);
                    Order.Client = new Client() { Persone = Persone };
                    Order.WorkType = db.WorkTypes.Find(new WorkType() { WorkTypeId=workType}.WorkTypeId);
                    Order.Dates = new Dates() {
                        DateOfReception =DateTime.Now,
                        DeadLine=DateTime.Now.AddDays(5),
                        AuthorDeadLine= DateTime.Now.AddDays(4)
                    };
                    Order.Subject = db.Subjects.Find(new Subject() { SubjectId=subj}.SubjectId);
                    Order.Money = new Money() {Price=price, Prepayment=prepayment };
                    Order.Status = db.Statuses.Find(new Status() { StatusId = 2 }.StatusId);
                    Order.Source = db.Sources.Find(new Source() { SourceId=source}.SourceId);
                    Order.User = db.Users.Where(e=>e.UserId==Usver.UserId).FirstOrDefault();
                    db.Orderlines.Add(Order);
                    db.SaveChanges();                    
                    
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


        ////=================================COMANDS FOR USER DATA OPERATIONS =====================

        //private RelayCommand addNewUserCommand;
        //public RelayCommand AddNewUserCommand => addNewUserCommand ?? (addNewUserCommand = new RelayCommand(
        //            (obj) =>
        //            {
        //                AddNewUserByRegUser();
        //            }
        //            ));

        //private void AddNewUserByRegUser()
        //{
        //    if (Usver.AccessName != "Мастер-админ")
        //    {
        //        dialogService.ShowMessage("Нет прав доступа");
        //        return;
        //    }
        //    UserRegistration usver;
        //    usver = new UserRegistration();            
        //    showWindow.ShowWindow(usver);
        //}




    }
}
