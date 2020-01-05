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

                    var resPhone1 = db.Contacts.Where(c => c.Phone1 == contacts.Phone1 ||
                                                     c.Phone1 == contacts.Phone2 ||
                                                     c.Phone1 == contacts.Phone3).FirstOrDefault();
                    if (resPhone1 != null)
                        return resPhone1.ContactsId;
                    else
                    {
                        var resPhone2 = db.Contacts.Where(c => c.Phone2 == contacts.Phone1 ||
                                                     c.Phone2 == contacts.Phone2 ||
                                                     c.Phone2 == contacts.Phone3).FirstOrDefault();
                        if (resPhone2 != null)
                            return resPhone2.ContactsId;
                        else
                        {
                            var resPhone3 = db.Contacts.Where(c => c.Phone3 == contacts.Phone1 &&
                                                     c.Phone3 == contacts.Phone2 &&
                                                     c.Phone3 == contacts.Phone3).FirstOrDefault();
                            if (resPhone3 != null)
                                return resPhone3.ContactsId;
                            else
                            {
                                var resEmail1 = db.Contacts.Where(c => c.Email1 == contacts.Email1 ||
                                                     c.Email1 == contacts.Email2).FirstOrDefault();
                                if (resEmail1 != null)
                                    return resEmail1.ContactsId;
                                else
                                {
                                    var resEmail2 = db.Contacts.Where(c => c.Email2 == contacts.Email1 ||
                                                     c.Email2 == contacts.Email2).FirstOrDefault();
                                    if (resEmail2 != null)
                                        return resEmail2.ContactsId;
                                    else
                                    {
                                        var resVK = db.Contacts.Where(c => c.VK == contacts.VK).FirstOrDefault();
                                        if (resVK != null)
                                            return resVK.ContactsId;
                                        else
                                        {
                                            var resFB = db.Contacts.Where(c => c.FaceBook == contacts.FaceBook).FirstOrDefault();
                                            if (resFB != null)
                                                return resFB.ContactsId;
                                            else
                                            {
                                                var resSkype = db.Contacts.Where(c => c.Skype == contacts.Skype).FirstOrDefault();
                                                if (resSkype != null)
                                                    return resSkype.ContactsId;
                                                else
                                                    return 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
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
    }
}
