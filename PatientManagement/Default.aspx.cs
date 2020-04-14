using PatientManagement.Manager;
using PatientManagement.Model;
using PatientManagement.Util;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace PatientManagement
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowList();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var fileAddress = Server.MapPath("Patient.xml");

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(fileAddress);
            var parentElement = xmlDoc.CreateElement("Patient");

            var patient = new Patient()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Telephone = txtTelephoneNumber.Text,
                Email = txtEmailAddress.Text,
                Gender = (Gender)Convert.ToUInt32(ddGenderSelect.SelectedValue),
                Notes = txtNotes.Text,
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                IsDeleted = false
            };

            PatientManager.Insert(patient, fileAddress);

            ClearField();
            ShowList();
        }

        private void ClearField()
        {
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtTelephoneNumber.Text = string.Empty;
            txtEmailAddress.Text = string.Empty;
            ddGenderSelect.SelectedIndex = 0;
            txtNotes.Text = string.Empty;
        }

        private void ShowList()
        {
            using (var ds = new DataSet())
            {
                ds.ReadXml(Server.MapPath("~/Patient.xml"));
                
                foreach (DataTable table in ds.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var emailCrypted = EncryptionServices.Decrypt(row["Email"].ToString(), "Email");
                        var telpehoneCrypted = EncryptionServices.Decrypt(row["Telephone"].ToString(), "Telephone");

                        row["Email"] = emailCrypted;
                        row["Telephone"] = telpehoneCrypted;
                    }
                }

                if (ds.Tables.Count > 0)
                {
                    gvPatients.DataSource = ds.Tables[0];
                    gvPatients.DataBind();
                }
            }
        }

        protected void GvPatients_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPatients.EditIndex = e.NewEditIndex;

            ShowList();
        }

        protected void GvPatients_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var id = gvPatients.DataKeys[e.RowIndex].Value.ToString();
            var fileAddress = Server.MapPath("Patient.xml");

            PatientManager.Delete(id, fileAddress);

            ShowList();
        }

        protected void GvPatients_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var fileAddress = Server.MapPath("Patient.xml");
            var myNewValues = e.NewValues;

            var id = gvPatients.DataKeys[e.RowIndex].Value.ToString();

            PatientManager.Update(myNewValues, fileAddress, id);

            gvPatients.EditIndex = -1;

            ShowList();
        }

        protected void GvPatients_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPatients.EditIndex = -1;

            ShowList();
        }
    }
}