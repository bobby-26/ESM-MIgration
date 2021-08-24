<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPromotionDemotion.aspx.cs"
    Inherits="CrewPromotionDemotion" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Promotion</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
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
    </telerik:RadCodeBlock>
    <style type="text/css">
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }

        .checkRtl {
            direction: rtl;
        }

        .fon {
            font-size: small !important;
        }
    </style>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewList" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmSignOn" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:TabStrip ID="MenuSignOn" runat="server" Title="Promotion/Demotion" OnTabStripCommand="CrewSignOn_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%" CssClass="scrolpan">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="99%">
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblEmployeeCode" Text="File No." runat="server"></telerik:RadLabel>

                    </td>
                    <td style="width: 20%">
                        <telerik:RadTextBox ID="txtEmployeeCode" runat="server" Width="180px" CssClass="readonlytextbox" ReadOnly="True" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td style="width: 11%">
                        <telerik:RadLabel ID="lblEmployeeName" Text="Employee" runat="server"></telerik:RadLabel>

                    </td>
                    <td style="width: 20%">
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" CssClass="readonlytextbox" ReadOnly="True" Enabled="false"
                            Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblPresentRank" Text="Present Rank" runat="server"></telerik:RadLabel>

                    </td>
                    <td style="width: 18%">
                        <telerik:RadTextBox ID="txtPresentRank" runat="server" CssClass="readonlytextbox" Width="180px" ReadOnly="True" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" Text="Vessel" runat="server"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="True" Width="180px" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignedOn" Text="Signed On" runat="server"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSignedOn" runat="server" CssClass="readonlytextbox" ReadOnly="True" Width="180px" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReliefDue" Text="Relief Due" runat="server"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtReliefDue" CssClass="readonlytextbox" ReadOnly="True" Width="180px" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPromotionGrade" Text="Promotion Grade" runat="server"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPromotionGrade" CssClass="readonlytextbox" ReadOnly="True" Width="180px" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBToD" Text="BToD" runat="server"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtBtod" CssClass="readonlytextbox" Width="180px" ReadOnly="True" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEToD" Text="EToD" runat="server"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEtod" CssClass="readonlytextbox" Width="180px" ReadOnly="True" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastActivity" Text="Last Activity" runat="server"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtActivity" CssClass="readonlytextbox" Width="180px" ReadOnly="True" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" Text="From Date" runat="server"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFromDate" CssClass="readonlytextbox" Width="180px" ReadOnly="True" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" Text="To Date" runat="server"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtToDate" CssClass="readonlytextbox" Width="180px" ReadOnly="True" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPromotedDemotedRank" Text="Promoted/Demoted Rank" runat="server"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" CssClass="input" ID="ucRank" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select Category" Filter="Contains" Width="180px" AppendDataBoundItems="true" InputCssClass="dropdown_mandatory"
                            AutoPostBack="true" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td colspan="2">
                        <asp:CheckBox ID="chkShowAllRank" runat="server" Text="Show All Ranks" AutoPostBack="true"
                            OnCheckedChanged="OnSelectRankChk" />
                        <asp:CheckBox ID="chkShowDemotedRank" runat="server" Text="Demotion" AutoPostBack="true"
                            OnCheckedChanged="OnSelectRankChk" /></td>

                    <td>
                        <telerik:RadLabel ID="lblRemarks" Text="Remarks" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" Width="180px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPromotionDemotionDate" Text="Promotion/Demotion On" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtPromotionDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPromDem" runat="server" OnItemDataBound="gvPromDem_ItemDataBound"
                AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvPromDem_NeedDataSource" OnItemCommand="gvPromDem_ItemCommand" EnableHeaderContextMenu="true" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Rank" Name="Rank" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No" AllowSorting="true" ShowSortIcon="true">
                            <HeaderStyle Width="4%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%#Container.DataSetIndex+1 %>
                                <telerik:RadLabel runat="server" ID="lbldtkey" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblEmployeeid" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To" AllowSorting="true" ShowSortIcon="true" ColumnGroupName="Rank">
                            <HeaderStyle Width="13%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankTo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKTO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From" ColumnGroupName="Rank">
                            <HeaderStyle Width="13%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKFROM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Promotion / Demotion On" ColumnGroupName="Rank">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="By" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="12%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPromotedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROMOTEDDEMOTEDBY") %>'></telerik:RadLabel>
                                <b>
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="On : "></telerik:RadLabel>
                                </b>
                                <telerik:RadLabel ID="lblPDDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="8%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSDESC") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="15%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action">
                            <HeaderStyle Width="7%" HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Comments" CommandName="Comment" ID="cmdComment" ToolTip="Comments">
                                <span class="icon"><i class="fas fa-comments"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="Attachment" ID="cmdAtt" ToolTip="Attachment">
                                <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <%-- <asp:ImageButton runat="server" AlternateText="Comment" ImageUrl='<%$ PhoenixTheme:images/communication.png%>'
                                    CommandName="Comment" ID="cmdComment"
                                    ToolTip="Save"></asp:ImageButton>--%>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="smddelete" ToolTip="Cancel">
                                <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">

                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="false" SaveScrollPosition="false" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
