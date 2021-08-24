<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMscatControlActionNeeded.aspx.cs" Inherits="InspectionMscatControlActionNeeded" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvControlActionNeeded").height(browserHeight - 40);
            });

        </script>
         <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvControlActionNeeded.ClientID %>"));
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
    <form id="frmBasicSubCause" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Control Action Needs" Visible="false"></eluc:Title>
            <table id="tblBasicCauseSearch" width="50%">
                <tr>
                    <td width="20%">
                        <telerik:RadLabel ID="lblAccidentDescription" runat="server" Text="Accident Description"></telerik:RadLabel>
                    </td>
                    <td width="30%">
                        <eluc:Hard ID="ddlcause" runat="server" OnTextChangedEvent="ddlcause_TextChanged" AppendDataBoundItems="true"
                            HardList='<%# PhoenixRegistersHard.ListHard(1, 206) %>' HardTypeCode="206" AutoPostBack="true" Width="90%" />
                    </td>
                </tr>
                <tr>
                    <td width="20%">
                        <telerik:RadLabel ID="lblContactType" runat="server" Text="Contact Type"></telerik:RadLabel>
                    </td>
                    <td width="30%">
                        <telerik:RadComboBox ID="ddlContactType" runat="server" AutoPostBack="true" AllowCustomText="true" EmptyMessage="Type to Select" Width="90%"
                            AppendDataBoundItems="true" OnTextChanged="ddlContactType_TextChanged">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td width="20%">
                        <telerik:RadLabel ID="lblImmediateCause" runat="server" Text="Immediate Cause"></telerik:RadLabel>
                    </td>
                    <td width="30%">
                        <telerik:RadComboBox runat="server" ID="ddlImmediateCause" AutoPostBack="true" OnTextChanged="ddlImmediateCause_indexchanged"
                            AllowCustomText="true" EmptyMessage="Type to Select" Width="90%">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td width="20%">
                        <telerik:RadLabel ID="lblBasicCause" runat="server" Text="Basic Cause"></telerik:RadLabel>
                    </td>
                    <td width="30%">
                        <telerik:RadComboBox ID="ddlBasicCause" runat="server" AutoPostBack="true" Width="90%"
                            AllowCustomText="true" EmptyMessage="Type to Select" OnTextChanged="ddlBasicCause_TextChanged">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuInspectionControlActionNeeds" runat="server" OnTabStripCommand="InspectionControlActionNeeds_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvControlActionNeeded" runat="server" AllowPaging="true" AllowCustomPaging="true"
                AutoGenerateColumns="False" Font-Size="11px" EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvControlActionNeeded_NeedDataSource"
                Width="100%" CellPadding="3" ShowFooter="true" AllowSorting="true"
                ShowHeader="true" EnableViewState="true" OnItemCommand="gvControlActionNeeded_ItemCommand" OnItemDataBound="gvControlActionNeeded_ItemDataBound">
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
                        <telerik:GridTemplateColumn HeaderText="S.No" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDSERIALNUMBER">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSequenceNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtSequenceNumberEdit" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="3" IsPositive="true" Width="45px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtSequenceNumberAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="3" IsPositive="true" Width="45px"></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Control Action Needs" HeaderStyle-Width="70%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblControlActionNeededId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLACTIONNEEDEDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblControlActionNeeded" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLACTIONNEEDED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblControlActionNeededIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLACTIONNEEDEDID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtControlActionNeededEdit" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLACTIONNEEDED") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtControlActionNeededAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200" Width="98%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" HeaderText="Description">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDescriptionHeader" runat="server">Description</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                    CssClass="gridinput" MaxLength="200"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtDescriptionAdd" runat="server" CssClass="gridinput" MaxLength="200"></asp:TextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="20%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20%"></ItemStyle>
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
                            <FooterStyle HorizontalAlign="Center" Width="60px" />
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
        </telerik:RadAjaxPanel>
        <eluc:Status runat="server" ID="ucStatus" />
    </form>
</body>
</html>
