<%@ Control Language="C#" AutoEventWireup="false" EnableViewState="true" Inherits="Northwind.TestSite.UserControls.PagingControl" Codebehind="PagingControl.ascx.cs" %>
<asp:HiddenField ID="rpp" runat="server" Value="0" />
<table width="100%" class="pagingcontainer">
<tr>
<td class="pagingtextsmall">
<asp:Panel ID="pnlHeader" runat="server">
<asp:Literal ID="lblFound" runat="server" EnableViewState="false" />
</asp:Panel>
</td>
<td class="paginggotopage" style="text-align:right;">
<asp:Panel ID="pnlPagePer" runat="server">
Display: <asp:DropDownList ID="cboPagePer" runat="server" AutoPostBack="true" EnableViewState="true" />
</asp:Panel>
</td>
</tr>
<tr>
<td class="sortbox">
<asp:PlaceHolder ID="pnlSort" runat="server"></asp:PlaceHolder>
</td>
<td class="paginggotopage" style="text-align:right;">
<asp:Panel ID="pnlGotoContainer" runat="server">
Go to page: &nbsp;&nbsp;<asp:PlaceHolder ID="pnlGotoPage" runat="server" />
</asp:Panel>
</td>
</tr>
</table>