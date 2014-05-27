<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="TerritoryItem.aspx.cs" Inherits="Northwind.TestSite.TerritoryItem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pageblock">

<h1>Territory [<asp:Label ID="lblTerritoryId" runat="server" />]</h1>

<div style="margin-bottom:10px;">
<span class="prompt">Description:</span><br />
<asp:TextBox ID="txtDescription" runat="server" CssClass="textbox" Width="350px" />
</div>

<div style="margin-bottom:10px;">
<span class="prompt">Region:</span><br />
<asp:DropDownList ID="cboRegion" runat="server" />
</div>

<div align="right">
<asp:Button id="cmdSave" runat="server" Text="Save" CssClass="button" />
<asp:Button id="cmdCancel" runat="server" Text="Cancel" CssClass="button" />
</div>

</div>
</asp:Content>
