<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardVesselPositionAlerts.aspx.cs"
    Inherits="DashboardVesselPositionAlerts" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>

<html lang="en">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixdashboard.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/bootstrap/css/bootstrap.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/DashboardNew.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/main.js"></script>

         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <style>
            .gainVal {
                background: #2A6629 !important;
                color: white !important;
                padding: 3px 5px !important;
            }

            .lossVal {
                background: #F01511 !important;
                color: white !important;
                padding: 3px 5px !important;
            }

            .noVal {
                padding: 3px 5px !important;
            }

            .displayRed {
                color: red !important;
                padding: 3px 5px !important;
            }

            .displayBlue {
                color: blue !important;
                padding: 3px 5px !important;
            }

            .customIconAlert {
                width: 13px !important;
                -webkit-width: 13px !important;
                -moz-width: 13px !important;
                -ms-width: 13px !important;
                height: 13px !important;
                -ms-height: 13px !important;
                -webkit-height: 13px !important;
                -moz-height: 13px !important;
                cursor: pointer !important;
                white-space: nowrap !important;
            }
              .RadGrid .rgHeader, .RadGrid th.rgResizeCol,.RadGrid .rgRow td,.RadGrid .rgAltRow td {
            padding: 2px !important;
            font-size:10px !important;
            /*padding-right: 2px !important;*/
        }

         .RadGrid .rgRow td,.RadGrid .rgAltRow td {
            padding: 2px !important;
            font-size:11px !important;
            /*padding-right: 2px !important;*/
        }
         .RadGrid .item-style td
        {
            height: 10px !important;
            vertical-align: middle !important;
        }
        </style>
        <script type="text/javascript">
       function Resize() {

                   TelerikGridResize($find("<%= gvMeasure.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
    </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1"
            runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlFleetSummary" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <telerik:RadAjaxPanel runat="server" ID="pnlFleetSummary">
                <%--                <div id="dataVizWrap">--%>

                    <telerik:RadGrid ID="gvMeasure" runat="server" AutoGenerateColumns="false" GridLines="None"
                        Width="100%" OnItemDataBound="gvMeasure_RowDataBound" OnNeedDataSource="gvMeasure_NeedDataSource"
                        ShowHeader="true" EnableViewState="false" Font-Size="11px" >
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false">
                            <HeaderStyle Width="102px" />
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderStyle-Width="160px">
                                    <ItemStyle Wrap="False" HorizontalAlign="left" VerticalAlign="Middle" Width="60px"></ItemStyle>
                                    <HeaderStyle Wrap="false" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblHeaderMeasure" runat="server" Text="Measure Name"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span>

                                            <telerik:RadLabel ID="lblMeasureName" runat="server" CssClass="tableMeasureName" Text='<%#DataBinder.Eval(Container, "DataItem.MEASURENAME")%>'></telerik:RadLabel>
                                             <asp:LinkButton runat="server" AlternateText="Possible Cause" CommandName="INFO" ID="ImgCauseExistsYN" ToolTip="Possible Cause">
                                <span class="icon"><i class="fas fa-info-circle"></i></span>
                                </asp:LinkButton>
                                        </span>
                                        <telerik:RadLabel ID="lblMeasureActualName" Visible="false" runat="server" CssClass="tableMeasureName" Text='<%#DataBinder.Eval(Container, "DataItem.COLUMNNAME")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="1" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true"  />
                        </ClientSettings>
                    </telerik:RadGrid>
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            </telerik:RadAjaxPanel>


    </form>
</body>
</html>
