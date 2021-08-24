<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerReportMasterPage.aspx.cs" Inherits="OwnerReportMasterPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function PaneResized() {
                var sender = $find('RadSplitter1');
                var browserHeight = $telerik.$(window).height();
                sender.set_height(browserHeight - 40);
                $telerik.$(".#gvSection").height($telerik.$("#navigationPane").height() - 65);
            }
        </script>
        <style>
            button {
              text-decoration: none;
              display: inline-block;
              padding: 1px 8px;
            }
            
            button:hover {
              background-color: #ddd;
              color: black;
            }
            
            .previous {
              background-color:#2295f1;
              color: white;
            }

            .default {
              background-color:#2295f1;
              color: white;
            }
            
            .next {
              background-color: #2295f1;
              color: white;
            }
            
            .round {
              border-radius: 50%;
            }

            .rgGroupCol {
                padding-left: 0 !important;
                padding-right: 0 !important;
                font-size: 1px !important;
            }

            .rgExpand,
            .rgCollapse {
                display: none !important;
            }

            .RadGrid th.rgResizeCol, .RadGrid .rgRow td, .RadGrid .rgAltRow td {
                padding-left: 35px !important;
                padding-right: 2px !important;
                height: 5px !important;
            }

            .yellow {
                color: black !important;
                font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
                
            }

            .white {
                color: black !important;
                font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
                text-align-last: right !important;
                font-weight:bolder !important;              
            }

            .rgDataDiv {
                height: 78vh !important;
            }
            .RadGrid .rgRow, .RadGrid .rgAltRow, .RadGrid .rgEditRow, .RadGrid .rgFooter, .RadGrid .rgGroupHeader {
            /*height: 36px;*/
            height: calc(0.429em + 16px) !important;
        }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server"></telerik:RadScriptManager>
        <%--<telerik:RadSkinManager ID="RadSkinManager1" runat="server" />--%>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="88%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table runat="server" id="table1" style="width: 100%; background-color:rgb(194, 220, 252); background-image: linear-gradient(rgb(244, 248, 250), rgb(233, 242, 251) 50%, rgb(221, 231, 245) 50%, rgb(228, 237, 248));">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="MV/MT" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtvesselname" runat="server" CssClass="yellow" Visible="false"></telerik:RadLabel>
                        <telerik:RadComboBox ID="ddlVessel" DropDownCssClass="drpdwn" runat="server" OnSelectedIndexChanged="ddlVessel_TextChanged" Width="180" EnableLoadOnDemand="True" AutoPostBack="true"
                                    DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" EmptyMessage="Type to select vessel" Filter="Contains" MarkFirstMatch="true" EnableTextSelection="true">
                                </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbltype" runat="server" Text="Type" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtType" runat="server" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtFlag" runat="server" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOwner" runat="server" Text="Owner" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtOwner" runat="server" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMonth" runat="server" Text="Month" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtMonth" runat="server" CssClass="yellow"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFleetManager" runat="server" Text="Fleet Mgr" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtFM" runat="server" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTechSup" runat="server" Text="Tech Supt" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtTechSup" runat="server" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMarinsupt" runat="server" Text="Marine Supt" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtMarineSupt" runat="server" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblmaster" runat="server" Text="Master" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtMaster" runat="server" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCheng" runat="server" Text="C Engr" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtCheng" runat="server" CssClass="yellow"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblstatus" runat="server" Text="Status" CssClass="white"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblstatusname" runat="server" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblsection" runat="server" CssClass="white" Text="Section"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblnexturl" runat="server" CssClass="white"  Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblprvurl" runat="server" CssClass="white" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblcurrenturl" runat="server" CssClass="white" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblnextsectionid" runat="server" CssClass="white"  Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblprvsectionid" runat="server" CssClass="white" Visible="false"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblsectionname" runat="server" CssClass="yellow"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <button type="button" class="previous" id="btnpre" runat="server" onserverclick="cmdPrevious_Click" >&laquo; Previous Section</button>
                        <button type="button" class="next" id="btnnext" runat="server" onserverclick="cmdNext_Click" >Next Section &raquo;</button>
                    </td>
                    <td>
                        <button type="button" class="default" id="btndefault" runat="server" onserverclick="cmddefault_Click" >Set as Default Home Page</button>
                    </td>                    
                </tr>
            </table>
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="500px" Width="100%">
                <telerik:RadPane ID="navigationPane" runat="server" Width="20%" Height="80%">

                    <table>
                        <tr>
                            <td width="90%" valign="top">
                                <telerik:RadMonthYearPicker ID="ucDate" runat="server" OnSelectedDateChanged="ucDate_SelectedDateChanged" AutoPostBack="true" Width="70%" DateInput-DisplayDateFormat="MMM-yyyy"></telerik:RadMonthYearPicker>
                                 <asp:LinkButton runat="server" ID="lnkprevmonth" OnClick="lnkprevmonth_Click" Width="16px">
                                            <img src="../css/Theme1/images/previous.png" style="margin: 1%;vertical-align:middle;" title="Previous Month" />
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="lnknextmonth" OnClick="lnknextmonth_Click" Width="16px">
                                            <img src="../css/Theme1/images/next.png" style="margin: 1%;vertical-align:middle;" title="Next Month" />
                                </asp:LinkButton>
                            </td>
                            <td width="10%" valign="top">
                                <asp:LinkButton runat="server" ID="lnkPdf" OnClick="lnkPdf_Click">
                                            <img src="../css/Theme1/images/pdf.png" style="margin: 1%;vertical-align:middle;" title="Export PDF" />
                                </asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <telerik:RadGrid ID="gvSection" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="false"
                                    AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" CellSpacing="0" GridLines="None" EnableViewState="false" OnItemCommand="gvSection_ItemCommand"
                                    OnNeedDataSource="gvSection_NeedDataSource" OnItemDataBound="gvSection_ItemDataBound" OnColumnCreated="gvSection_ColumnCreated" OnItemCreated="gvSection_ItemCreated">
                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" DataKeyNames="FLDORSECTIONID">
                                        <GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldName="FLDORCATEGORYNAME" SortOrder="Ascending" />
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="FLDORCATORDER" SortOrder="Ascending" />
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions>
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Section" HeaderStyle-Width="160px" ItemStyle-Width="160px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#0070c0">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkname" runat="server" CommandName="LINK" Text='<%# DataBinder.Eval(Container, "DataItem.FLDORSECTIONNAME")%>'></asp:LinkButton>
                                                    <telerik:RadLabel ID="lblSectionId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDORSECTIONID"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblurl" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDURL"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSHORTCODE")%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings>
                                        <Resizing AllowColumnResize="true" />
                                        <Selecting AllowRowSelect="true" />
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                    </table>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar2" runat="server" CollapseMode="Forward" Height="100%">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="RadPane1" runat="server" Width="100%" Height="99.6%">
                    <div id="moreinfo" runat="server" style="width: 100%; height: 99.6%">
                        <iframe runat="server" id="ifMoreInfo" style="width: 100%; height: 99.6%" frameborder="0"></iframe>
                    </div>
                </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
