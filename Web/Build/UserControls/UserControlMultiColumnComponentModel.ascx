<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnComponentModel.ascx.cs"
    Inherits="UserControlMultiColumnComponentModel" %>
<telerik:RadComboBox RenderMode="Lightweight" ID="gvMulticolumn" runat="server" Width="200" Height="150" NoWrap="true"
    DataTextField="FLDNAME" DataValueField="FLDMODELID" HighlightTemplatedItems="true" DropDownWidth="300" EnableScreenBoundaryDetection="true"
    EmptyMessage="select from the dropdown or type supplier" EnableLoadOnDemand="True" ShowMoreResultsBox="true" Filter="Contains" 
    EnableVirtualScrolling="true" OnItemsRequested="gvMulticolumn_ItemsRequested" DropDownCssClass="virtualRadComboBox" OnTextChanged="gvMulticolumn_TextChanged">
    <HeaderTemplate>
        <ul>
            <li class="col1">Maker</li>
            <li class="col2">Type/Model</li>
        </ul>
    </HeaderTemplate>
    <ItemTemplate>
        <ul>
            <li class="col5">
                <%# DataBinder.Eval(Container.DataItem, "FLDMAKE") %></li>
            <li class="col5">
                <%# DataBinder.Eval(Container.DataItem, "FLDTYPE") %></li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>
