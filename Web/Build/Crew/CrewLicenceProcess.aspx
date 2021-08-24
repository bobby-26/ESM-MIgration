<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceProcess.aspx.cs"
    Inherits="CrewLicenceProcess" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="ajaxToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Warn List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLicReq" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewCourseCertificateEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader">
                    <eluc:Title runat="server" ID="Title3" Text="Licence Request" ShowMenu="true"></eluc:Title>
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div id="divGuidance" runat="server">
                    <table id="tblGuidance">
                        <tr>
                            <td>
                                <asp:Label ID="lblnote" runat="server" CssClass="guideline_text">Note: 1. Scroll down for Cancelled requests.
                            <br />
                            2. Click on the Flag link button in the Cancelled requests to view the requested details for the seafarer.
                            </asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuLicenceList" runat="server" OnTabStripCommand="MenuLicenceList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: +1">
                    <asp:GridView ID="gvLicReq" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                        Width="100%" CellPadding="3" ShowHeader="true" OnRowEditing="gvLicReq_RowEditing"
                        OnRowCancelingEdit="gvLicReq_RowCancelingEdit" OnRowDataBound="gvLicReq_RowDataBound"
                        EnableViewState="false" OnRowUpdating="gvLicReq_RowUpdating" AllowSorting="true"
                        OnSorting="gvLicReq_Sorting"  
                        OnRowCommand="gvLicReq_RowCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Select for 'Make Payment'" Visible ="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="25px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="'Cov.Letter'">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="25px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelectCL" runat="server"  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRequestNoHeader" runat="server">Request No</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblProcessId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROCESSID")%>'></asp:Label>
                                    <asp:Label ID="lblFlagId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFLAGID")%>'></asp:Label>
                                    <asp:Label ID="lblSupplierCode" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDADDRESSCODE")%>'></asp:Label>
                                    <asp:Label ID="lblAmount" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>'></asp:Label>
                                    <asp:Label ID="lblCurrencyId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCURRENCYID")%>'></asp:Label>
                                    <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDVESSELLIST")%>'></asp:Label>
                                    <asp:Label ID="lblBudgetCode" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDBUDGETID")%>'></asp:Label>
                                    <asp:Label ID="lblPaymentBy" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPAYMENTBY")%>'></asp:Label>
                                    <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>'></asp:Label>
                                    <asp:Label ID="lblBankId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDBANKID")%>'></asp:Label>
                                    <asp:Label ID="lblBillToCompanyId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDBILLTOCOMPANYID")%>'></asp:Label>
                                    <asp:Label ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREFNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblRankHeader" runat="server" CommandName="Sort" CommandArgument="FLDFLAG"
                                        >Flag</asp:LinkButton>
                                    <img id="FLDFLAG" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%#DataBinder.Eval(Container, "DataItem.FLDFLAGNAME")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vessel">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME").ToString().TrimEnd(',') %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCrewChangeDateHeader" runat="server">Crew Change</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCREWCHANGEDATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRequestedDate" runat="server" Text="Requested"></asp:Label>
                                    <eluc:ToolTip ID="ucToolTipRequestedDate" runat="server" Text='Time shown is GMT' />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCREATEDDATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCRAReqdDateHeader" runat="server">CRA Requested</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCRADATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCompanyAddressToBeDisplayedInApplnFormHeader" runat="server">Company</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCOMPANYADDRINREPORT")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblCompanyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPANYADDR") %>'></asp:Label>
                                    <eluc:Company ID="ddlCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                        CssClass="input" runat="server" SelectedCompany='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPANYADDR") %>'
                                        AppendDataBoundItems="true" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAuthorizedRepresentativeHeader" runat="server" >Authorized Representative</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDAUTHORIZED")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtAuthorisedRep" runat="server" CssClass="input" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAUTHORIZED")%>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDesignationHeader" runat="server">Designation</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                  <asp:Label ID="lblDesignation" runat="server" Text ='<%#DataBinder.Eval(Container, "DataItem.FLDDESIGNATION")%>'></asp:Label>   
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDesignation" runat="server" CssClass="input" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDESIGNATION")%>' ></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStatusHeader" runat="server">Status</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDSTATUSNAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblBillToHeader" runat="server">Bill To</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDBILLTOCOMPANY")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblReqIdEdit" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROCESSID")%>'></asp:Label>
                                    <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input txtNumber" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>'
                                        MaxLength="14" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budget Code">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSUBACCOUNT")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:BudgetCode runat="server" ID="ucBudgetCodeEdit" AppendDataBoundItems="true"
                                        AutoPostBack="true" CssClass="dropdown_mandatory" BudgetCodeList='<%# PhoenixRegistersBudget.ListBudget() %>'
                                        SelectedBudgetSubAccount='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl='<%$ PhoenixTheme:images/te_edit.png%>'
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Make Payment" ImageUrl="<%$ PhoenixTheme:images/generate-po.png %>"
                                    CommandName="PO" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdGeneratePO"
                                    ToolTip="Make Payment"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <eluc:Number ID="txtnopage" runat="server" Width="20px" 
                                IsInteger="true" CssClass="input" />
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <b>
                    <asp:Literal ID="lblCancelledRequests" runat="server" Text="Cancelled Requests"></asp:Literal></b>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuLicenceCancelList" runat="server" OnTabStripCommand="MenuLicenceCancelList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="div2" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvLicReqCancel" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                        Width="100%" CellPadding="3" ShowHeader="true" OnRowDataBound="gvLicReqCancel_RowDataBound"
                        EnableViewState="false" AllowSorting="true" OnSorting="gvLicReqCancel_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRequestNumberHeader" runat="server">Request No</asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblProcessId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROCESSID")%>'></asp:Label>
                                    <asp:Label ID="lblFlagId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFLAGID")%>'></asp:Label>
                                    <asp:Label ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREFNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblRankHeader" runat="server" CommandName="Sort" CommandArgument="FLDFLAG"
                                        >Flag</asp:LinkButton>
                                    <img id="FLDFLAG" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%#DataBinder.Eval(Container, "DataItem.FLDFLAGNAME")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vessel">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME").ToString().TrimEnd(',') %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCrewChangeDateHeader" runat="server">Crew Change</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCREWCHANGEDATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRequestedDateHeader" runat="server">Requested</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCREATEDDATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCRAReqdDateHeader" runat="Server">CRA Requested</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCRADATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <%-- <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCancelledDateHeader" runat="server">Cancelled Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCANCELDATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCancelledByHeader" runat="Server">Cancelled By</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCANCELLEDBY")%>
                                On  <%#DataBinder.Eval(Container, "DataItem.FLDCANCELDATE", "{0:dd/MMM/yyyy}")%>
                                     </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStatusHeader" runat="server">Status</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDSTATUSNAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <%--<br />--%>
                <div id="divPage2" style="position: relative; z-index: 0">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblCRPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblCRPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblCRRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdCRPrevious" runat="server" OnCommand="CRPagerButtonClick"
                                    CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdCRNext" OnCommand="CRPagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                  <eluc:Number ID="txtCRnopage" runat="server" Width="20px" 
                                IsInteger="true" CssClass="input" />
                                <asp:Button ID="btnCRGo" runat="server" Text="Go" OnClick="cmdCRGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <%--<eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnApprove_Click" OKText="Yes" CancelText="No" /> --%>
                <eluc:UserControlStatus ID="ucStatus" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
