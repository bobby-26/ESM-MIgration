<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersOffshoreDocumentCourse.aspx.cs"
    Inherits="RegistersOffshoreDocumentCourse" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Course</title>
<telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersDocumentCourse" runat="server" submitdisabledcontrols="true">
    <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden"/>       
                    
                    <eluc:TabStrip ID="MenuCourseCost" runat="server" OnTabStripCommand="CourseCost_TabStripCommand" TabStrip="true">
                    </eluc:TabStrip>
            
                    <table id="tblConfigureDocumentCourse" >
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"   ></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtSearchCourse" runat="server" CssClass="input" ></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblDocumentCode" runat="server" Text="Code"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCourseCode" runat="server" CssClass="input"></telerik:RadTextBox>
                            </td>
                        
                            <td>
                                <telerik:RadLabel ID="lblDocumentType" runat="server" Text="Document Category"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Hard runat="server" ID="ucDocumentType" AppendDataBoundItems="true"
                                    HardTypeCode="103"  ShortNameFilter="0,1,2,3,5,6,7,8,"/>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input" Visible="false"/>
                            </td>
                            
                         </tr>
                         <tr>
                            <td>
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="input" Visible="false"/>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input" Visible="false"/>
                            </td>
                        </tr>      
                    </table>
                
                  <eluc:TabStrip ID="MenuRegistersDocumentCourse" runat="server" OnTabStripCommand="RegistersDocumentCourse_TabStripCommand">
                    </eluc:TabStrip>
                
                    <telerik:RadGrid ID="gvDocumentCourse" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                         CellPadding="3" OnItemCommand="gvDocumentCourse_ItemCommand" OnItemDataBound="gvDocumentCourse_ItemDataBound"
                        OnSorting="gvDocumentCourse_Sorting"
                        AllowSorting="true"  ShowFooter="false"
                        ShowHeader="true" EnableViewState="false" AllowPaging="true"  AllowCustomPaging="true" OnNeedDataSource="gvDocumentCourse_NeedDataSource" RenderMode="Lightweight"
                                GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"    >
                           
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed"  CommandItemDisplay="None" >
                    <NoRecordsTemplate>
                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                            Font-Bold="true">
                        </telerik:RadLabel>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                        <Columns>
                        
                            <telerik:GridTemplateColumn HeaderText="Document Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Wrap="true"  HorizontalAlign="Left"    Width="120px"  />
                               
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkDocumentType" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPENAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn    HeaderText="Document Category">
                                
                                <HeaderStyle Wrap="true"  HorizontalAlign="Left"      Width="165px"     />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"/>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME"))%>'></telerik:RadLabel>
                                </ItemTemplate>                             
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Course" AllowSorting="true" SortExpression="FLDCOURSE" Visible="true">    
                                 <HeaderStyle Wrap="true" HorizontalAlign="Left"   Width="300px"  />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"/>
                                
                                <HeaderTemplate>
                                    <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDCOURSE"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                  
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCourse" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                             

                            <telerik:GridTemplateColumn HeaderText="Active Y/N">
                                 <HeaderStyle Wrap="true"  HorizontalAlign="Left"     Width="100px"    />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"/>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLocalActive" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDLOCALACTIVE").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>                            
                            <telerik:GridTemplateColumn HeaderText="Expiry Y/N">
                                 <HeaderStyle Wrap="true"  HorizontalAlign="Left"      Width="100px"    />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"/>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblExpiry" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDEXPIRY").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Abbreviation" AllowSorting="true" SortExpression="FLDABBREVIATION">
                                 <HeaderStyle Wrap="true"       Width="100px"     HorizontalAlign="Left"  />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"/>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAbbreviation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABBREVIATION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn    HeaderText="Offshore Stage">
                                 <HeaderStyle Wrap="true"       Width="100px"     HorizontalAlign="Left"  />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"/>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStageName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGE") %>'></telerik:RadLabel>
                                </ItemTemplate>                             
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Offshore Mandatory Y/N">
                                 <HeaderStyle Wrap="true"       Width="100px"     HorizontalAlign="Left"  />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"/>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>  
                                 <telerik:RadLabel ID="lblMandatoryYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>                                                                      
                                </ItemTemplate>                             
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waiver Y/N" >
                               <HeaderStyle Wrap="true"       Width="100px"     HorizontalAlign="Left"  />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"/>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" /> 
                                <ItemTemplate>     
                                <telerik:RadLabel ID="lblWaiverYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDWAIVERYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>                                                                                                     
                                </ItemTemplate>                               
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="User Group to allow Waiver">
                              <HeaderStyle Wrap="true"       Width="100px"     HorizontalAlign="Left"  />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"/>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" /> 
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblUserGroup" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>'></telerik:RadLabel>
                                    <asp:LinkButton id="ImgUserGroup" runat="server"  ></asp:LinkButton>
                                    <%--ImageUrl="<%$ PhoenixTheme:images/te_view.png%>"--%>
                                    <eluc:Tooltip ID="ucUserGroup" runat="server" Width="180px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>' />
                                </ItemTemplate>                              
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Show in 'Additional Documents' on Crew Planner Y/N">

                              <HeaderStyle Wrap="true"       Width="100px"     HorizontalAlign="Left"  />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"/>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />  
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAdditionDocYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDADDITIONALDOCYNNAME")) %>'></telerik:RadLabel>
                                </ItemTemplate>                               
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Requires Authentication Y/N">
                                 <HeaderStyle Wrap="true"       Width="100px"     HorizontalAlign="Left"  />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"/>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAuthReqYnYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATIONREQYNNAME")) %>'></telerik:RadLabel>
                                </ItemTemplate>                               
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Show in Master's checklist onboard Y/N">
                                <HeaderStyle Wrap="true"       Width="100px"     HorizontalAlign="Left"  />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"/>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblShowMasterChecklistYn" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSHOWINMASTERCHECKLISTYN")) %>'></telerik:RadLabel>
                                </ItemTemplate>                               
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Photocopy acceptable Y/N">
                                <HeaderStyle Wrap="true"       Width="100px"     HorizontalAlign="Left"  />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"/>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" /> 
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPhotocopyYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDPHOTOCOPYACCEPTABLEYN")) %>'></telerik:RadLabel>
                                </ItemTemplate>                               
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Map in Competence subcategory Y/N">
                               <HeaderStyle Wrap="true"       Width="100px"     HorizontalAlign="Left"  />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"/>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />  
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCompetenceSubcategoryYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSHOWINSUBCATEGORYYN")) %>'></telerik:RadLabel>
                                </ItemTemplate>                               
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Single use Y/N">
                               <HeaderStyle Wrap="true"       Width="100px"     HorizontalAlign="Left"  />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"/>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSingleUseYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSINGLEUSEYNNAME")) %>'></telerik:RadLabel>
                                </ItemTemplate>                               
                            </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                        ScrollHeight="100%"  EnableNextPrevFrozenColumns="true" />  
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
     
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>