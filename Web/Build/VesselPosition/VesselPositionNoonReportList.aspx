<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionNoonReportList.aspx.cs" Inherits="VesselPositionNoonReportList" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Noon Report List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvNoonReport.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
    <style>
        .RadGrid .rgHeader, .RadGrid th.rgResizeCol {
            padding: 2px !important;
            font-size:8px !important;
            /*padding-right: 2px !important;*/
        }
        .rgview td
        {
            padding: 2px !important;
            font-size:smaller !important;
            /*padding-right: 2px !important;*/      
        }

    </style>
</head>
<body>
    <form id="frmNoonReport" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlNoonReportData" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlNoonReportData" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div>
                <asp:LinkButton runat="server" AlternateText="Edit" ID="ImgRed">
                                <span class="icon" style="color:darkred;"><i class="fas fa-exclamation"></i></span>
                </asp:LinkButton>
                <telerik:RadLabel ID="lblPassingthroughHRAnote" runat="server" Text="Passing through HRA within next 14 days"></telerik:RadLabel>

                <asp:LinkButton runat="server" AlternateText="Edit" ID="ImgYellow">
                                <span class="icon" style="color:orange;"><i class="fas fa-exclamation"></i></span>
                </asp:LinkButton>
                <telerik:RadLabel ID="lblCallingUSPortnote" runat="server" Text="Calling US Port within 7 days"></telerik:RadLabel>

                <asp:LinkButton runat="server" AlternateText="Edit" ID="ImgGreen">
                                <span class="icon" style="color:green;"><i class="fas fa-exclamation"></i></span>
                </asp:LinkButton>

                <telerik:RadLabel ID="lblEntryintoECAnote" runat="server" Text="Entry into ECA"></telerik:RadLabel>
            </div>
            <eluc:TabStrip ID="MenuNoonReportList" runat="server" OnTabStripCommand="NoonReportList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator ID="RadFormDecorator1" DecorationZoneID="gvNoonReport" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

            <telerik:RadGrid ID="gvNoonReport" RenderMode="Lightweight" runat="server" AutoGenerateColumns="False" GridLines="None"
                  OnItemCommand="gvNoonReport_RowCommand" OnItemDataBound="gvNoonReport_ItemDataBound"
                AllowSorting="true" OnNeedDataSource="gvNoonReport_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false" ShowFooter="false"
                OnSortCommand="gvNoonReport_SortCommand" ShowHeader="true" EnableViewState="false" AllowCustomPaging="true" AllowPaging="true" CssClass="rgview">

                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDNOONREPORTID">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Alert" HeaderStyle-Width="55px" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassingthroughHRA" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISPASSINGTHROUGHHRA") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCallingUSPort" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISUSWATERS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEntryintoECA" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDECAYN") %>'></telerik:RadLabel>

                                <asp:LinkButton runat="server" AlternateText="SentYN" CommandName="SENTYN" ID="ImgSentYN" Visible="false" ToolTip="Report Not Sent to Office">
                                <span class="icon" style="color:orangered;" ><i class="fas fa-exclamation"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="RangeAlert" CommandName="RANGEALERT" ID="ImgRangeAlert" Visible="false"
                                    ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDRANGEALERT") %>' >
                                <span class="icon" style="color:red;" ><i class="fas fa-exclamation-circle"></i></span>
                                </asp:LinkButton>
                                <b>
                                    <asp:LinkButton runat="server" AlternateText="PassingthroughHRA" CommandName="PASSINGTHROUGHHRA" ID="ImgSymPassingthroughHRA" Visible="false"
                                        ToolTip="Passing through HRA within next 14 days" >
                                <span class="icon" style="color:darkred;" ><i class="fas fa-exclamation"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="CallingUSPort" CommandName="CALLINGUSPORT" ID="ImgSymCallingUSPort" Visible="false"
                                        ToolTip="Calling US Port within 7 days">
                                <span class="icon" style="color:orange;" ><i class="fas fa-exclamation"></i></span>
                                    </asp:LinkButton>


                                    <asp:LinkButton runat="server" AlternateText="EntryintoECA" CommandName="ENTRYINTOECA" ID="ImgSymEntryintoECA" Visible="false"
                                        ToolTip="Entry into ECA">
                                <span class="icon" style="color:green;" ><i class="fas fa-exclamation"></i></span>
                                    </asp:LinkButton>

                                    <eluc:Tooltip ID="ucPassingthroughHRA" runat="server" Text="Passing through HRA within next 14 days" />
                                    <eluc:Tooltip ID="ucCallingUSPort" runat="server" Text="Calling US Port within 7 days" />
                                    <eluc:Tooltip ID="ucEntryintoECA" runat="server" Text="Entry into ECA" />
                                    <eluc:Tooltip ID="ucRangeAlert" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANGEALERT") %>' />

                                </b>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" HeaderText="Date" AllowSorting="true" SortExpression="FLDNOONREPORTDATE">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNoonReportID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOONREPORTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReportType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIdlYn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIDLYN") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkNoonReportID" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDNOONREPORTDATE")) %>'
                                    runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItem %>'></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Flag" CommandName="FLAG" ID="imgFlag" ToolTip="IDL Crossing" Visible="false">
                                <span class="icon"><i class="fas fa-exclamation-triangle"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="6%" HeaderText="Voyage No." HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVoyageNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOYAGENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="22px" HeaderText="B/L" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBallastYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALLASTYESNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblLati" runat="server" Text="Lat"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLat" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLAT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn >
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblLong" runat="server" Text="Long"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLog" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONG") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="5%" HeaderText="Wind Force" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWindForce" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWINDFORCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="At Port / Next Port" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNextPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATPORTNEXTPORT") %>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucNextPortNoonTip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATPORTNEXTPORT") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" HeaderText="ETA" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStartDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDETA")) + " " + DataBinder.Eval(Container,"DataItem.FLDETA", "{0:HH:mm}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="5%" HeaderText="ME RPM" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRPM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMERPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="5%" HeaderText="Slip" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSLIP" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSLIP") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="5%" HeaderText="Speed" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSpeedHidden" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHARTERSPEED") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSpeed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOGSPEED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="5%" HeaderText="C/P Speed" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCharterSpeed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHARTERSPEED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lbllCP" runat="server" Text="C/P"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCPSpeed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCPSPEED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="5%" HeaderText="FO Cons" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHFOCPHidden" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHFOCHARTERCOUNSUMPTION") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblHfoCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHFOOILCONSUMPTIONQTY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="5%" HeaderText="ME FO<br \>Cons" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMdoCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEFOOILCONSUMPTIONQTY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="5%" HeaderText="ME FO<br \>C/P" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHfoCP" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHFOCHARTERCOUNSUMPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="5%" HeaderText="ME DO Cons" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHMDOCPHidden" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMDOCHARTERCOUNSUMPTION") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDOCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMDOOILCONSUMPTIONQTY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="5%" HeaderText="DO C/P" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHMDCP" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMDOCHARTERCOUNSUMPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="5%" HeaderText="FO Cat<br \>Fines" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFOCatfine" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFOCATFINES") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblllCP" runat="server" Text="C/P"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCPHfoCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCPFUELOILCONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCPCons" runat="server" Text="C/P"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCPMdoCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCPDIESELOILCONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="5%" HeaderText="Oil Major<br \>Cargo" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle> 
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOilMajorCargoOnboard" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILMAJORCARGOONBOARDYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Oil Major" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOilMajor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILMAJOR") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="5%" HeaderText="Overdue" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOverDue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="7%" HeaderText="Reviewed<br \>By" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle> 
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblreviewedby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWEDBY") %>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucreviewedby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWEDBY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Reviewed<br \>Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle> 
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblreviewedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="60px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="REVIEW" ID="cmdReview" ToolTip="Review" Visible="false">
                                <span class="icon"><i class="fas fa-user-check-approved"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Reset" CommandName="RESET" ID="cmdReset" ToolTip="Reset" Visible="false">
                                <span class="icon"><i class="fas fa-redo"></i></span>
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
