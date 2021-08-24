<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementRAProcessList.aspx.cs" Inherits="DocumentManagementRAProcessList" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Assessment Process</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">      
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            EnableAJAX="false" Height="100%">           
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />                
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rblOldNew" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Table" OnSelectedIndexChanged="rblOldNew_SelectedIndexChanged">
                                <asp:ListItem Text="Old" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="New" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>

                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRefNumber" runat="server" Text='Ref Number'>
                                    </telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtRefNo" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lbltypes" runat="server" Text='Type'>
                                    </telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Category ID="ucType" runat="server" AppendDataBoundItems="true" CssClass="input"
                                AutoPostBack="True" OnTextChangedEvent="ucType_SelectedIndexChanged" Visible="true" />
                            <telerik:RadComboBox ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                                AppendDataBoundItems="true" Visible="false">
                            </telerik:RadComboBox>

                        </td>
                        <td>
                             <telerik:RadLabel ID="lblRevNos" runat="server" Text='Vessel'>
                                    </telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                CssClass="input" OnTextChangedEvent="ucVessel_SelectedIndexChanged" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblactivityconditions" runat="server" Text='Activity/Condition'>
                                    </telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtActivity" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <eluc:TabStrip ID="MenuProcess" runat="server" OnTabStripCommand="MenuProcess_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvRiskAssessmentProcess" Height="86%" runat="server" AllowMultiRowSelection="true"
                    AllowSorting="true" AllowPaging="true" AllowCustomPaging="true" CellSpacing="0" EnableHeaderContextMenu="true"
                    GridLines="None" OnItemCommand="gvRiskAssessmentProcess_ItemCommand" OnItemDataBound="gvRiskAssessmentProcess_ItemDataBound"
                    OnNeedDataSource="gvRiskAssessmentProcess_NeedDataSource" ShowFooter="false" EnableViewState="false" Width="100%">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowCustomPaging="true"
                        AllowNaturalSort="false" AutoGenerateColumns="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <NoRecordsTemplate>
                            <table id="Table2" runat="server" width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                            Font-Bold="true">
                                        </telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Type">
                                <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME")  %>'>
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRAType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATYPE")  %>' Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Activity/Condition">
                                <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblJobActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEXT").ToString().Length > 35 ? (DataBinder.Eval(Container, "DataItem.FLDTEXT").ToString().Substring(0, 35) + "...") : DataBinder.Eval(Container, "DataItem.FLDTEXT").ToString() %>'>
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRiskAssessmentProcessID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTPROCESSID")  %>' Visible="false">
                                    </telerik:RadLabel>
                                    <eluc:ToolTip ID="ucJobActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEXT") %>' TargetControlId="lblJobActivity" Width="400px" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Ref Number">
                                <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFNO")  %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status">
                                <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")  %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Issued Date">
                                <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblIssuedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDISSUEDATE"))  %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rev No">
                                <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRevNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISION")  %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Process PDF" CommandName="RAPROCESS" ID="cmdRAProcess" ToolTip="Show PDF">
                                    <span class="icon"><i class="fas fa-chart-bar"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
