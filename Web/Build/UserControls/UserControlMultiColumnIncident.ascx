<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnIncident.ascx.cs"
    Inherits="UserControlMultiColumnIncident" %>
<telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="gvMulticolumn" Width="240px" Height="200px"
    DataTextField="FLDINCIDENTREFNO" DataValueField="FLDINSPECTIONINCIDENTID" HighlightTemplatedItems="true" DropDownAutoWidth="Enabled" ShowMoreResultsBox="true"
    EmptyMessage="select from the dropdown or type Name" MarkFirstMatch="true" EnableLoadOnDemand="true" Filter="Contains" OnTextChanged="gvMulticolumn_TextChanged"
    EnableVirtualScrolling="true" OnItemsRequested="gvMulticolumn_ItemsRequested" DropDownCssClass="virtualRadComboBox">
    <HeaderTemplate>
        <ul>
            <li class="col5">Incident</li>
            <li class="col3">Vessel</li>
        </ul>
    </HeaderTemplate>
    <ItemTemplate>
        <ul>
            <li class="col5"><%# DataBinder.Eval(Container.DataItem, "FLDINCIDENTREFNO") %></li>
            <li class="col3"><%# DataBinder.Eval(Container.DataItem, "FLDVESSELNAME") %></li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>