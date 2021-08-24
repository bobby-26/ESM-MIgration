<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlNTBRReasonList.ascx.cs"
    Inherits="UserControlNTBRReasonList" %>
<%--<asp:ListBox ID="uclNTBRReason" DataTextField="FLDREASON" DataValueField="FLDREASONID" SelectionMode="Multiple" OnDataBound="uclNTBRReason_DataBound" 
    runat="server">    
</asp:ListBox>--%>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="chkboxlist" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="uclNTBRReason" DataTextField="FLDREASON" DataValueField="FLDREASONID"  Localization-CheckAll="--Check All--"
        CheckBoxes="true" ShowCheckAll="true" runat="server" OnDataBound="uclNTBRReason_DataBound"
        ></telerik:RadListBox> 
</div>

