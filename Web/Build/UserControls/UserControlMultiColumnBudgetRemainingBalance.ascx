<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnBudgetRemainingBalance.ascx.cs" Inherits="UserControls_UserControlMultiColumnBudgetRemainingBalance" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="RadMCBudget" Width="300" Height="150"
    DataTextField="FLDDESCRIPTION" DataValueField="FLDBUDGETID" HighlightTemplatedItems="true" DropDownWidth="350" ShowMoreResultsBox="true"
    EmptyMessage="select from the dropdown or type budget code" MarkFirstMatch="true" EnableLoadOnDemand="true" Filter="Contains" OnTextChanged="RadMCBudget_TextChanged"
    EnableVirtualScrolling="true" OnItemsRequested="RadMCBudget_ItemsRequested" DropDownCssClass="virtualRadComboBox" OnItemDataBound="RadMCBudget_ItemDataBound">

    <HeaderTemplate>
        <ul>
            <li class="col2">Code</li>
            <li class="col5">Description</li>
         
</li>
            
        </ul>
    </HeaderTemplate>
    <ItemTemplate>
        <ul>
            <li class="col2">
                <%# DataBinder.Eval(Container.DataItem, "FLDSUBACCOUNT") %></li>
            <li class="col5">
                <%# DataBinder.Eval(Container.DataItem, "FLDDESCRIPTION") %></li>
         <%--   <li class="col5">
                <%# DataBinder.Eval(Container.DataItem, "FLDBUDGETGROUP") %></li>--%>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>
<eluc:Hard ID="ucBudgetGroup" AppendDataBoundItems="true" runat="server" AutoPostBack="true" OnTextChangedEvent="ucBudgetGroup_TextChangedEvent"/>