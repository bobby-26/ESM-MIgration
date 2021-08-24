<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnPMSPTWA.ascx.cs"
    Inherits="UserControlMultiColumnPMSPTWA" %>
<telerik:RadComboBox RenderMode="Lightweight" ID="gvMulticolumn" runat="server" Width="200" Height="150" NoWrap="true" Culture="es-ES"
    DataTextField="FLDCAPTION" DataValueField="FLDFORMID" HighlightTemplatedItems="true" DropDownWidth="300px" EnableScreenBoundaryDetection="true"
    EmptyMessage="select from the dropdown or type Job Details" EnableLoadOnDemand="True" ShowMoreResultsBox="true" Filter="Contains" 
    EnableVirtualScrolling="true" OnItemsRequested="gvMulticolumn_ItemsRequested" DropDownCssClass="virtualRadComboBox" OnTextChanged="gvMulticolumn_TextChanged">
    <HeaderTemplate>
        <ul>
            <li class="col1">Form No</li>
            <li class="col3">Category</li>
            <li class="col3">Name</li>
            <li class="col2">Remarks</li>
        </ul>
    </HeaderTemplate>
    
    <ItemTemplate>
        <ul>
            <li class="col1">
                <%# DataBinder.Eval(Container.DataItem, "FLDFORMNO") %></li>
            <li class="col3">
                <%# DataBinder.Eval(Container.DataItem, "FLDCATEGORYNAME") %></li>
            <li class="col3">
                <%# DataBinder.Eval(Container.DataItem, "FLDCAPTION") %></li>
            <li class="col2">
                <%# DataBinder.Eval(Container.DataItem, "FLDPURPOSE") %></li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>
