<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="OrderDetailList.aspx.cs" Inherits="Northwind.TestSite.OrderDetailList" %>
<%@ Register src="UserControls/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pageblock">

<h1>Order Details</h1>
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

<asp:BoundField DataField="OrderId" HeaderText="Id" />
<asp:TemplateField HeaderText="Product">
<ItemTemplate>
<asp:HyperLink ID="linkProduct" runat="server" />
</ItemTemplate>
</asp:TemplateField>


<asp:BoundField DataField="UnitPrice" HeaderText="Price" />
<asp:BoundField DataField="Quantity" HeaderText="Quantity" />
<asp:BoundField DataField="Discount" HeaderText="Discount" />

<asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="showtoptext" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="80">
<ItemTemplate>
<asp:HyperLink ID="linkEdit" runat="server" Text="Edit" />
</ItemTemplate>
</asp:TemplateField>

</Columns>
</asp:GridView>

</div>
</asp:Content>
