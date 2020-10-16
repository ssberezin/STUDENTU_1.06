using STUDENTU_1._06.Model.HelpModelClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTU_1._06.Model.DBModelClasses
{
    class UserAccessRights : Helpes.ObservableObject
    {

        public int NameOfAccessId { get; set; }

        [Column("AccessName", TypeName = "nvarchar")]
        [MaxLength(50)]
        private string accessName;
        public string AccessName
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

        public UserAccessRights()
        { 

        }
    }
}
