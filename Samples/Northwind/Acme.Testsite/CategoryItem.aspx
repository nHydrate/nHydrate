<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="CategoryItem.aspx.cs" Inherits="Northwind.TestSite.CategoryItem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pageblock">

<h1>Category [<asp:Label ID="lblCategoryId" runat="server" />]</h1>

<div style="margin-bottom:10px;">
<span class="prompt">Name:</span><br />
<asp:TextBox ID="txtName" runat="server" CssClass="textbox" Width="350px" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">Description:</span><br />
<asp:TextBox ID="txtDescription" runat="server" CssClass="textbox" Width="350px" />
</div>

<div align="right">
<asp:Button id="cmdSave" runat="server" Text="Save" CssClass="button" />
<asp:Button id="cmdCancel" runat="server" Text="Cancel" CssClass="button" />
</div>

</div>
</asp:Content>
