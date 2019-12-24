using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using System;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using System.ComponentModel;

namespace STUDENTU_1._06.ViewModel
{
    //Class for operations with Direction table
    public  class _Direction : Helpes.ObservableObject
    {
        public ObservableCollection<Direction> DirRecords { get; set; }
        public ObservableCollection<Direction> AuthorDirections { get; set; }

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

        private Direction selectedDir;
        public Direction SelectedDir
        {
            get { return selectedDir; }
            set
            {
                if (selectedDir != value)
                {
                    selectedDir = value;
                    OnPropertyChanged(nameof(SelectedDir));
                }
            }
        }

        private Direction selectedDir2;
        public Direction SelectedDir2
        {
            get { return selectedDir2; }
            set
            {
                if (selectedDir2 != value)
                {
                    selectedDir2 = value;
                    OnPropertyChanged(nameof(SelectedDir2));
                }
            }
        }


        //for fast delete from AuthorDirections
        private int index;
        public int Index
        {
            get { return index; }
            set
            {
                if (index != value)
                {
                    index = value;
                    OnPropertyChanged(nameof(Index));
                }
            }
        }

        public _Direction()
        {
            Dir = new Direction();
            SelectedDir = new Direction();
            SelectedDir2 = new Direction();
            DirRecords = new ObservableCollection<Direction>();
            AuthorDirections = new ObservableCollection<Direction>();
            LoadDirectionsData();
            showWindow = new DefaultShowWindowService();
            dialogService= new  DefaultDialogService();
           
        }

        IDialogService dialogService;
        IShowWindowService showWindow;

        //load data array from "Directions" table
        public void LoadDirectionsData()
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
                    Dir = DirRecords[0];
                    SelectedDir2 = Dir;
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
                        showWindow.ShowDialog(editDirection);
                    }
                    ));

        //==================Commands for edit DIRECTIONS =========================================

        private RelayCommand addDirectionCommand;
        public RelayCommand AddDirectionCommand => addDirectionCommand ?? (addDirectionCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddDir(obj as string);
                    }
                    ));

        private RelayCommand deleteDirectionCommand;
        public RelayCommand DeleteDirectionCommand => deleteDirectionCommand ??
            (deleteDirectionCommand = new RelayCommand((selectedItem) =>
            {                
                DeleteDir();
            }
           ));

        private RelayCommand editDirectionCommand;
        public RelayCommand EditDirectionCommand => editDirectionCommand ?? (editDirectionCommand = new RelayCommand(
                    (obj) =>
                    {                       
                        EditDir(obj as string);
                    }
                    ));

        //===================THIS METHOD IS FOR ADD RECORDS IN DIRECTIONS TABLE==============        
       public void  AddDir(string newDirName)
        {

            Dir.DirectionName = newDirName;
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res1 = db.Directions.Any(o => o.DirectionName == Dir.DirectionName);
                    if (!res1)
                    {
                        if (!string.IsNullOrEmpty(Dir.DirectionName) || Dir.DirectionName != "---")
                        {
                            Dir.DirectionName = Dir.DirectionName.ToLower();
                            Dir.DirectionName.Trim();
                            if (Dir.DirectionName[0] == ' ')
                            {
                                dialogService.ShowMessage("Нельзя добавить пустую строку");                               
                                return;
                            }
                            db.Directions.Add(Dir);
                            db.SaveChanges();
                            DirRecords.Clear();
                            LoadDirectionsData();
                            Dir = new Direction();
                            SelectedDir2 = Dir;
                        }
                        else
                            return;                        
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
                   
                            if ( Dir.DirectionName != "---" &&!CheckRecordBeforDelete(Dir))
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
                                    AuthorDirections.Remove(Dir);
                                    DirRecords.Remove(Dir);
                                   
                                    Dir = DirRecords[0];
                                    SelectedDir2 = Dir;
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

        //check if the record has links with other tables before deleting
        private bool CheckRecordBeforDelete(Direction dir)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    //check in Orderlines table
                    var res = db.Orderlines;
                    foreach (OrderLine item in res)
                        if (item.Direction.DirectionId == dir.DirectionId ||
                           item.Direction.DirectionName == dir.DirectionName)
                            return true;
                    //if previos check  in Orderlines table wasn't true - check in Author table
                    var authorRes = db.Authors;
                    foreach (Author item in authorRes)
                        foreach(Direction i in item.Direction)
                        if (i.DirectionId == dir.DirectionId ||
                           i.DirectionName == dir.DirectionName)
                            return true;
                    return false;

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
            //есть подозрение, что такой подход не очень то правомерен, но пока лень с этим заморачиваться
            return false;
        }


        //===================THIS METHOD IS FOR EDIT RECORDS IN DIRECTIONS TABLE==============
        public void EditDir(string newDirName)
        {
            if (Dir.DirectionName == "---")
            {
                dialogService.ShowMessage("Нельзя редактировать эту запись");             
                return;
            } 
            Dir.DirectionName = newDirName;
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res1 = db.Directions.Find(Dir.DirectionId);
                    if (res1 != null)
                    {
                        //changing DB
                        res1.DirectionName = Dir.DirectionName.ToLower();
                        Dir.DirectionName.Trim();
                            
                        db.SaveChanges();
                        DirRecords.Clear();
                        LoadDirectionsData();
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


        //===================================COMMAND FOR ADD DIRECTIONS INTO AuthorDirections ============      

        private RelayCommand addAuthorDirectionCommand;
        public RelayCommand AddAuthorDirectionCommand => addAuthorDirectionCommand ??
            (addAuthorDirectionCommand = new RelayCommand((obj) =>
            {             
                    AddAuthorDirection();
            }
           ));

        private void AddAuthorDirection()
        {
            if (FindDir(SelectedDir2) || string.IsNullOrEmpty(SelectedDir2.DirectionName)|| SelectedDir2.DirectionName=="---")
            {
                dialogService.ShowMessage("Нельязя добавить эту запись");
            }
            else            
                AuthorDirections.Add(SelectedDir2);


        }

        //here we check AuthorDirections for the added item
        private bool FindDir(Direction dir)
        {
           
            foreach (Direction item in AuthorDirections)
                if (dir.DirectionId == item.DirectionId )
                    return true;
            return false;
        }

        //===================================COMMAND FOR DELETE DIRECTIONS FROM AuthorDirections ============      

        private RelayCommand delFromAuthorDirectionCommand;
        public RelayCommand DelFromAuthorDirectionCommand => delFromAuthorDirectionCommand ??
            (delFromAuthorDirectionCommand = new RelayCommand((selectedItem) =>
            {
                DelFromAuthorDirection();
            }
           ));

        private void DelFromAuthorDirection()
        {            
                AuthorDirections.Remove(AuthorDirections[Index]);
        }
    }
   
}
