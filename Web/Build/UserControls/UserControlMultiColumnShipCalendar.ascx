<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnShipCalendar.ascx.cs"
    Inherits="UserControlMultiColumnShipCalendar" %>


<telerik:RadComboBox RenderMode="Lightweight" ID="gvMulticolumn" runat="server" Width="200" Height="150" NoWrap="true"
    DataTextField="FLDDATE" DataValueField="FLDSHIPCALENDARID" HighlightTemplatedItems="true" DropDownWidth="300" EnableScreenBoundaryDetection="true"
    EmptyMessage="select from the dropdown or type supplier" EnableLoadOnDemand="true" ShowMoreResultsBox="true" Filter="Contains" 
    EnableVirtualScrolling="true" OnItemsRequested="gvMulticolumn_ItemsRequested" DropDownCssClass="virtualRadComboBox" OnTextChanged="gvMulticolumn_TextChanged">
    <HeaderTemplate>
        <ul>
            <li class="col3">ID</li>
            <li class="col3">Date</li>
            <li class="col3">Clock</li>
        </ul>
    </HeaderTemplate>
    <ItemTemplate>
        <ul>
            <li class="col3">
                <%# DataBinder.Eval(Container.DataItem, "FLDSHIPCALENDARID") %></li>
            <li class="col3">
                <%# DataBinder.Eval(Container.DataItem, "FLDDATE","{0:dd/MMM/yyyy}") %></li>
            <li class="col3">
                <%# DataBinder.Eval(Container.DataItem, "FLDCLOCKNAME") %></li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>
