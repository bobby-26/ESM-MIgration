<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlStatusList.ascx.cs"
    Inherits="UserControls_UserControlStatusList" %>
<%--<div runat="server" id="divStatusList" style="overflow-y: auto; overflow-x: hidden;height: 80px">
    <asp:ListBox runat="server" ID="lstStatus" RepeatLayout="Flow" DataTextField="FLDHARDNAME" SelectionMode="Multiple"
        DataValueField="FLDHARDCODE" OnDataBound="lstStatus_DataBound" OnTextChanged="lstStatus_TextChanged"></asp:ListBox>
</div>--%>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="divStatusList" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstStatus" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" 
        CheckBoxes="true" ShowCheckAll="true" runat="server" OnDataBound="lstStatus_DataBound" 
        OnTextChanged="lstStatus_TextChanged"></telerik:RadListBox>
</div>
