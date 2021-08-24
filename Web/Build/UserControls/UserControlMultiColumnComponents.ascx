<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnComponents.ascx.cs"
    Inherits="UserControlMultiColumnComponents" %>
<telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="RadMCComponents" Width="300" Height="150"
    DataTextField="FLDCOMPONENTNAME" DataValueField="FLDCOMPONENTID" HighlightTemplatedItems="true" DropDownWidth="300" ShowMoreResultsBox="true"
    EmptyMessage="select from the dropdown or type component" MarkFirstMatch="true" EnableLoadOnDemand="true" Filter="Contains" OnTextChanged="RadMCComponents_TextChanged"
    EnableVirtualScrolling="true" OnItemsRequested="RadMCComponents_ItemsRequested" DropDownCssClass="virtualRadComboBox" OnItemDataBound="RadMCComponents_ItemDataBound">
    <HeaderTemplate>
        <ul>
            <li class="col4">Component No.</li>
            <li class="col5">Component Name</li>
        </ul>
    </HeaderTemplate>
    <ItemTemplate>
        <ul>
            <li class="col4">
                <%# DataBinder.Eval(Container.DataItem, "FLDCOMPONENTNUMBER") %></li>
            <li class="col5">
                <%# DataBinder.Eval(Container.DataItem, "FLDCOMPONENTNAME") %></li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>
