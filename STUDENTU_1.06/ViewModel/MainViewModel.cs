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

namespace STUDENTU_1._06.ViewModel
{
    class MainViewModel : Helpes.ObservableObject
    {
        public ObservableCollection<Records> Records { get; set; }
        public DateFormatConverter DateFormatConverter { get; set; }
      
        private Persone selectedPersone;
        private Window mainWindow;


        public Persone SelectedOrderLine
        {
            get { return selectedPersone; }
            set
            {
                if (selectedPersone != value)
                {
                    selectedPersone = value;

                    OnPropertyChanged(nameof(selectedPersone));
                }
            }
        }

        IShowWindowService showWindow;

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
                            AuthorNickName = item.Author.Persone.NickName,
                            ClientName = item.Client.Persone.Name+' '+ item.Client.Persone.Patronimic,
                            SubName = item.Direction.DirectionName
                        };
                        Records.Add(record);
                    }
                }
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (OverflowException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (System.Data.Entity.Core.EntityCommandExecutionException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (System.Data.Entity.Core.EntityException ex)
                {
                    MessageBox.Show(ex.Message);
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
                    //need befor ferst start of programm and Db isn't created
                    if (db.Statuses.Count() == 0)
                    {
                        db.Statuses.Add(new Status() { StatusName = "принят" });
                        db.Statuses.Add(new Status() { StatusName = "готов" });
                        db.Statuses.Add(new Status() { StatusName = "выполняется" });
                        db.Statuses.Add(new Status() { StatusName = "отдан зказчику" });
                        db.Statuses.Add(new Status() { StatusName = "на оценке" });
                        db.SaveChanges();
                    }                    
                    if (db.Directions.Count() == 0)
                    {
                        db.Directions.Add(new Direction() { DirectionName = "---" });
                        db.SaveChanges();
                    }
                    if (db.WorkTypes.Count() == 0)
                    {
                        db.WorkTypes.Add(new WorkType() { TypeOfWork = "---" });
                        db.SaveChanges();
                    }
                    if (db.Subjects.Count() == 0)
                    {
                        db.Subjects.Add(new Subject() { SubName = "---" });
                        db.SaveChanges();
                    }                    
                    if (db.Authors.Count() == 0)
                    {
                        Persone = new Persone() { NickName = "---" };
                        Author = new Author();
                        Persone.Author.Add(Author);
                        db.Persones.Add(Persone);
                        db.SaveChanges();
                    }
                    if (db.Sources.Count() == 0)
                    {
                        db.Sources.Add(new Source() {SourceName = "---" });
                        db.SaveChanges();
                    }


                }
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (OverflowException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (System.Data.Entity.Core.EntityCommandExecutionException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (System.Data.Entity.Core.EntityException ex)
                {
                    MessageBox.Show(ex.Message);
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

    }
}
