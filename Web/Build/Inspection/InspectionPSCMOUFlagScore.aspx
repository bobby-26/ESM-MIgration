<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPSCMOUFlagScore.aspx.cs" Inherits="Inspection_InspectionPSCMOUFlagScore" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Flag </title>
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
    <form id="frmFlag" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Ship Type Score" Visible="false"></eluc:Title>
            <br />
            <table id="tblConfigure" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblFlag" runat="server" Text="Flag"></asp:Literal>
                    </td>
                        <td>
                            <eluc:Flag ID="ucFlag" runat="server" AppendDataBoundItems="true" OnTextChangedEvent="ucFlag_TextChangedEvent" AutoPostBack="true" />
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
                                <telerik:RadLabel ID="lblflagHeader" runat="server" >Flag&nbsp;</telerik:RadLabel>                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblflagId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGSCOREID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblflag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblflagEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGSCOREID") %>'></telerik:RadLabel>
                                 <eluc:Flag ID="ucFlagEdit" runat="server" FlagList='<%# PhoenixRegistersFlag.ListFlag() %>' SelectedFlag='<%# DataBinder.Eval(Container, "DataItem.FLDFLAGID") %>' AppendDataBoundItems="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Flag ID="ucFlagAdd" runat="server" FlagList='<%# PhoenixRegistersFlag.ListFlag() %>' AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                       <telerik:GridTemplateColumn HeaderText="PSC MOU">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblpscmouHeader" runat="server" >PSC MOU&nbsp;</telerik:RadLabel>                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpscmouId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGSCOREID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblpscmou" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPSCMOUREGION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblpscmouEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGSCOREID") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlpscmouregionNameedit" runat="server" Filter="Contains" ></telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlpscmouregionNameAdd" runat="server" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                       <telerik:GridTemplateColumn HeaderText="Flag Performance">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblflagperfHeader" runat="server" >Flag Performance&nbsp;</telerik:RadLabel>                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblflagperfId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGSCOREID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblflagperf" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGPERFORMANCENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblflagperfEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGSCOREID") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlflagperfedit" runat="server" Filter="Contains" ></telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlflagperfAdd" runat="server" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
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

                        <telerik:GridTemplateColumn   HeaderText="Flag is IMO Audited " HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIMOAudit" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDFLAGIMOAUDIT")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkIMOAuditEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDFLAGIMOAUDIT").ToString().Equals("YES"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkIMOAuditAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn   HeaderText="All Certificates Issued by Flag " HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCertificatesIssued" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCERTISSUEBYFLAG")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkCertificatesIssuedEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCERTISSUEBYFLAG").ToString().Equals("YES"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkCertificatesIssuedAdd" runat="server"></telerik:RadCheckBox>
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
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
