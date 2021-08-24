<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnComponentType.ascx.cs" Inherits="UserControlMultiColumnComponentType" %>

<telerik:RadComboBox RenderMode="Lightweight" ID="gvMulticolumn" runat="server" Width="200" Height="150" NoWrap="true"
    DataTextField="FLDCOMPONENTNAME" DataValueField="FLDGLOBALCOMPONENTTYPEID" HighlightTemplatedItems="true" DropDownWidth="300" EnableScreenBoundaryDetection="true"
    EmptyMessage="select from the dropdown or type supplier" EnableLoadOnDemand="True" ShowMoreResultsBox="true" Filter="Contains" 
    EnableVirtualScrolling="true" OnItemsRequested="gvMulticolumn_ItemsRequested" DropDownCssClass="virtualRadComboBox" OnTextChanged="gvMulticolumn_TextChanged">
    <HeaderTemplate>
        <ul>
            <li class="col2">Number</li>
            <li class="col5">Name</li>
        </ul>
    </HeaderTemplate>
    <ItemTemplate>
        <ul>
            <li class="col2">
                <%# DataBinder.Eval(Container.DataItem, "FLDCOMPONENTNUMBER") %></li>
            <li class="col5">
                <%# DataBinder.Eval(Container.DataItem, "FLDCOMPONENTNAME") %></li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>
