<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlAddressTypeList.ascx.cs"
    Inherits="UserControls_UserControlAddressTypeList" %>
<%--<div runat="server" id="chkboxlist" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <asp:ListBox ID="lstAddressType" DataTextField="FLDNAME" DataValueField="FLDADDRESSCODE"
        SelectionMode="Multiple" runat="server" OnDataBound="lstAddressType_DataBound"
        OnTextChanged="lstAddressType_TextChanged"></asp:ListBox>
</div>--%>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="chkboxlist" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstAddressType" DataTextField="FLDNAME" DataValueField="FLDADDRESSCODE" Localization-CheckAll="--Check All--"
        CheckBoxes="true" ShowCheckAll="true" runat="server" OnTextChanged="lstAddressType_TextChanged"></telerik:RadListBox>
    <%--OnDataBound="lstAddressType_DataBound" --%>
</div>
