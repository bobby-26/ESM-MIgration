<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionTMSAMatrixCategoryContentArchivedList.aspx.cs" Inherits="Inspection_InspectionTMSAMatrixCategoryContentArchivedList" %>

<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TMSA Matrix</title>
    <telerik:RadCodeBlock runat="server" ID="DivHeader">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
        <style>
            /*Vertical Splitbars*/
            .rspCollapseBarExpand, .rspCollapseBarExpandOver,
            .rspCollapseBarCollapse, .rspCollapseBarCollapseOver {
                height: 35px !important; /*the height of your button-image */
                line-height: 35px !important; /*the height of your button-image */
                width: 10px !important;
                background-position: 0 !important;
            }

            .RadSplitter .rspCollapseBarExpand:before,
            .RadSplitter .rspCollapseBarCollapse:before {
                font-size: 14px !important;
                width: 10px !important;
            }
        </style>

        <script language="Javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31
               && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDocumentCategory" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager3"></telerik:RadWindowManager>

        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:TabStrip ID="MenuDocumentCategoryMain" runat="server" Title="Archived List" OnTabStripCommand="MenuDocumentCategoryMain_TabStripCommand"></eluc:TabStrip>

        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuPhoenixQuery" runat="server" OnTabStripCommand="MenuPhoenixQuery_TabStripCommand"></eluc:TabStrip>
            <table width="50%" runat="server">
                <tr>
                    <td>Reference Number</td>
                    <td>
                        <telerik:RadComboBox ID="ddlRefNo" runat="server" DataTextField="FLDREFERENCENUMBER" DataValueField="FLDREVIEWSCHEDULEID" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlRefNo_SelectedIndexChanged" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTMSAMatrix" runat="server" AutoGenerateColumns="false" Font-Size="11px" GridLines="None"
                Width="100%" AllowSorting="true" AllowPaging="false" Height="100%" CellPadding="3" OnNeedDataSource="gvTMSAMatrix_NeedDataSource"
                OnItemCommand="gvTMSAMatrix_RowCommand" EnableViewState="false" OnItemDataBound="gvTMSAMatrix_ItemDataBound" ShowFooter="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="Bottom"
                    GroupsDefaultExpanded="true" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="true" GroupLoadMode="Client" DataKeyNames="FLDCONTENTID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="120px" />
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldName="FLDCATEGORYNAME" HeaderText="Element" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDCATEGORYNAME" HeaderText="Element" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldName="FLDSTAGE" HeaderText="Stage" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDSTAGE" FieldAlias="Stage" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Procedures">
                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="80px"></HeaderStyle>
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="80px"></ItemStyle>
                            <ItemTemplate>                                
                             <asp:LinkButton runat="server" AlternateText="Procedure" CommandName="PROCDURE" ID="lnkprocedure" ToolTip="Procedures" Width="16px">
                                <span class="icon"><i class="fa-file-contract-af"></i></span>
                            </asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Responsibility">
                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="90px"></HeaderStyle>
                            <ItemStyle Wrap="true" Width="90px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblresposibility" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAMELIST" ) %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Comply">
                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" runat="server" Enabled="false" EnableViewState="true" OnClick="chkSelect_Click" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLIANCE").ToString().Equals("1")?true:false %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Objective Evidence">
                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="100px"></HeaderStyle>
                            <ItemStyle Wrap="true" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblid1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOBJECTIVEEVIDENCE" ) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <div id="divReports" runat="server" style="width: auto; border-width: 1px; border-style: solid; border: 0px solid #9f9fff">
                                    <table id="tblReports" runat="server">
                                    </table>
                                </div>
                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" Visible="false" ID="cmdAtt" ToolTip="Upload Evidence">
                                                 <span class="icon"> <i class="fa fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="BPG (E.g.)">
                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="50px"></HeaderStyle>
                            <ItemStyle Wrap="true" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="BPGATTACHMENT" ID="BPGAtt" ToolTip="BPG Example">
                                                 <span class="icon"> <i class="fa fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Accepted">
                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblaccepted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISOFFICEACCEPTED" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTENTID" ) %>'></telerik:RadLabel>
                                <asp:LinkButton runat="server" AlternateText="OfficeChecks" CommandName="OFFICECHECKS" ID="cmdOfficeChecks"
                                    ToolTip="Office Checks"><span class="icon"><i class="fas fa-user-astronaut"></i></span></asp:LinkButton>

                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="ADD" Visible="false" ID="cmdAdd"
                                    ToolTip="Add New"><span class="icon"><i class="fa fa-plus-circle"></i></span></asp:LinkButton>
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
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
