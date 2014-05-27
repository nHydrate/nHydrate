<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="CustomerItem.aspx.cs" Inherits="Northwind.TestSite.CustomerItem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pageblock">

<h1>Customer [<asp:Label ID="lblCustomerId" runat="server" />]</h1>

<div style="margin-bottom:10px;">
<span class="prompt">Company:</span><br />
<asp:TextBox ID="txtCompany" runat="server" CssClass="textbox" Width="350px" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">Contact:</span><br />
<asp:TextBox ID="txtContact" runat="server" CssClass="textbox" Width="350px" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">Address:</span><br />
<asp:TextBox ID="txtAddress" runat="server" CssClass="textbox" Width="350px" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">City:</span><br />
<asp:TextBox ID="txtCity" runat="server" CssClass="textbox" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">Zip:</span><br />
<asp:TextBox ID="txtZip" runat="server" CssClass="textbox" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">Phone:</span><br />
<asp:TextBox ID="txtPhone" runat="server" CssClass="textbox" />
</div>

<div align="right">
<asp:Button id="cmdSave" runat="server" Text="Save" CssClass="button" />
<asp:Button id="cmdCancel" runat="server" Text="Cancel" CssClass="button" />
</div>

</div>
</asp:Content>
