<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlBatchList.ascx.cs" Inherits="UserControls_UserControlBatchList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div runat="server" id="divBatchList" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstBatch" DataTextField="FLDBATCH" DataValueField="FLDBATCHID"
        runat="server" CheckBoxes="true" ShowCheckAll="true" SelectionMode="Multiple" OnDataBound="lstFleet_DataBound">
    </telerik:RadListBox>
</div>
