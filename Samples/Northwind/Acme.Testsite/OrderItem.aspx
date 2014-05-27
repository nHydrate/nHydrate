<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="OrderItem.aspx.cs" Inherits="Northwind.TestSite.OrderItem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pageblock">

<h1>Order [<asp:Label ID="lblOrderId" runat="server" />]</h1>

<div style="margin-bottom:10px;">
<span class="prompt">Ship Address:</span><br />
<asp:TextBox ID="txtAddress" runat="server" CssClass="textbox" Width="350px" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">Ship City:</span><br />
<asp:TextBox ID="txtCity" runat="server" CssClass="textbox" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">Ship Zip:</span><br />
<asp:TextBox ID="txtZip" runat="server" CssClass="textbox" />
</div>

<div align="right">
<asp:Button id="cmdSave" runat="server" Text="Save" CssClass="button" />
<asp:Button id="cmdCancel" runat="server" Text="Cancel" CssClass="button" />
</div>

</div>
</asp:Content>
