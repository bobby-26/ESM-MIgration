<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlInspectionChapterList.ascx.cs" Inherits="UserControlInspectionChapterList" %>

<%--<asp:ListBox ID="lstChapter" DataTextField="FLDCHAPTERNAME" DataValueField="FLDCHAPTERID" OnDataBound="lstChapter_DataBound"
    OnTextChanged="lstChapter_TextChanged" SelectionMode="Multiple" runat="server">
</asp:ListBox>--%>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="chkboxlist" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstChapter" DataTextField="FLDCHAPTERNAME" DataValueField="FLDCHAPTERID" 
        CheckBoxes="true" ShowCheckAll="true"  runat="server" OnDataBound="lstChapter_DataBound"
        OnTextChanged="lstChapter_TextChanged"></telerik:RadListBox>   
</div>
