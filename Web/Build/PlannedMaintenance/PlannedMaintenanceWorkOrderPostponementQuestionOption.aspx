<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderPostponementQuestionOption.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceWorkOrderPostponementQuestionOption" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <%--<script type="text/javascript">
            function pageLoad() {
                PaneResized();
            }

            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvOption");
                grid._gridDataDiv.style.height = (browserHeight - 250) + "px";
            }
        </script>--%>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuRAPostponementQuestions" runat="server" OnTabStripCommand="MenuRAPostponementQuestions_TabStripCommand"></eluc:TabStrip>
            <table id="tblCargo" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblQuestion" runat="server" Text="Question"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtQuestion" runat="server" CssClass="input" Width="45%" TextMode="MultiLine" Resize="Both"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuOption" runat="server" OnTabStripCommand="MenuOption_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOption" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvOption_NeedDataSource" OnItemCommand="gvOption_ItemCommand"
                ShowFooter="true" ShowHeader="true" EnableViewState="true" Height="70%" OnSortCommand="gvOption_SortCommand"
                EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDOPTIONID">
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
                        <telerik:GridTemplateColumn HeaderText="Order No">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOptionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTIONORDERNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblOptionIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTIONID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtSortEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTIONORDERNO") %>'
                                    CssClass="gridinput_mandatory" MaxLength="3" Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSortAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="200" ToolTip="Enter Sort No" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Option" AllowSorting="true" SortExpression="FLDOPTIONNAME">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOption" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTIONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtOptionEdit" runat="server" CssClass="gridinput_mandatory" TextMode="MultiLine" Resize="Both"
                                    MaxLength="200" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTIONNAME") %>' Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtOptionAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                    ToolTip="Enter Option Name" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Active Y/N">
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActive" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISACTIVESTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkActive" runat="server" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDISACTIVE").ToString().Equals("1")? true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                               <telerik:RadCheckBox ID="chkActiveAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Comment Y/N">
                            <HeaderStyle Width="12%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComment" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISCOMMENTSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkComment" runat="server" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDISCOMMENT").ToString().Equals("1")? true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                               <telerik:RadCheckBox ID="chkCommentAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Comment Caption">
                            <HeaderStyle Width="23%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCommentCaption" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMENTCAPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCommentCaption" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMENTCAPTION") %>'
                                    TextMode="MultiLine" Resize="Both" Height="60px" Width="200px"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                               <telerik:RadTextBox ID="txtCommentCaptionAdd" runat="server" TextMode="MultiLine" Resize="Both" Height="60px" Width="200px"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
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
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
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
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
