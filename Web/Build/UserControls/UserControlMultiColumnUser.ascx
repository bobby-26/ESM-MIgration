<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnUser.ascx.cs" Inherits="UserControlMultiColumnUser" %>
    <telerik:RadToolTip ID="RadToolTip1" runat="server" TargetControlID="RadMCUser" ShowEvent="OnMouseOver" HideEvent="LeaveTargetAndToolTip" Position="TopCenter"></telerik:RadToolTip>
    <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="RadMCUser" Width="300" Height="200" OnItemDataBound="RadMCUser_ItemDataBound" MinFilterLength="2"
        DataTextField="FLDFIRSTNAME" DataValueField="FLDUSERCODE" HighlightTemplatedItems="true" DropDownAutoWidth="Enabled" ShowMoreResultsBox="true"
        EmptyMessage="select from the dropdown or type Name" MarkFirstMatch="true" EnableLoadOnDemand="true" Filter="Contains" OnTextChanged="RadMCUser_TextChanged"
        EnableVirtualScrolling="true" OnItemsRequested="RadMCUser_ItemsRequested" DropDownCssClass="virtualRadComboBox" ItemRequestTimeout="1500">
        <HeaderTemplate>
            <ul>
                <li class="col3">Department</li>
                <li class="col1">UserName</li>
                <li class="col3">Name / Designation</li>
                <li class="col2">Email</li>
            </ul>
        </HeaderTemplate>
        <ItemTemplate>
            <ul>
                <li class="col3"><%# DataBinder.Eval(Container.DataItem, "FLDDEPARTMENTNAME") %></li>
                <li class="col1"><%# DataBinder.Eval(Container.DataItem, "FLDUSERNAME") %></li>
                <li class="col3"><%# DataBinder.Eval(Container.DataItem, "FLDFIRSTNAME" )+" "+DataBinder.Eval(Container.DataItem, "FLDMIDDLENAME" )+" "+DataBinder.Eval(Container.DataItem, "FLDLASTNAME" )+" / "+DataBinder.Eval(Container.DataItem, "FLDDESIGNATIONNAME" ) %></li>
                <li class="col2"><%# DataBinder.Eval(Container.DataItem, "FLDEMAIL") %></li>
            </ul>
        </ItemTemplate>
    </telerik:RadComboBox>
    <input type="hidden" id="hdnEmail" runat="server" />
