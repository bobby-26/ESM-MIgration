<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionCDISIREVerificationSummaryView.aspx.cs" Inherits="Inspection_InspectionCDISIREVerificationSummaryView" %>

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
    <title>Verification Summary View</title>
    <telerik:RadCodeBlock runat="server" ID="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

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
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status runat="server" ID="ucStatus" />
                    <eluc:TabStrip ID="MenuPhoenixQuery" runat="server" OnTabStripCommand="MenuPhoenixQuery_TabStripCommand"></eluc:TabStrip>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCDISIREMatrix" runat="server" AutoGenerateColumns="false" Font-Size="11px" GridLines="None"
                        Width="100%" AllowSorting="true" AllowPaging="false" Height="100%" CellPadding="3" OnNeedDataSource="gvCDISIREMatrix_NeedDataSource"
                        OnItemCommand="gvCDISIREMatrix_RowCommand" OnItemDataBound="gvCDISIREMatrix_ItemDataBound" ShowFooter="false">
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                            AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="Bottom"
                            GroupsDefaultExpanded="true" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="true" GroupLoadMode="Client" DataKeyNames="FLDCONTENTID">
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                            <HeaderStyle Width="102px" />
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
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblid1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOBJECTIVEEVIDENCE" ) %>'></telerik:RadLabel>
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
                                    <HeaderStyle VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <ItemStyle Wrap="true" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadCheckBox ID="chkSelect" runat="server" OnClick="EvidenceRequired" EnableViewState="false" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDEVIDENCEREQD").ToString().Equals("1")?true:false %>'  />
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
                                <telerik:GridTemplateColumn HeaderText="BPG Example">
                                    <HeaderStyle VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <ItemStyle Wrap="true" Width="100px"></ItemStyle>
                                    <ItemTemplate>
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
                                <telerik:GridTemplateColumn HeaderText="Office Remarks">
                                    <HeaderStyle VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <ItemStyle Wrap="true" Width="150px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblOfficeremarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEREMARKS" ) %>'></telerik:RadLabel>                                        
                                    </ItemTemplate>
                                    <%--                                    <FooterTemplate>
                                            <telerik:RadTextBox ID="txtObj" runat="server" CssClass="input" Rows="2" TextMode="MultiLine"
                                                ></telerik:RadTextBox>
                                    </FooterTemplate>--%>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Onboard Remarks">
                                    <HeaderStyle VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <ItemStyle Wrap="true" Width="150px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblOnboardremarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS" ) %>'></telerik:RadLabel>                                        
                                    </ItemTemplate>
                                    <%--                                    <FooterTemplate>
                                            <telerik:RadTextBox ID="txtObj" runat="server" CssClass="input" Rows="2" TextMode="MultiLine"
                                                ></telerik:RadTextBox>
                                    </FooterTemplate>--%>
                                </telerik:GridTemplateColumn>
                            </Columns>

                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Action" Visible="false">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTENTID" ) %>'></telerik:RadLabel>
                                        <asp:LinkButton runat="server" ID="Edit" CommandArgument="<%# Container.DataItem %>"
                                            CommandName="EDIT" ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                            ToolTip="Delete"><span class="icon"><i class="fas fa-trash"></i></asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Comments" CommandName="COMMENTS" ID="cmdComments"
                                            ToolTip="Comments"><span class="icon"><i class="fas fa-comments"></i></span></asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="ClientComments" CommandName="CLIENTCOMMENTS" ID="cmdClientComments"
                                            ToolTip="Client BPG"><span class="icon"><i class="fas fa-user-edit"></i></span></asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="OnboardChecks" CommandName="ONBOARDCHECKS" ID="cmdOnboardChecks"
                                            ToolTip="Onboard / Office Checks"><span class="icon"><i class="fas fa-clipboard-check"></i></span></asp:LinkButton>

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
    </form>
</body>
</html>
