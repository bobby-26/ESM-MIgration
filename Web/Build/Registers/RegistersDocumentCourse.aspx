<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDocumentCourse.aspx.cs"
    Inherits="RegistersDocumentCourse" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentCategory" Src="~/UserControls/UserControlDocumentCategory.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Course</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersDocumentCourse" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuCourseCost" runat="server" OnTabStripCommand="CourseCost_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <%--<eluc:TabStrip ID="MenuTitle" runat="server" Title="Course"></eluc:TabStrip>--%>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table id="tblConfigureDocumentCourse" cellspacing="4" cellpadding="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSearchCourse" runat="server" CssClass="input" Width="250px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDocumentCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCourseCode" runat="server" CssClass="input" Width="250px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true"
                            Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDocumentType" runat="server" Text="Document Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <%-- <eluc:Hard runat="server" ID="ucDocumentType" AppendDataBoundItems="true"
                            HardTypeCode="103" Width="255px" />--%>
                        <eluc:Hard runat="server" ID="ucDocumentType" AppendDataBoundItems="true" HardTypeCode="103" Width="255px"
                            HardList='<%# PhoenixRegistersHard.ListHard(1, 103, 0, "0,1,2,3,5,6,7,10,9") %>' ShortNameFilter="0,1,2,3,5,6,7,10,9" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" Width="255px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCharter" runat="server" Text="Charterer"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCharter" runat="server" AllowCustomText="true" EmptyMessage="Type to Select" Width="250px">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Document Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddldoccategory" runat="server" AllowCustomText="true" EmptyMessage="Type to Select" Width="250px">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkincludeinactive" Text="Include Inactive" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersDocumentCourse" runat="server" OnTabStripCommand="RegistersDocumentCourse_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDocumentCourse" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvDocumentCourse_ItemCommand" OnNeedDataSource="gvDocumentCourse_NeedDataSource" Height="80%"
                OnSortCommand="gvDocumentCourse_SortCommand" OnItemDataBound="gvDocumentCourse_ItemDataBound" EnableViewState="false" GroupingEnabled="false"
                EnableHeaderContextMenu="true" ShowFooter="false">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
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
                        <telerik:GridTemplateColumn HeaderText="Document Type" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDocumentTypeHeader" runat="server">
                                    Document Type&nbsp;
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkDocumentType" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPENAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="140px" HeaderText="Document Category">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCategoryHeader" runat="server" Text="Document Category"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowSorting="true" HeaderText="Course" HeaderStyle-Width="200px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblCourseHeader" runat="server" CommandName="Sort" CommandArgument="FLDCOURSE">
                                    Course&nbsp;</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCourse" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRankHeader" runat="server" Text="Rank"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblRankName" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="RANKLIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDRANKNAME"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>   
                                <%--<telerik:RadLabel ID="lblRankName" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDRANKNAME"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDRANKNAME"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Vessel Type" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                           
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" ID="lblVesselType" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>  
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Owner" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblVesselTypeHeader" runat="server" Text="Owner"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" ID="lblowner" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDOWNERLIST"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>  
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Company" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                           
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" ID="lblCompany" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANIES"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>  
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                          <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Flag" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                          
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" ID="lblFlag" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDFLAG"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>  
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="G. Comp" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                          
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" ID="lblGcomp" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>  
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Maker" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                          
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" ID="lblmaker" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDMAKERNAME"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>  
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Vessels" HeaderStyle-Wrap="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                          
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" ID="lblappvessel" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDAPPLICABLEVESSEL"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>  
                                <%--<telerik:RadLabel ID="lblVesselType" runat="server" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>' Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE"))%>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N" HeaderStyle-Wrap="true" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblLocalActiveHeader" runat="server">
                                    Active Y/N
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLocalActive" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDLOCALACTIVE").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Y/N" HeaderStyle-Wrap="true" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblExpiryHeader" runat="server">
                                    Expiry Y/N
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpiry" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDEXPIRY").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Abbreviation" AllowSorting="true" Visible="false" SortExpression="FLDABBREVIATION" HeaderStyle-Width="90px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblAbbreviationHeader" runat="server" CommandName="Sort" CommandArgument="FLDABBREVIATION">
                                    Abbreviation&nbsp;</asp:LinkButton>
                                <%-- <img id="FLDABBREVIATION" runat="server" visible="false" />--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAbbreviation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABBREVIATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Stage">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStageName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderStyle-Wrap="true" HeaderText="Mandatory Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMandatoryYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderStyle-Wrap="true" HeaderText="Waiver Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWaiverYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderStyle-Wrap="true" HeaderText="Waivers" HeaderTooltip="SKK">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                 <asp:LinkButton runat="server" ID="lblwaiveruser" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="VESSELTYPELIST" CommandArgument="<%# Container.DataSetIndex %>" ToolTip='<%# (DataBinder.Eval(Container,"DataItem.FLDGROUPNAME"))%>'>                          
                                <span class="icon"><i class="fas fa-bell-slash"></i></span></asp:LinkButton>  
                                <telerik:RadLabel ID="lblUserGroup" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>' CssClass="tooltip" ClientIDMode="AutoID"></telerik:RadLabel>
                                <eluc:Tooltip ID="ucUserGroup" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>' TargetControlId="lblUserGroup" />
                                <%-- <asp:LinkButton ID="ImgUserGroup" runat="server"><i class="fas fa-glasses"></i> </asp:LinkButton>
                                <eluc:Tooltip ID="ucUserGroup" runat="server" Width="180px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>' />--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Add. Doc. on CP Y/N" HeaderStyle-Wrap="true" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="true" Width="190px" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAdditionDocYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDADDITIONALDOCYNNAME")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Req. Auth. Y/N" HeaderStyle-Wrap="true" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="true" Width="160px" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthReqYnYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATIONREQYNNAME")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderStyle-Wrap="true" HeaderText="Master's checklist Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShowMasterChecklistYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSHOWINMASTERCHECKLISTYN")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Photocopy Y/N" HeaderStyle-Wrap="true" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" Width="150px" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPhotocopyYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDPHOTOCOPYACCEPTABLEYN")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="ATTACHMENT" ID="cmdAtt" ToolTip="Upload Material" Visible="false">
                                        <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="CREATETEST" ID="cmdTest" ToolTip="Create Test Questions" Visible="false">
                                        <span class="icon"><i class="far fa-newspaper"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
