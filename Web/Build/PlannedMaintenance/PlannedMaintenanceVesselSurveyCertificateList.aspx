<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyCertificateList.aspx.cs"
    Inherits="PlannedMaintenance_PlannedMaintenanceVesselSurveyCertificateList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCPort" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Certificate List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="dvscriptsk">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmSurveyScheduleCertificates" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlSurveyScheduleCertificates">
        <ContentTemplate>
            <div class="subHeader">
                <asp:Literal ID="lblSurveyScheduleCertificates" runat="server" Text="Certificates List"></asp:Literal>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            </div>
            <div id="div1">
                <table width="100%" cellspacing="5">
                    <tr>
                        <td>
                            <asp:Literal ID="lblSurveyNumber" runat="server" Text="Survey Number"></asp:Literal>
                            <eluc:Error ID="Error1" runat="server" Text="" Visible="false"></eluc:Error>
                            <eluc:Status ID="Status1" runat="server" Text="" Visible="false"></eluc:Status>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurveyNumber" runat="server" Width="150px" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblSurvey" runat="server" Text="Survey"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurvey" runat="server" Width="250px" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVessel" runat="server" Width="150px" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblSurveyType" runat="server" Text="Survey Type"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurveyType" runat="server" Width="250px" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divControls">
                <div style="position: relative; width: 15px;">
                    <eluc:TabStrip ID="MenuSurveyCertificates" runat="server" OnTabStripCommand="MenuSurveyCertificates_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvCertificates" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnSelectedIndexChanging="gvCertificates_SelectedIndexChanging" Width="100%" CellPadding="3"
                        ShowHeader="true" EnableViewState="false" OnRowCreated="gvCertificates_RowCreated"
                        OnRowCommand="gvCertificates_RowCommand"
                        OnRowDataBound="gvCertificates_RowDataBound">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCertificateCodeHeader" runat="server" Text="Certificate Code"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="30px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lbllCertificateCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATECODE") %>'></asp:Label>
                                    <asp:Label ID="lblDtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:Label ID="lblPrvDtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRVDTKEY") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lbllCertificateCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATECODE") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Certificates">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCertificatesHeader" runat="server" Text="Certificates"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCertificateId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATEID") %>'></asp:Label>
                                    <asp:Label ID="lblCertificates" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>'></asp:Label>
                                    <asp:Label ID="lblSheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCHEDULEID") %>'></asp:Label>
                                    <asp:Label ID="lblCertificateType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATESTATUS") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblCertificateId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATEID") %>'></asp:Label>
                                    <asp:Label ID="lblCertificates" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>'></asp:Label>
                                    <asp:Label ID="lblSheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCHEDULEID") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCertificateNoHeader" runat="server" Text="Number"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCertificateNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENO") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCertificateNoEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENO") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issue Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblIssueDateHeader" runat="server" Text="Issue Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIssueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFISSUE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date runat="server" ID="txtDateOfIssueEdit" CssClass="gridinput_mandatory"
                                        Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE")) %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Expiry Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblExpiryDateHeader" runat="server" Text="Expiry Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblExpiryDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFEXPIRY")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date runat="server" ID="txtDateOfExpiryEdit" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY")) %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issuing Authority">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblIssuingAuthorityHeader" runat="server">Issuing Authority&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIssuingAuthority" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITYNAME") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:AddrType ID="ucIssuingAuthorityEdit" runat="server" AddressType="134,137,334"
                                        AddressList='<%# PhoenixRegistersAddress.ListAddress("134,137,334") %>' AppendDataBoundItems="true"
                                        CssClass="dropdown_mandatory" SelectedAddress='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITY") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Place of Issue">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblIssuingPlaceHeader" runat="server">Place of Issue
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIssuingPlace" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:UCPort ID="ddlPort" runat="server" AppendDataBoundItems="true" CssClass="input"
                                        SeaportList='<%# PhoenixRegistersSeaport.ListSeaport() %>' SelectedSeaport='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDPORT") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remarks">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRemarksHeader" runat="server" Text="Remarks"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgCertificateComments" runat="server" ToolTip="Remarks" CommandName="REMARKS"
                                        ImageUrl="<%$ PhoenixTheme:images/te_view.png %>" CommandArgument='<%# Container.DataItemIndex %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="CmdEdit" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSelect"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/72.png %>"
                                        CommandName="CmdCOC" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCoc"
                                        ToolTip="Report COC"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                        <eluc:Status runat="server" ID="ucStatus" />
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
