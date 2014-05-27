<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="RegionList.aspx.cs" Inherits="Northwind.TestSite.RegionList" %>
<%@ Register src="UserControls/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pageblock">

<h1>Regions</h1>
<p>
There is a relationship from Regions->Territories Details, so you can walk the relationship from the grid.
</p>
<p>
<asp:Literal ID="lblHeader" runat="server" />
</p>
<asp:Label ID="lblError" runat="server" CssClass="errortext" />
    
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

<asp:BoundField DataField="RegionId" HeaderText="Id" />
<asp:BoundField DataField="RegionDescription" HeaderText="Description" />

<asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="showtoptext" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="120">
<ItemTemplate>
<asp:HyperLink ID="linkEdit" runat="server" Text="Edit" /> | 
<asp:LinkButton ID="linkDelete" runat="server" Text="Delete" /> | 
<asp:HyperLink ID="linkTerritories" runat="server" Text="Territories" />
</ItemTemplate>
</asp:TemplateField>

</Columns>
</asp:GridView>

<div align="right" style="margin-top:10px">
<asp:Button ID="cmdAdd" runat="server" Text="Add New Item" CssClass="button" />
</div>

</div>
</asp:Content>
