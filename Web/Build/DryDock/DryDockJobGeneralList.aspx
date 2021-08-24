<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockJobGeneralList.aspx.cs" Inherits="DryDockJobGeneralList" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <title>Untitled Page</title>
    <telerik:radcodeblock id="rad1" runat="server">
          <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvJobGeneral.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
        <script type="text/javascript" language="javascript">
            function checkSelectedYN(btn) {
                document.getElementById(btn).click();
            }
            function selectJob(jobid, obj) {
                AjxPost("functionname=selectjob|jobid=" + jobid + "|checked=" + obj.checked + "|jobregister=2", SitePath + "PhoenixWebFunctions.aspx", null, false);
            }
        </script>
    </telerik:radcodeblock>

</head>
<body>
    <form id="frmJobGeneralList" runat="server">
        <telerik:radscriptmanager id="radscript1" runat="server"></telerik:radscriptmanager>
        <telerik:radskinmanager id="radskin1" runat="server"></telerik:radskinmanager>

        <telerik:radajaxpanel id="RadAjaxPanel1" runat="server" loadingpanelid="RadAjaxLoadingPanel1" >
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            
                <eluc:TabStrip ID="MenuStandardJobGeneral" runat="server" OnTabStripCommand="StandardJobGeneral_TabStripCommand"
                    TabStrip="true"/>
            
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td style="padding-right:30px">
                        <telerik:RadLabel ID="lblJobNumber" runat="server" Text="Job Number"></telerik:RadLabel>
                    </td>
                    <td style="padding-right:30px">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtJobNumber" runat="server"  Width="220px" MaxLength="50"></telerik:RadTextBox>
                    </td>
                    <td style="padding-right:30px">
                        <telerik:RadLabel ID="lblJobTitle" runat="server" Text="Job Title"></telerik:RadLabel>

                    </td>
                    <td style="padding-right:30px">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtJobTitle" runat="server" Width="220px"></telerik:RadTextBox>
                        
                    </td>
                   <td style="padding-right:30px">
                        <telerik:RadLabel ID="lblJobType" runat="server" Text="Job Type"></telerik:RadLabel>

                    </td>
                   <td style="padding-right:30px">
                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlJobType" runat="server"  Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
        
                <eluc:TabStrip ID="MenuJobGeneral" runat="server" OnTabStripCommand="JobGeneral_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvJobGeneral" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None"
                OnNeedDataSource="gvJobGeneral_NeedDataSource" 
                OnItemDataBound="gvJobGeneral_ItemDataBound"
                OnItemCommand="gvJobGeneral_ItemCommand"
                OnUpdateCommand="gvJobGeneral_UpdateCommand"    EnableHeaderContextMenu="true"  GroupingEnabled="false"> 
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDJOBID" TableLayout="Fixed" Height="10px" >
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
                        <telerik:GridTemplateColumn >
                            <HeaderStyle Wrap="true"   />
                            <ItemTemplate>
                                <asp:ImageButton ID="cmdAttachments" runat="server" AlternateText="Attachment"  CommandName="ATTACHMENT" ToolTip="Attachment"     CommandArgument='<%# Container.DataSetIndex %>'    ImageUrl='Session["images"]/no-attachment.png'>
                                
                                </asp:ImageButton>
                                <asp:Label runat="server" ID="lnkbtn11"  ></asp:Label>
                                 
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Number" AllowSorting="true">    
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              <HeaderStyle Wrap="true"    Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              <HeaderStyle Wrap="true"     Width="25%" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTitle" runat="server" CommandName="VIEW"  CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Job Type" >
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              <HeaderStyle Wrap="true"     Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="RadLabel2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDJOBTYPE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Description">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                              <HeaderStyle Wrap="true"    Width="10%" />
                            <ItemTemplate>

                                <telerik:RadLabel ID="lblJobDescription" runat="server" Text='Details' ClientIDMode="AutoID"></telerik:RadLabel>
                                <eluc:ToolTip ID="ucJobDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDESCRIPTION") %>' TargetControlId="lblJobDescription" />


                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Type">
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>
                              <HeaderStyle Wrap="true"    Width="15%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDVESSELTYPENAME").ToString().TrimEnd(',')%>
                            </ItemTemplate>
                            <EditItemTemplate>
                              
                                    <telerik:RadListBox CheckBoxes="true" ID="cblVesselType" runat="server" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE"
                                        DataSource="<%# PhoenixRegistersHard.ListHard(1,81)%>" Width="100%">
                                    </telerik:RadListBox>
                                
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Select" >
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkSelectedYN" BackColor="Transparent"  CommandName="SELECTJOB"/>
                                <telerik:RadButton runat="server" ID="cmdSelectedYN" Visible="true" Text="<%# Container.DataSetIndex %>" CommandName="SELECTJOB" CommandArgument="<%# Container.DataSetIndex %>" Width="0px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle  VerticalAlign="Middle"></HeaderStyle>
                            
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>

                                </asp:LinkButton>
                                <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="VIEW" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdView"
                                    ToolTip="View" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-binoculars"></i></span>
                                </asp:LinkButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:LinkButton runat="server" AlternateText="Distribute"
                                    CommandName="DISTRIBUTE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDistribute"
                                    ToolTip="Distribute" Width="20px" Height="20px">
                                     <span class="icon"><i class="fas fa-shipping-fast"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img23" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"  width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
               <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <asp:Label id="Img1" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOverdue" runat="server" Text="* Vessel Job Description Changed By Office"></telerik:RadLabel>
                    </td>
                    <td>
                          <asp:Label id="Img2" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDue" runat="server" Text="* Vessel Job Description Changed By Vessel"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
    </telerik:radajaxpanel>
    </form>
</body>
</html>
