﻿
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Model.DBModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Views;
using STUDENTU_1._06.Views.EditOrderWindows.RuleOrderLineWindows;

namespace STUDENTU_1._06.ViewModel
{
    public partial class ForEditOrder : Helpes.ObservableObject
    {
        public ObservableCollection<Contacts> ContactsRecords { get; set; }

        private Window editWindow;
        private Window editDirection;

        bool saved = false;//флаг, для того, чтоб понимать, был ли сохранен заказ в первый раз или нет.

        IDialogService dialogService;
        IShowWindowService showWindow;

        //==========================================PROPERTIES============================================
        //for set value of order number. Need to work more            

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


        private _Contacts _contacts;
        public _Contacts _Contacts
        {
            get { return _contacts; }
            set
            {
                if (_contacts != value)
                {
                    _contacts = value;
                    OnPropertyChanged(nameof(_Contacts));
                }
            }
        }


        private Dates date;
        public Dates Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged(nameof(Date));
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

        //this.DataContext = new ForEditOrder(this, new DefaultShowWindowService(),
        //    new DefaultDialogService());

        //public ForEditOrder(Window editWindow, DefaultShowWindowService showWindow,
        //   IDialogService dialogService)
        public ForEditOrder()
        {

            DefaultLoadData();
            Order = new OrderLine { OrderNumber = GetOrderNumber() };
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
            //this.showWindow = showWindow;
            //this.dialogService = dialogService;
        }

        private void DefaultLoadData()
        {
            ContactsRecords = new ObservableCollection<Contacts>();
            BlackListRecords = new ObservableCollection<BlackListHelpModel>();
            Author = new Author();
            _Contacts = new _Contacts();
            Date = new Dates();
            _Dir = new _Direction();
            Persone = new Persone();
            PersoneDescription = new PersoneDescription();
            Price = new Money();
            _Status = new _Status();
            _Subj = new _Subject();
            _Source = new _Source();
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
                      
                        SaveOrderChanges();
                    }
                    ));
        //эта хрень осталась не востребованной т.к. не удалась первичная задумка
        private void SaveOrderChanges()
        {
            if (saved)
                dialogService.ShowMessage("Заказ уже сохранен");
            else
                SaveNewOrder();
        }

        private RelayCommand createDobleNewOrderLine;
        public RelayCommand CreateDobleNewOrderLine => createDobleNewOrderLine ?? (createDobleNewOrderLine = new RelayCommand(
                    (obj) =>
                    {

                        if (saved)
                        {
                            if (dialogService.YesNoDialog("Разбить заказ на подзаказы?") == true)                       
                                DoubleOrder();
                       
                        }
                    }
                    ));

        bool doubleSave = false;
        private void DoubleOrder()
        {

            Order = new OrderLine() { OrderNumber = TMPStaticClass.CurrentOrder.OrderNumber };
            Order.Saved = false;
            Order.ParentOrder = false;
            Order.DescriptionForClient = TMPStaticClass.CurrentOrder.DescriptionForClient;
            Order.WorkDescription = TMPStaticClass.CurrentOrder.WorkDescription;
            Order.Variant = TMPStaticClass.CurrentOrder.Variant;
            _Contacts.Contacts = TMPStaticClass.CurrentOrder.Client.Persone.Contacts;
            Order.Client = new Client() { Persone = Persone };
            Persone = TMPStaticClass.CurrentOrder.Client.Persone;
            _Dir.Dir = TMPStaticClass.CurrentOrder.Direction;
            // _Dir.Dir = db.Directions.Find(new Direction() { DirectionId=1}.DirectionId);
            _WorkType.WorkType = TMPStaticClass.CurrentOrder.WorkType;
            // _WorkType.WorkType= db.WorkTypes.Find(new WorkType() { WorkTypeId = 1 }.WorkTypeId);
            Date = new Dates();
            Date.DateOfReception = TMPStaticClass.CurrentOrder.Dates.DateOfReception;
            Date.DeadLine = TMPStaticClass.CurrentOrder.Dates.DeadLine;
            _Subj.Subj = TMPStaticClass.CurrentOrder.Subject;
            //_Subj.Subj = db.Subjects.Find(new Subject() {SubjectId=1 }.SubjectId);
            Price = new Money();
            _Status.Status = TMPStaticClass.CurrentOrder.Status;
            // _Status.Status = db.Statuses.Find(new Status() {StatusId=1 }.StatusId);
            _Source.Source = TMPStaticClass.CurrentOrder.Source;
            //_Source.Source = db.Sources.Find(new Source() {SourceId=1 }.SourceId);
            saved = false;
            doubleSave = true;
            dialogService.ShowMessage("Действие выполнено");

            
        }

        private void SaveNewOrder()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    if (doubleSave)
                    {
                        
                        db.Persones.Attach(Persone);
                        db.Directions.Attach(_Dir.Dir);
                        db.WorkTypes.Attach(_WorkType.WorkType);
                        db.Dates.Add(Date);
                        db.Subjects.Attach(_Subj.Subj);
                        db.Moneys.Add(Price);
                        db.Statuses.Attach(_Status.Status);
                        db.Sources.Attach(_Source.Source);                        
                    }
                    Persone.Contacts = _Contacts.Contacts;
                    Order.Direction = db.Directions.Find(_Dir.Dir.DirectionId);
                    Order.WorkType = db.WorkTypes.Find(_WorkType.WorkType.WorkTypeId);
                    Order.Client = new Client() { Persone = Persone };
                    Order.Subject = db.Subjects.Find(_Subj.Subj.SubjectId);                    
                    Order.Source = db.Sources.Find(_Source.Source.SourceId);
                    Order.Dates = Date;
                    Order.Money = Price;
                    if (!doubleSave)                    
                        _Status.Status = db.Statuses.Find(new Status() { StatusId = 2 }.StatusId);
                    Order.Status = _Status.Status;
                    
                    Order.Saved = true;

                    db.Configuration.AutoDetectChangesEnabled = false;
                    db.Configuration.ValidateOnSaveEnabled = false;

                    //db.Orderlines.Attach(Order);
                    db.Orderlines.Add(Order);
                   
                    db.SaveChanges();
                    if (doubleSave)
                         EditOrderCount(TMPStaticClass.CurrentOrder.OrderLineId,TMPStaticClass.CurrentOrder.OrderNumber);
                    dialogService.ShowMessage("Данные о заказе сохранены");
                    SaveOrderToTMPOrder();
                    //TMPStaticClass.CurrentOrder = Order;
                    saved = true;
                    doubleSave = false;
                    
                   
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

        //тут мы копируем все поля из окна EditOrder.xaml в буферную переменную 
        //here we copy all the fields from the EditOrder.xaml window to the buffer variable
        private void SaveOrderToTMPOrder()
        {
            TMPStaticClass.CurrentOrder = Order;
            // TMPStaticClass.CurrentOrder = new OrderLine() { OrderNumber = Order.OrderNumber };
            TMPStaticClass.CurrentOrder.WorkInCredit = Order.WorkInCredit;
            TMPStaticClass.CurrentOrder.DescriptionForClient = Order.DescriptionForClient;
            TMPStaticClass.CurrentOrder.WorkDescription = Order.WorkDescription;
            TMPStaticClass.CurrentOrder.Variant = Order.Variant;
            Persone.Contacts = _Contacts.Contacts;
            //TMPStaticClass.CurrentOrder.Client = new Client() { Persone = Persone };
            //TMPStaticClass.CurrentOrder.Client.Persone.Contacts = _Contacts.Contacts;
            TMPStaticClass.CurrentOrder.Client.Persone = Persone;
            TMPStaticClass.CurrentOrder.Direction = _Dir.Dir;
            TMPStaticClass.CurrentOrder.WorkType = _WorkType.WorkType;
            TMPStaticClass.CurrentOrder.Dates.DateOfReception = Date.DateOfReception;
            TMPStaticClass.CurrentOrder.Dates.DeadLine = Date.DeadLine;
            TMPStaticClass.CurrentOrder.Subject= _Subj.Subj;
            TMPStaticClass.CurrentOrder.Money = Price;
            TMPStaticClass.CurrentOrder.Status = _Status.Status;
            TMPStaticClass.CurrentOrder.Source = _Source.Source;
        }

        //to edit OrderCount in Order
        private void EditOrderCount(int orderId, int orderNumber)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    
                    var res = db.Orderlines.Where(c => c.OrderNumber == orderNumber).ToList();
                    int count = res.Count;
                    
                    if (!CheckPreviosOrder(orderId, orderNumber))
                        foreach (var i in res)
                            i.OrderCount = count;
                    else
                        foreach (var i in res)
                            i.OrderCount = -count;



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

        //проверяем предыдущий заказ на наличие подзаказов. 
        // check the previous order for sub-orders
        private bool CheckPreviosOrder(int orderId, int oredrNumber)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res = db.Orderlines.Where(c => c.OrderLineId == orderId - 1);
                    if (res != null && (res as OrderLine).OrderNumber == oredrNumber|| res != null && !(res as OrderLine).ParentOrder)
                        return true;
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

            return false;

        }

        //EditOrderLineCommand

        //===========================================COMMAND FOR EDIT ORDER ======================================
        private RelayCommand editOrderLineCommand;

        public RelayCommand EditOrderLineCommand => editOrderLineCommand ?? (editOrderLineCommand = new RelayCommand(
                    (obj) =>
                    {
                        EditOrderLine();
                    }
                    ));
        //========================================================================================================================

        private void EditOrderLine()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    db.Entry(Order).State = EntityState.Modified;
                    Persone.Contacts = _Contacts.Contacts;
                    Order.Client = new Client() { Persone = Persone };
                    Order.Direction = db.Directions.Find(_Dir.Dir.DirectionId);
                    Order.WorkType = db.WorkTypes.Find(_WorkType.WorkType.WorkTypeId);
                    Order.Dates = Date;
                    Order.Subject = db.Subjects.Find(_Subj.Subj.SubjectId); ;
                    Order.Money = Price;
                    _Status.Status = db.Statuses.Find(new Status() { StatusId = 2 }.StatusId);
                    Order.Status = _Status.Status;
                    Order.Source = db.Sources.Find(_Source.Source.SourceId);
                    Order.Saved = true;
                    db.SaveChanges();
                    dialogService.ShowMessage("Данные о заказе сохранены");
                    TMPStaticClass.CurrentOrder = Order;
                    saved = true;
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


        //this method not used
        //этот метод пока нигде не задейстован, но пусть типа будет
        //взят от сюдава https://professorweb.ru/my/entity-framework/6/level3/3_6.php
        public static void Update<TEntity>(TEntity entity, DbContext context)
    where TEntity : class
        {
            //// Настройки контекста
            //context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            context.Entry<TEntity>(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        //==================================COMMAND FOR CLOSE WINDOW ==========================
        private RelayCommand closeWindowCommand;

        public RelayCommand CloseWindowCommand => closeWindowCommand ?? (closeWindowCommand = new RelayCommand(
                    (obj) =>
                    {
                        CloseWindow(obj as Window);
                    }
                    ));
        //========================================================================================================================

        private void CloseWindow(Window window)
        {
            window.Close();
        }


        //==================================COMMAND FOR CLOSE WINDOW ==========================
        private RelayCommand cancelSaveContactsCommand;

        public RelayCommand CancelSaveContactsCommand => cancelSaveContactsCommand ?? (cancelSaveContactsCommand = new RelayCommand(
                    (obj) =>
                    {

                        CloseWindow(obj as Window);
                    }
                    ));

        //call RuleOrderLineWindow
        private RelayCommand newRuleOrderLineWindowCommand;
        public RelayCommand NewRuleOrderLineWindowCommand =>
            newRuleOrderLineWindowCommand ?? (newRuleOrderLineWindowCommand = new RelayCommand(
                    (obj) =>
                    {
                        RuleOrderLineWindow ruleOrderLineWindow = new RuleOrderLineWindow();
                        showWindow.ShowDialog(ruleOrderLineWindow);

                    }
                    ));

        //=====================Command for call AddContactsWindow.xaml ======================================
        private RelayCommand newEditContactCommand;
        public RelayCommand NewEditContactCommand => newEditContactCommand ??
            (newEditContactCommand = new RelayCommand(
                    (obj) =>
                    {
                        _Contacts.TmpContacts = _Contacts.Contacts;
                        _Contacts.NewEditContacts(new AddContactsWindow(obj));
                    }
                    ));

    }
}
