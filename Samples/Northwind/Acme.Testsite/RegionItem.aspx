<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="false" CodeBehind="RegionItem.aspx.cs" Inherits="Northwind.TestSite.RegionItem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pageblock">

<h1>Region [<asp:Label ID="lblRegionId" runat="server" />]</h1>

<div style="margin-bottom:10px;">
<span class="prompt">Description:</span><br />
<asp:TextBox ID="txtDescription" runat="server" CssClass="textbox" Width="350px" />
</div>

<div align="right">
<asp:Button id="cmdSave" runat="server" Text="Save" CssClass="button" />
<asp:Button id="cmdCancel" runat="server" Text="Cancel" CssClass="button" />
</div>

</div>
</asp:Content>
