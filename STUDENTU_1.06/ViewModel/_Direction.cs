﻿using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using System;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;

namespace STUDENTU_1._06.ViewModel
{
    //Class for operations with Direction table
    public  class _Direction : Helpes.ObservableObject
    {
        public _Direction()
        {
            Dir = new Direction();
            DirRecords = new ObservableCollection<Direction>();
            LoadDirectionsData();
            showWindow = new DefaultShowWindowService();
            dialogService= new  DefaultDialogService();
        }

        IDialogService dialogService;
        IShowWindowService showWindow;

        public ObservableCollection<Direction> DirRecords { get; set; }

        private Direction dir;
        public Direction Dir
        {
            get { return dir; }
            set
            {
                if (dir != value)
                {
                    dir = value;
                    OnPropertyChanged(nameof(Dir));
                }
            }
        }

        //load data array from "Directions" table
        private void LoadDirectionsData()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var list = db.Directions.ToList<Direction>();
                    foreach (var item in list)
                    {
                        DirRecords.Add(
                        new Direction
                        {
                            DirectionId = item.DirectionId,
                            DirectionName = item.DirectionName
                        });
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

        //=====================Command for call Editing wondows ======================================
        private RelayCommand newEditDirection;
        public RelayCommand NewEditDirection => newEditDirection ?? (newEditDirection = new RelayCommand(
                    (obj) =>
                    {
                        EditDirection editDirection = new EditDirection(obj);
                        editDirection.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(editDirection);
                    }
                    ));

        //==================Commands for edit DIRECTIONS =========================================

        private RelayCommand addDirectionCommand;
        public RelayCommand AddDirectionCommand => addDirectionCommand ?? (addDirectionCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddDir();
                    }
                    ));

        private RelayCommand deleteDirectionCommand;
        public RelayCommand DeleteDirectionCommand => deleteDirectionCommand ??
            (deleteDirectionCommand = new RelayCommand((selectedItem) =>
            {
                if (selectedItem == null) return;
                DeleteDir();
            }
           ));

        private RelayCommand editDirectionCommand;
        public RelayCommand EditDirectionCommand => editDirectionCommand ?? (editDirectionCommand = new RelayCommand(
                    (selectedItem) =>
                    {
                        if (selectedItem == null) return;
                        EditDir();
                    }
                    ));

        //===================THIS METHOD IS FOR ADD RECORDS IN DIRECTIONS TABLE==============        
       public void  AddDir()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res1 = db.Directions.Any(o => o.DirectionName == Dir.DirectionName);
                    if (!res1)
                    {
                        if (!string.IsNullOrEmpty(Dir.DirectionName))
                        {
                            Dir.DirectionName = Dir.DirectionName.ToLower();
                            db.Directions.Add(Dir);
                            db.SaveChanges();
                            DirRecords.Clear();
                            LoadDirectionsData();
                            Dir = new Direction();
                           
                        }
                        else
                            return ;
                    }
                    else
                        dialogService.ShowMessage("Уже есть такое название в базе данных");
                    
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

        //===================THIS METHOD IS FOR DELETE RECORDS IN DIRECTIONS TABLE==============
        public void DeleteDir()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res = db.Orderlines;
                   
                            if (Dir.DirectionId != 1)
                            {
                                if (dialogService.YesNoDialog("Точно нужно удалить эту запись?") == true)
                                {
                                    //changing DB
                                    //we find all the records in which we have the desired Id and make a replacement
                                    foreach (OrderLine order in res)
                                    {
                                        if (order.Direction.DirectionId == Dir.DirectionId)
                                            order.Direction = db.Directions.Find(new Direction() { DirectionId = 1 }.DirectionId);
                                    }
                                    db.Directions.Remove(db.Directions.Find(Dir.DirectionId));
                                    db.SaveChanges();

                                    //changing collection
                                    DirRecords.Remove(Dir);
                                }
                            }
                            else
                                dialogService.ShowMessage("Нельзя удалить эту запись");                         
                        
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

        //===================THIS METHOD IS FOR EDIT RECORDS IN DIRECTIONS TABLE==============
        public void EditDir()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res1 = db.Directions.Find(Dir.DirectionId);
                    if (res1 != null)
                    {
                        //changing DB
                        res1.DirectionName = Dir.DirectionName.ToLower();
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

    }
}