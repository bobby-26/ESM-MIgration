<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnPort.ascx.cs" Inherits="UserControlMultiColumnPort" %>

<telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="RadMCPort" Width="300" Height="150"
    DataTextField="FLDSEAPORTNAME" DataValueField="FLDSEAPORTID" HighlightTemplatedItems="true" DropDownWidth="300" ShowMoreResultsBox="true"
    EmptyMessage="select from the dropdown or type Port" MarkFirstMatch="true" EnableLoadOnDemand="true" Filter="Contains" OnTextChanged="RadMCPort_TextChanged"
    EnableVirtualScrolling="true" OnItemsRequested="RadMCPort_ItemsRequested" DropDownCssClass="virtualRadComboBox">
    <HeaderTemplate>
        <ul>
            <li class="col4">Sea Port</li>
            <li class="col5">Country</li>
           <%-- <li class="col3">Airport</li>--%>
        </ul>
    </HeaderTemplate>
    <ItemTemplate>
        <ul>
            <li class="col4">
                <%# DataBinder.Eval(Container.DataItem, "FLDSEAPORTNAME") %></li>
            <li class="col5">
                <%# DataBinder.Eval(Container.DataItem, "FLDCOUNTRYNAME") %></li>
            <%--<li class="col3">
                <%# DataBinder.Eval(Container.DataItem, "FLDAIRPORTNAME") %></li>--%>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>
