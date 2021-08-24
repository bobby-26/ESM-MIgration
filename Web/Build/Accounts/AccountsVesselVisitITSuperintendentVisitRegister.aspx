<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselVisitITSuperintendentVisitRegister.aspx.cs" Inherits="AccountsVesselVisitITSuperintendentVisitRegister" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Visit Register</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .hidden {
                display: none;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVisitRegister" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" ></telerik:RadSkinManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:TabStrip ID="MenuVisitMain" runat="server" OnTabStripCommand="MenuVisitMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                CssClass="hidden" />
            <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="height: 56%; width: 99.6%;"></iframe>
            <eluc:TabStrip ID="MenuVisit" runat="server" OnTabStripCommand="MenuVisit_TabStripCommand"></eluc:TabStrip>
            <asp:HiddenField ID="hdnScroll" runat="server" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVisit" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                GridLines="None" Width="100%" Height="34%" CellPadding="3" OnItemDataBound="gvVisit_ItemDataBound" ShowHeader="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                AllowSorting="true" OnSorting="gvVisit_Sorting" OnItemCommand="gvVisit_ItemCommand" OnNeedDataSource="gvVisit_NeedDataSource" EnableViewState="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDVISITID">
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
                        <telerik:GridTemplateColumn HeaderText="Employee Id" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVisitId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVISITID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkEmployeeId" runat="server" CommandName="Select" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:LinkButton>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Employee Name" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipEmpname" TargetControlId="lblEmployeeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEFULLNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Visit Start" AllowSorting="true" SortExpression="FLDFROMDATE" ShowSortIcon="true" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Visit End" AllowSorting="true" SortExpression="FLDTODATE" ShowSortIcon="true" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblToDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTODATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name" AllowSorting="true" SortExpression="FLDVESSELNAME" ShowSortIcon="true" HeaderStyle-Width="9%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Visit Status" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvisitstaus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVISITSTATUS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELLATIONREMARKS") %>' />
                                <telerik:RadLabel ID="lblclaimamount" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLAIMAMOUNT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblreimbursementamount" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREIMBURSEMENTAMOUNT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblreimbursementcurrency" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREIMBURSEMENTCURRENCY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Claims Status" HeaderStyle-Width="12%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClaims" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLAIMSTATUS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblClaimstatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLAIMSHORTCODE") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVisitType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVISITTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExpenseTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPENSETYPECODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Form No" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDFORMNUMBER" FilterControlWidth="12%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblForm" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PIC Name" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPICName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAME") %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipPIC" TargetControlId="lblPICName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICFULLNAME")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="15%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="SELECT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                    ID="cmdApprove" ToolTip="Approve" CommandName="Approve" CommandArgument='<%# Container.DataItem %>'></asp:ImageButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />

                                <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                    CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataItem %>' ID="cmdNoAttachment"
                                    ToolTip="No Attachment" Visible="false"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="Attachment" CommandArgument='<%# Container.DataItem %>' ID="cmdAttachment"
                                    ToolTip="Attachment"></asp:ImageButton>

                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Generate PDF" ImageUrl="<%$ PhoenixTheme:images/pdf.png %>"
                                    CommandName="GENERATE" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdGenPdf" ToolTip="Generate PDF"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Approval History" ImageUrl="<%$ PhoenixTheme:images/document_view.png %>"
                                    CommandName="ARROVALHISTORY" CommandArgument='<%# Container.DataItem %>'
                                    ID="imbApprovalHistory" ToolTip="Approval History"></asp:ImageButton>
                                <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />

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
