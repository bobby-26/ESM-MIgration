<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnComponentTypeJob.ascx.cs"
    Inherits="UserControlMultiColumnComponentTypeJob" %>
<telerik:RadComboBox RenderMode="Lightweight" ID="gvMulticolumn" runat="server" Width="200" Height="150" NoWrap="true" Culture="es-ES"
    DataTextField="FLDJOBCODE" DataValueField="FLDGLOBALCOMPONENTTYPEJOBMAPID" HighlightTemplatedItems="true" DropDownWidth="300px" EnableScreenBoundaryDetection="true"
    EmptyMessage="select from the dropdown or type Job Details" EnableLoadOnDemand="True" ShowMoreResultsBox="true" Filter="Contains" 
    EnableVirtualScrolling="true" OnItemsRequested="gvMulticolumn_ItemsRequested" DropDownCssClass="virtualRadComboBox" OnTextChanged="gvMulticolumn_TextChanged">
    <HeaderTemplate>
        <ul>
            <li class="col2">Code</li>
            <li class="col4">Title</li>
            <li class="col3">Frequency</li>
        </ul>
    </HeaderTemplate>
    
    <ItemTemplate>
        <ul>
            <li class="col2">
                <%# DataBinder.Eval(Container.DataItem, "FLDJOBCODE") %></li>
            <li class="col4">
                <%# DataBinder.Eval(Container.DataItem, "FLDJOBTITLE") %></li>
            <li class="col3">
                <%# DataBinder.Eval(Container.DataItem, "FLDFREQUENCYNAME") %>/ <%# DataBinder.Eval(Container.DataItem, "FLDCOUNTERFREQUENCYNAME") %></li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>
