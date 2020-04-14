<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PatientManagement._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Patient</h2>
    <h3>Please fill in the form</h3>

    <div class="form-group">
        <label for="txtFirstName">First name</label>
        <asp:TextBox class="form-control" ID="txtFirstName" placeholder="Pedro" runat="server" required="required"></asp:TextBox>
        <div class="invalid-feedback">Enter a first name please.</div>
    </div>
    <div class="form-group">
        <label for="txtLastName">Last name</label>
        <asp:TextBox class="form-control" ID="txtLastName" placeholder="Vieira" runat="server" required="required"></asp:TextBox>
        <div class="invalid-feedback">Enter a last name please.</div>
    </div>
    <div class="form-group">
        <label for="txtTelephoneNumber" class="col-2 col-form-label">Telephone</label>
        <asp:TextBox class="form-control" ID="txtTelephoneNumber" placeholder="1-(555)-555-5555" runat="server" required="required"></asp:TextBox>
        <div class="invalid-feedback">Enter a telephone number please.</div>
    </div>
    <div class="form-group">
        <label for="txtEmailAddress">Email address</label>
        <asp:TextBox class="form-control" TextMode="Email" ID="txtEmailAddress" aria-describedby="emailHelp" placeholder="Enter email" runat="server" required="required"></asp:TextBox>
        <small id="emailHelp" class="form-text text-muted">We'll never share your email with anyone else.</small>
        <div class="invalid-feedback">Enter an email please.</div>
    </div>
    <div class="form-group">
        <label for="ddGenderSelect">Gender select</label>
        <asp:DropDownList class="form-control" ID="ddGenderSelect" runat="server">
            <asp:ListItem Selected="True" Value="0"> Female </asp:ListItem>
            <asp:ListItem Selected="False" Value="1"> Male </asp:ListItem>
            <asp:ListItem Selected="False" Value="2"> Other </asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <label for="txtNotes">Notes</label>
        <asp:TextBox class="form-control" ID="txtNotes" TextMode="MultiLine" Rows="3" placeholder="Please insert your notes here" runat="server"></asp:TextBox>
    </div>
    <asp:Button ID="btnSubmit" class="btn btn-primary" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
    <br /><br />
    <div>
        <asp:GridView DataKeyNames="id" ID="gvPatients" CssClass="table table-striped" runat="server" OnRowEditing="GvPatients_RowEditing" 
                                                                                                        OnRowDeleting="GvPatients_RowDeleting"
                                                                                                        OnRowUpdating="GvPatients_RowUpdating"
                                                                                                        OnRowCancelingEdit="GvPatients_RowCancelingEdit">
            <Columns>
                <asp:CommandField ShowEditButton="true" />
                <asp:CommandField ShowDeleteButton="true" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
