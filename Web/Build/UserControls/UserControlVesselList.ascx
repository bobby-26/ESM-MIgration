<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVesselList.ascx.cs"
    Inherits="UserControlVesselList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="divVesselList" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstVessel" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" 
        CheckBoxes="true" ShowCheckAll="true" runat="server" OnDataBound="lstVessel_DataBound" 
       OnTextChanged="lstVessel_TextChanged"></telerik:RadListBox>
</div>
