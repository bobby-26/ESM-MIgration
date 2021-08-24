<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlFleetList.ascx.cs"
    Inherits="UserControlFleetList" %>
<%--<div runat="server" id="divFleetList" style="overflow-y: auto; overflow-x: hidden;
    height: 80px">
    <asp:ListBox ID="lstFleet" DataTextField="FLDFLEETDESCRIPTION" DataValueField="FLDFLEETID"
        SelectionMode="Multiple" runat="server" OnDataBound="lstFleet_DataBound"></asp:ListBox>
</div>--%>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="chkboxlist" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstFleet" DataTextField="FLDFLEETDESCRIPTION" DataValueField="FLDFLEETID" 
        CheckBoxes="true" ShowCheckAll="true"  runat="server"
        OnDataBound="lstFleet_DataBound"></telerik:RadListBox> 
</div>

