<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlEquipmentMakerModel.ascx.cs" Inherits="UserControlMakerModel" %>

<telerik:RadComboBox RenderMode="Lightweight" ID="gvEquipmentmaaker" runat="server" Width="400" Height="150" Culture="es-ES"
 DataTextField="FLDTYPE" DataValueField="FLDMODELID" HighlightTemplatedItems="true" DropDownWidth="400" EnableScreenBoundaryDetection="true"
    EmptyMessage="select from the dropdown " EnableLoadOnDemand="True" ShowMoreResultsBox="true" Filter="Contains" 
    EnableVirtualScrolling="true" OnItemsRequested="gvEquipmentmaker_ItemsRequested" DropDownCssClass="virtualRadComboBox">

        <HeaderTemplate>
        <ul>
            <li class="col3">Component</li>
            <li class="col2">Maker</li>
            <li class="col1">Model</li>
        </ul>
    </HeaderTemplate>

        <ItemTemplate>
        <ul>
            <li class="col3">
                <%# DataBinder.Eval(Container.DataItem, "FLDCOMPONENTNAME") %></li>
            <li class="col2">
                <%# DataBinder.Eval(Container.DataItem, "FLDMAKE") %></li>
            <li class="col1">
                <%# DataBinder.Eval(Container.DataItem, "FLDTYPE") %></li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>

