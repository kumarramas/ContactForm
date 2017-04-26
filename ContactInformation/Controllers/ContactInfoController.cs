using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactInformation.Models;
using System.Text.RegularExpressions;


namespace ContactInformation.Controllers
{
    public class ContactInfoController : Controller
    {
        Database1Entities1 dbObject = new Database1Entities1();
        
        public ViewResult GetContactInfo(int? ContactInfoId)
        {
            try
            {
                List<ContactInformation.Models.ContactInformation> lstContactInfo = new List<Models.ContactInformation>();
                lstContactInfo = GetContactInformation(ContactInfoId);
                return View("ContactInformation", lstContactInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        private List<ContactInformation.Models.ContactInformation> GetContactInformation(int? ContactInfoId)
        {
            try
            {
                List<ContactInformation.Models.ContactInformation> lstContactInfo = new List<Models.ContactInformation>();
                if (ContactInfoId == 0)
                {
                    lstContactInfo = (from i in dbObject.ContactInfoes
                                      select new Models.ContactInformation
                                      {
                                          ContactInfoId = i.ContactInfoId,
                                          FirstName = i.FirstName,
                                          LastName = i.LastName,
                                          Email = i.Email,
                                          PhoneNumber =  i.PhoneNumber, 
                                          Status = i.Status.Value
                                      }).ToList();
                }
                else
                {
                    lstContactInfo = (from i in dbObject.ContactInfoes
                                      where i.ContactInfoId == ContactInfoId
                                      select new Models.ContactInformation
                                      {
                                          ContactInfoId = i.ContactInfoId,
                                          FirstName = i.FirstName,
                                          LastName = i.LastName,
                                          Email = i.Email,
                                          PhoneNumber = i.PhoneNumber,
                                          Status = i.Status.Value
                                      }).ToList();
                }

                foreach (var obj in lstContactInfo)
                {
                    obj.PhoneNumber = obj.PhoneNumber.Insert(6,"-");
                    obj.PhoneNumber = obj.PhoneNumber.Insert(3, "-");
                }
                return lstContactInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult UpdateContactInfo(int ContactInfoId,string FirstName, string LastName,string Email, string PhoneNumber, bool Status)
        {
            try
            {
                List<ContactInformation.Models.ContactInformation> lstContactInfo = new List<Models.ContactInformation>();
                PhoneNumber = PhoneNumber.Replace("-", "");
                if (ContactInfoId == 0)  //New
                {
                    if (ModelState.IsValid)
                    {
                        ContactInformation.ContactInfo objContactInfo = new ContactInformation.ContactInfo();
                        objContactInfo.FirstName = FirstName;
                        objContactInfo.LastName = LastName;
                        objContactInfo.Email = Email;
                        objContactInfo.PhoneNumber = PhoneNumber;
                        objContactInfo.Status = Status;
                        dbObject.ContactInfoes.Add(objContactInfo);
                        dbObject.SaveChanges();
                        lstContactInfo.Add(new Models.ContactInformation
                        {
                            ContactInfoId = objContactInfo.ContactInfoId,
                            FirstName = objContactInfo.FirstName,
                            LastName = objContactInfo.LastName,
                            Email = objContactInfo.Email,
                            PhoneNumber = objContactInfo.PhoneNumber,
                            Status = objContactInfo.Status.Value
                        });
                    }
                    return View("ContactInformation", lstContactInfo);
                }
                else
                {  //Update
                    if (ModelState.IsValid)
                    {
                        var result = (from i in dbObject.ContactInfoes
                                      where i.ContactInfoId == ContactInfoId
                                      select i).FirstOrDefault();
                        result.FirstName = FirstName;
                        result.LastName = LastName;
                        result.Email = Email;
                        result.PhoneNumber = PhoneNumber;
                        result.Status = Status;
                        dbObject.SaveChanges();
                        lstContactInfo = GetContactInformation(ContactInfoId);
                    }
                    return View("ContactInformation", lstContactInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult RemoveContactInfo(int ContactInfoId)
        {
            try
            {
                List<ContactInformation.Models.ContactInformation> lstContactInfo = new List<Models.ContactInformation>();
                var result = (from i in dbObject.ContactInfoes
                                where i.ContactInfoId == ContactInfoId
                                select i).FirstOrDefault();
                var retResult = dbObject.ContactInfoes.Remove(result);
                dbObject.SaveChanges();
                lstContactInfo = GetContactInformation(ContactInfoId);
                return View("ContactInformation", lstContactInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}