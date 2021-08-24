<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselCertificates.aspx.cs" Inherits="RegistersVesselCertificates" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Certificate" Src="~/UserControls/UserControlCertificate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Certificates</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersVesselCertificates" DecoratedControls="All" />
    <form id="frmRegistersVesselCertificates" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblConfigureVesselCertificates" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" MaxLength="100" ReadOnly="true" CssClass="readonlytextbox" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCertificate" runat="server" Text="Certificate"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCertificateName" runat="server" MaxLength="200" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersVesselCertificates" runat="server" OnTabStripCommand="RegistersVesselCertificates_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselCertificates" Height="85%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvVesselCertificates_ItemCommand" OnItemDataBound="gvVesselCertificates_ItemDataBound" EnableViewState="false" ShowFooter="true" ShowHeader="true" OnNeedDataSource="gvVesselCertificates_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDTKEY">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle Width="4%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Certificate">
                            <HeaderStyle Width="18%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCertificatesId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATESID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkCertificateName" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDTKeyedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselCertificatesIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATESID") %>'></telerik:RadLabel>
                                <telerik:RadDropDownList ID="ucCertificateEdit" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="99%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList ID="ucCertificateAdd" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="99%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCertificateNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENO") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCertificateNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENO") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCertificateNumberAdd" runat="server" CssClass="gridinput_mandatory" Width="99%"
                                    MaxLength="200" ToolTip="Enter Certificate Number">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issue On">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtDateOfIssueEdit" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>' Width="120px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtDateOfIssueAdd" CssClass="gridinput_mandatory" Width="120px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry On">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateOfExpiry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtDateOfExpiryEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>' Width="120px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtDateOfExpiryAdd" Width="120px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Authority">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssuingAuthority" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITYNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITYNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:AddrType ID="ucIssuingAuthorityEdit" runat="server" AddressType="134,137,334" AddressList='<%# PhoenixRegistersAddress.ListAddress("134,137,334") %>'
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" SelectedAddress='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITY") %>' Width="99%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:AddrType ID="ucIssuingAuthorityAdd" runat="server" AddressType="134,137,334" AddressList='<%# PhoenixRegistersAddress.ListAddress("134,137,334") %>'
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="99%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtRemarksEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                    MaxLength="500" Width="99%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtRemarksAdd" runat="server" Width="99%"
                                    MaxLength="500">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-alt-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                    ToolTip="Attachment"><span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Archive"
                                    CommandName="Archive" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdArchive"
                                    ToolTip="Archive"><span class="icon"><i class="fas fa-envelope-open-text"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Save" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDocumentsExpired" runat="server" Text="* Documents Expired"></telerik:RadLabel>
                    </td>
                    <td>
                        <img id="Img5" src="<%$ PhoenixTheme:images/purple.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDocumentsExpiringin30Days" runat="server" Text="* Documents Expiring in 30 Days"></telerik:RadLabel>
                    </td>
                    <td>
                        <img id="Img2" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDocumentsExpiringin60Days" runat="server" Text="* Documents Expiring in 60 Days"></telerik:RadLabel>
                    </td>
                </tr>
            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

