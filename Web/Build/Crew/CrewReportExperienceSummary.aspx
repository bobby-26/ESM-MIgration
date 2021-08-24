<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportExperienceSummary.aspx.cs"
    Inherits="CrewReportExperienceSummary" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Short Contract</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="ReportNotRelievedOnTime" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
               
                    <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false">
                    </eluc:TabStrip>
              
                        <table width="100%">
                            <tr style="color: Blue">

                            &nbsp;&nbsp;
                    <span id="Span3" class="icon" style="align-self: center" runat="server"><font color="blue" size="2px">Note : To view the Guidelines, place the mouse pointer over the&nbsp;&nbsp;<i class="fas fa-question-circle"></i></span>&nbsp;&nbsp;button.</font>
                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="GuidlinesTooltip" Width="300px" ShowEvent="onmouseover" 
                        RelativeTo="Element" Animation="Fade" TargetControlID="Span3" IsClientID="true"
                        HideEvent="LeaveToolTip" Position="MiddleRight" EnableRoundedCorners="true" HideDelay="5000"
                        Text="Please note: <br/> 1) kindly select the course to be checked,course name selected will be listed if done. <br/> 2) Sea Time on this Type of Tanker is based on the vessel type selected in the filter in all ranks combined">
                    </telerik:RadToolTip>
                     
                            </tr>
                        </table>
                    
                        <table style="border-collapse: collapse;" border="1">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblPrinciple" runat="Server" Text="Principle"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Address ID="ddlPrinciple" AddressType="126" runat="server" AppendDataBoundItems="true"
                                                    CssClass="input_mandatory" />
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtFileNo" runat="server"></telerik:RadTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblShowExperiecein" runat="server" Text="Show Experience in"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="rblShowin" runat="server" RepeatDirection="Horizontal" EmptyMessage="Type to select Experience" Filter="Contains" MarkFirstMatch="true">
                                                    <items>
                                                    <telerik:RadComboBoxItem Value="1" Selected="True" Text="Years"></telerik:RadComboBoxItem>
                                                    <telerik:RadComboBoxItem Value="0" Text="Months"></telerik:RadComboBoxItem>
                                                    </items>
                                                </telerik:RadComboBox>
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblChooseBetween" runat="server" Text="Choose Between "></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="rblPrinciple" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="rblPrinciple_SelectedIndexChanged" RepeatDirection="Horizontal" EmptyMessage="Type to select Principle" Filter="Contains" MarkFirstMatch="true">
                                                    <items>
                                                    <telerik:RadComboBoxItem Value="0" Selected="True" Text="Primary Manager"></telerik:RadComboBoxItem>
                                                    <telerik:RadComboBoxItem Value="1" Text="Principle"></telerik:RadComboBoxItem>
                                                    </items>
                                                </telerik:RadComboBox>
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblShowAllRecords" runat="server" Text="Show Records"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadRadioButtonList ID="rblShowAllRecords" runat="server" Layout="Flow" Columns="2" Direction="Horizontal">
                                                    <items>
                                                    <telerik:ButtonListItem Value="1" Selected="True" Text="Last"></telerik:ButtonListItem>
                                                    <telerik:ButtonListItem Value="0" Text="ALL"></telerik:ButtonListItem>
                                                    </items>
                                                </telerik:RadRadioButtonList>
                                            </td>
                                        </tr>
                            </tr>
                        </table>
                        </td> </tr>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr valign="top">
                                        <td colspan="2">
                                            <telerik:RadLabel ID="lblVesselType" runat="server" Text=" Vessel Type"></telerik:RadLabel>

                                            <eluc:VesselType ID="lstVesselType" runat="server" AppendDataBoundItems="true" />

                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>

                                            <eluc:Vessel ID="lstVessel" runat="server" AppendDataBoundItems="true"
                                                VesselsOnly="true" Width="240px" EntityType="VSL" ActiveVesselsOnly="true" AssignedVessels="true" height="100px" />

                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>

                                            <eluc:Pool ID="lstPool" runat="server" AppendDataBoundItems="true" height="100px" />

                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>

                                            <eluc:Rank ID="lstRank" runat="server" AppendDataBoundItems="true" height="100px" width="250px"/>

                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>

                                            <telerik:RadListBox ID="lstCourse" runat="server" AppendDataBoundItems="true" SelectionMode="Multiple" Width="280px" height="100px">
                                            </telerik:RadListBox>

                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr> </table>
      
                <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                </eluc:TabStrip>

                <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" Height="55%"
                    AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
                    GridLines="None" OnSortCommand="gvCrew_SortCommand" OnNeedDataSource="gvCrew_NeedDataSource"
                    OnItemDataBound="gvCrew_ItemDataBound" ShowFooter="false" EnableViewState="false"
                    OnItemCommand="gvCrew_ItemCommand" AutoGenerateColumns="false">
                    <sortingsettings sortedbackcolor="#FFF6D6" enableskinsortstyles="false"></sortingsettings>
                    <mastertableview editmode="InPlace" insertitempageindexaction="ShowItemOnCurrentPage"
                        headerstyle-font-bold="true" showheaderswhennorecords="true" allownaturalsort="false"
                        autogeneratecolumns="false" datakeynames="FLDEMPLOYEEID" tablelayout="Fixed"
                        commanditemdisplay="Top">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false"
                    ShowRefreshButton="false" />
                    <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                    Font-Bold="true">
                                </telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" HeaderStyle-Width="50px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                       
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="File No" AllowSorting="false" HeaderStyle-Width="70px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        
                        
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDFILENO")%>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="175px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                       
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' />
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="70px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                       
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Batch" AllowSorting="false" HeaderStyle-Width="90px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                       
                           
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" HeaderStyle-Width="150px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                        
                            
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type of Licence" AllowSorting="false" HeaderStyle-Width="140px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        
                          
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDLICENCE")%>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="If DEC for Oil In Hands" AllowSorting="false" HeaderStyle-Width="105px"
                        ShowSortIcon="true">
                             <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                            
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDDCEYN")%>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Sea Time In Rank" AllowSorting="false" HeaderStyle-Width="100px"
                        ShowSortIcon="true">
                             <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                          
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDRANKEXPERIENCE")%>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Calendar Time with Operator" AllowSorting="false" HeaderStyle-Width="110px"
                        ShowSortIcon="true">
                             <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                            
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDOPERATOREXPERIENCE")%>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sea Time on this Type of Tanker" AllowSorting="false" HeaderStyle-Width="115px"
                        ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                           
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDTANKEREXP")%>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                       <telerik:GridTemplateColumn HeaderText="Sea Time on all Types on Tankers" AllowSorting="false" HeaderStyle-Width="125px"
                        ShowSortIcon="true">
                           <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                           
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDALLTANKEREXP")%>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Courses" AllowSorting="false" HeaderStyle-Width="90px"
                        ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                           
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCOURSELIST").ToString().TrimEnd(',') %>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                    </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </mastertableview>
                    <clientsettings enablerowhoverstyle="true" allowcolumnsreorder="true" reordercolumnsonclient="true"
                        allowcolumnhide="true" columnsreordermethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                    EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </clientsettings>
                </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
