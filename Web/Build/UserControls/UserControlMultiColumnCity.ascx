<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnCity.ascx.cs"
    Inherits="UserControlMultiColumnCity" %>

<telerik:RadComboBox RenderMode="Lightweight" ID="RadMCCity" runat="server" Width="300" Height="150"
    DataTextField="FLDCITYNAME" DataValueField="FLDCITYID" HighlightTemplatedItems="true" DropDownWidth="300" ShowMoreResultsBox="true"
    EmptyMessage="select from the dropdown or type city" MarkFirstMatch="true" EnableLoadOnDemand="True" Filter="Contains" OnTextChanged="RadMCCity_TextChanged"
    EnableVirtualScrolling="true" OnItemsRequested="RadMCCity_ItemsRequested" DropDownCssClass="virtualRadComboBox">
    <HeaderTemplate>
        <table style="width: 300px; text-align: left">
            <tr>
                <td style="width: 100px;">City</td>
                <td style="width: 100px;">State</td>
                <td style="width: 100px;">Country</td>
            </tr>
        </table>
    </HeaderTemplate>
    <ItemTemplate>
        <table style="width: 300px; text-align: left">
            <tr>
                <td style="width: 100px;">
                    <%# DataBinder.Eval(Container.DataItem, "FLDCITYNAME") %>        
                </td>
                <td style="width: 100px;">
                    <%# DataBinder.Eval(Container.DataItem, "FLDSTATENAME") %>        
                </td>
                <td style="width: 100px;">
                    <%# DataBinder.Eval(Container.DataItem, "FLDCOUNTRYNAME") %>        
                </td>
            </tr>
        </table>
    </ItemTemplate>
</telerik:RadComboBox>
