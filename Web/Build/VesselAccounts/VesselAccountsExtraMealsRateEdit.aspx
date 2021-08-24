<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsExtraMealsRateEdit.aspx.cs"
    Inherits="VesselAccountsExtraMealsRateEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Extra Meals Default Rate</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuDefaultRate" runat="server" OnTabStripCommand="MenuDefaultRate_TabStripCommand"></eluc:TabStrip>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccountType" runat="server" Text="Account Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlAccountType" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select Type" Filter="Contains" MarkFirstMatch="true" OnSelectedIndexChanged="ddlAccountType_OnSelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="Owners" Value="-1" />
                                <telerik:RadComboBoxItem Text="Charterer" Value="-2" />

                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RateperManday" runat="server" Text="Rate per Manday"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtDefaultRate" CssClass="input" runat="server" Width="100px" MaxLength="7"
                            DecimalPlace="2" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActualVictuallingRate" runat="server" Text="Actual Victualling Rate"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkVictualRate" runat="server" OnCheckedChanged="chkVictualRate_OnCheckedChanged"
                            AutoPostBack="true" />
                    </td>
                </tr>
            </table>
              <telerik:RadGrid RenderMode="Lightweight" ID="gvExtraMealsDefaultRate" runat="server" AllowCustomPaging="false" AllowSorting="true"
                AllowPaging="true" CellSpacing="0" GridLines="None" EnableViewState="false"
                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvExtraMealsDefaultRate_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <Columns>
                     

                        <telerik:GridTemplateColumn HeaderText="Account Type">
                            <HeaderStyle />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDACCOUNTTYPE"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Default Rate">
                            <HeaderStyle/>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                   <%#((DataRowView)Container.DataItem)["FLDDEFAULTRATE"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                       
                        <telerik:GridTemplateColumn HeaderText="Actual Victualling RateY/N">
                            <HeaderStyle />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                              <%#((DataRowView)Container.DataItem)["FLDACTUALVICTUALINGRATE"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
       
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
