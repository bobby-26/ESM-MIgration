<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardVesselCertificateList.aspx.cs" Inherits="DashboardVesselCertificateList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Certificate List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="certificateslink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersVesselCertificates" runat="server" autocomplete="off" submitdisabledcontrols="true">

        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlVesselCertificatesEntry">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <eluc:Status runat="server" ID="ucStatus" />
                <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <asp:Literal ID="lblVesselCertificates" runat="server" Text="Vessel Certificates"></asp:Literal>
                        </div>
                    </div>
                    <div id="divFind" style="position: relative;">
                        <table id="tblConfigureVesselCertificates" width="100%">
                            <tr>
                                <td>
                                    <asp:Literal ID="lblCertificate" runat="server" Text="Certificate"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCertificateName" runat="server" ReadOnly="true" MaxLength="200" CssClass="readonlytextbox" Width="360px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="position: relative;">
                        <eluc:TabStrip ID="MenuRegistersVesselCertificates" runat="server" OnTabStripCommand="RegistersVesselCertificates_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: +1;">
                        <asp:GridView GridLines="None" ID="gvVesselCertificates" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCommand="gvVesselCertificates_RowCommand" OnRowDataBound="gvVesselCertificates_ItemDataBound"
                            ShowFooter="false" ShowHeader="true" EnableViewState="false"
                            OnSorting="gvVesselCertificates_Sorting" AllowSorting="true" >
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vessel Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDVESSELNAME"
                                            ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                        <asp:LinkButton ID="lblCertificateNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME">Vessel Name&nbsp;</asp:LinkButton>
                                        <img id="FLDVESSELNAME" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                        <asp:Label ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></asp:Label>
                                        <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                        <asp:Label ID="lblCertificatesId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATEID") %>'></asp:Label>
                                        <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Number">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblCertificateNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDCERTIFICATENO">Number&nbsp;</asp:LinkButton>
                                        <img id="FLDCERTIFICATENO" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCertificateNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Issue Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblDateOfIssueHeader" runat="server" CommandName="Sort" CommandArgument="FLDDATEOFISSUE">Issue Date&nbsp;</asp:LinkButton>
                                        <img id="FLDDATEOFISSUE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDateOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expiry Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblDateOfExpiryHeader" runat="server" CommandName="Sort" CommandArgument="FLDDATEOFEXPIRY">Expiry Date&nbsp;</asp:LinkButton>
                                        <img id="FLDDATEOFEXPIRY" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDateOfExpiry" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNOEXPIRY").ToString().Equals("1") ? "Permanent" : General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFEXPIRY"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Authority">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblIssuingAuthorityHeader" runat="server">Authority&nbsp;
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblIssuingAuthority" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITYNAME") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblRemarksHeader" runat="server">Remarks&nbsp;
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="Attachment" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                            ToolTip="Attachment"></asp:ImageButton>
                                        <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divPage" style="position: relative;">
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
                                <td width="20px">&nbsp;
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
                        </table>
                    </div>
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                            </td>
                            <td>
                                <asp:Literal ID="lblDocumentsExpired" runat="server" Text="* Documents Expired"></asp:Literal>
                            </td>
                            <td>
                                <img id="Img5" src="<%$ PhoenixTheme:images/purple.png%>" runat="server" />
                            </td>
                            <td>
                                <asp:Literal ID="lblDocumentsExpiringin30Days" runat="server" Text="* Documents Expiring in 30 Days"></asp:Literal>
                            </td>
                            <td>
                                <img id="Img2" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                            </td>
                            <td>
                                <asp:Literal ID="lblDocumentsExpiringin60Days" runat="server" Text="* Documents Expiring in 60 Days"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>


