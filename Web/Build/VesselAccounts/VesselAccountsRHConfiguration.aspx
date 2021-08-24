<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsRHConfiguration.aspx.cs"
    MaintainScrollPositionOnPostback="true" Inherits="VesselAccountsRHConfiguration" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rest Hour Start</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function pageLoad() {
                PaneResized();
                fade('statusmessage');
            }
            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvCrewList");
                grid._gridDataDiv.style.height = (browserHeight - 250) + "px";
            }
        </script>
        <script type="text/javascript">
            function ConfirmRankReset(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmRankReset.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmRestHourCrewList" runat="server" height="100%">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuRHGeneral" runat="server" OnTabStripCommand="RHGeneral_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <asp:Button ID="ucConfirmRankReset" runat="server" OnClick="ucConfirmRankReset_Click" CssClass="hidden" />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblnote" runat="server" EnableViewState="false" Text="Notes:" Font-Bold="true" ForeColor="Blue"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbl1" runat="server" Text="1." ForeColor="Blue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToAdd" runat="server" Text="To Add a Crew member to the crew list click '" ForeColor="Blue"></telerik:RadLabel>
                        <i class="fa fa-plus-circle"></i>
                        <telerik:RadLabel ID="lblselect" runat="server" Text="' select the crew member and click '" ForeColor="Blue"></telerik:RadLabel>
                        <i class="fa fa-plus-circle"></i>
                        <telerik:RadLabel ID="lblintheaction" runat="server"
                            Text="' in the action column.Do this for all crew members to be added." ForeColor="Blue">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lbl2" runat="server" Text="2." ForeColor="Blue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToAddaCrewmember" runat="server" Text="To Add a Crew member who are not available on the crew list, insert their name in the red box in the bottom line, select ther rank from the dropdown list insert the signon date and Relief Due date" ForeColor="Blue"></telerik:RadLabel>
                        <telerik:RadLabel ID="lbltxt" runat="server" Text=" and click '" ForeColor="Blue"></telerik:RadLabel>
                        <i class="fas fa-plus-circle"></i>
                        <telerik:RadLabel ID="lblintheactioncolumn" runat="server" Text="' in the action column." ForeColor="Blue"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbl3" runat="server" Text="3." ForeColor="Blue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblclicking" runat="server" Text="Edit each by clicking '" ForeColor="Blue"></telerik:RadLabel>
                        <i class="fas fa-edit"></i>
                        <telerik:RadLabel ID="lblifnecessary"
                            runat="server" Text="' and Change the file number if necessary." ForeColor="Blue">
                        </telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <table width="30%" cellpadding="1px" cellspacing="1px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShowOnly" runat="server" Text="Show Only"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlactive" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlactive_OnSelectedIndexChanged" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Text=" Active " Value="1" Selected="True" />
                                <telerik:RadComboBoxItem Text="In-Active " Value="0" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuCrewList" runat="server" OnTabStripCommand="CrewList_TabStripCommand" CssClass="hidden"></eluc:TabStrip>
            <telerik:RadGrid ID="gvCrewList" runat="server" AutoGenerateColumns="False" Width="100%" OnNeedDataSource="gvCrewList_NeedDataSource"
                CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemDataBound="gvCrewList_ItemDataBound" OnItemCommand="gvCrewList_ItemCommand"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellPadding="3" ShowHeader="true" ShowFooter="false" EnableViewState="false" OnSortCommand="gvCrewList_SortCommand" OnUpdateCommand="gvCrewList_UpdateCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDRESTHOUREMPLOYEEID">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Sr.No">
                            <HeaderStyle HorizontalAlign="Left" Width="5%" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDROWNUMBER")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle HorizontalAlign="Left" Width="25%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblrhempid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESTHOUREMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRHstartid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESTHOURSTARTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmpId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblShipCalendarId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPCALENDARID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblGot" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGTOVERTIME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAllowYN" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOWYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtEmpNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No">
                            <HeaderStyle HorizontalAlign="Left" Width="8%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'
                                    Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtFileNoAdd" runat="server" CssClass="input" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle HorizontalAlign="Left" Width="23%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblrankid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Rank ID="ucRankAdd" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    RankList='<%# PhoenixRegistersRank.ListRank()%>' Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="SignOn Date">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignondate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucSignonDateAdd" runat="server" CssClass="input_mandatory" DatePicker="true" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Relief Due Date">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE", "{0:dd/MMM/yyyy}")%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucReliefDueDateAdd" runat="server" CssClass="input_mandatory" DatePicker="true" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Start Date" Visible="false">
                            <HeaderStyle Width="0px" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStartDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTARTDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListCalendar">
                                    <telerik:RadTextBox ID="ucStartDate" runat="server" Width="130px" CssClass="readonlytextbox"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTARTDATE", "{0:dd/MMM/yyyy}")%>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton runat="server" ID="cmdShowCalender" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." />
                                    <telerik:RadTextBox ID="txtShipCalendarId" runat="server" Width="1px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPCALENDARID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User Name" Visible="false">
                            <HeaderStyle Width="0px" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbluserid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container, "DataItem.FLDUSERNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlUserNameEdit" runat="server" AppendDataBoundItems="true"
                                    CssClass="dropdown_mandatory" DataTextField="FLDUSERNAME" DataValueField="FLDUSERCODE" Filter="Contains">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Watch Keeper" Visible="false">
                            <HeaderStyle Width="0px" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblwatchkeeper" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWATCHKEEPER").ToString().Equals("1")) ? "Yes" : "No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlWatchKeeping" runat="server" CssClass="input" DataTextField="FLDITEM"
                                    DataValueField="FLDROW" Filter="Contains">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ActiveYN">
                            <HeaderStyle Width="8%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblactiveyn" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDACTIVEYN").ToString().Equals("1")) ? "Yes" : "No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblactiveynEdit" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDACTIVEYN"))%>' Visible="false"></telerik:RadLabel>
                                <telerik:RadCheckBox ID="chkactiveyn" runat="server" Checked='<%# (DataBinder.Eval(Container, "DataItem.FLDACTIVEYN").ToString().Equals("1")) ? true : false %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                            <ItemStyle HorizontalAlign="Center" Wrap="False" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit" Visible="false">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Reset" Visible="false" CommandName="WORKHOURS" ID="cmdWorkHours" ToolTip="Scheduled Work Hours">
                                    <span class="icon"><i class="fas fa-clock"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Attendance" CommandName="CREWATTENDANCE" ID="cmdCrewAttendance" ToolTip="Work and Rest Hour Records">
                                    <span class="icon"><i class="fas fa-user-clock"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Reset" CommandName="RESET" ID="cmdReset" ToolTip="Reset">
                                    <span class="icon"><i class="fas fa-redo-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Map" Visible="false" CommandName="MAP" ID="cmdMap" ToolTip="Edit">
                                    <%--<span class="icon"><i class="fas fa-edit"></i></span>--%>
                                </asp:LinkButton>
                                <%--<asp:ImageButton runat="server" AlternateText="Reset" ImageUrl="<%$ PhoenixTheme:images/refresh.png%>"
                                        CommandName="RESET" ID="cmdRankReset" ToolTip="Reset" ImageAlign="Top">
                                </asp:ImageButton>--%>
                                <asp:LinkButton runat="server" AlternateText="CR6B Report" CommandName="CR6BREPORT" ID="cmdReport" ToolTip="CR6B Report">
                                    <span class="icon"><i class="fas fa-chart-bar"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="CR6C Report" CommandName="CR6CREPORT" ID="cmdCR6CReport" ToolTip="CR6C Report" Visible="false">
                                    <span class="icon"><i class="fas fa-chart-bar"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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
