<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionCDISIREMatrixCategoryContentList.aspx.cs" EnableEventValidation="false" Inherits="Inspection_InspectionCDISIREMatrixCategoryContentList" %>

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
    <title>Document Category</title>
    <telerik:RadCodeBlock runat="server" ID="DivHeader">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <style>
        /*Vertical Splitbars*/
        .rspCollapseBarExpand, .rspCollapseBarExpandOver,
        .rspCollapseBarCollapse, .rspCollapseBarCollapseOver{
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
        <script type="text/javascript">
            function confirmReview(args) {
                if (args) {
                    __doPostBack("<%=confirmReview.UniqueID %>","");
                }
            }
        </script>
        <script type="text/javascript">
            function confirmCDISIREUnlock(args) {
                if (args) {
                    __doPostBack("<%=confirmCDISIREUnlock.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDocumentCategory" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager2"></telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxPanel1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tvwDocumentCategory">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="tvwDocumentCategory" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel2" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:TabStrip ID="MenuDocumentCategoryMain" runat="server" Title="CDI/SIRE Matrix" OnTabStripCommand="MenuDocumentCategoryMain_TabStripCommand"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" SplitBarsSize="10" Height="93%" Width="100%">
            <telerik:RadPane ID="navigationPane" runat="server" Width="30%" Height="93%">
                <eluc:TreeView runat="server" ID="tvwDocumentCategory" RootText="ROOT" OnNodeClickEvent="ucTree_SelectNodeEvent"></eluc:TreeView>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward" Height="100%">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server" Width="70%" Height="93%">
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="80%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status runat="server" ID="ucStatus" />
                    <table>
                        <tr style="position: absolute">
                            <telerik:RadLabel runat="server" ID="lblSelectedNode"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCategoryId" runat="server"></telerik:RadLabel>
                        </tr>
                    </table>
                    <br />
                    <table width="50%">
                        <tr>
                            <td>Client</td>
                            <td>
                                <telerik:RadComboBox ID="ddlCompany" runat="server" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" AutoPostBack="true"
                                    DataTextField="FLDOILMAJORCOMPANYNAME" DataValueField="FLDOILMAJORCOMPANYID" Width="200px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="100%" cellpadding="5" runat="server" id="configpart" visible="false">
                        <tr>
                            <td>Category Name
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDocumentCategory" CssClass="input_mandatory" Width="180px" Height="20px" MaxLength="100" runat="server"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Category Number
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCategoryNumber" runat="server" CssClass="gridinput_mandatory"
                                    onkeypress="return isNumberKey(event)" Width="180px" Height="20px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Category Short Code
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtcategoryshortcode" CssClass="input_mandatory" Width="180px" Height="20px" MaxLength="100" runat="server"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Company</td>
                            <td>
                                <eluc:Company ID="ucCompany" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="180px" Height="20px" />
                            </td>
                        </tr>
                        <tr>
                            <td>Active
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkActiveyn" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>No of Columns
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtnoofcolumns" runat="server" CssClass="gridinput_mandatory"
                                    onkeypress="return isNumberKey(event)" Width="180px" Height="20px" AutoPostBack="true" OnTextChanged="txtnoofcolumns_changed">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:Button ID="btn" runat="server" Visible="false" Text="Submit" OnClick="btn_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="Pnlcolumnlabel" runat="server">
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlcolumns" runat="server">
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <%--                <eluc:TabStrip ID="MenuPhoenixQuery" runat="server" OnTabStripCommand="MenuPhoenixQuery_TabStripCommand">
                </eluc:TabStrip>--%>
                        </tr>
                    </table>
                    <eluc:TabStrip ID="MenuPhoenixQuery" runat="server" OnTabStripCommand="MenuPhoenixQuery_TabStripCommand"></eluc:TabStrip>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCDISIREMatrix" runat="server" AutoGenerateColumns="false" Font-Size="11px" GridLines="None"
                        Width="100%" AllowSorting="true" AllowPaging="false" Height="100%" CellPadding="3" OnNeedDataSource="gvCDISIREMatrix_NeedDataSource"
                        OnItemCommand="gvCDISIREMatrix_RowCommand" OnItemDataBound="gvCDISIREMatrix_ItemDataBound" ShowFooter="false">
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                            AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="Bottom"
                            GroupsDefaultExpanded="true" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="true" GroupLoadMode="Client" DataKeyNames="FLDCONTENTID">
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                            <%--<HeaderStyle Width="102px" />--%>
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldName="FLDCATEGORYNAME"  FieldAlias="Category" />
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="FLDCATEGORYNAME" FieldAlias="Category" />
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Procedures">
                                    <HeaderStyle VerticalAlign="Middle" Width="200px"></HeaderStyle>
                                    <ItemStyle Wrap="true" Width="200px"></ItemStyle>
                                    <ItemTemplate>
                                        <div id="divForms" runat="server" style="width: auto; border-width: 1px; border-style: solid; border: 0px solid #9f9fff">
                                            <table id="tblForms" runat="server">
                                            </table>
                                        </div>
                                    </ItemTemplate>
<%--                                    <FooterTemplate>
                                        <span id="spnPickListCategoryAdd">
                                            <telerik:RadTextBox ID="txtCategoryAdd" runat="server" Width="170px" Enabled="False" CssClass="input_mandatory"></telerik:RadTextBox>
                                            <asp:LinkButton ID="btnShowCategoryAdd" runat="server" Text="..">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                            </asp:LinkButton>
                                            <telerik:RadTextBox ID="txtCategoryidAdd" runat="server" Width="1px" CssClass="hidden"></telerik:RadTextBox>
                                        </span>
                                    </FooterTemplate>--%>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Objective Evidence">
                                    <HeaderStyle VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <ItemStyle Wrap="true" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblid1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOBJECTIVEEVIDENCE" ) %>'></asp:LinkButton>
                                        <div id="divReports" runat="server" style="width: auto; border-width: 1px; border-style: solid; border: 0px solid #9f9fff">
                                            <table id="tblReports" runat="server">
                                            </table>
                                        </div>
                                    </ItemTemplate>
<%--                                    <FooterTemplate>
                                            <telerik:RadTextBox ID="txtObj" runat="server" CssClass="input" Rows="2" TextMode="MultiLine"
                                                ></telerik:RadTextBox>
                                    </FooterTemplate>--%>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Responsibility">
                                    <HeaderStyle VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <ItemStyle Wrap="true" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblresposibility" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAMELIST" ) %>'></telerik:RadLabel>
                                    </ItemTemplate>
<%--                                    <FooterTemplate>
                                        <div id="dvClass" runat="server" class="input" style="overflow: auto; width: 100%;
                                            height: 95px">
                                            <telerik:RadCheckBoxList ID="chkDeptList" runat="server" CssClass="input" Direction="Vertical" Enabled="true" Layout="Flow" 
                                            Height="100%" RepeatDirection="Vertical" Width="100%">
                                            </telerik:RadCheckBoxList>
                                        </div>
                                    </FooterTemplate>--%>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Evidence Required">
                                    <HeaderStyle VerticalAlign="Middle" Width="70px"></HeaderStyle>
                                    <ItemStyle Wrap="true" Width="70px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadCheckBox ID="chkSelect" runat="server" EnableViewState="false" OnClick="EvidenceRequired" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDEVIDENCEREQD").ToString().Equals("1")?true:false %>'  />
                                        <telerik:RadLabel ID="lblDTkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblrequired" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVIDENCEREQD") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                        <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" Visible="false" ID="cmdAtt" ToolTip="Upload Evidence">
                                                 <span class="icon"> <i class="fa fa-paperclip"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
<%--                                    <FooterTemplate>
                                        <div id="dvClass" runat="server" class="input" style="overflow: auto; width: 100%;
                                            height: 95px">
                                            <telerik:RadCheckBoxList ID="chkDeptList" runat="server" CssClass="input" Direction="Vertical" Enabled="true" Layout="Flow" 
                                            Height="100%" RepeatDirection="Vertical" Width="100%">
                                            </telerik:RadCheckBoxList>
                                        </div>
                                    </FooterTemplate>--%>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="BPG (E.g.)">
                                    <HeaderStyle VerticalAlign="Middle" Width="50px"></HeaderStyle>
                                    <ItemStyle Wrap="true" Width="50px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblBPGAttach" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISBPGATTACHMENT") %>'></telerik:RadLabel>
                                        <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="BPGATTACHMENT" ID="BPGAtt" ToolTip="BPG Example">
                                                 <span class="icon"> <i class="fa fa-paperclip"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
<%--                                    <FooterTemplate>
                                        <div id="dvClass" runat="server" class="input" style="overflow: auto; width: 100%;
                                            height: 95px">
                                            <telerik:RadCheckBoxList ID="chkDeptList" runat="server" CssClass="input" Direction="Vertical" Enabled="true" Layout="Flow" 
                                            Height="100%" RepeatDirection="Vertical" Width="100%">
                                            </telerik:RadCheckBoxList>
                                        </div>
                                    </FooterTemplate>--%>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Onboard/Office Status">
                                    <HeaderStyle VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <ItemStyle Wrap="true" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblonboardofficestatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONBOARDOFFICESTATUS" ) %>'></telerik:RadLabel>                                        
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="130px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTENTID" ) %>'></telerik:RadLabel>
                                        <asp:LinkButton runat="server" ID="Edit" CommandArgument="<%# Container.DataItem %>"
                                            CommandName="EDIT" ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                            ToolTip="Delete"><span class="icon"><i class="fas fa-trash"></i></asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Comments" CommandName="COMMENTS" ID="cmdComments"
                                            ToolTip="Comments"><span class="icon"><i class="fas fa-comments"></i></span></asp:LinkButton>
                                        <asp:ImageButton runat="server" AlternateText="ClientComments" ImageUrl="<%$ PhoenixTheme:images/vendor-detail.png %>" CommandName="CLIENTCOMMENTS" ID="cmdClientComments"
                                            ToolTip="Client BPG"></asp:ImageButton>
                                        <asp:LinkButton runat="server" AlternateText="OnboardChecks" CommandName="ONBOARDCHECKS" ID="cmdOnboardChecks"
                                            ToolTip="Onboard / Office Checks"><span class="icon"><i class="fas fa-check-square-ec"></i></span></asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Archive" Visible="false" CommandName="ARCHIVE" ID="cmdarchive"
                                            ToolTip="Archive List"><span class="icon"><i class="fas fa-download"></i></span></asp:LinkButton>
                                        <%--                        <telerik:RadLabel ID="lblprocid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDUREID" ) %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="lblcategoryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTMSACATEGORYID" ) %>'></telerik:RadLabel>--%>
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
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="50" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />                            
                            <Scrolling AllowScroll="true" UseStaticHeaders="false" SaveScrollPosition="true" ScrollHeight="100%" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                    <asp:Button ID="confirmReview" runat="server" Text="confirm" OnClick="confirmReview_Click" />
                    <asp:Button ID="confirmCDISIREUnlock" runat="server" Text="confirmUnlock" OnClick="confirmCDISIREUnlock_Click" />
                </telerik:RadAjaxPanel>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
