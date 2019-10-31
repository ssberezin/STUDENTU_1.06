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
using STUDENTU_1._06.Views.EditOrderWindows;
using System.Collections.ObjectModel;
using STUDENTU_1._06.Model.HelpModelClasses;
using STUDENTU_1._06.Views.EditOrderWindows.Evaluation;

namespace STUDENTU_1._06.ViewModel
{
    public partial class ForEditOrder : Helpes.ObservableObject
    {
        public ObservableCollection<AuthorsRecord> AuthorsRecords { get; set; }
        public ObservableCollection<AuthorsRecord> SelectedAuthorsRecords { get; set; }

        private AuthorsRecord authorsRecord;
        public AuthorsRecord AuthorsRecord
        {
            get { return authorsRecord; }
            set
            {
                if (authorsRecord != value)
                {
                    authorsRecord = value;
                    OnPropertyChanged(nameof(AuthorsRecord));
                }
            }
        }

        private Author selectedExecuteAuthor;
        public Author SelectedExecuteAuthor
        {
            get { return selectedExecuteAuthor; }
            set
            {
                if (selectedExecuteAuthor != value)
                {
                    selectedExecuteAuthor = value;
                    OnPropertyChanged(nameof(SelectedExecuteAuthor));
                }
            }
        }

        private AuthorsRecord executeAuthor;
        public AuthorsRecord ExecuteAuthor
        {
            get { return executeAuthor; }
            set
            {
                if (executeAuthor != value)
                {
                    executeAuthor = value;
                    OnPropertyChanged(nameof(ExecuteAuthor));
                }
            }
        }

        private Contacts selectetdAuthorContacts;
        public Contacts SelectetdAuthorContacts
        {
            get { return selectetdAuthorContacts; }
            set
            {
                if (selectetdAuthorContacts != value)
                {
                    selectetdAuthorContacts = value;
                    OnPropertyChanged(nameof(SelectetdAuthorContacts));
                }
            }
        }


        //call RuleOrderLineWindow
        private RelayCommand newRuleOrderLineWindowCommand;
        public RelayCommand NewRuleOrderLineWindowCommand =>
            newRuleOrderLineWindowCommand ?? (newRuleOrderLineWindowCommand = new RelayCommand(
                    (obj) =>
                    {
                        RuleOrderLineWindow ruleOrderLineWindow = new RuleOrderLineWindow(obj);
                        ruleOrderLineWindow.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(ruleOrderLineWindow);
                    }
                    ));

        //=============================fill listbox "Authors" if check "All authors"====================


        private RelayCommand allAuthorsCallCommand;
        public RelayCommand AllAuthorsCallCommand =>
            allAuthorsCallCommand ?? (allAuthorsCallCommand = new RelayCommand(
                    (obj) =>
                    {

                        AuthorsRecords.Clear();
                        AuthorsCall("AllAuthors");
                    }
                    ));

        //=============================fill listbox "Authors" if check "Authors by direction"====================
        private RelayCommand themAuthorsCallCommand;
        public RelayCommand ThemAuthorsCallCommand =>
            themAuthorsCallCommand ?? (themAuthorsCallCommand = new RelayCommand(
                    (obj) =>
                    {

                        AuthorsRecords.Clear();
                        AuthorsCall("ThemAuthors");
                    }
                    ));

        private void AuthorsCall(string param)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    if (_Dir.Dir.DirectionName == "---")
                        param = "AllAuthors";
                    var contacts = db.Contacts.Include("Persone").ToList();
                    switch (param)
                    {
                        
                        case "AllAuthors":
                            var result = db.Authors.Include("Persone")
                                                   .ToList();

                            AuthorsRecord record;

                            //   Order.Client=new Client() { Persone=Persone};
                            foreach (Author item in result)
                            {
                                Contacts = new Contacts()
                                {
                                    Phone1 = item.Persone.Contacts.Phone1,
                                    Phone2 = item.Persone.Contacts.Phone2,
                                    Phone3 = item.Persone.Contacts.Phone3,
                                    Email1 = item.Persone.Contacts.Email1,
                                    Email2 = item.Persone.Contacts.Email2,
                                    VK = item.Persone.Contacts.VK,
                                    FaceBook = item.Persone.Contacts.FaceBook
                                };
                                record = new AuthorsRecord
                                {
                                   
                                    Author = new Author()
                                    {
                                        AuthorId = item.AuthorId,
                                        Source = item.Source,
                                        Rating = item.Rating,
                                        Punctually = item.Punctually,
                                        CompletionCompliance = item.CompletionCompliance,
                                        WorkQuality = item.WorkQuality,
                                        Responsibility = item.Responsibility
                                    },
                                    Persone = new Persone()
                                    {
                                        PersoneId = item.Persone.PersoneId,
                                        PersoneDescription = item.Persone.PersoneDescription,
                                        Name = item.Persone.Name,
                                        Surname = item.Persone.Surname,
                                        Patronimic = item.Persone.Patronimic,
                                        Sex = item.Persone.Sex,
                                        NickName = item.Persone.NickName
                                    }
                                    //Persone = item.Persone,                                                                       
                                    // Contacts = item.Persone.Contacts
                                    //Contacts = new Contacts()
                                    //{
                                    //    Phone1 = item.Persone.Contacts.Phone1,
                                    //    Phone2 = item.Persone.Contacts.Phone2,
                                    //    Phone3 = item.Persone.Contacts.Phone3,
                                    //    Email1 = item.Persone.Contacts.Email1,
                                    //    Email2 = item.Persone.Contacts.Email2,
                                    //    VK = item.Persone.Contacts.VK,
                                    //    FaceBook = item.Persone.Contacts.FaceBook
                                    //}
                                };
                                AuthorsRecords.Add(record);
                            }

                            break;
                        case "ThemAuthors":
                            var result1 = db.Orderlines.Include("Direction").
                                                        Include("Author").
                                                        ToList();

                            var tmp = (from item in result1
                                       where (item.Direction.DirectionName == _Dir.Dir.DirectionName)
                                       select new AuthorsRecord
                                       {
                                           Author=item.Author,
                                           //AuthorRecordId = item.Author.AuthorId,
                                           Persone = new Persone() {
                                               PersoneId= item.Author.Persone.PersoneId,
                                               PersoneDescription = item.Author.Persone.PersoneDescription,
                                               Name = item.Author.Persone.Name,
                                               Surname = item.Author.Persone.Surname,
                                               Patronimic = item.Author.Persone.Patronimic,
                                               Sex = item.Author.Persone.Sex,
                                               NickName = item.Author.Persone.NickName
                                           },                                          
                                           Contacts = item.Author.Persone.Contacts
                                       });
                            //AuthorsRecords record;
                            foreach (var item in result1)
                            {
                                if (item.Direction.DirectionName == _Dir.Dir.DirectionName)
                                {
                                    record = new AuthorsRecord
                                    {
                                        Author = item.Author,
                                        Persone = new Persone()
                                        {
                                            PersoneId = item.Author.Persone.PersoneId,
                                            PersoneDescription = item.Author.Persone.PersoneDescription,
                                            Name = item.Author.Persone.Name,
                                            Surname = item.Author.Persone.Surname,
                                            Patronimic = item.Author.Persone.Patronimic,
                                            Sex = item.Author.Persone.Sex,
                                            NickName = item.Author.Persone.NickName
                                        },                                        
                                        Contacts = item.Author.Persone.Contacts
                                    };
                                    AuthorsRecords.Add(record);
                                }
                            }
                            break;
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

        //=============================fill listbox "AuthorsAvaluat" if press button "+"====================
        private RelayCommand addSelectedAuthorCommand;
        public RelayCommand AddSelectedAuthorCommand =>
            addSelectedAuthorCommand ?? (addSelectedAuthorCommand = new RelayCommand(
                    (obj) =>
                    {
                        AddSelectedAuthor();
                    }
                    ));
        private void AddSelectedAuthor()
        {
            if (AuthorsRecord.Persone.NickName != "---")
            {
                SelectedAuthorsRecords.Add(AuthorsRecord);
                Order.Money.Evaluation.Authors.Add(AuthorsRecord.Author);
                //Order.Money.Evaluation.Authors.Add(new Author()
                //{
                //    AuthorId = AuthorsRecord.AuthorRecordId,
                //    Source = AuthorsRecord.Source
                //});
            }
            else
            {
                dialogService.ShowMessage("Нельзя добавить эту запись");
            }
            
        }

        //=============================edit listbox "AuthorsAvaluat" if press button "-"====================
        private RelayCommand delSelectedAuthorCommand;
        public RelayCommand DelSelectedAuthorCommand =>
            delSelectedAuthorCommand ?? (delSelectedAuthorCommand = new RelayCommand(
                    (obj) =>
                    {
                        DelSelectedAuthor();
                    }
                    ));

        private void DelSelectedAuthor()
        {
            SelectedAuthorsRecords.Remove(AuthorsRecord);
            Order.Money.Evaluation.Authors.Remove(AuthorsRecord.Author);
            //Order.Money.Evaluation.Authors.Remove(new Author()
            //                                        {
            //                                            AuthorId = AuthorsRecord.AuthorRecordId,
            //                                            Source = author.Source
            //                                        });
        }

        //=============================Call window SetFinalAvaluationWindow.xaml========================================

        private RelayCommand setExecuteAuthorCommand;
        public RelayCommand SetExecuteAuthorCommand =>
            setExecuteAuthorCommand ?? (setExecuteAuthorCommand = new RelayCommand(
                    (obj) =>
                    {
                        SetFinalAvaluationWindow setFinalAvaluationWindow = new SetFinalAvaluationWindow(obj);
                        setFinalAvaluationWindow.Owner = Application.Current.MainWindow;
                        showWindow.ShowWindow(setFinalAvaluationWindow);                        
                    }
                    ));

   

    }
}
