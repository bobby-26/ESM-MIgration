<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSignOffReasonList.ascx.cs" Inherits="UserControlSignOffReasonList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div runat="server" id="divSignOffReasonList" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstReason" DataTextField="FLDREASON" DataValueField="FLDREASONID"
        runat="server" CheckBoxes="true" ShowCheckAll="true" SelectionMode="Multiple" OnDataBound="lstReason_DataBound" Width="240px">
    </telerik:RadListBox>
</div>
