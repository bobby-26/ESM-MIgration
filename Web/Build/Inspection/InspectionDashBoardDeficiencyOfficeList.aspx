<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDashBoardDeficiencyOfficeList.aspx.cs" Inherits="InspectionDashBoardDeficiencyOfficeList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplitter" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvDeficiency.ClientID %>"));
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
    <form id="frmDeficiency" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Status ID="ucStatus" runat="server" />
            <%--<eluc:TabStrip ID="MenuDeficiencyGeneral" TabStrip="true" runat="server" OnTabStripCommand="DeficiencyGeneral_TabStripCommand"></eluc:TabStrip>--%>
            <eluc:TabStrip ID="MenuDeficiency" runat="server" OnTabStripCommand="Deficiency_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvDeficiency" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" CellPadding ="3" ShowHeader="true" EnableViewState="false"
                EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvDeficiency_ItemCommand" OnItemDataBound="gvDeficiency_ItemDataBound"
                OnSelectedIndexChanging="gvDeficiency_SelectIndexChanging" OnNeedDataSource="gvDeficiency_NeedDataSource" OnSortCommand="gvDeficiency_SortCommand"
                OnDeleteCommand="gvDeficiency_DeleteCommand" OnPreRender="gvDeficiency_PreRender">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDEFICIENCYID">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="RCA" Name="RCA" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Width="4%" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <HeaderTemplate>
                                <telerik:RadCheckBox ID="chkAllRemittance" runat="server" Text="" AutoPostBack="true"
                                    OnPreRender="CheckAll" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" runat="server" OnCheckedChanged="SaveCheckedValues" AutoPostBack="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn HeaderText="">
                            <HeaderStyle Width="4%" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" Screen='<%# "Inspection/InspectionToolTipDeficiency.aspx?deficiencyid=" + DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYID").ToString() + "&vesselid=" + DataBinder.Eval(Container,"DataItem.FLDVESSELID").ToString()%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Ref. No" AllowSorting="true" SortExpression="FLDREFERENCENUMBER">
                            <HeaderStyle Width="13%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDeficiencyid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRaisedfrom" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAISEDFROM") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSourceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDeficiencyType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text="0"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkRefNumber" runat="server" CommandName="Select" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Company">
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYSHORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issued" AllowSorting="true" SortExpression="FLDISSUEDDATE">
                            <HeaderStyle Width="9%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssuedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Source">
                            <HeaderStyle Width="11.5%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSource" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Source Ref">
                            <HeaderStyle Width="11.5%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSourceRefNo" CommandName="SHOWSOURCE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCEREFERENCENUMBER").ToString()%>'></asp:LinkButton>
                                <%--<eluc:ToolTip ID="ucToolTipInspectionRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCEREFERENCENUMBER") %>' TargetControlId="lnkSourceRefNo" />--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle Width="7%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNonConformanceType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Deficiency Category">
                            <HeaderStyle Width="12%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDeficiencyCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Chapter">
                            <HeaderStyle Width="15%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChapter" Width="150px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHAPTERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PSC Code / VIR Condition">
                            <HeaderStyle Width="8%" HorizontalAlign="Left" Wrap="true" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDKEYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Req" ColumnGroupName="RCA">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkRCAreqd" runat="server" Enabled="false" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDISRCAREQUIRED").ToString().Equals("1")?true:false %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comp" ColumnGroupName="RCA">
                            <HeaderStyle Width="6%" HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkRCAcompleted" runat="server" Enabled="false" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDISRCACOMPLETED").ToString().Equals("1")?true:false %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="7%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="14%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit" Visible="false">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="CANCELDEFICIENCY" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAtt" ToolTip="Attachment">
                                    <%--<span class="icon"><i class="fas fa-paperclip"></i></span>--%>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Corrective Actions" CommandName="CORRECTIVEACTIONS" ID="cmdCorrectiveActions" Visible="false" ToolTip="View Corrective and Preventive Actions">
                                    <span class="icon"><i class="fas fa-eye"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Deficiency Report" CommandName="DEFICIENCYREPORT"
                                    ID="cmdDeficiencyReport" ToolTip="Deficiency Report">
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
