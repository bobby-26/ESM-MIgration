<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCurrencyList.ascx.cs"
    Inherits="UserControlCurrencyList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div runat="server" id="divCurrencyList" style="overflow-y: auto; overflow-x: hidden; height: 80px">
<telerik:RadListBox RenderMode="Lightweight" ID="lstCurrency" DataTextField="FLDCURRENCYNAME" DataValueField="FLDCURRENCYID"
        runat="server" CheckBoxes="true" ShowCheckAll="true" SelectionMode="Multiple">
</telerik:RadListBox>
</div>