<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlInspectionTopicList.ascx.cs" Inherits="UserControlInspectionTopicList" %>

<%--<asp:ListBox ID="lstTopic" DataTextField="FLDTOPICNAME" DataValueField="FLDTOPICID" OnDataBound="lstTopic_DataBound"
    OnTextChanged="lstTopic_TextChanged" SelectionMode="Multiple" runat="server">
</asp:ListBox>--%>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="chkboxlist" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstTopic" DataTextField="FLDTOPICNAME" DataValueField="FLDTOPICID" 
        CheckBoxes="true" ShowCheckAll="true"  runat="server" OnDataBound="lstTopic_DataBound"
        OnTextChanged="lstTopic_TextChanged"></telerik:RadListBox>
</div>
