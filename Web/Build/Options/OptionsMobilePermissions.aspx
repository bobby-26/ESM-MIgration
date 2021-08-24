<%@ Page Language="C#" AutoEventWireup="True" CodeFile="OptionsMobilePermissions.aspx.cs"
    Inherits="OptionsMobilePermissions" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head2" runat="server">
    <title>Device Access</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmMobilePermissions" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblConfigureDevice">
                <tr>
                    <td width="8%" style="text-align: right">
                        <telerik:RadLabel ID="lblDevice" runat="server" Text="Device"></telerik:RadLabel>
                    </td>
                    <td width="12%">
                        <telerik:RadTextBox ID="txtDevice" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td width="8%" style="text-align: right">
                        <asp:Literal ID="lblUserName" runat="server" Text="User Name"></asp:Literal>
                    </td>
                    <td width="12%">
                        <telerik:RadTextBox ID="txtUsername" runat="server" MaxLength="100"></telerik:RadTextBox>
                    </td>
                    <td width="8%" style="text-align: right">
                        <telerik:RadLabel ID="lblDeviceType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td width="12%">
                        <telerik:RadTextBox ID="txtDeviceType" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td width="8%" style="text-align: right">
                        <telerik:RadLabel ID="lblLanguage" runat="server" Text="Language"></telerik:RadLabel>
                    </td>
                    <td width="12%">
                        <telerik:RadTextBox ID="txtLanguage" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td width="8%" style="text-align: right">
                        <telerik:RadLabel ID="lblEffective" runat="server" Text="Effective Date"></telerik:RadLabel>
                    </td>
                    <td width="12%">
                        <eluc:Date ID="txtEffective" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td width="8%" style="text-align: right">
                        <telerik:RadLabel ID="lblActiveYN" runat="server" Text="Active Y/N"></telerik:RadLabel>
                    </td>
                    <td width="12%" style="text-align: left">
                        <table>
                            <tr>
                                <td width="20%" style="text-align: left">
                                    <telerik:RadCheckBox ID="chkActive" runat="server"></telerik:RadCheckBox>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                    <td width="8%" style="text-align: right">
                        <telerik:RadLabel ID="lblRegion" runat="server" Text="Region"></telerik:RadLabel>
                    </td>
                    <td width="12%">
                        <telerik:RadTextBox ID="txtRegion" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td width="8%" style="text-align: right">
                        <telerik:RadLabel ID="lblManufacturer" runat="server" Text="Manufacturer"></telerik:RadLabel>
                    </td>
                    <td width="12%">
                        <telerik:RadTextBox ID="txtManufacturer" runat="server" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td></td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuOptionsMobilePermissions" runat="server" OnTabStripCommand="RegistersMobilePermissions_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvMobilePermissions" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvMobilePermissions_ItemCommand" OnItemDataBound="gvMobilePermissions_ItemDataBound"
                ShowFooter="true" ShowHeader="true" EnableViewState="true" OnSortCommand="gvMobilePermissions_SortCommand"
                OnNeedDataSource="gvMobilePermissions_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDEVICE">
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
                        <telerik:GridTemplateColumn HeaderText="Application" AllowSorting="true" SortExpression="FLDAPPCODE">
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApplication" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDAPPCODE") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%--  <EditItemTemplate>
                                <telerik:RadTextBox ID="lblApplicationEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPCODE") %>'
                                    CssClass="gridinput_mandatory" MaxLength="20" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>--%>
                            <%--   <FooterTemplate>
                                <telerik:RadTextBox ID="txtApplicationAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="6" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Device" AllowSorting="true" SortExpression="FLDDEVICE">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDevice" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDDEVICE") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEVICE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%--  <EditItemTemplate>
                                <telerik:RadTextBox ID="lblDeviceEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEVICE") %>'
                                    CssClass="gridinput_mandatory" MaxLength="20" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDeviceAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="6" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" AllowSorting="true" SortExpression="FLDDEVICETYPE">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDeviceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDeviceType" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDDEVICETYPE") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEVICETYPE") %>'></telerik:RadLabel>
                                <%--<asp:LinkButton ID="lnkDeviceType" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEVICETYPE") %>'></asp:LinkButton>--%>
                            </ItemTemplate>
                            <%-- <EditItemTemplate>
                                <telerik:RadLabel ID="lblDeviceTypeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEVICEID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtDeviceTypeAdd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEVICETYPE") %>'
                                    CssClass="gridinput_mandatory" MaxLength="20" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDeviceTypeAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="20" ToolTip="Enter Device Type" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Manufacturer" Visible="false" AllowSorting="true" SortExpression="FLDMANUFACTURER">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblManufacturer" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDMANUFACTURER") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANUFACTURER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%-- <EditItemTemplate>
                                <telerik:RadTextBox ID="lblManufacturerEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANUFACTURER") %>'
                                    CssClass="gridinput_mandatory" MaxLength="-1" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtManufacturerAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="6" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Region" Visible="false" AllowSorting="true" SortExpression="FLDREGION">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRegion" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDREGION") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDREGION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%--  <EditItemTemplate>
                                <telerik:RadTextBox ID="lblRegionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREGION") %>'
                                    CssClass="gridinput_mandatory" MaxLength="20" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtRegionAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="6" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Language" Visible="false" AllowSorting="true" SortExpression="FLDLANGUAGE">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLanguage" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDLANGUAGE") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDLANGUAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%-- <EditItemTemplate>
                                <telerik:RadTextBox ID="lblLanguageEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLANGUAGE") %>'
                                    CssClass="gridinput_mandatory" MaxLength="20" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtLanguageAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="6" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="OSVersion" Visible="false" AllowSorting="true" SortExpression="FLDAPPOSVERSION">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOSVersion" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDAPPOSVERSION") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPOSVERSION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%-- <EditItemTemplate>
                                <telerik:RadTextBox ID="lblOSVersionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPOSVERSION") %>'
                                    CssClass="gridinput_mandatory" MaxLength="20" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtOSVersionAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="6" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="SDK" Visible="false" AllowSorting="true" SortExpression="FLDAPPSDKVERSION">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSDKVersion" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDAPPSDKVERSION") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPSDKVERSION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%--  <EditItemTemplate>
                                <telerik:RadTextBox ID="lblSDKVersionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPSDKVERSION") %>'
                                    CssClass="gridinput_mandatory" MaxLength="20" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSDKVersionAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="6" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Model" Visible="false" AllowSorting="true" SortExpression="FLDMODEL">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblModel" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDMODEL") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%--  <EditItemTemplate>
                                <telerik:RadTextBox ID="lblModelEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODEL") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtModelAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="6" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Identity" AllowSorting="true" SortExpression="FLDUNIQUEDEVICEDTKEY">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbluuid" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDUNIQUEDEVICEDTKEY") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNIQUEDEVICEDTKEY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%-- <EditItemTemplate>
                                <telerik:RadTextBox ID="lbluuidEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNIQUEDEVICEDTKEY") %>'
                                    CssClass="gridinput_mandatory" MaxLength="100" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtuuidAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="6" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Username" AllowSorting="true" SortExpression="FLDUSERNAME">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUserId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblUsername" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%-- <EditItemTemplate>
                                <telerik:RadTextBox ID="lblUsernameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="100" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtUsernameAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="6" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Effective Till" AllowSorting="true" ShowSortIcon="true">
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEffective" runat="server" ToolTip='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE","{0:dd/MMM/yyyy}")) %>' Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucEffectiveEdit" runat="server" Tooltip='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE","{0:dd/MMM/yyyy}")) %>' Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE","{0:dd/MMM/yyyy}")) %>' CssClass="input_mandatory" />
                            </EditItemTemplate>
                            <%-- <FooterTemplate>
                                <eluc:Date runat="server" ID="ucEffectiveAdd" CssClass="input_mandatory" Width="100%" />
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active" AllowSorting="true">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVE").ToString()) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVE").ToString().Equals("Yes"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <%--  <FooterTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Logged" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLoginActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDLOGINACTIVE").ToString()) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox AutoPostBack="true" ID="chkLoginActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDLOGINACTIVE").ToString().Equals("Yes"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <%--   <FooterTemplate>
                                <telerik:RadCheckBox ID="chkLoginActiveYNAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <%--<FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="5" EnableNextPrevFrozenColumns="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
