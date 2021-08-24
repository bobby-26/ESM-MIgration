<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnOwnerBudgetCodeT.ascx.cs" Inherits="UserControlMultiColumnOwnerBudgetCodeT" %>

<telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="gvMulticolumn" Width="360px" Height="200px"
    DataTextField="FLDOWNERBUDGETCODE" DataValueField="FLDOWNERBUDGETCODEID" HighlightTemplatedItems="true" DropDownAutoWidth="Enabled" ShowMoreResultsBox="true"
    EmptyMessage="select from the dropdown or type Name" MarkFirstMatch="true" EnableLoadOnDemand="true" Filter="Contains" OnTextChanged="gvMulticolumn_TextChanged"
    EnableVirtualScrolling="true" OnItemsRequested="gvMulticolumn_ItemsRequested" DropDownCssClass="virtualRadComboBox" NoWrap="true">
    <HeaderTemplate>
        <ul>
            <li class="col2">Owner Budget Code</li>
            <li class="col2">Owner Budget Group</li>
            <li class="col2">Mapped Budget Code</li>
            <li class="col2">Description</li>
        </ul>
    </HeaderTemplate>
    <ItemTemplate>
        <ul>
            <li class="col2"><%# DataBinder.Eval(Container.DataItem, "FLDOWNERBUDGETCODE") %></li>
            <li class="col2"><%# DataBinder.Eval(Container.DataItem, "FLDOWNERBUDGETGROUP") %></li>
            <li class="col2"><%# DataBinder.Eval(Container.DataItem, "FLDSUBACCOUNT") %></li>
            <li class="col2"><%# DataBinder.Eval(Container.DataItem, "FLDOWNERCODEDESCRIPTION") %></li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>
