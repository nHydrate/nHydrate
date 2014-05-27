<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="ProductList.aspx.cs" Inherits="Northwind.TestSite.ProductList" %>
<%@ Register src="UserControls/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pageblock">

<h1>Products</h1>
<p>
There is a relationship from Products->Order Details, so you can walk the relationship from the grid.
</p>
<p>
<asp:Literal ID="lblHeader" runat="server" />
</p>

<uc1:PagingControl ID="PagingControl1" runat="server" />    
<asp:GridView ID="grdItem" runat="server" 
AllowPaging="false" 
AllowSorting="false" 
AutoGenerateColumns="false"
Width="100%"
CssClass="grid"
RowStyle-CssClass="gridrownormal"
AlternatingRowStyle-CssClass="gridrowalternate"
HeaderStyle-CssClass="gridheader"
EmptyDataText="There are currently no items"
EmptyDataRowStyle-CssClass="emptygrid"
> 
<Columns>

<asp:BoundField DataField="ProductId" HeaderText="Id" />
<asp:BoundField DataField="ProductName" HeaderText="Name" />
<asp:BoundField DataField="QuantityPerUnit" HeaderText="Quan/Unit" />
<asp:BoundField DataField="UnitPrice" HeaderText="Price" />
<asp:BoundField DataField="UnitsInStock" HeaderText="In Stock" />
<asp:BoundField DataField="UnitsOnOrder" HeaderText="On Order" />

<asp:TemplateField HeaderText="Supplier">
<ItemTemplate>
<asp:HyperLink ID="lblSupplier" runat="server" />
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Category">
<ItemTemplate>
<asp:HyperLink ID="lblCategory" runat="server" />
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="showtoptext" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100">
<ItemTemplate>
<asp:HyperLink ID="linkEdit" runat="server" Text="Edit" /> | 
<asp:HyperLink ID="linkOrderDetails" runat="server" Text="Order Details" />
</ItemTemplate>
</asp:TemplateField>

</Columns>
</asp:GridView>

</div>
</asp:Content>
