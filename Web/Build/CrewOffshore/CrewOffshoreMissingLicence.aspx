<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreMissingLicence.aspx.cs" Inherits="CrewOffshoreMissingLicence" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCrewAboutUsBy" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlFamilyNok">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader">
                    <eluc:Title runat="server" ID="ttlAboutUsBy" Text="Missing Licence" ShowMenu="false">
                    </eluc:Title>
                    <br />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table cellspacing="1" cellpadding="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblFlag" runat="server" Text="Flag"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVesselFlag" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRank" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblFirstName" runat="server" Text="First Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblMiddleName" runat="server" Text="Middle Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblLastName" runat="server" Text="Last Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblApproxdateofCrewChange" runat="server" Text="Approx date of Crew Change"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtCrewChangeDate" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <asp:Literal ID="lblCRArequiredDate" runat="server" Text="CRA required Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtCraDate" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblPlannedJoiningDate" runat="server" Text="Planned Joining Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtPlannedJoinDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPaymentBy" runat="server" Text="Payment By"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList runat="server" ID="rblPaymentBy" RepeatDirection="Horizontal">
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:Literal ID="lblBilltoCompany" runat="server" Text="Bill to Company"></asp:Literal>
                        </td>
                        <td colspan="3">
                            <eluc:UserControlCompany ID="ddlLiabilitycompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                Readonly="true" CssClass="input_mandatory" runat="server" SelectedCompany="7"
                                AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="240px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <hr />
                <asp:GridView ID="gvMissingLicence" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnRowDataBound="gvMissingLicence_RowDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                    EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="25px"></ItemStyle>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" Checked="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Licence Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblDocumentId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDOCUMENTID") %>'></asp:Label>
                                <asp:Label ID="lblType" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTYPE") %>'></asp:Label>
                                <%#DataBinder.Eval(Container, "DataItem.FLDLICENCE")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Missing/Expired">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDMISSINGYN").ToString() == "1" ? "Missing" : (DataBinder.Eval(Container, "DataItem.FLDEXPIREDYN").ToString() == "1" ? "Expired" : "")%>
                                <asp:Label ID="lblMissingYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMISSINGYN")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Flag">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDFLAGNAME")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <font color="blue"><asp:Literal ID="lblNoteThelicenseswhichareshowinghereasExpiredwillbeexpirein3monthsContractPeriodfromtheplannedjoiningdate" runat="server" Text="Note: The licenses, which are showing here as ‘Expired’ will be expire in 3 months + Contract Period from the planned joining date."></asp:Literal> </font>
                <br />
                <b><asp:Literal ID="lblLicencerequests" runat="server" Text="Licence requests"></asp:Literal></b>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvLicReq" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" OnRowEditing="gvLicReq_RowEditing" OnRowCancelingEdit="gvLicReq_RowCancelingEdit"  
                        OnRowDataBound="gvLicReq_RowDataBound" EnableViewState="false" AllowSorting="true"
                         OnRowCommand="gvLicReq_RowCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Sl No">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDROWNUMBER") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblFlagHeader" runat="server" CommandName="Sort" CommandArgument="FLDFLAG"
                                        ForeColor="White">Flag</asp:LinkButton>
                                    <img id="FLDFLAG" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblProcessId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROCESSID")%>'></asp:Label>
                                    <asp:Label ID="lblFlagId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFLAGID") %>'></asp:Label>
                                    <asp:Label ID="lblFlag" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFLAGNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vessel Name">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME").ToString().TrimEnd(',') %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblLicenceHeader" runat="server" CommandName="Sort" CommandArgument="FLDLICENCE"
                                        ForeColor="White">Licence Requested</asp:LinkButton>
                                    <img id="FLDLICENCE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDLICENCE").ToString().TrimEnd(',')%>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCrewChangeDate" runat="server" Text="Crew Change Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCREWCHANGEDATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRequestedDate" runat="server" Text="Requested Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCREATEDDATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRequestedBy" runat="server" Text="Requested By"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCREATEDBY")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCRASTATUS")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/red-symbol.png %>"
                                        CommandName="CANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <%--<table width="100%" border="0" class="datagrid_pagestyle">
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
                        <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                        </asp:TextBox>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                            Width="40px"></asp:Button>
                    </td>
                </tr>
            </table>--%>
                    <eluc:Status ID="ucStatus" runat="server" />
                    <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="InitiateLicenceRequest"
                        OKText="Ok" CancelText="Cancel" Visible="false" />
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
