<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersRank.aspx.cs" Inherits="RegistersRank"
    MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rank</title>
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
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblConfigureRank" width="100%">
                <tr>
                    <td width="5%">
                        <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td width="45%">
                        <telerik:RadTextBox ID="txtRankCode" runat="server" MaxLength="6" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td width="5%">
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td width="45%">
                        <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersRank" runat="server" OnTabStripCommand="RegistersRank_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRank" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvRank_ItemCommand" OnItemDataBound="gvRank_ItemDataBound"
                EnableViewState="true" OnSortCommand="gvRank_SortCommand" ShowFooter="true" OnNeedDataSource="gvRank_NeedDataSource"
                ShowHeader="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDRANKID">
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
                        <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" SortExpression="FLDRANKCODE">
                            <HeaderStyle Width="15%" Wrap="false" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtRankCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'
                                    CssClass="gridinput_mandatory" MaxLength="6" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtRankCodeAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="6" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" SortExpression="FLDRANKNAME">
                            <HeaderStyle Width="25%" Wrap="false" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkRankName" runat="server" CommandName="EDIT" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblRankIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtRankNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="100" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtRankNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="100"
                                    ToolTip="Enter Rank Name" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Level" AllowSorting="true" SortExpression="FLDLEVEL">
                            <HeaderStyle Width="14%" Wrap="false" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLevel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--<asp:TextBox ID="txtLevelEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVEL") %>'
                                    CssClass="gridinput_mandatory"></asp:TextBox>
                                <ajaxtoolkit:maskededitextender id="txtNumberLevelMaskEdit" runat="server" targetcontrolid="txtLevelEdit"
                                    mask="999" masktype="Number" inputdirection="LeftToRight" autocomplete="false">
                                    </ajaxtoolkit:maskededitextender>--%>
                                <telerik:RadMaskedTextBox runat="server" ID="txtNumberLevelMaskEdit" Width="100%" Mask="###" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVEL") %>'></telerik:RadMaskedTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <%--<asp:TextBox ID="txtLevelAdd" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                <ajaxtoolkit:maskededitextender id="txtNumberLevelMask" runat="server" targetcontrolid="txtLevelAdd"
                                    oninvalidcssclass="MaskedEditError" mask="999" masktype="Number" inputdirection="LeftToRight"
                                    autocomplete="false">
                                    </ajaxtoolkit:maskededitextender>--%>
                                <telerik:RadMaskedTextBox runat="server" ID="txtNumberLevelMask" Width="100%" Mask="###" CssClass="gridinput_mandatory"></telerik:RadMaskedTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Crew Sort" AllowSorting="true" SortExpression="FLDCREWSORT">
                            <HeaderStyle Width="15%" Wrap="false" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCrewSort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWSORT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--<asp:TextBox ID="txtCrewSortEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWSORT") %>'
                                    CssClass="gridinput_mandatory"></asp:TextBox>
                                <ajaxtoolkit:maskededitextender id="txtNumberCrewSortMaskEdit" runat="server" targetcontrolid="txtCrewSortEdit"
                                    mask="999" masktype="Number" inputdirection="LeftToRight" autocomplete="false">
                                    </ajaxtoolkit:maskededitextender>--%>
                                <telerik:RadMaskedTextBox runat="server" ID="txtNumberCrewSortMaskEdit" Width="100%" Mask="###" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWSORT") %>'></telerik:RadMaskedTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <%--<asp:TextBox ID="txtCrewSortAdd" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                <ajaxtoolkit:maskededitextender id="txtNumberCrewSortMask" runat="server" targetcontrolid="txtCrewSortAdd"
                                    mask="999" masktype="Number" inputdirection="LeftToRight" autocomplete="false">
                                    </ajaxtoolkit:maskededitextender>--%>
                                <telerik:RadMaskedTextBox runat="server" ID="txtNumberCrewSortMask" Width="100%" Mask="###" CssClass="gridinput_mandatory"></telerik:RadMaskedTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Group Rank" AllowSorting="true" SortExpression="FLDGROUPRANK">
                            <HeaderStyle Width="25%" Wrap="false" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGroupRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPRANK") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlGroupRankEdit" runat="server" Width="100%" Filter="Contains"></telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlGroupRankAdd" runat="server" Width="100%" DropDownHeight="400px" Filter="Contains" EnableDirectionDetection="true"></telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Group" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="22%" Wrap="false" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUP") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ucGroupEdit" runat="server" AppendDataBoundItems="true" HardList='<%# PhoenixRegistersHard.ListHard(1, 51) %>'
                                    HardTypeCode="51" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPID") %>' Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ucGroupAdd" runat="server" HardTypeCode="51" AppendDataBoundItems="true"
                                    HardList='<%# PhoenixRegistersHard.ListHard(1, 51) %>' Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Office Crew" AllowSorting="true" SortExpression="" HeaderStyle-Wrap="true">
                            <HeaderStyle Width="21%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOfficeCrew" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICECREW") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ucOfficeCrewEdit" runat="server" AppendDataBoundItems="true" HardList='<%# PhoenixRegistersHard.ListHard(1, 50) %>'
                                    HardTypeCode="50" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICECREWID") %>' Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ucOfficeCrewAdd" runat="server" HardTypeCode="50" AppendDataBoundItems="true"
                                    HardList='<%# PhoenixRegistersHard.ListHard(1, 50) %>' Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Level" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="22%" Wrap="false" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLevelType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ucLevelTypeEdit" runat="server" AppendDataBoundItems="true" HardList='<%# PhoenixRegistersHard.ListHard(1, 118) %>'
                                    HardTypeCode="118" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELTYPE") %>' Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ucLevelTypeAdd" runat="server" HardTypeCode="118" AppendDataBoundItems="true"
                                    HardList='<%# PhoenixRegistersHard.ListHard(1, 118) %>' Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Licence req" AllowSorting="true" SortExpression="" HeaderStyle-Wrap="true">
                            <HeaderStyle Width="13%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLicenceReq" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDLICENCEREQ")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkLicenceReqEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDLICENCEREQYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkLicenceReqAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CBA Applicable" AllowSorting="true" SortExpression="" HeaderStyle-Wrap="true">
                            <HeaderStyle Width="19%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCBAAPPLICABLE").ToString() == string.Empty ? "" : (DataBinder.Eval(Container, "DataItem.FLDCBAAPPLICABLE").ToString() == "1" ? "Officer" : "Ratings")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="ddlCBAApplicable" runat="server" Width="100%">
                                    <Items>
                                        <telerik:DropDownListItem Value="" Text="--Select--" />
                                        <telerik:DropDownListItem Value="1" Text="Officer" />
                                        <telerik:DropDownListItem Value="0" Text="Rating" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList ID="ddlCBAApplicableAdd" runat="server" Width="100%" EnableDirectionDetection="true">
                                    <Items>
                                        <telerik:DropDownListItem Value="" Text="--Select--" />
                                        <telerik:DropDownListItem Value="1" Text="Officer" />
                                        <telerik:DropDownListItem Value="0" Text="Rating" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="Active" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="12%" Wrap="false" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Top4 Y/N">
                            <HeaderStyle Width="11%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTop4YN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDTOPFOURYESNO")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkTop4YNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDTOPFOURYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkTop4YNAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Top2 Y/N">
                            <HeaderStyle Width="11%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTop2YN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDTOPTWOYESNO")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkTop2YNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDTOPTWOYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkTop2YNAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave"
                                    ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel"
                                    ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" EnableNextPrevFrozenColumns="false" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
