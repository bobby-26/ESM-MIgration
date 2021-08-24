<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCountryList.ascx.cs"
    Inherits="UserControlCountryList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="chkboxlist" style="overflow-y: auto; overflow-x: hidden;
    height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstCountry" DataTextField="FLDCOUNTRYNAME" DataValueField="FLDCOUNTRYCODE"
        runat="server" CheckBoxes="true" ShowCheckAll="true" SelectionMode="Multiple">
    </telerik:RadListBox>
</div>
