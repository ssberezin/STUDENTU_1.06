using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using System;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using STUDENTU_1._06.Views.EditOrderWindows;
using STUDENTU_1._06.Model.DBModelClasses;

namespace STUDENTU_1._06.ViewModel
{
    public class _Source : Helpes.ObservableObject
    {

        public _Source()
        {
            Source = new Source();
           
            SourcesRecords = new ObservableCollection<Source>();
           
            LoadSourcesData();
            showWindow = new DefaultShowWindowService();
            dialogService = new DefaultDialogService();
        }

        IDialogService dialogService;
        IShowWindowService showWindow;
        public ObservableCollection<Source> SourcesRecords { get; set; }

        private Source source;
        public Source Source
        {
            get { return source; }
            set
            {
                if (source != value)
                {
                    source = value;
                    OnPropertyChanged(nameof(Source));
                }
            }
        }

        //load data array from "Sources" table
        private void LoadSourcesData()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var list = db.Sources.ToList<Source>();
                    foreach (var item in list)
                    {
                        SourcesRecords.Add(
                        new Source
                        {
                            SourceId = item.SourceId,
                            SourceName = item.SourceName
                        });
                    }
                    Source = SourcesRecords[0];
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

        //=====================Commands for call Editing wondow ======================================

        private RelayCommand newEditSource;
        public RelayCommand NewEditSource => newEditSource ?? (newEditSource = new RelayCommand(
                    (obj) =>
                    {
                        EditSourceWindow editSourceWindow = new EditSourceWindow(obj);
                        editSourceWindow.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(editSourceWindow);
                    }
                    ));

        //==================Commands for edit SOURCES =========================================


        private RelayCommand addSourceCommand;
        public RelayCommand AddSourceCommand => addSourceCommand ?? (addSourceCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddSource();
                    }
                    ));

        private RelayCommand deleteSourceCommand;
        public RelayCommand DeleteSourceCommand => deleteSourceCommand ??
            (deleteSourceCommand = new RelayCommand((selectedItem) =>
            {
                if (selectedItem == null) return;
                DeleteSource();
            }
            ));

        private RelayCommand editSourceCommand;
        public RelayCommand EditSourceCommand => editSourceCommand ?? (editSourceCommand = new RelayCommand(
                    (selectedItem) =>
                    {
                        if (selectedItem == null) return;
                        EditSource(); ;
                    }
                    ));
      
        //===================THIS METHOD IS FOR DELETE RECORDS FROM SOURCE TABLE==============        
        public void DeleteSource()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res = db.Orderlines;
                    if (Source.SourceId != 1)
                    {
                        if (dialogService.YesNoDialog("Точно нужно удалить эту запись?") == true)
                        {
                            //changing DB
                            //we find all the records in which we have the desired Id and make a replacement
                            foreach (OrderLine order in res)
                            {
                                if (order.Source.SourceId == Source.SourceId)
                                    order.Source = db.Sources.Find(new Source() { SourceId = 1 }.SourceId);
                            }
                            db.Sources.Remove(db.Sources.Find(Source.SourceId));
                            db.SaveChanges();
                            //changing collection
                            SourcesRecords.Remove(Source);
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
        private bool CheckRecordBeforDelete(Source source)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    //check in Orderlines table
                    var res = db.Orderlines;
                    foreach (OrderLine item in res)
                        if (item.Source.SourceId == source.SourceId ||
                           item.Source.SourceName == source.SourceName)
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

        //===================THIS METHOD IS FOR ADD RECORDS IN SOURCE TABLE==============
        public void AddSource()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res3 = db.Sources.Any(o => o.SourceName == Source.SourceName);
                    if (!res3)
                    {
                        if (!string.IsNullOrEmpty(Source.SourceName))
                        {
                            Source.SourceName.Trim();
                            if (Source.SourceName[0] == ' ')
                            {
                                dialogService.ShowMessage("Нельзя добавить пустую строку");
                                return;
                            }
                            db.Sources.Add(Source);
                            db.SaveChanges();
                            SourcesRecords.Clear();
                            LoadSourcesData();
                            Source = new Source();
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

        //===================THIS METHOD IS FOR EDIT RECORDS IN SOURCE TABLE==============
        public void EditSource()
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    var res3 = db.Sources.Find(Source.SourceId);
                    if (res3 != null)
                    {
                        //changing DB
                        res3.SourceName = Source.SourceName;
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