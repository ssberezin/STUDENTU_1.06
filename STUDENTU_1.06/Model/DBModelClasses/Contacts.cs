using STUDENTU_1._06.Model.HelpModelClasses.DialogWindows;
using STUDENTU_1._06.Model.HelpModelClasses.ShowWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STUDENTU_1._06.Model
{
    public class Contacts
    {
        public Contacts()
        {
            this.Phone1 = "+380";
            this.Phone2 = "---";
            this.Phone3 = "---";
            this.Email1 = "---";
            this.Email2 = "---";           
            this.Adress = "---";
            this.Skype = "---";
            this.VK = "---";
            this.FaceBook = "---";
            this.Persone = new ObservableCollection<Persone>();

        }
        [Column("Phone1", TypeName = "ntext")]
        [MaxLength(13)]
        public string Phone1 { get; set; }
        [Column("Phone2", TypeName = "ntext")]
        [MaxLength(13)]
        public string Phone2 { get; set; }
        [Column("Phone3", TypeName = "ntext")]
        [MaxLength(13)]
        public string Phone3 { get; set; }

        [Column("Email1", TypeName = "ntext")]
        [MaxLength(100)]
        public string Email1 { get; set; }
        [Column("Email2", TypeName = "ntext")]
        [MaxLength(100)]
        public string Email2 { get; set; }

        [Column("Adress", TypeName = "ntext")]
        [MaxLength(255)]
        public string Adress { get; set; }
        [Column("Skype", TypeName = "ntext")]
        [MaxLength(100)]
        public string Skype { get; set; }
        [Column("VK", TypeName = "ntext")]
        [MaxLength(100)]
        public string VK { get; set; }
        [Column("FaceBook", TypeName = "ntext")]
        [MaxLength(100)]
        public string FaceBook { get; set; }

        public int ContactsId { get; set; }

        //  public virtual Persone Persone { get; set; }
        public virtual ObservableCollection<Persone> Persone { get; set; }

        //check for validation of Contacts fields
        public bool ContactsValidation()
        {
            bool flag = true;

            if (Phone1 == "+380" && Phone2 == "---" && Phone3 == "---" && Email1 == "---" && Email2 == "---" && VK == "---" && FaceBook == "---")
                flag = false;

            return flag;
        }

        IDialogService dialogService;
        IShowWindowService showWindow;


        public int CheckContacts(Contacts contacts)
        {
            using (StudentuConteiner db = new StudentuConteiner())
            {
                try
                {
                    int count = db.Contacts.Count();
                    if (count == 0) return 0;

                   
                    var res = db.Contacts.ToList();
                    if (res == null) return 0;
                    foreach (var item in res)
                    {
                        if (item.Phone1!="---"&&(item.Phone1 == contacts.Phone1 || item.Phone1 == contacts.Phone2 || item.Phone1 == contacts.Phone3)||
                            item.Phone2 != "---" && (item.Phone2 == contacts.Phone2 || item.Phone2 == contacts.Phone3)||
                            item.Phone2 != "---" && item.Phone3 == contacts.Phone3 ||
                            item.Email1!="---"&&(item.Email1==contacts.Email1|| item.Email1 == contacts.Email2 )||
                            item.Email2 != "---" && (item.Email2 == contacts.Email2) ||
                            item.VK !="---"&&item.VK == contacts.VK ||
                            item.FaceBook!="---"&&item.FaceBook == contacts.FaceBook||
                            item.Skype!="---"&&item.Skype==item.Skype)
                            return item.ContactsId;
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
            return 0;
        }

        //public static bool operator ==(Contacts obj1, Contacts obj2)
        //{


        //    if ((obj1.Phone1 == obj2.Phone1) && (obj1.Phone2 == obj2.Phone2) &&
        //        (obj1.Phone3 == obj2.Phone3) && (obj1.Email1 == obj2.Email1) &&
        //        (obj1.Email2 == obj2.Email2) && (obj1.VK == obj2.VK) &&
        //        (obj1.FaceBook == obj2.FaceBook) && (obj1.Skype == obj2.Skype))
        //        return true;
        //    return false;
        //}

        //public static bool operator !=(Contacts obj1, Contacts obj2)
        //{

        //    if ((obj1.Phone1 != obj2.Phone1) || (obj1.Phone2 != obj2.Phone2) ||
        //        (obj1.Phone3 != obj2.Phone3) || (obj1.Email1 != obj2.Email1) ||
        //        (obj1.Email2 != obj2.Email2) || (obj1.VK != obj2.VK) ||
        //        (obj1.FaceBook != obj2.FaceBook) || (obj1.Skype != obj2.Skype))
        //        return true;
        //    return false;
        //}
    }
}
