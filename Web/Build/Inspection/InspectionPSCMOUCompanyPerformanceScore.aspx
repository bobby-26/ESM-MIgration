<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPSCMOUCompanyPerformanceScore.aspx.cs" Inherits="Inspection_InspectionPSCMOUCompanyPerformanceScore" %>


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Comapny Performance Matrix </title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvMenuPSCMOU").height(browserHeight - 40);
            });

        </script>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvMenuPSCMOU.ClientID %>"));
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
    <form id="frmClass" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Title runat="server" ID="ucTitle" Text="Ship Type Score" Visible="false"></eluc:Title>
                        <eluc:TabStrip ID="MenuOrderFormMain" runat="server" TabStrip="true" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"></eluc:TabStrip>
            <br />
            <table id="tblConfigure" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblpscmou" runat="server" Text="PSC MOU"></asp:Literal>
                    </td>
                        <td>
                            <telerik:RadComboBox ID="ddlCompany" runat="server" OnTextChanged="ddlCompany_TextChanged" AutoPostBack="true" Width="200px" >
                                
                            </telerik:RadComboBox>
                        </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuPSCMOU" runat="server" OnTabStripCommand="MenuPSCMOU_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvMenuPSCMOU" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None" 
                AllowPaging="true" AllowCustomPaging="true" Width="100%" CellPadding="3" OnNeedDataSource="gvMenuPSCMOU_NeedDataSource" OnSortCommand="gvMenuPSCMOU_SortCommand" OnItemCommand="gvMenuPSCMOU_ItemCommand"
                AllowSorting="true" ShowFooter="false" OnItemDataBound="gvMenuPSCMOU_ItemDataBound"
                ShowHeader="true" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDPERFORMANCEID">
                    <NoRecordsTemplate>
                        <table width="99.9%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                       <telerik:GridTemplateColumn HeaderText="PSC MOU" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblpscmouHeader" runat="server" >PSC MOU&nbsp;</telerik:RadLabel>                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpscmouId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblpscmou" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPSCMOUREGION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblpscmouEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlpscmouregionNameedit" runat="server" Filter="Contains" ></telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlpscmouregionNameAdd" runat="server" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                       <telerik:GridTemplateColumn HeaderText="Deficiency Index">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDefindexHeader" runat="server" >Deficiency Index&nbsp;</telerik:RadLabel>                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDefindexId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDefindex" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYINDEX") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDefindexEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlDefindexedit" runat="server" Filter="Contains" ></telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlDefindexAdd" runat="server" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                       <telerik:GridTemplateColumn HeaderText="Detention Index">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDetindexHeader" runat="server" >Detention Index&nbsp;</telerik:RadLabel>                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDetindexId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDetindex" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETENTIONINDEX") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDetindexEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlDetindexedit" runat="server" Filter="Contains" ></telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlDetindexAdd" runat="server" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                       <telerik:GridTemplateColumn HeaderText="Company Performance">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCompanyperfHeader" runat="server" >Company Performance&nbsp;</telerik:RadLabel>                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompanyperfId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompanygperf" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYPERFORMANCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCompanyperfEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERFORMANCEID") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlCompanyperfedit" runat="server" Filter="Contains" ></telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlCompanyperfAdd" runat="server" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Weightage">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPSCMOUWEIGHTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucScoreEdit" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPSCMOUWEIGHTAGE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucScoreAdd" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel" ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="ADD" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
