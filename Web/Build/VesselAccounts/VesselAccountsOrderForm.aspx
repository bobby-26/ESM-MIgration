<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsOrderForm.aspx.cs"
    Inherits="VesselAccountsOrderForm" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="../UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function ConfirmSend(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmSent.UniqueID %>", "");
                }
            }
            function ConfirmSendtooffice(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmSenttoOffice.UniqueID %>", "");
                }
            }
            function ConfirmPOCreate(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmPoCreate.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }

        .fon {
            font-size: small !important;
        }
    </style>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmBondReq" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmBondReq" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="90%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRequestNo" runat="server" Text="Order No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRefNo" MaxLength="50" CssClass="input" Width="180px"></telerik:RadTextBox>
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="400px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true" CssClass="fon"
                            HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true" ContentScrolling="Auto"
                            Text="">
                            <font color="blue"><b>
                                <telerik:RadLabel ID="lblNote" runat="server" Text="Note:"></telerik:RadLabel>
                            </b>
                                <telerik:RadLabel ID="lblForembedded" runat="server" Text="For embedded search, use '%' symbol. (Eg. Order No.: %xxxx)"></telerik:RadLabel>
                            </font>
                        </telerik:RadToolTip>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="Order Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                        <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOrderStatus" runat="server" Text="Order Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" CssClass="input"
                            HardTypeCode="41" ShortNameFilter="PEN,RCD,CNL,QTD,ORD" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuBondReq" runat="server" OnTabStripCommand="MenuBondReq_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvBondReq" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvBondReq_ItemCommand" OnItemDataBound="gvBondReq_ItemDataBound" EnableHeaderContextMenu="true"
                ShowFooter="false" ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvBondReq_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="">
                            <HeaderStyle Width="5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgReqSent" runat="server" ImageUrl="<%$ PhoenixTheme:images/14.png %>"
                                    Visible="false" />
                                <asp:Image ID="imgReqSenttoARC" runat="server" ImageUrl="<%$ PhoenixTheme:images/green.png %>"
                                    Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order No.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDORDERID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDDTKEY"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDVESSELID"] %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="SELECT"><%# ((DataRowView)Container.DataItem)["FLDREFERENCENO"] %></asp:LinkButton>
                                <telerik:RadLabel ID="lblPurchaseOrderId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDPURCHASEORDERID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblNewProcess" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDNEWPROCESSYN"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsStockYN" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDISSTOCKYN"] %>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order On">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDORDERDATE", "{0:dd/MMM/yyyy}"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Status">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDORDERSTATUS"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received On">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRECEIVEDDATE", "{0:dd/MMM/yyyy}"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Form No." UniqueName="POFORMNO">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDPURCHASEFORMNO"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Form Status" UniqueName="POFORMSTATUS">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDPURCHASEFORMSTATUS"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="SELECT" ID="cmdDetails" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Copy" CommandName="COPY" ID="cmdCopy" ToolTip="Copy Requisition">
                                    <span class="icon"><i class="fas fa-copy"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="Attachment" ID="cmdAtt" ToolTip="Attachment"><span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Order Cancel">
                                    <span class="icon"><i class="far fa-times-circle"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Send to Vessel" CommandName="SENDTOOFFICE" ID="cmdsendtoARC" ToolTip="Send to Office">
                                    <span class="icon"><i class="fas fa-check-circle"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Approve" CommandName="APPROVE" ID="cmdApprove" ToolTip="Place Order">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Send to Arc Marine" CommandName="SENDTOVENDOR" ID="cmdSend" ToolTip="Send to Arc Marine">
                                    <span class="icon"><i class="fas fa-share-square"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Create Po" CommandName="CREATEPOREQ" ID="cmdCreatePOReq" ToolTip="Create PO">
                                    <span class="icon"><i class="fas fa-copy"></i></span>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <asp:Image ID="imgReqSent" runat="server" ImageUrl="<%$ PhoenixTheme:images/14.png %>"
                            ImageAlign="Top" />
                        &nbsp;
                            <telerik:RadLabel ID="lblReqSent" runat="server" Text=" * Sent to Office"></telerik:RadLabel>
                        &nbsp;&nbsp;
                            <asp:Image ID="imgReqSenttoARC" runat="server" ImageUrl="<%$ PhoenixTheme:images/Green.png %>"
                                ImageAlign="Top" />
                        &nbsp;
                            <telerik:RadLabel ID="lblReqSentARC" runat="server" Text=" * Sent to ARC Marine"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <%--<eluc:ConfirmMessage runat="server" ID="ucConfirmSent" Visible="false" Text="" OnConfirmMesage="ucConfirmSent_OnClick"></eluc:ConfirmMessage>
            <eluc:ConfirmMessage runat="server" ID="ucConfirmSenttoOffice" Visible="false" Text="" OnConfirmMesage="ucConfirmSenttoOffice_OnClick"></eluc:ConfirmMessage>--%>
            <asp:Button ID="ucConfirmSent" runat="server" OnClick="ucConfirmSent_OnClick" CssClass="hidden" />
            <asp:Button ID="ucConfirmSenttoOffice" runat="server" OnClick="ucConfirmSenttoOffice_OnClick" CssClass="hidden" />
            <asp:Button ID="ucConfirmPoCreate" runat="server" OnClick="ucConfirmPoCreate_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
