<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDocumentRequiredLicence.aspx.cs"
    Inherits="RegistersDocumentRequiredLicence" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="~/UserControls/UserControlDocuments.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselList" Src="~/UserControls/UserControlVesselList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Docs(Licence)</title>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvDocumentsRequired.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersDocumentsRequired" DecoratedControls="All" />
    <form id="frmRegistersDocumentsRequired" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server"  >

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <eluc:TabStrip ID="MenuVesselList" runat="server" OnTabStripCommand="MenuVesselList_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuFlag" runat="server" OnTabStripCommand="Flag_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table id="tblConfigureDocumentsRequired" width="100%">
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td align="left">
                        <telerik:RadTextBox ID="txtVesselName" runat="server" CssClass="readonlytextbox"
                            ReadOnly="true" Text="" Enabled="false" Width="80%">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Flag ID="ucFlag" runat="server" AppendDataBoundItems="true"
                            AutoPostBack="true" CssClass="dropdown_mandatory" OnTextChangedEvent="ucRank_TextChangedEvent" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true"
                            AutoPostBack="true" CssClass="dropdown_mandatory" OnTextChangedEvent="ucRank_TextChangedEvent" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" AutoPostBack="true" CssClass="dropdown_mandatory" OnTextChangedEvent="ucRank_TextChangedEvent"/>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersDocumentsRequired" runat="server" OnTabStripCommand="RegistersDocumentsRequired_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDocumentsRequired"  runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvDocumentsRequired_ItemCommand" OnItemDataBound="gvDocumentsRequired_ItemDataBound" EnableViewState="false"
                ShowFooter="true" ShowHeader="true" OnNeedDataSource="gvDocumentsRequired_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELDOCUMENTID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkType" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblVesselDocumentIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELDOCUMENTID") %>'></telerik:RadLabel>
                                <eluc:Hard runat="server" ID="ucDocumentTypeEdit" HardTypeCode='<%# ((int)PhoenixHardTypeCode.CREWDOCUMENTTYPE).ToString() %>'
                                    ShortNameFilter="LIC" HardList='<%# PhoenixRegistersHard.ListHard(1, ((int)PhoenixHardTypeCode.CREWDOCUMENTTYPE), byte.Parse("0"), "LIC" ) %>'
                                    AppendDataBoundItems="false" CssClass="dropdown_mandatory" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'
                                    AutoPostBack="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard runat="server" ID="ucDocumentTypeAdd" HardTypeCode='<%# ((int)PhoenixHardTypeCode.CREWDOCUMENTTYPE).ToString() %>'
                                    HardList='<%# PhoenixRegistersHard.ListHard(1, ((int)PhoenixHardTypeCode.CREWDOCUMENTTYPE), byte.Parse("0"), "LIC" ) %>'
                                    AutoPostBack="true" CssClass="dropdown_mandatory" AppendDataBoundItems="false"
                                    ShortNameFilter="LIC" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Documents">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocuments" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.DOCUMENTNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDocumentIDEdit" runat="server" CommandName="EDIT"
                                    Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'>
                                </telerik:RadLabel>
                                <eluc:Documents runat="server" ID="ucDocumentsEdit" SelectedDocument='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'
                                    DocumentList='<%# PhoenixRegistersDocumentLicence.ListDocumentLicence(null, General.GetNullableInteger(ucRank.SelectedRank)) %>'
                                    CssClass="dropdown_mandatory" AutoPostBack="false"   Width="80%"/>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Documents runat="server" ID="ucDocumentsAdd" DocumentList='<%# PhoenixRegistersDocumentLicence.ListDocumentLicence(null, General.GetNullableInteger(ucRank.SelectedRank)) %>'
                                    CssClass="dropdown_mandatory" EnableViewState="true" AutoPostBack="false"  Width="80%"/>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Set" Visible="false">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSet" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSET") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtSetEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSET") %>'
                                    CssClass="gridinput_mandatory" MaxLength="2"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSetAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="2"
                                    ToolTip="Enter Set"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Flag Endorsement Y/N">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFlagEndYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGEND") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkFlagEndYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDFLAGENDYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkFlagEndYNAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Paid By Owner Directly">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaidByOwner" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAIDBYOWNER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkPaidByOwnerEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDPAIDBYOWNERDIRECTLY").ToString().Equals("1"))?true:false %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkPaidByOwnerAdd" runat="server" />
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
                                    <span class="icon"><i class="fas fa-trash"></i></span>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
