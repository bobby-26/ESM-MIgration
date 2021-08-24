<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlNationalityList.ascx.cs"
    Inherits="UserControlNationalityList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="divNationalityList" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstNationality" DataTextField="FLDNATIONALITY" DataValueField="FLDCOUNTRYCODE" 
        CheckBoxes="true" ShowCheckAll="true" runat="server" ></telerik:RadListBox>    
</div>
