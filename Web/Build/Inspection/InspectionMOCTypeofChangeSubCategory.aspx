<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCTypeofChangeSubCategory.aspx.cs"
    Inherits="InspectionMOCTypeofChangeSubCategory" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MOC SubCategory</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvMOCSubCategory.ClientID %>"));
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
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadFormDecorator ID="rfdinstruction" RenderMode="LightWeight" runat="server"
                DecoratedControls="All" EnableRoundedCorners="true" DecorationZoneID="divFind"></telerik:RadFormDecorator>
            <table id="tblConfiguremoccategory">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCategory" runat="server" Width="270px" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                            AutoPostBack="true" AppendDataBoundItems="true" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Category">
                        </telerik:RadComboBox>
                        <%--<telerik:RadComboBox ID="ddlCategory" runat="server" Width="300px" CssClass="input_mandatory"
                                OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AppendDataBoundItems="true"
                                AutoPostBack="true" />--%>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuMOCSubCategory" runat="server" OnTabStripCommand="MOCSubCategory_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvMOCSubCategory" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvMOCSubCategory_ItemCommand"
                OnItemDataBound="gvMOCSubCategory_ItemDataBound" ShowHeader="true"
                EnableViewState="false" AllowSorting="true" OnSorting="gvMOCSubCategory_Sorting"
                AllowPaging="true" AllowCustomPaging="true" GridLines="None"
                OnNeedDataSource="gvMOCSubCategory_NeedDataSource" RenderMode="Lightweight"
                GroupingEnabled="false" EnableHeaderContextMenu="true">

                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" SortExpression="FLDSHORTCODE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <HeaderStyle Width="10%" Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="SubCategory" AllowSorting="true" SortExpression="FLDMOCSUBCATEGORYNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <HeaderStyle Width="25%" Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCSUBCATEGORYNAME").ToString().Length > 50 ? DataBinder.Eval(Container, "DataItem.FLDMOCSUBCATEGORYNAME").ToString().Substring(0, 50) + "..." : DataBinder.Eval(Container, "DataItem.FLDMOCSUBCATEGORYNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="SubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCSUBCATEGORYNAME") %>' />
                                <telerik:RadLabel ID="lblSubCategoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCSUBCATEGORYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Proposer Level">
                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                            <HeaderStyle Width="15%" Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProposer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSERROLE") %>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblProposerRole" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSERROLEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="4%" HeaderText="Info">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Proposer More Info" CommandName="PROPOSERINFO" ID="cmdProposerMoreInfo" ToolTip="Proposer More Info">
                                 <span class="icon"><i class="fas fa-user-tie"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=" Approval Level 1">
                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                            <HeaderStyle Width="15%" Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPermanantApprover" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERMANENTAPPROVERROLE") %>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblPermanantApproverRole" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERMANENTAPPROVERROLEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="4%" HeaderText="Info">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Permanant Approver More Info" CommandName="PERMANENTAPPROVERINFO" ID="cmdPermanantApproverMoreInfo" ToolTip="Permanant Approver More Info">
                                 <span class="icon"><i class="fas fa-user-tie"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=" Approval Level 2">
                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                            <HeaderStyle Width="15%" Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTempApprover" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPORARYAPPROVERROLE") %>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblTempApproverRole" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPORARYAPPROVERROLEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="4%" HeaderText="Info">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Temp Approver More Info" CommandName="TEMPAPPROVERINFO" ID="cmdTempApproverMoreInfo" ToolTip="Temp Approver More Info">
                                 <span class="icon"><i class="fas fa-user-tie"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                        
                        <telerik:GridTemplateColumn HeaderText="Responsible Person">
                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                            <HeaderStyle Width="15%" Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblResponsiblePerson" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESPONSIBLEPERSONROLE") %>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblresponsiblepersonrole" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESPONSIBLEPERSONROLEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="4%" HeaderText="Info">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Responsible Person More Info" CommandName="RESPONSIBLEINFO" ID="cmdresponsiblepersonmoreinfo" ToolTip="Responsible Person More Info">
                                 <span class="icon"><i class="fas fa-user-tie"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>

                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
