<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnInspector.ascx.cs"
    Inherits="UserControlMultiColumnInspector" %>
<telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="RadMCUser" Width="200" Height="200" OnItemDataBound="RadMCUser_ItemDataBound"
    DataTextField="FLDUSERNAME" DataValueField="FLDUSERCODE" HighlightTemplatedItems="true" DropDownAutoWidth="Enabled" ShowMoreResultsBox="true"
    EmptyMessage="select from dropdown or type Name" MarkFirstMatch="true" EnableLoadOnDemand="true" Filter="Contains" OnTextChanged="RadMCUser_TextChanged"
    EnableVirtualScrolling="true" OnItemsRequested="RadMCUser_ItemsRequested" DropDownCssClass="virtualRadComboBox">
    <HeaderTemplate>
        <ul>
            <li class="col3">Name</li>
            <li class="col4">Designation</li>
        </ul>
    </HeaderTemplate>
    <ItemTemplate>
        <ul>
            <li class="col3"><%# DataBinder.Eval(Container.DataItem, "FLDUSERNAME") %></li>
            <li class="col4"><%# DataBinder.Eval(Container.DataItem, "FLDDESIGNATIONNAME" ) %></li>
        </ul>
    </ItemTemplate>
</telerik:RadComboBox>