<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="OrderList.aspx.cs" Inherits="Northwind.TestSite.OrderList" %>
<%@ Register src="UserControls/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pageblock">

<h1>Orders</h1>
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

<asp:BoundField DataField="OrderId" HeaderText="Order Id" />
<asp:BoundField DataField="CustomerId" HeaderText="Customer Id" />
<asp:TemplateField HeaderText="Customer Name">
<ItemTemplate>
<%# DataBinder.Eval(Container.DataItem, "CustomerItem.ContactName")%>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:MM-dd-yyyy}" />
<asp:BoundField DataField="ShipName" HeaderText="Ship Name" />

<asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="showtoptext" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="80">
<ItemTemplate>
<asp:HyperLink ID="linkEdit" runat="server" Text="Edit" /> | 
<asp:HyperLink ID="linkOrderDetails" runat="server" Text="Details" />
</ItemTemplate>
</asp:TemplateField>

</Columns>
</asp:GridView>

</div>
</asp:Content>
