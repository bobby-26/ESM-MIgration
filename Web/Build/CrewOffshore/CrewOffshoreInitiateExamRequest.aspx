

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreInitiateExamRequest.aspx.cs" Inherits="CrewOffshoreInitiateExamRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
       <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmExamReq" runat="server" submitdisabledcontrols="true">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
    <telerik:RadAjaxPanel runat="server" ID="pnlExamReq" >
            <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
            <eluc:status id="ucStatus" runat="server" />
                    <eluc:tabstrip id="CrewMenu" runat="server" Title="Test History" ontabstripcommand="CrewMenu_TabStripCommand"></eluc:tabstrip>
                <table runat="server" id="tblExam" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblExam" runat="server" Text="Exam" Visible="false"></telerik:RadLabel>
                        </td>
                        <td>                                                        
                            <telerik:RadComboBox ID="ddlExam" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Visible="false"></telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTargetDate" runat="server" Text="Target Date" Visible="false"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtTargetDate" runat="server" CssClass="input_mandatory" Visible="false" />
                        </td>
                    </tr>
                </table> 
            
                   <table width="100%">
                <tr align="left">
                      <td><telerik:RadLabel ID="lblfileno"  runat="server" Text="File No."></telerik:RadLabel>

                    </td>
                    <td>
                         <telerik:RadTextBox ID="txtfileno" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblfirstname"  runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td >
                        <telerik:RadTextBox ID="lblfname" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRANKname"  runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>                        
                        <telerik:RadTextBox ID="lblrname" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                                 
                </tr>
               </table>
                    <eluc:TabStrip ID="CrewQueryMenu" runat="server" OnTabStripCommand="CrewQueryMenu_TabStripCommand">
                    </eluc:TabStrip>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvExamRequest" runat="server" AutoGenerateColumns="False" Font-Size="11px" height="335px" OnNeedDataSource="gvExamRequest_NeedDataSource"
                        Width="100%" CellPadding="3" OnItemDataBound="gvExamRequest_ItemDataBound" GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="true" AllowCustomPaging="true" 
                        ShowHeader="true" EnableViewState="false" AllowSorting="true" OnSortCommand="gvExamRequest_SortCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
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
                            <telerik:GridTemplateColumn HeaderText="Date Attended">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDateAttended" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEATTENDED")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>                                  
                            <telerik:GridTemplateColumn HeaderText="Vessel">
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblvesselhead" runat="server" Text="Vessel" ></telerik:RadLabel>
                                     <asp:LinkButton Visible="false" ID="lnkVesselHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME"
                                         Text="Vessel"></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn Visible="false" HeaderText="Employee Name">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                         Text="Employee Name"></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblEmployeeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkEmployeeName" Visible="false" runat="server" CommandArgument="<%#Container.DataItem%>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME")%>' CommandName="GETEMPLOYEE"></asp:LinkButton>                                    
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn Visible="false" HeaderText="File No">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>                            
                            <telerik:GridTemplateColumn HeaderText="Rank">
                                 <ItemTemplate>
                                    <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Course">
                                <HeaderTemplate>
                                     <telerik:RadLabel ID="lblcoursehead" runat="server" Text="Course" ></telerik:RadLabel>
                                    <asp:LinkButton Visible="false"  ID="lnkCourseHeader" runat="server" CommandName="Sort" CommandArgument="FLDCOURSENAME"
                                        Text="Course"></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCourse" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Exam">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblExam" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Target Date" Visible="false">
                                 <ItemTemplate>
                                    <telerik:RadLabel ID="lblTargetDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>  
                            <telerik:GridTemplateColumn HeaderText="Score">
                                 <ItemTemplate>
                                    <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>    
                            <telerik:GridTemplateColumn HeaderText="Result">
                              <ItemTemplate>
                                    <telerik:RadLabel ID="lblExamResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMRESULTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn> 
                              
                        </Columns>
                     <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                    </MasterTableView>
                          <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                    </telerik:RadGrid>
                  </telerik:RadAjaxPanel>
    </form>
</body>
</html>
