<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlInspectionChapter.ascx.cs"
    Inherits="UserControlInspectionChapter" %>

 <%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
    
<telerik:RadComboBox DropDownPosition="Static" ID="ddlInspectionChapter" runat="server" DataTextField="FLDCHAPTERNAME" DataValueField="FLDCHAPTERID" EnableLoadOnDemand="True"
    OnDataBound="ddlInspectionChapter_DataBound" OnTextChanged="ddlInspectionChapter_TextChanged" EmptyMessage="Type to select Inspection chapter" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
