<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPortList.ascx.cs" 
 Inherits="UserControls_UserControlPortList" %>

<%--<asp:ListBox ID="lstPort" DataTextField="FLDSEAPORTNAME" DataValueField="FLDSEAPORTID"
    SelectionMode="Multiple" runat="server" Width="240px" OnDataBound="lstPort_DataBound">    
</asp:ListBox>  --%>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="divchklist" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstPort" DataTextField="FLDSEAPORTNAME" DataValueField="FLDSEAPORTID" 
        CheckBoxes="true" ShowCheckAll="true" runat="server" OnDataBound="lstPort_DataBound"></telerik:RadListBox>
</div>  
