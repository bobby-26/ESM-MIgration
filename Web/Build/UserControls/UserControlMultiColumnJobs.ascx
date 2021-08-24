<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnJobs.ascx.cs" Inherits="UserControlMultiColumnJobs" %>

<telerik:RadComboBox RenderMode="Lightweight" ID="gvMulticolumn" runat="server" Width="200" Height="150" NoWrap="true"
    DataTextField="FLDJOBTITLE" DataValueField="FLDJOBID" HighlightTemplatedItems="true" DropDownWidth="300" EnableScreenBoundaryDetection="true"
    EmptyMessage="select from the dropdown or type job" EnableLoadOnDemand="True" ShowMoreResultsBox="true" Filter="Contains" 
    EnableVirtualScrolling="true" OnItemsRequested="gvMulticolumn_ItemsRequested" DropDownCssClass="virtualRadComboBox" 
    OnTextChanged="gvMulticolumn_TextChanged" OnItemDataBound="gvMulticolumn_ItemDataBound">
    <HeaderTemplate>
        <ul>
            <li class="col3">Job Code</li>
            <li class="col3">Job Title</li>
        </ul>
    </HeaderTemplate>
    <ItemTemplate>
        <ul>
            <li class="col3">
                <%# DataBinder.Eval(Container.DataItem, "FLDJOBCODE") %></li>
            <li class="col3">
                <%# DataBinder.Eval(Container.DataItem, "FLDJOBTITLE") %></li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>
