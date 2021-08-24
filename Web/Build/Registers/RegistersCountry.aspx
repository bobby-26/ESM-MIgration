<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersCountry.aspx.cs"
    Inherits="RegistersCountry" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Country</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

         <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= RadGrid1.ClientID %>"));
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
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <div class="demo-container" id="demo-container">
            <div class="gridPositioning">
                <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
                </telerik:RadWindowManager>
                <telerik:RadFormDecorator ID="RadFormDecorator" runat="server" DecoratedControls="All" DecorationZoneID="tblCountry" EnableRoundedCorners="true" />
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status runat="server" ID="ucStatus" />
                    <div class="demo-container no-bg size-wide">
                        <table id="tblCountry" width="100%">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtAbbreviation" runat="server" MaxLength="6" CssClass="input"></telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblEURegulation" runat="server" Text="EU Regulation"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlEURedulation" runat="server">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Value="Dummy" Text="--Select--" />
                                            <telerik:RadComboBoxItem runat="server" Value="1" Text="Yes" />
                                            <telerik:RadComboBoxItem runat="server" Value="0" Text="No" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblActive" runat="server" Text="Active"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlActive" runat="server">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Value="Dummy" Text="--Select--" />
                                            <telerik:RadComboBoxItem runat="server" Value="1" Text="Yes" />
                                            <telerik:RadComboBoxItem runat="server" Value="0" Text="No" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <eluc:TabStrip ID="MenuRegistersCountry" runat="server" OnTabStripCommand="RegistersCountry_TabStripCommand"></eluc:TabStrip>

                    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="RadGrid1" DecoratedControls="All" EnableRoundedCorners="true" />
                    <%--                <div id="demo" class="demo-container no-bg">--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" Height="90%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" OnSortCommand="RadGrid1_SortCommand" OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemDataBound="RadGrid1_ItemDataBound" ShowFooter="true"
                        OnItemCommand="RadGrid1_ItemCommand" AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDCOUNTRYCODE" TableLayout="Fixed" CommandItemDisplay="Top">
                            <HeaderStyle Width="102px" />
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Code" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDABBREVIATION">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAbbreviation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABBREVIATION") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtAbbreviationEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABBREVIATION") %>'
                                            CssClass="gridinput_mandatory" MaxLength="6" Width="100%">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtAbbreviationAdd" Width="100%" runat="server" CssClass="gridinput_mandatory" MaxLength="6">
                                        </telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="115px" AllowSorting="true" SortExpression="FLDCOUNTRYNAME">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCountryCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYCODE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblCountryName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkCountryName" runat="server" CommandName="EDIT"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblCountryCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYCODE") %>'></telerik:RadLabel>
                                        <telerik:RadTextBox ID="txtCountryNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'
                                            CssClass="gridinput_mandatory" MaxLength="200" Width="100%">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtCountryNameAdd" Width="100%" runat="server" CssClass="gridinput_mandatory" MaxLength="6">
                                        </telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Nationality" HeaderStyle-Width="115px" AllowSorting="true" SortExpression="FLDNATIONALITY">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtNationalityEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>'
                                            CssClass="gridinput_mandatory" MaxLength="200" Width="100%">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtNationalityAdd" runat="server" Width="100%" CssClass="gridinput_mandatory" MaxLength="6">
                                        </telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="ISD Code" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDISDCODE">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblISDCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISDCODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtISDCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISDCODE") %>'
                                            CssClass="gridinput" MaxLength="200" Width="100%">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtISDCodeAdd" runat="server" Width="100%" MaxLength="6">
                                        </telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Alpha2 Code" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDALPHA2CODE">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAplha2Code" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALPHA2CODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtAplha2CodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALPHA2CODE") %>'
                                             MaxLength="5" Width="100%">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtAplha2CodeAdd" runat="server" Width="100%" MaxLength="6">
                                        </telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Alpha3 Code" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDALPHA3CODE">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAplha3Code" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALPHA3CODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtAplha3CodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALPHA3CODE") %>'
                                             MaxLength="5" Width="100%">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtAplha3CodeAdd" runat="server" Width="100%" MaxLength="6">
                                        </telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Numerical Code" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDNUMERICALCODE">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblNumericalCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMERICALCODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtNumericalCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMERICALCODE") %>'
                                             MaxLength="5" Width="100%">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtNumericalCodeAdd" runat="server" Width="100%" MaxLength="6">
                                        </telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="FIPS Code" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDFIPSCODE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFIPSCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIPSCODE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtFIPSCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIPSCODE") %>'
                                            CssClass="input" MaxLength="5" Width="100%">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtFIPSCodeAdd" runat="server" Width="100%" MaxLength="6">
                                        </telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="EU Regulation YN" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDEUCOUNTRYYN">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEURegulation" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDEUCOUNTRYYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadCheckBox ID="chkEURegulationEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDEUCOUNTRYYN").ToString().Equals("1"))?true:false %>' Width="100%"></telerik:RadCheckBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadCheckBox ID="chkEURegulationAdd" runat="server" Width="100%" MaxLength="6">
                                        </telerik:RadCheckBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Active Y/N" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDEUCOUNTRYYN">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadCheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>' Width="100%"></telerik:RadCheckBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server" Width="100%" MaxLength="6">
                                        </telerik:RadCheckBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Sanctions Y/N" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDSANCTIONYN">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSanctionYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSANCTIONYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadCheckBox ID="chkSanctionYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSANCTIONYN").ToString().Equals("1"))?true:false %>' Width="100%"></telerik:RadCheckBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadCheckBox ID="chkSanctionYNAdd" runat="server" Width="100%" MaxLength="6">
                                        </telerik:RadCheckBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                 <telerik:GridTemplateColumn HeaderText="Display Budget Y/N" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDSANCTIONYN">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblbudgetYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDBUDGETYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadCheckBox ID="chkBudgetYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDBUDGETYN").ToString().Equals("1"))?true:false %>' Width="100%"></telerik:RadCheckBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadCheckBox ID="chkBudgetYNAdd" runat="server" Width="100%" MaxLength="6">
                                        </telerik:RadCheckBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
								<telerik:GridTemplateColumn HeaderText="Allow Visa" HeaderStyle-Width="75px" AllowSorting="false" >
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVisaAllow" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVISAALLOWEDYESNO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadCheckBox ID="chkVisaAllowedYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDVISAALLOWED").ToString().Equals("1"))?true:false %>' Width="100%"></telerik:RadCheckBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadCheckBox ID="chkVisaAllowedYNAdd" runat="server" Width="100%" >
                                        </telerik:RadCheckBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDREMARKS">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtRemarksEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                            CssClass="gridinput" MaxLength="200" Width="100%">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtRemarksAdd" runat="server" Width="100%" MaxLength="6">
                                        </telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                                    <HeaderStyle />
                                    <ItemStyle Wrap="false" />
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" CommandName="SEAPORT" ID="cmdSeaPort" ToolTip="Sea Port">                                    
                                    <span class="icon"><i class="fas fa-ship"></i></span> 
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" CommandName="VISA" ID="cmdVisa" ToolTip="Country Visa">
                                     <span class="icon"><i class="fab fa-cc-visa"></i></span>
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
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                        </asp:LinkButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>
            </div>
        </div>
    </form>
</body>
</html>
