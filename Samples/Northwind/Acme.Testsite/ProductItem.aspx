<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="ProductItem.aspx.cs" Inherits="Northwind.TestSite.ProductItem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pageblock">

<h1>Product [<asp:Label ID="lblProductId" runat="server" />]</h1>

<div style="margin-bottom:10px;">
<span class="prompt">Name:</span><br />
<asp:TextBox ID="txtName" runat="server" CssClass="textbox" Width="350px" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">Supplier:</span><br />
<asp:DropDownList ID="cboSupplier" runat="server" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">Category:</span><br />
<asp:DropDownList ID="cboCategory" runat="server" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">Quantity/Unit:</span><br />
<asp:TextBox ID="txtQuantityPerUnit" runat="server" CssClass="textbox" Width="350px" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">Price:</span><br />
<asp:TextBox ID="txtPrice" runat="server" CssClass="textbox" Width="350px" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">In Stock:</span><br />
<asp:TextBox ID="txtInStock" runat="server" CssClass="textbox" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">On Order:</span><br />
<asp:TextBox ID="txtOnOrder" runat="server" CssClass="textbox" />
</div>

<div align="right">
<asp:Button id="cmdSave" runat="server" Text="Save" CssClass="button" />
<asp:Button id="cmdCancel" runat="server" Text="Cancel" CssClass="button" />
</div>

</div>
</asp:Content>
