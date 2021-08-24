<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAProcessList.aspx.cs"
    Inherits="InspectionRAProcessList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Assessment Process</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            }
            function ConfirmIssue(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmIssue.UniqueID %>", "");
                }
            }
            function ConfirmRevision(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmRevision.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" Localization-OK="Yes" Localization-Cancel="No" Width="100%">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRefNo" runat="server" Text="Ref Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtHazardNo" runat="server" Width="250px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCategory" runat="server" AppendDataBoundItems="true" Width="250px"
                            AutoPostBack="True" DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" OnSelectedIndexChanged="ddlCategory_Changed" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlStatus" runat="server" AppendDataBoundItems="true" AutoPostBack="true" Width="250px"
                            OnSelectedIndexChanged="ddlStatus_Changed" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="1" Text="Draft" />
                                <telerik:RadComboBoxItem Value="2" Text="Approved" />
                                <telerik:RadComboBoxItem Value="3" Text="Issued" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuProcess" runat="server" OnTabStripCommand="MenuProcess_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvRiskAssessmentProcess" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" ShowHeader="true" EnableViewState="true"
                OnItemDataBound="gvRiskAssessmentProcess_ItemDataBound" OnItemCommand="gvRiskAssessmentProcess_ItemCommand" OnNeedDataSource="gvRiskAssessmentProcess_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDRISKASSESSMENTPROCESSID">
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
                        <telerik:GridTemplateColumn HeaderText="Ref Number">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReferencid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEID")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInstallcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTALLCODE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date">
                            <HeaderStyle HorizontalAlign="Left" Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"]) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle HorizontalAlign="Left" Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <HeaderStyle HorizontalAlign="Left" Width="17%" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProcessName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSNAME")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process">
                            <HeaderStyle HorizontalAlign="Left" Width="17%" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobActivity" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDJOBACTIVITY").ToString()%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Risks/Aspects" Visible="false">
                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRiskAssessmentProcessID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTPROCESSID")  %>'
                                    Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWorkActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKACTIVITY")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle HorizontalAlign="Left" Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatusID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblActiveyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rev No">
                            <HeaderStyle HorizontalAlign="Left" Width="7%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issued Date">
                            <HeaderStyle HorizontalAlign="Left" Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDISSUEDDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Edited By" Visible="false">
                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEditedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEDITEDBY")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" Width="14%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDITROW" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Approve" CommandName="APPROVE" ID="imgApprove" ToolTip="Approve">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Issue" CommandName="ISSUE" ID="imgIssue" ToolTip="Issue">
                                    <span class="icon"><i class="fas fa-check-circle"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Revision" CommandName="REVISION" ID="imgrevision" ToolTip="Revision">
                                    <span class="icon"><i class="fas fa-registered"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Process PDF" CommandName="RAPROCESS" ID="cmdRAProcess" ToolTip="Show PDF">
                                    <span class="icon"><i class="fas fa-chart-bar"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Revision" CommandName="VIEWREVISION" ID="cmdRevisions" ToolTip="View Revisions">
                                    <span class="icon"><i class="fas fa-eye"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Copy" CommandName="COPY" ID="imgCopy" ToolTip="Copy">
                                    <span class="icon"><i class="fas fa-copy"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Copy JHA to Company" CommandName="OFFICECOPY" ID="imgOfficeCopy" ToolTip="Copy JHA to Company">
                                    <span class="icon"><i class="fas fa-file-import"></i></span>
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
            <asp:Button ID="ucConfirm" runat="server" OnClick="ucConfirm_Click" CssClass="hidden" />
            <asp:Button ID="ucConfirmIssue" runat="server" OnClick="ucConfirmIssue_Click" CssClass="hidden" />
            <asp:Button ID="ucConfirmRevision" runat="server" OnClick="ucConfirmRevision_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
