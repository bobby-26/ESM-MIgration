<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyCertificateEndorsementList.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceVesselSurveyCertificateEndorsementList" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Endorsements</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <form id="frmSurveyCertificate" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlSurveyCertificate">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader">
                    <eluc:Title runat="server" ID="Title1" Text="Endorsements" ShowMenu="true">
                    </eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="width: auto; float: right; margin-top: -26px">
                    <eluc:TabStrip ID="MenuSurveyScheduleHeader" runat="server" OnTabStripCommand="MenuSurveyScheduleHeader_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div style="position: relative; width: 15px;">
                    <eluc:TabStrip ID="MenuSurveyCertificate" runat="server" OnTabStripCommand="MenuSurveyCertificate_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvSurveyCertificate" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" OnSelectedIndexChanging="gvSurveyCertificate_SelectedIndexChanging"
                        Width="100%" CellPadding="3" OnRowDataBound="gvSurveyCertificate_OnRowDataBound"
                        ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField HeaderText="Certificate Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCertificatesHeader" runat="server" Text="Certificate Code"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCertificateCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATECODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Certificate">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCertificatesHeader" runat="server" Text="Certificate"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCertificates" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>'></asp:Label>
                                    <asp:Label ID="lblCertificateId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATEID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Certificate.No">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSurveyNoHeader" runat="server" Text="Certificate.No"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCertificateNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENO") %>'></asp:Label>
                                    <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                    <asp:Label ID="lblDtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date of Issue">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblIssueDateheader" runat="server" Text="Date of Issue"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIssueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFISSUE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date of Expiry">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblExpiryDateheader" runat="server" Text="Date of Expiry"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblExpiryDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFEXPIRY")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Survey">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSurveyName" runat="server" Text="Survey"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSurveyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSURVEYID") %>'></asp:Label>
                                    <asp:LinkButton ID="lblSurveyNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSURVEYNUMBER") %>'
                                        CommandArgument='<%#Container.DataItemIndex %>' CommandName="SELECT"></asp:LinkButton>
                                    <asp:Label ID="lblSheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCHEDULEID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issuing Authority">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblIssuingAuthorityHeader" runat="server">Issuing Authority&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIssuingAuthority" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITYNAME") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Place of Issue">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblIssuingPlaceHeader" runat="server">Place of Issue
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIssuingPlace" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Endorsement Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblEndorseHeader" runat="server" Text="Endorsement Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEndorsementDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDENDORSEDATE")) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Certificate Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblStatusHeader" runat="server" Text="Certificate Type"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>--%>
                             <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSelect"
                                        ToolTip="Edit"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
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
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
