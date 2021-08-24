<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnInspection.ascx.cs"
    Inherits="UserControlMultiColumnInspection" %>

<telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="gvMulticolumn" Width="300" Height="200"
    DataTextField="FLDAUDITSCHEDULENAME" DataValueField="FLDREVIEWSCHEDULEID" HighlightTemplatedItems="true" DropDownAutoWidth="Enabled" ShowMoreResultsBox="true"
    EmptyMessage="select from the dropdown or type Name" MarkFirstMatch="true" EnableLoadOnDemand="true" Filter="Contains" OnTextChanged="gvMulticolumn_TextChanged"
    EnableVirtualScrolling="true" OnItemsRequested="gvMulticolumn_ItemsRequested" DropDownCssClass="virtualRadComboBox">
    <HeaderTemplate>
        <ul>
            <li class="col5">Inspection</li>
            <li class="col3">Vessel</li>
        </ul>
    </HeaderTemplate>
    <ItemTemplate>
        <ul>
            <li class="col5"><%# DataBinder.Eval(Container.DataItem, "FLDAUDITSCHEDULENAME") %></li>
            <li class="col3"><%# DataBinder.Eval(Container.DataItem, "FLDVESSELNAME") %></li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>
