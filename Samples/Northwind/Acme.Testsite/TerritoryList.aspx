<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="TerritoryList.aspx.cs" Inherits="Northwind.TestSite.TerritoryList" %>
<%@ Register src="UserControls/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pageblock">

<h1>Territories</h1>

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

<asp:BoundField DataField="TerritoryId" HeaderText="Id" />
<asp:BoundField DataField="TerritoryDescription" HeaderText="Description" />

<asp:TemplateField HeaderText="Region">
<ItemTemplate>
<asp:HyperLink ID="lblRegion" runat="server" />
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="showtoptext" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100">
<ItemTemplate>
<asp:HyperLink ID="linkEdit" runat="server" Text="Edit" />
</ItemTemplate>
</asp:TemplateField>

</Columns>
</asp:GridView>

</div>
</asp:Content>
