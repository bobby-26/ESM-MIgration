<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsReimbursement.aspx.cs"
    Inherits="VesselAccountsReimbursement" %>


<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ReimbursementRecovery" Src="~/UserControls/UserControlReimbursementRecovery.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reimbursment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>
</head>
<body>
    <form id="frmCrewList" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%" Width="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuReimbursementGeneral" runat="server" TabStrip="true" Title="Reimbursments" OnTabStripCommand="MenuReimbursementGeneral_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuReimbursement" runat="server" OnTabStripCommand="MenuReimbursement_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvRem" runat="server" AutoGenerateColumns="False" Width="100%" Height="88%" ShowHeader="true" EnableViewState="false"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowSorting="true" EnableHeaderContextMenu="true" ShowFooter="true"
                OnNeedDataSource="gvRem_NeedDataSource" OnItemDataBound="gvRem_ItemDataBound" OnItemCommand="gvRem_ItemCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" CommandItemDisplay="Top"
                    AutoGenerateColumns="false" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" DataKeyNames="FLDREIMBURSEMENTID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Ref. No." HeaderStyle-Width="125px">
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDREFERENCENO"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblreimbursementid" runat="server" Text='<%#((DataRowView) Container.DataItem)["FLDREIMBURSEMENTID"]%>' Visible="false"></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDRANKCODE"].ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="270px" ItemStyle-Width="260px">
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"]%> /  <%# ((DataRowView)Container.DataItem)["FLDFILENO"]%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:VesselCrew ID="ddlEmployeeAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    Width="250px" EmployeeList='<%#PhoenixVesselAccountsEmployee.ListVesselCrew(PhoenixSecurityContext.CurrentSecurityContext.VesselID, 0) %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reimbursement/Recovery" HeaderStyle-Width="220px">
                            <ItemTemplate>
                                <%# GetName(((DataRowView)Container.DataItem)["FLDEARNINGDEDUCTION"].ToString())%>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDISATTACHMENT"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="ddlEarDed" runat="server" CssClass="input_mandatory">
                                    <Items>
                                        <telerik:DropDownListItem Text="--Select--" Value=""></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Reimbursement(B.O.C)" Value="1"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Reimbursement(Monthly / OneTime)" Value="2"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Reimbursement(E.O.C)" Value="3"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Recovery(B.O.C)" Value="-1"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Recovery(Monthly / OneTime)" Value="-2"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Recovery(E.O.C)" Value="-3"></telerik:DropDownListItem>
                                    </Items>
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList ID="ddlEarDedAdd" runat="server" CssClass="input_mandatory" EnableDirectionDetection="true" ExpandDirection="Up">
                                    <Items>
                                        <telerik:DropDownListItem Text="--Select--" Value=""></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Reimbursement(B.O.C)" Value="1"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Reimbursement(Monthly / OneTime)" Value="2"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Reimbursement(E.O.C)" Value="3"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Recovery(B.O.C)" Value="-1"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Recovery(Monthly / OneTime)" Value="-2"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Recovery(E.O.C)" Value="-3"></telerik:DropDownListItem>
                                    </Items>
                                </telerik:RadDropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose" HeaderStyle-Width="185px">
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDHARDNAME"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:ReimbursementRecovery ID="ddlDesc" runat="server" CssClass="input_mandatory"
                                    AppendDataBoundItems="true" HardList="<%#PhoenixRegistersReimbursementRecovery.ListReimbursementRecovery(1,0,1)%>" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:ReimbursementRecovery ID="ddlDescAdd" runat="server" CssClass="input_mandatory"
                                    AppendDataBoundItems="true" HardList="<%#PhoenixRegistersReimbursementRecovery.ListReimbursementRecovery(1,0,1)%>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDDESCRIPTION"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="500" Text='<%# ((DataRowView)Container.DataItem)["FLDDESCRIPTION"]%>' Width="170px">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="500" Width="165px">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDCURRENCYCODE"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Currency ID="ddlCurrency" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    CurrencyList="<%#PhoenixRegistersCurrency.ListCurrency(1) %>" Width="80px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Currency ID="ddlCurrencyAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    CurrencyList="<%#PhoenixRegistersCurrency.ListCurrency(1) %>" Width="80px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDAMOUNT"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtAmount" runat="server" CssClass="input_mandatory" Width="90px"
                                    Text='<%# ((DataRowView)Container.DataItem)["FLDAMOUNT"]%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtAmountAdd" runat="server" CssClass="input_mandatory" Width="90px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Approved Amount" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDAPPROVEDAMOUNT").ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <FooterStyle HorizontalAlign="Center"/>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                    ToolTip="Upload Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave" ToolTip="Save">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel" ToolTip="Cancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="ADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd" ToolTip="Add New">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" Position="Bottom" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>
            <%--            <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                        RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                        HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true" 
                        Text="B.O.C - Begining of Contract <br/> E.O.C - End of Contract">
                    </telerik:RadToolTip>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
