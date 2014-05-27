<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="CategoryList.aspx.cs" Inherits="Northwind.TestSite.CategoryList" %>
<%@ Register src="UserControls/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pageblock">

<h1>Categories</h1>
<p>
There is a relationship from Category->Product, so you can walk the relationship from the grid.
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

<asp:BoundField DataField="CategoryId" HeaderText="Id" />
<asp:BoundField DataField="CategoryName" HeaderText="Name" />
<asp:BoundField DataField="Description" HeaderText="Description" />

<asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="showtoptext" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="80">
<ItemTemplate>
<asp:HyperLink ID="linkEdit" runat="server" Text="Edit" /> | 
<asp:HyperLink ID="linkProducts" runat="server" Text="Products" />
</ItemTemplate>
</asp:TemplateField>

</Columns>
</asp:GridView>

</div>
</asp:Content>
