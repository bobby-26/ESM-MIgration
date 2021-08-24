<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnPMSRA.ascx.cs"
    Inherits="UserControlMultiColumnPMSRA" %>
<telerik:RadComboBox RenderMode="Lightweight" ID="gvMulticolumn" runat="server" Width="200" Height="150" NoWrap="true" Culture="es-ES"
    DataTextField="FLDREFNO" DataValueField="FLDRISKASSESSMENTID" HighlightTemplatedItems="true" DropDownWidth="300px" EnableScreenBoundaryDetection="true"
    EmptyMessage="select from the dropdown or type Job Details" EnableLoadOnDemand="True" ShowMoreResultsBox="true" Filter="Contains" 
    EnableVirtualScrolling="true" OnItemsRequested="gvMulticolumn_ItemsRequested" DropDownCssClass="virtualRadComboBox" OnTextChanged="gvMulticolumn_TextChanged">
    <HeaderTemplate>
        <ul>
            <li class="col4">Category</li>
            <li class="col3">Activity</li>
            <li class="col2">Reference No</li>
        </ul>
    </HeaderTemplate>
    
    <ItemTemplate>
        <ul>
            <li class="col4">
                <%# DataBinder.Eval(Container.DataItem, "FLDCATEGORY") %></li>
            <li class="col3">
                <%# DataBinder.Eval(Container.DataItem, "FLDACTIVITY") %></li>
            <li class="col2">
                <%# DataBinder.Eval(Container.DataItem, "FLDREFNO") %></li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>
