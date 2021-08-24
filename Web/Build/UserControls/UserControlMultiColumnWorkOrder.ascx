<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnWorkOrder.ascx.cs"
    Inherits="UserControlMultiColumnWorkOrder" %>
<telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="RadWorkorder" Width="300" Height="150" CheckBoxes="true"
    DataTextField="FLDTITLE" DataValueField="FLDWORKORDERID" HighlightTemplatedItems="true" DropDownWidth="300" ShowMoreResultsBox="true"
    EmptyMessage="select from the dropdown or type job title" MarkFirstMatch="true" EnableLoadOnDemand="true" Filter="Contains" OnTextChanged="RadWorkorder_TextChanged"
    EnableVirtualScrolling="true" OnItemsRequested="RadWorkorder_ItemsRequested" DropDownCssClass="virtualRadComboBox" OnItemDataBound="RadWorkorder_ItemDataBound">
    <HeaderTemplate>
        <ul>
            <li class="col4">Component No</li>
            <li class="col5">Title</li>
        </ul>
    </HeaderTemplate>
    <ItemTemplate>
        <ul>
            <li class="col4">
                <%# DataBinder.Eval(Container.DataItem, "FLDCOMPONENTNUMBER") %></li>
            <li class="col5">   
                <%# DataBinder.Eval(Container.DataItem, "FLDTITLE") %></li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>
