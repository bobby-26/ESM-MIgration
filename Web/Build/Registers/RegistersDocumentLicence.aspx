<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDocumentLicence.aspx.cs"
    Inherits="RegistersDocumentLicence" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Licence</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvDocumentLicence.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
                fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersDocumentLicence" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuLicenceCost" runat="server" OnTabStripCommand="LicenceCost_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <%--<eluc:TabStrip ID="MenuTitle" runat="server" Title="Licence"></eluc:TabStrip>--%>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table id="tblConfigureDocumentLicence">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLicence" runat="server" Text="Licence"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSearchLicence" runat="server" MaxLength="100" CssClass="input"
                            Width="360px">
                        </telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadCheckBox ID="chkincludeinactive" Text="Include Inactive" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersDocumentLicence" runat="server" OnTabStripCommand="RegistersDocumentLicence_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDocumentLicence" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvDocumentLicence_ItemCommand" OnNeedDataSource="gvDocumentLicence_NeedDataSource"
                OnItemDataBound="gvDocumentLicence_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="false"
                OnSortCommand="gvDocumentLicence_SortCommand">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left"
                    CommandItemDisplay="None">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="130px" HeaderText="Document Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDocumentType" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPENAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="135px" HeaderText="Document Category">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCategoryHeader" runat="server" Text="Document Category"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Abbreviation" HeaderStyle-Width="80px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAbbreviation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABREVATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Licence" HeaderStyle-Width="210px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLicence" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDLICENCE") %>'></telerik:RadLabel>
                               <%-- <asp:LinkButton ID="lnkLicense" runat="server" CommandArgument='<%# Bind("FLDDOCUMENTID") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDLICENCE") %>'></asp:LinkButton>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRankHeader" runat="server" Text="Rank"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblRankNameList" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="RANKLIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDRANKNAMELIST"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>
                                <%--<telerik:RadLabel ID="lblRankName" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDRANKNAME"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDRANKNAME"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Vessel Type" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblVesselType" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Owner" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblVesselTypeHeader" runat="server" Text="Owner"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblowner" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDOWNERLIST"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Company" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblCompany" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANIES"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Flag" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblFlag" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDFLAG"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLocalActive" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDLOCALACTIVE").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Office Crew" HeaderStyle-Width="80px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOfficeCrew" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICECREW") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Group" HeaderStyle-Width="80px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUP") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Applies To" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Stage" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStageName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Mandatory Y/N" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANDATORYYNNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Waiver Y/N" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWaiver" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVERYNNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User Group to allow waiver" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUserGroup" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="ImgUserGroup" runat="server">
                                        <span><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                                <%--<asp:ImageButton id="ImgUserGroup" runat="server" ImageUrl="<%$ PhoenixTheme:images/te_view.png%>" CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>--%>
                                <eluc:Tooltip ID="ucUserGroup" runat="server" Width="180px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Addition Doc. Y/N" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAdditionDocYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDADDITIONALDOCYNNAME")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Requires Authentication Y/N" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthReqYnYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATIONREQYNNAME")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Show in Master checklist onboard Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShowMasterChecklistYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSHOWINMASTERCHECKLISTYN")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Photocopy acceptable Y/N" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPhotocopyYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDPHOTOCOPYACCEPTABLEYN")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
