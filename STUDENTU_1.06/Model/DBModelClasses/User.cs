
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using STUDENTU_1._06.Helpes;
using STUDENTU_1._06.Model;
using STUDENTU_1._06.Views;
using System;
using System.Linq;
using System.Windows;

using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using System.ComponentModel;


namespace STUDENTU_1._06.Model
{
    public enum NameOfUserAccess
    {
        FullAdmin,
        Admin
    }

    public class User : Helpes.ObservableObject
    {
        [Column("AccessName", TypeName = "nvarchar")]
        [MaxLength(50)]
        private NameOfUserAccess accessName;
        public NameOfUserAccess AccessName
        {
            get { return accessName; }
            set
            {
                if (value != accessName)
                {
                    accessName = value;
                    OnPropertyChanged(nameof(AccessName));
                }
            }
        }

        //for authorezition by user
        [Column("UserNickName", TypeName = "nvarchar")]
        [MaxLength(30)]
        private string userNickName;
        public string UserNickName
        {
            get { return userNickName; }
            set
            {
                if (value != userNickName)
                {
                    userNickName = value;
                    OnPropertyChanged(nameof(UserNickName));
                }
            }
        }

        //Password field
        [Column("Pass", TypeName = "nvarchar")]
        [MaxLength(30)]
        private string pass;
        public string Pass
        {
            get { return pass; }
            set
            {
                if (value != pass)
                {
                    pass = value;
                    OnPropertyChanged(nameof(Pass));
                }
            }
        }

        public User()
        {
            this.OrderLine = new ObservableCollection<OrderLine>();
            this.UserNickName = null;
            this.Pass = null;
        }
        public int UserId { get; set; }
     

        public virtual Persone Persone { get; set; }
        public virtual ObservableCollection<OrderLine> OrderLine { get; set; }


        

        public object Clone()
        {
            return new User()
            {
                UserId = this.UserId,
                //Login = this.Login,
                //PassWord = this.PassWord,
                Persone = (Persone)this.Persone.Clone(),
                OrderLine = new ObservableCollection<OrderLine>(this.OrderLine)
            };
        }

        

    }
}
