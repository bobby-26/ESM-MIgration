<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPSCMOUShipTypeScoreRegister.aspx.cs" Inherits="Inspection_InspectionPSCMOUShipTypeScoreRegister" %>

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
    <title>Ship Type </title>
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
    <form id="frmRASeverity" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Ship Type Score" Visible="false"></eluc:Title>
            <br />
            <table id="tblConfigure" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblShipType" runat="server" Text="Ship Type"></asp:Literal>
                    </td>
                        <td>
                            <eluc:Hard ID="ucVesselType" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="ucVesselType_TextChanged" HardList='<%# PhoenixRegistersHard.ListHard(1, 81) %>'
                                HardTypeCode="81" Width="270px" />
                        </td>
                    <td>
                        <asp:Literal ID="lblpscmou" runat="server" Text="PSC MOU"></asp:Literal>
                    </td>
                        <td>
                            <telerik:RadComboBox ID="ddlCompany" runat="server" OnTextChanged="ddlCompany_TextChanged" AutoPostBack="true" Width="200px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                        </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuPSCMOU" runat="server" OnTabStripCommand="MenuPSCMOU_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvMenuPSCMOU" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" CellPadding="3" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvMenuPSCMOU_NeedDataSource" OnSortCommand="gvMenuPSCMOU_SortCommand" OnItemCommand="gvMenuPSCMOU_ItemCommand"
                AllowSorting="true" ShowFooter="true" OnItemDataBound="gvMenuPSCMOU_ItemDataBound"
                ShowHeader="true" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="Ship Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblshiptypeHeader" runat="server" >Ship Type&nbsp;</telerik:RadLabel>                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblshiptypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPTYPESCOREID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblshiptype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblshiptypeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPTYPESCOREID") %>'></telerik:RadLabel>
                                <eluc:Hard ID="ucVesselTypeEdit" runat="server" AppendDataBoundItems="true" HardList='<%# PhoenixRegistersHard.ListHard(1, 81) %>'
                                    HardTypeCode="81" Width="270px" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPTYPE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ucVesselTypeAdd" runat="server" AppendDataBoundItems="true" HardList='<%# PhoenixRegistersHard.ListHard(1, 81) %>'
                                    HardTypeCode="81" Width="270px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Paris MOU">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblParisScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARISWEIGHTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucParisScoreEdit" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARISWEIGHTAGE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucParisScoreAdd" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Tokyo MOU">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTokyoScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOKYOWEIGHTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucTokyoScoreEdit" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOKYOWEIGHTAGE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucTokyoScoreAdd" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Latin American MOU">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLatinScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLATINWEIGHTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucLatinScoreEdit" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLATINWEIGHTAGE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucLatinScoreAdd" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Asia-Pacific MOU">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAsiaScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASIAWEIGHTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucAsiaScoreEdit" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASIAWEIGHTAGE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucAsiaScoreAdd" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Caribbean MOU ">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCaribbeanScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARIBBEANWEIGHTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucCaribbeanScoreEdit" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARIBBEANWEIGHTAGE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucCaribbeanScoreAdd" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Mediterranean MOU ">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMediterraneanScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEDITERRANEANWEIGHTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucMediterraneanScoreEdit" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEDITERRANEANWEIGHTAGE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucMediterraneanScoreAdd" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Indian MOU ">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIndianScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINDIANWEIGHTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucIndianScoreEdit" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINDIANWEIGHTAGE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucIndiannScoreAdd" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>


                       <telerik:GridTemplateColumn HeaderText="Black MOU ">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBlackScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBLACKWEIGHTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucBlackScoreEdit" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBLACKWEIGHTAGE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucBlackScoreAdd" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                     <telerik:GridTemplateColumn HeaderText="Riyadh MOU ">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRiyadhScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRIYADHWEIGHTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucRiyadhScoreEdit" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRIYADHWEIGHTAGE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucRiyadhScoreAdd" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                   <telerik:GridTemplateColumn HeaderText="USCG MOU ">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUSCGScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSCGWEIGHTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucUSCGScoreEdit" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                    MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSCGWEIGHTAGE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucUSCGScoreAdd" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
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
