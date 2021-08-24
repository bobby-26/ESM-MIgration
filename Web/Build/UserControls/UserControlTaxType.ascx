<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlTaxType.ascx.cs"
    Inherits="UserControlTaxType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadRadioButtonList runat="server" ID="rblValuePercentage" Direction="Horizontal">
    <Items>
        <telerik:ButtonListItem Text="Value" Value="1"  />
        <telerik:ButtonListItem Text="Percentage" Value="2" Selected="true" />
    </Items>
</telerik:RadRadioButtonList>
