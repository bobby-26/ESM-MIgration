<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionProsperReports.aspx.cs" Inherits="Inspection_InspectionProsperReports" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCRank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCVesseltype" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCPool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCLQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCOfficerlist" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="num" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Performance Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript">
            function checkTextAreaMaxLength(textBox, e, length) {

                var mLen = textBox["MaxLength"];
                if (null == mLen)
                    mLen = length;

                var maxLength = parseInt(mLen);
                if (!checkSpecialKeys(e)) {
                    if (textBox.value.length > maxLength - 1) {
                        if (window.event)//IE
                            e.returnValue = false;
                        else//Firefox
                            e.preventDefault();
                    }
                }
            }

            function checkSpecialKeys(e) {
                if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                    return false;
                else
                    return true;
            }
        </script>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvProspercomplist.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
               fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmprosper" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Performance Report" Visible="false" ShowMenu="true" />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr align="left">
                    <td>
                        <telerik:RadLabel ID="lblrank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCRank ID="ddlrank" Width="200" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbllevel" runat="server" Text="Level"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlLevel" runat="server" Width="200"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr align="left">
                    <td>
                        <telerik:RadLabel ID="lblemployeename" runat="server" Text="Employee Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtname" runat="server" CssClass="input" Width="200"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="Literal1" runat="server" Text="Performance Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlcategory" runat="server" Width="200"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr align="left">
                    <td>
                        <telerik:RadLabel ID="lblvesseltype" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlvesseltype" runat="server" Width="200"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="Literal2" runat="server" Text="Score Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <eluc:num ID="txtminscore" runat="server" MaxLength="6" IsInteger="true" IsPositive="false" CssClass="input" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="AND"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:num ID="txtmaxscore" runat="server" MaxLength="6" IsInteger="true" IsPositive="false" CssClass="input" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr align="left">
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Card Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlcardstatus" runat="server" Width="200"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="ProsperMenu" runat="server" OnTabStripCommand="Prosper_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvProspercomplist" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowHeader="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnItemDataBound="gvProspercomplist_ItemDataBound" AllowPaging="true" AllowCustomPaging="true"
                DataKeyNames="FLDCYCLEID" OnNeedDataSource="gvProspercomplist_NeedDataSource"
                OnItemCommand="gvProspercomplist_ItemCommand" AllowSorting="true" OnSorting="gvProspercomplist_Sorting">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table width="99.9%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="File No">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDFILENO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="170px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblcycleid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCYCLEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <%--<telerik:RadLabel ID="lblfamilyid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'></telerik:RadLabel>--%>
                                <asp:LinkButton ID="lnkEmployeeid" runat="server" CommandName="GETEMPLOYEE" CommandArgument="<%# Container.DataItem %>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblcycleid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCYCLEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkEmployeeid1" runat="server" CommandArgument="<%# Container.DataItem %>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>'></asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblrank" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From" HeaderStyle-Width="150px" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCYCLESTARTDATE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblfrom" runat="server" Visible="true" Text=' <%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCYCLESTARTDATE"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtfromdateedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCYCLESTARTDATE") %>' CssClass="input"></eluc:Date>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To" HeaderStyle-Width="150px" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCYCLEENDDATE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblto" runat="server" Visible="true" Text=' <%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCYCLEENDDATE"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txttodateedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCYCLEENDDATE") %>' CssClass="input"></eluc:Date>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total No.of Vessel in This Cycle">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDVESSELINCYCLE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Score In This Cycle">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkmeasureName" runat="server" CommandArgument="<%#Container.DataItem%>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTiplastmodify" runat="server" Text='<%# "Modified On: " +DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE","{0:dd/MMM/yyyy}") %>' />

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total No.of Vessel Till Date">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDTOTALVESSEL")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Historical Average Till Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDHISTORY")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Card Status">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCARDSTATUS")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="85px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Attachment" ID="cmdXAtt" ToolTip="Attachment" CommandName="Attachment" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="Edit" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <%--<img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />--%>
                                <asp:LinkButton runat="server" Visible="false" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Navigate" CommandName="NAVIGATE" ID="cmdnavigate"
                                    ToolTip="View Score">
                                     <span class="icon"><i class="fas fa-receipt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Refresh"
                                    CommandName="REFRESH" ID="ImageButton1" ToolTip="Update Score">
                                    <span class="icon"><i class="fas fa-sync-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

