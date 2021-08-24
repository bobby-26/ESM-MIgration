<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnVesselSupplier.ascx.cs"
    Inherits="UserControlMultiColumnVesselSupplier" %>


<telerik:RadComboBox RenderMode="Lightweight" ID="gvMulticolumn" runat="server" Width="200" Height="150" NoWrap="true"
    DataTextField="FLDNAME" DataValueField="FLDSUPPLIERCODE" HighlightTemplatedItems="true" DropDownWidth="300" EnableScreenBoundaryDetection="true"
    EmptyMessage="select from the dropdown or type supplier" EnableLoadOnDemand="True" ShowMoreResultsBox="true" Filter="Contains" 
    EnableVirtualScrolling="true" OnItemsRequested="gvMulticolumn_ItemsRequested" DropDownCssClass="virtualRadComboBox" OnTextChanged="gvMulticolumn_TextChanged">
    <HeaderTemplate>
        <ul>
            <li class="col3">Code</li>
            <li class="col3">Name</li>
        </ul>
    </HeaderTemplate>
    <ItemTemplate>
        <ul>
            <li class="col3">
                <%# DataBinder.Eval(Container.DataItem, "FLDCODE") %></li>
            <li class="col3">
                <%# DataBinder.Eval(Container.DataItem, "FLDNAME") %></li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>
