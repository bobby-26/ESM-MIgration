<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreEvaluationEdit.aspx.cs" Inherits="CrewOffshoreEvaluationEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCandidateEvaluation" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="divContainer" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">


            <asp:Button runat="server" ID="confirm" OnClick="ucConfirm_Click" />


            <eluc:TabStrip ID="CrewMenuGeneral" Title="Evaluation Questions" runat="server" OnTabStripCommand="CrewMenuGeneral_TabStripCommand"></eluc:TabStrip>

            <div>
                <table width="100%">
                    <tr>

                        <td>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="First Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>

                        <td>
                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Middle Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>

                        <td>
                            <telerik:RadLabel ID="RadLabel3" runat="server" Text="Last Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <telerik:RadLabel ID="RadLabel4" runat="server" Text="Employee Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>

                        <td>
                            <telerik:RadLabel ID="RadLabel5" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="lblExpectedJoiningVessel" Visible="false" runat="server" Text="Expected Joining Vessel"></telerik:RadLabel>
                        </td>
                        <td colspan="2">
                            <eluc:Vessel ID="ddlVessel" runat="server" VesselsOnly="true" AppendDataBoundItems="true"
                                CssClass="input_mandatory" Visible="false" AutoPostBack="true" Width="150px" AssignedVessels="true" />
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank of the Person to be Proposed"></telerik:RadLabel>
                        </td>
                        <td colspan="2">
                            <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ddlRank_Changed"
                                CssClass="input_mandatory" Width="150px" />
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="lblExpectedJoiningDate" runat="server" Text="Expected Joining Date"></telerik:RadLabel>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtJoinDate" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <br />
                            <b>
                                <telerik:RadLabel ID="lblInterview" runat="server" Text="Questionnaire"></telerik:RadLabel>
                            </b>
                        </td>
                    </tr>
                </table>
                <%-- <asp:GridView ID="gvInterview" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnRowCommand="gvInterview_RowCommand" OnRowDataBound="gvInterview_RowDataBound"
                                OnRowCancelingEdit="gvInterview_RowCancelingEdit" OnRowUpdating="gvInterview_RowUpdating" OnRowEditing="gvInterview_RowEditing"
                                ShowFooter="false" EnableViewState="false">
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvInterview" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvInterview_NeedDataSource"
                    OnItemDataBound="gvInterview_ItemDataBound"
                    OnItemCommand="gvInterview_ItemCommand"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <HeaderStyle Width="102px" />

                        <Columns>

                            <telerik:GridTemplateColumn HeaderText="Question" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-VerticalAlign="Middle" HeaderStyle-Width="40%">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDTKey" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblQuestionId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblQuestion" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUESTION")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Answer" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="35%"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAnswerid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWERID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblAnswer" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWER") %>'></telerik:RadLabel>
                                    <asp:RadioButtonList ID="rblAnswerEdit" runat="server" RepeatDirection="Vertical" RepeatColumns="1"></asp:RadioButtonList>
                                </ItemTemplate>
                                <%--  <EditItemTemplate>
                                                <asp:RadioButtonList ID="rblAnswerEdit" runat="server" RepeatDirection="Horizontal" RepeatColumns="3"></asp:RadioButtonList>
                                            </EditItemTemplate>--%>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Score" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSCORE")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRemakrs" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtRemarksEdit" runat="server" CssClass="input" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREMARKS") %>'></telerik:RadTextBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
                <table>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" Width="500Px">
                            </asp:RadioButtonList>
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="RadLabel6" runat="server" Text="Daily Rate(USD)"></telerik:RadLabel>
                        </td>
                        <td colspan="2">
                            <%--<telerik:RadTextBox ID="txtSalAgreed" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>--%>
                            <eluc:Number ID="txtSalAgreed" runat="server" CssClass="input" MaxLength="12"
                                DecimalPlace="2" />
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="lblAssessmentEvaluationbySuperintendentName" runat="server" Text="Interviewer Name"></telerik:RadLabel>
                        </td>
                        <td colspan="2">
                            <telerik:RadTextBox ID="txtSuperintendentName" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="lblDate" runat="server" Text="Interview Date"></telerik:RadLabel>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text="Comments"></telerik:RadLabel>
                        </td>
                        <td colspan="2">
                            <telerik:RadTextBox ID="txtRemaks" runat="server" TextMode="MultiLine" Width="400px" Height="30px"
                                CssClass="input_mandatory">
                            </telerik:RadTextBox>
                        </td>
                        <td colspan="2"></td>
                    </tr>

                </table>
                <asp:Panel ID="pnlgminterview" Width="100%" runat="server">
                    <b>
                        <telerik:RadLabel ID="Literal1" runat="server" Text="Head Office Questionnaire"></telerik:RadLabel>
                    </b>

                    <%--<asp:GridView ID="gvGmInterview" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="3" OnRowCommand="gvGmInterview_RowCommand" OnRowDataBound="gvGmInterview_RowDataBound"
                                    OnRowCancelingEdit="gvGmInterview_RowCancelingEdit" OnRowUpdating="gvGmInterview_RowUpdating" OnRowEditing="gvGmInterview_RowEditing"
                                    ShowFooter="false" EnableViewState="false">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                    <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvGmInterview" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvGmInterview_NeedDataSource"
                        OnItemDataBound="gvGmInterview_ItemDataBound"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <HeaderStyle Width="102px" />
                            <Columns>

                                <telerik:GridTemplateColumn HeaderText="Question" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40%"
                                    HeaderStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDTKey" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblQuestionId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblQuestion" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUESTION")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Answer" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="35%"
                                    HeaderStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAnswerid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWERID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblAnswer" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWER") %>'></telerik:RadLabel>
                                        <asp:RadioButtonList ID="rblAnswerEdit" runat="server" RepeatDirection="Vertical" RepeatColumns="1"></asp:RadioButtonList>
                                    </ItemTemplate>
                                    <%--  <EditItemTemplate>
                                                <asp:RadioButtonList ID="rblAnswerEdit" runat="server" RepeatDirection="Horizontal" RepeatColumns="3"></asp:RadioButtonList>
                                            </EditItemTemplate>--%>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Score" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%"
                                    HeaderStyle-VerticalAlign="Middle">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSCORE")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRemakrs" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                        <telerik:RadTextBox ID="txtRemarksEdit" runat="server" CssClass="input" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREMARKS") %>'></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                    <table>
                        <tr>
                            <td colspan="2">
                                <telerik:RadLabel ID="Literal2" runat="server" Text="GM Status"></telerik:RadLabel>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblgmstatus" runat="server" RepeatDirection="Horizontal" Width="500Px">
                                    <asp:ListItem Value="3">Rejected</asp:ListItem>
                                    <asp:ListItem Value="5">Approved</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <telerik:RadLabel ID="Literal3" runat="server" Text="GM Name"></telerik:RadLabel>
                            </td>
                            <td colspan="2">
                                <telerik:RadTextBox ID="txtgmname" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                            </td>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <telerik:RadLabel ID="Literal4" runat="server" Text="GM Interview Date"></telerik:RadLabel>
                            </td>
                            <td colspan="2">
                                <eluc:Date ID="txtgminterviewdate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <telerik:RadLabel ID="Literal5" runat="server" Text="GM Comments"></telerik:RadLabel>
                            </td>
                            <td colspan="2">
                                <telerik:RadTextBox ID="txtgmcomment" runat="server" TextMode="MultiLine" Width="400px" Height="30px"
                                    CssClass="input_mandatory">
                                </telerik:RadTextBox>
                            </td>
                            <td colspan="2"></td>
                        </tr>

                    </table>

                </asp:Panel>
                <b><telerik:RadLabel ID="dochead" runat="server" Visible="false" Text="Document status as on date:"></telerik:RadLabel></b>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvinterviewdocument" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" EnableViewState="false" Visible="false" OnNeedDataSource="gvinterviewdocument_NeedDataSource"
                    OnItemDataBound="gvinterviewdocument_ItemDataBound"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <HeaderStyle Width="102px" />

                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Document Type" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbldoctype" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Document Name/No" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbldocname" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Date of Issue" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbldocissue" runat="server" Text='<%# General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDISSUEDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Date of Expiry" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbldocexpire" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                              <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbldocstatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>


                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

            </div>

            <eluc:Status ID="ucStatus" runat="server" />
        </div>
        <%--<eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="ucConfirm_Click" OKText="Ok"
            CancelText="Cancel" Visible="false" />--%>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Ok" Localization-Cancel="Cancel" Width="100%"></telerik:RadWindowManager>

    </form>
</body>
</html>
