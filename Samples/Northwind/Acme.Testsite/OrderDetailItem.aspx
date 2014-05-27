<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="OrderDetailItem.aspx.cs" Inherits="Northwind.TestSite.OrderDetailItem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pageblock">

<h1>Order Detail [<asp:Label ID="lblOrderId" runat="server" />]</h1>

<div style="margin-bottom:10px;">
<span class="prompt">Product:</span><br />
<asp:DropDownList ID="cboProduct" runat="server" />
<span>This is part of the primary key and cannot be changed. This is the database design.</span>
</div>

<div style="margin-bottom:10px;">
<span class="prompt">Price:</span><br />
<asp:TextBox ID="txtPrice" runat="server" CssClass="textbox" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">Quantity:</span><br />
<asp:TextBox ID="txtQuantity" runat="server" CssClass="textbox" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">Discount:</span><br />
<asp:TextBox ID="txtDiscount" runat="server" CssClass="textbox" />
</div>

<div align="right">
<asp:Button id="cmdSave" runat="server" Text="Save" CssClass="button" />
<asp:Button id="cmdCancel" runat="server" Text="Cancel" CssClass="button" />
</div>

</div>
</asp:Content>
