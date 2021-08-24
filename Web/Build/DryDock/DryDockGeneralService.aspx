<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockGeneralService.aspx.cs" Inherits="DryDockGeneralService" ValidateRequest="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Responsibilty" Src="~/UserControls/UserControlDiscipline.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:radcodeblock id="radcodeblock1" runat="server">
          <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
   
    <script type="text/javascript" language="javascript">
        function selectJobDetail(jobdetailid, obj) {
            AjxPost("functionname=selectjobdetail|jobdetailid=" + jobdetailid + "|checked=" + obj.checked, SitePath + "PhoenixWebFunctions.aspx", null, false);
        }
    </script>
          <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvGeneralServicesLineItem.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
         </telerik:radcodeblock>
</head>
<body>
    <form id="frmGeneralService" runat="server" autocomplete="off">
        <telerik:radscriptmanager id="radscript1" runat="server"></telerik:radscriptmanager>
        <telerik:radskinmanager id="radskin1" runat="server"></telerik:radskinmanager>

        <telerik:radajaxpanel id="AjaxPanel1" runat="server" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            
                <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
           
                <eluc:TabStrip ID="MenuGeneralServiceSpecification" runat="server" OnTabStripCommand="GeneralServiceSpecification_TabStripCommand"></eluc:TabStrip>

            
            <table  cellpadding="1" cellspacing="3">
                <tr>
                    <td style="padding-right:30px"  >
                        <telerik:RadLabel ID="lblJobType" runat="server" Text="Job Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlJobType" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" DefaultMessage="--Select--">
                        </telerik:RadComboBox>
                    </td>
                    <td rowspan="9">
                        <%--<b>Include</b>--%>
                        <telerik:RadCheckBoxList ID="cblInclude" runat="server" RepeatDirection="Vertical" Visible="false">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right:30px"  >
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtNumber" CssClass="input_mandatory" MaxLength="10"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right:30px"  >
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtTitle" CssClass="input_mandatory" MaxLength="100"
                            Width="360px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td style="padding-right:30px"  >
                        <telerik:RadLabel ID="lblJobDescription" runat="server" Text="Job Description"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtJobDescription" runat="server" CssClass="input_mandatory" Width="60%"
                            TextMode="MultiLine" Rows="6">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>

                    <td style="padding-right:30px"  >
                        <telerik:RadLabel ID="lblResponsibilty" runat="server" Text="Responsibilty"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlResponsibilty" runat="server"  AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr runat="server" visible="false">
                   <td style="padding-right:30px"  >
                        <telerik:RadLabel ID="lblEstimatedDurationHrs" runat="server" Text="Estimated Duration (Hrs)"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtDuration"  Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-right:30px"  >
                        <telerik:RadLabel ID="lblCostClassification" runat="server" Text="Cost Classification"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCostClassification" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" DefaultMessage="--Select--">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblWorktobesurveyedby" runat="server" Text="Work to be surveyed by  "></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadCheckBoxList ID="cblWorkSurvey" runat="server" RepeatDirection="Horizontal">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblMaterial" runat="server" Text="Material"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadCheckBoxList ID="cblMaterial" runat="server" RepeatDirection="Horizontal">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblEnclosed" runat="server" Text="Enclosed"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadCheckBoxList ID="cblEnclosed" runat="server" RepeatDirection="Horizontal">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right:30px"  >
                        <telerik:RadLabel ID="lblEstimatedDays" runat="server" Text="Estimated Days"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblEstimatedDays" runat="server" Direction="Horizontal">
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                  <tr>
                        <td>
                            <telerik:RadLabel ID="lblbudgetcode" runat="server" Text="Budgetcode"></telerik:RadLabel>
                        </td>
                        <td>
                           <telerik:RadComboBox runat="server" ID="radddlbudgetcode"  AllowCustomText
                                ="true" EmptyMessage="Type to select Budgetcode" Width="180px"/>
                        </td>
                    </tr>
            </table>
            <hr />
           
                <eluc:TabStrip ID="MenuDryDockGeneralServicesLineItem" runat="server" OnTabStripCommand="DryDockGeneralServicesLineItem_TabStripCommand"></eluc:TabStrip>
         
                <telerik:RadGrid RenderMode="Lightweight" ID="gvGeneralServicesLineItem" runat="server" 
                     AllowSorting="true" 

                    CellSpacing="0" GridLines="None"    GroupingEnabled="false" 
                    
                    OnNeedDataSource="gvGeneralServicesLineItem_NeedDataSource"
                    OnItemDataBound ="gvGeneralServicesLineItem_ItemDataBound1"
                    OnItemCommand ="gvGeneralServicesLineItem_ItemCommand"
                    OnUpdateCommand="gvGeneralServicesLineItem_UpdateCommand"
                    OnDeleteCommand ="gvGeneralServicesLineItem_DeleteCommand"  >
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDDTKEY" TableLayout="Fixed"   ShowFooter="true"     EnableHeaderContextMenu="true"  >
                       
                        <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                        
                         <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Include"   >
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="5%"></ItemStyle>
                                <HeaderStyle Width="15%" />
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkSelectedYN" Text="" BackColor="Transparent" />
                                    <asp:Button runat="server" ID="cmdSelectedYN" Visible="true" Text="<%# Container.DataSetIndex %>" CommandName="SELECTJOB" Width="0px"  />
                                </ItemTemplate>
                                 <FooterTemplate>
                                  
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Job Detail" >
                                <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                  <HeaderStyle Width="35%" />
                                <ItemTemplate>
                                 <telerik:RadLabel ID="lbljobdetailid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDETAILID") %>'></telerik:RadLabel>
                                  <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>                                    
                                    <telerik:RadLabel ID="lblDetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                 <telerik:RadLabel ID="lbljobdetailidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDETAILID") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtDetailEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                        CssClass="gridinput_mandatory" ToolTip="Job Detail"      Width="100%"   ></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtDetailAdd" runat="server" CssClass="gridinput_mandatory" Width="100%"
                                        ToolTip="Job Detail"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Unit" >
                               <HeaderStyle Width="25%" />
                                <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                
                                <ItemTemplate>
                                
                                    <telerik:RadLabel ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                               </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Unit ID="ucUnitEdit" runat="server" AppendDataBoundItems="true" UnitList='<%# PhoenixRegistersUnit.ListUnit()%>'
                                        SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNIT") %>'     Width="100%"    />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Unit ID="ucUnitAdd"  runat="server" AppendDataBoundItems="true" UnitList='<%# PhoenixRegistersUnit.ListUnit()%>'     Width="100%" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                             
                                <ItemStyle Wrap="False" HorizontalAlign="Center" width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit" 
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Move Up" 
                                        CommandName="MOVEUP" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdMoveUp"
                                        ToolTip="Move Up">
                                        <span class="icon"><i class="fas fa-angle-double-up"></i></span>
                                    </asp:LinkButton>                                        
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Move Down" 
                                        CommandName="MOVEDOWN" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdMoveDown"
                                        ToolTip="Move Down">
                                        <span class="icon"><i class="fas fa-angle-double-down"></i></span>
                                    </asp:LinkButton>
                                     <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete" 
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                        ToolTip="Delete">
                                         <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                    </asp:LinkButton>                                                                                                            
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                        CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                        ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel" 
                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>                                   
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                        CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                        ToolTip="Add New">
                                        <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                      
                    </MasterTableView>
                   <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                </telerik:RadGrid>
          
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"  Visible="false"/>
            <eluc:Status ID="ucStatus" runat="server" Visible="false"/>
     
        </telerik:radajaxpanel>
    </form>
</body>
</html>
