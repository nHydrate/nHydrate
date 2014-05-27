<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="CustomerList.aspx.cs" Inherits="Northwind.TestSite.CustomerList" %>
<%@ Register src="UserControls/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pageblock">

<h1>Customers</h1>
<p>
There is a relationship from Customers->Orders, so you can walk the relationship from the grid.
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

<asp:BoundField DataField="CustomerId" HeaderText="Id" />
<asp:BoundField DataField="CompanyName" HeaderText="Company" />
<asp:BoundField DataField="ContactName" HeaderText="Contact" />
<asp:BoundField DataField="City" HeaderText="City" />
<asp:BoundField DataField="PostalCode" HeaderText="ZIP" />
<asp:BoundField DataField="Phone" HeaderText="Phone" />

<asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="showtoptext" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="80">
<ItemTemplate>
<asp:HyperLink ID="linkEdit" runat="server" Text="Edit" /> | 
<asp:HyperLink ID="linkOrders" runat="server" Text="Orders" />
</ItemTemplate>
</asp:TemplateField>

</Columns>
</asp:GridView>

</div>
</asp:Content>
