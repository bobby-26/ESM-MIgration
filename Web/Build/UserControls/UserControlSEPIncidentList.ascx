<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSEPIncidentList.ascx.cs" Inherits="UserControlSEPIncidentList" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div runat="server" id="divCheckboxList" class="input" style="overflow: auto; width: 80%;
                        height: 140px">
    <telerik:RadLabel ID="lblCheckboxList" runat="server" Text="Label" Visible="false"></telerik:RadLabel>
     <telerik:RadRadioButtonList RenderMode="Lightweight" ID="UcSEP" runat="server"  OnSelectedIndexChanged="UcSEP_SelectedIndexChanged"
         DataTextField="FLDDESCRIPTION"   DataValueField="FLDDTKEY" OnDataBound="UcSEP_DataBound" Height="40px">
    </telerik:RadRadioButtonList>    
</div>