using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using STUDENTU_1._06.Model.HelpModelClasses;
using System.Collections.ObjectModel;
using STUDENTU_1._06.Model.DBModelClasses;

namespace STUDENTU_1._06.ViewModel
{
    public partial class ForEditOrder : Helpes.ObservableObject
    {
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
                        AddDirSubjWorkType("Source");
                    }
                    ));

        private RelayCommand deleteSourceCommand;
        public RelayCommand DeleteSourceCommand => deleteSourceCommand ??
            (deleteSourceCommand = new RelayCommand((selectedItem) =>
            {
                if (selectedItem == null) return;
                DeleteDirSubjWorkType("Source");
            }
            ));

        private RelayCommand editSourceCommand;
        public RelayCommand EditSourceCommand => editSourceCommand ?? (editSourceCommand = new RelayCommand(
                    (selectedItem) =>
                    {
                        if (selectedItem == null) return;
                        EditDirSubjWorkType("Source");
                    }
                    ));
    }
}
