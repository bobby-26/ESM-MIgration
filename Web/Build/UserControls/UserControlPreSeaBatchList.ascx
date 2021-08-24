<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaBatchList.ascx.cs"
    Inherits="UserControlPreSeaBatchList" %>
<%--<div runat="server" id="divBatchList" style="overflow-y: auto; overflow-x: hidden; height:80px">
    <asp:ListBox ID="lstBatch" DataTextField="FLDBATCH" DataValueField="FLDBATCHID" SelectionMode="Multiple"
        runat="server" OnDataBound="lstFleet_DataBound"></asp:ListBox>
</div>--%>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="divBatchList" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstBatch" DataTextField="FLDBATCH" DataValueField="FLDBATCHID" 
        CheckBoxes="true" ShowCheckAll="true" runat="server"  OnDataBound="lstFleet_DataBound"></telerik:RadListBox>
</div>