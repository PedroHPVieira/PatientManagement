using PatientManagement.Model;
using PatientManagement.Util;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace PatientManagement.Manager
{
    public static class PatientManager
    {
        public static void Insert(Patient patient, string fileAddress)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(fileAddress);
            var parentElement = xmlDoc.CreateElement("Patient");

            XmlElement id = xmlDoc.CreateElement("Id");
            id.InnerText = Guid.NewGuid().ToString();

            XmlElement firstName = xmlDoc.CreateElement("FirstName");
            firstName.InnerText = patient.FirstName;

            XmlElement lastName = xmlDoc.CreateElement("LastName");
            lastName.InnerText = patient.LastName;

            XmlElement phone = xmlDoc.CreateElement("Telephone");
            var telephoneCrypted = EncryptionServices.Encrypt(patient.Telephone, "Telephone");
            phone.InnerText = telephoneCrypted;

            XmlElement email = xmlDoc.CreateElement("Email");
            var emailCrypted = EncryptionServices.Encrypt(patient.Email, "Email");
            email.InnerText = emailCrypted;

            XmlElement gender = xmlDoc.CreateElement("Gender");
            gender.InnerText = patient.Gender.ToString();

            XmlElement notes = xmlDoc.CreateElement("Notes");
            notes.InnerText = patient.Notes;

            XmlElement createdDate = xmlDoc.CreateElement("CreatedDate");
            createdDate.InnerText = patient.CreatedDate.ToString();

            XmlElement lastUpdatedDate = xmlDoc.CreateElement("LastUpdatedDate");
            lastUpdatedDate.InnerText = patient.LastUpdatedDate.ToString();

            XmlElement isDeleted = xmlDoc.CreateElement("IsDeleted");
            isDeleted.InnerText = patient.IsDeleted.ToString();

            parentElement.AppendChild(id);
            parentElement.AppendChild(firstName);
            parentElement.AppendChild(lastName);
            parentElement.AppendChild(phone);
            parentElement.AppendChild(email);
            parentElement.AppendChild(gender);
            parentElement.AppendChild(notes);
            parentElement.AppendChild(createdDate);
            parentElement.AppendChild(lastUpdatedDate);
            parentElement.AppendChild(isDeleted);

            xmlDoc.DocumentElement.AppendChild(parentElement);

            xmlDoc.Save(fileAddress);
        }

        public static void Update(IOrderedDictionary newValues, string fileAddress, string id)
        {
            var doc = XDocument.Load(fileAddress);

            var node = Find(id, doc);

            foreach (DictionaryEntry item in newValues)
            {
                if (item.Key.ToString() == "FirstName")
                {
                    node.SetElementValue("FirstName", item.Value);
                }
                else if (item.Key.ToString() == "LastName")
                {
                    node.SetElementValue("LastName", item.Value);
                }
                else if (item.Key.ToString() == "Telephone")
                {
                    var telephoneCrypted = EncryptionServices.Encrypt(item.Value.ToString(), "Telephone");
                    node.SetElementValue("Telephone", telephoneCrypted);
                }
                else if (item.Key.ToString() == "Email")
                {
                    var emailCrypted = EncryptionServices.Encrypt(item.Value.ToString(), "Email");
                    node.SetElementValue("Email", emailCrypted);
                }
                else if (item.Key.ToString() == "Gender")
                {
                    node.SetElementValue("Gender", item.Value);
                }
                else if (item.Key.ToString() == "Notes")
                {
                    node.SetElementValue("Notes", item.Value);
                }
            }

            node.SetElementValue("LastUpdatedDate", DateTime.Now.ToString());

            doc.Save(fileAddress);
        }

        public static XElement Find(string id, XDocument doc)
        {
            return doc.Descendants("Patient").FirstOrDefault(t => t.Element("Id").Value == id);
        }

        public static void Delete(string id, string fileAddress)
        {
            var doc = XDocument.Load(fileAddress);
            var node = doc.Descendants("Patient").FirstOrDefault(t => t.Element("Id").Value == id);

            node.SetElementValue("IsDeleted", "True");

            doc.Save(fileAddress);
        }
    }
}