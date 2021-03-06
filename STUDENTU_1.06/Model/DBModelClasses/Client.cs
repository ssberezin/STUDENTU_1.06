﻿
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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


namespace STUDENTU_1._06.Model
{
    public class Client: Helpes.ObservableObject, ICloneable
    {
            
        public Client()
        {
            this.OrderLine = new ObservableCollection<OrderLine>();
            this.Universities = new ObservableCollection<University>();
            this.Course = 1;
            //this.YearUniversityStart = DateTime.Now.AddMonths(-DateTime.Now.Month)
            //                            .AddDays(-DateTime.Now.Day)
            //                            .AddHours(-DateTime.Now.Hour)
            //                            .AddMinutes(DateTime.Now.Minute)
            //                            .AddSeconds(DateTime.Now.Second)
            //                            .AddSeconds(DateTime.Now.Millisecond);
            this.YearUniversityStart = GetYearUniversityStart(Course);
        }
        //[ForeignKey("OrderLine")]
        public int ClientId { get; set; }

        [Column(TypeName = "datetime2")]
        private DateTime yearUniversityStart;
        public DateTime YearUniversityStart
        {
            get { return yearUniversityStart; }
            set
            {
                if (yearUniversityStart != value)
                {
                    yearUniversityStart = value;
                    OnPropertyChanged(nameof(YearUniversityStart));
                }
            }
        }


        private int course;
        public int Course
        {
            get { return course; }
            set
            {
                if (course != value)
                {
                    course = value;
                    OnPropertyChanged(nameof(Course));
                }
            }
        }


        [Column("GroupName", TypeName = "ntext")]
        [MaxLength(255)]
        public string GroupName { get; set; }
        

        public virtual Persone Persone { get; set; }

        public virtual ObservableCollection<University> Universities { get; set; }
        public virtual ObservableCollection<OrderLine> OrderLine { get; set; }

        IDialogService dialogService;
        IShowWindowService showWindow;


        //не востребовано
        public Client CheckClient(Persone persone)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    return db.Clients.
                        Where(c => c.Persone.PersoneId == persone.PersoneId)
                        .FirstOrDefault();                    
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

        public DateTime GetYearUniversityStart(int val)
        {
            if (DateTime.Now.Month>7)
             return DateTime.Now.AddMonths(-DateTime.Now.Month)
                                                .AddDays(-DateTime.Now.Day)
                                                .AddHours(-DateTime.Now.Hour)
                                                .AddMinutes(DateTime.Now.Minute)
                                                .AddSeconds(DateTime.Now.Second)
                                                .AddSeconds(DateTime.Now.Millisecond)
                                                .AddYears(-val+1);
            else
                return DateTime.Now.AddMonths(-DateTime.Now.Month)
                                                .AddDays(-DateTime.Now.Day)
                                                .AddHours(-DateTime.Now.Hour)
                                                .AddMinutes(DateTime.Now.Minute)
                                                .AddSeconds(DateTime.Now.Second)
                                                .AddSeconds(DateTime.Now.Millisecond)
                                                .AddYears(-(val+1) + 1);
        }

        public object Clone()
        {
            return new Client()
            {
                ClientId = this.ClientId,
                Course = this.Course,
                GroupName = this.GroupName,
                Persone = (Persone)this.Persone.Clone(),
                Universities = new ObservableCollection<University>(this.Universities),
                OrderLine = new ObservableCollection<OrderLine>(this.OrderLine)

            };
        }

        public  bool CompareClients (Client obj1, Client obj2)
        {
            if ((obj1.Course == obj2.Course) && (obj1.GroupName == obj2.GroupName) &&
                 obj1.Persone.ComparePersons(obj1.Persone,obj2.Persone))
                return true;
            return false;
        }

       
    }
}
