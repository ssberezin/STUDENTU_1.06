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
            ContactsRecords = new ObservableCollection<Contacts>();
            BlackListRecords = new ObservableCollection<BlackListHelpModel>();
            Author = new Author();
            _Contacts = new _Contacts();
            Date = new Dates();
            _Dir = new _Direction();
            Order = new OrderLine { OrderNumber = GetOrderNumber() };
            Persone = new Persone();
            PersoneDescription = new PersoneDescription();
            Price = new Money();
            _Status = new _Status();
            _Subj = new _Subject();
            _Source = new _Source();
            _WorkType = new _WorkType();

            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
            //this.showWindow = showWindow;
            //this.dialogService = dialogService;
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
                        //   SaveNewOrder();
                        SaveOrderChanges();
                    }
                    ));

        private void SaveOrderChanges()
        {
            if (saved)
                SaveDoubleOrder();
            else
                SaveNewOrder();
        }


        private void SaveDoubleOrder()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    Order = new OrderLine() { OrderNumber = TMPStaticClass.CurrentOrder.OrderNumber };
                    Order.Client = new Client() { Persone = Persone };
                    Order.Direction = db.Directions.Find(_Dir.Dir.DirectionId);

                    Order.WorkType = db.WorkTypes.Find(_WorkType.WorkType.WorkTypeId);
                    Order.Dates = Date;
                    Order.Subject = db.Subjects.Find(_Subj.Subj.SubjectId); ;
                    Order.Money = Price;
                    _Status.Status = TMPStaticClass.CurrentOrder.Status;
                    Order.Status = _Status.Status;
                    Order.Source = db.Sources.Find(_Source.Source.SourceId);
                    Order.Saved = true;
                    //db.Entry(Order).State = Order.OrderLineId == 0 ? EntityState.Added : EntityState.Modified;
                    db.Orderlines.Add(Order);
                    db.SaveChanges();
                    dialogService.ShowMessage($"Заказ с номером {Order.OrderNumber} сохранен как новый");
                    TMPStaticClass.CurrentOrder = Order;
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

        private void SaveNewOrder()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {

                    Persone.Contacts = _Contacts.Contacts;
                    Order.Direction = db.Directions.Find(_Dir.Dir.DirectionId);
                    Order.Client = new Client() { Persone = Persone };
                    Order.WorkType = db.WorkTypes.Find(_WorkType.WorkType.WorkTypeId);
                    Order.Dates = Date;
                    Order.Subject = db.Subjects.Find(_Subj.Subj.SubjectId); ;
                    Order.Money = Price;
                    _Status.Status = db.Statuses.Find(new Status() { StatusId = 2 }.StatusId);
                    Order.Status = _Status.Status;
                    Order.Source = db.Sources.Find(_Source.Source.SourceId);
                    Order.Saved = true;
                    //db.Entry(Order).State = Order.OrderLineId == 0 ? EntityState.Added : EntityState.Modified;                                   
                    db.Orderlines.Add(Order);
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
