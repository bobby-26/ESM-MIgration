<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlJobParameterValue.ascx.cs" Inherits="UserControlJobParameterValue" %>
<%@ Import Namespace="SouthNests.Phoenix.PlannedMaintenance" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<telerik:RadListView ID="lstJobParameterValue" runat="server" ItemPlaceholderID="pnlJobParameterValue"
    DataKeyNames="FLDPARAMETERID" AllowMultiItemEdit="true" OnItemDataBound="lstJobParameterValue_ItemDataBound">
    <LayoutTemplate>
        <asp:Panel ID="pnlJobParameterValue" runat="server">
        </asp:Panel>
    </LayoutTemplate>
    <ItemTemplate>
    </ItemTemplate>
    <EditItemTemplate>
        <telerik:RadLabel ID="lblParameterOptionId" runat="server" Visible="false" Text='<%#Bind("FLDPARAMETEROPTIONID")%>'></telerik:RadLabel>
        <telerik:RadLabel ID="lblParameterId" runat="server" Visible="false" Text='<%#Bind("FLDPARAMETERID")%>'></telerik:RadLabel>
        <telerik:RadLabel ID="lblValueId" runat="server" Visible="false" Text='<%#Bind("FLDVALUEID")%>'></telerik:RadLabel>
        <telerik:RadLabel ID="lblParameterName" runat="server" Visible="false" Text='<%#Bind("FLDPARAMETERNAME")%>'></telerik:RadLabel>

        <label><%#Eval("FLDPARAMETERNAME")%>:<%# !DataBinder.Eval(Container,"DataItem.FLDMINVALUE").ToString().Equals("") ? "( " + DataBinder.Eval(Container,"DataItem.FLDMINVALUE") + " - " + DataBinder.Eval(Container,"DataItem.FLDMAXVALUE") + " )" : ""%></label>

        <telerik:RadTextBox ID="txtJobParameterValue" runat="server" Text='<%#Bind("FLDVALUE")%>'></telerik:RadTextBox>
        <eluc:Number ID="txtJobParameternumber" runat="server" CssClass="input" Width="100px" IsInteger="true"/>  
        <eluc:Decimal ID="txtJobParameterdecimal" runat="server" CssClass="input" Width="100px" />
        <telerik:RadDropDownList ID="ddlparameteroption" runat="server" DataTextField="FLDOPTIONNAME" DataValueField="FLDJOBPARAMETEROPTIONSID"></telerik:RadDropDownList>
        <telerik:RadLabel ID="lblParameterType" runat="server" Visible="false" Text='<%#Bind("FLDPARAMETERTYPE")%>'></telerik:RadLabel>
    </EditItemTemplate>
</telerik:RadListView>
