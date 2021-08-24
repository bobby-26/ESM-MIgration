<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlEngineTypeList.ascx.cs"
    Inherits="UserControlEngineTypeList" %>

<%--<asp:ListBox ID="lstEngineType" runat="server" DataTextField="FLDENGINENAME" SelectionMode="Multiple"
    DataValueField="FLDENGINEID" OnDataBound="lstEngineType_DataBound" >
</asp:ListBox>--%>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="chkboxlist" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstEngineType" DataTextField="FLDENGINENAME" DataValueField="FLDENGINEID" EnableLoadOnDemand="true"
        CheckBoxes="true" ShowCheckAll="true" SelectionMode="Multiple" runat="server" OnDataBound="lstEngineType_DataBound"
        ></telerik:RadListBox>
</div>

