<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreNewApplicantList.aspx.cs" Inherits="CrewOffshoreNewApplicantList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Query Activity</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
              <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvCrewSearch.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>


</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" DestroyOnClose="true"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="navigation" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <%--<eluc:TabStrip ID="CrewQueryActivity" runat="server" OnTabStripCommand="CrewQueryActivity_TabStripCommand"></eluc:TabStrip>--%>

            <eluc:TabStrip ID="CrewQueryMenu" runat="server" OnTabStripCommand="CrewQueryMenu_TabStripCommand"></eluc:TabStrip>


            <%-- <asp:GridView ID="gvCrewSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvCrewSearch_RowCommand" OnRowDataBound="gvCrewSearch_RowDataBound"
                    ShowHeader="true" EnableViewState="false" AllowSorting="true" OnSorting="gvCrewSearch_Sorting"
                    OnRowDeleting="gvCrewSearch_RowDeleting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSearch" runat="server" AutoGenerateColumns="False" Height="99%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvCrewSearch_NeedDataSource"
                OnItemCommand="gvCrewSearch_ItemCommand"
                OnItemDataBound="gvCrewSearch_ItemDataBound"
                OnSortCommand="gvCrewSearch_SortCommand"
                EnableHeaderContextMenu="true">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" DataKeyNames="FLDEMPLOYEEID">
                 <%--   <NestedViewSettings>
                        <ParentTableRelation>
                            <telerik:GridRelationFields MasterKeyField="FLDEMPLOYEEID" DetailKeyField="FLDEMPLOYEEID" />
                        </ParentTableRelation>
                    </NestedViewSettings>--%>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFirstnameHeader" runat="server">
                                    Name
                           
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblActiveyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME").ToString()%>'
                                    CommandName="GETEMPLOYEE"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Applied Rank"  AllowSorting ="true" SortExpression="FLDRANKNAME">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAppliedRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Contact No">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcontactno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHONENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Applied On">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAppliedOn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPLIEDON","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Last <BR/> Remarks">
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Zone" AllowFiltering="true" SortExpression="FLDZONE">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDZONE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CDC No">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSEAMANBOOKNO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="D.O.A.">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDDOA", "{0:dd/MMM/yyyy}") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTATUS") +" "+ DataBinder.Eval(Container, "DataItem.FLDSTATUSNAME")%>'
                                    ToolTip='<%#DataBinder.Eval(Container, "DataItem.FLDSTATUSDESCRIPTION")%>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsLoginCreated" runat="server" Visible="false"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDISLOGINCREATED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="D.O.B.">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDDATEOFBIRTH", "{0:dd/MMM/yyyy}") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn>
                            <HeaderStyle Width="20%" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Reversal"
                                    CommandName="REVERSAL" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdReversal"
                                    ToolTip="Recover Applicant">
                                    <span class="icon"><i class="fas fa-redo"></i></span>
                                </asp:LinkButton>

                               <%-- <asp:LinkButton Visible="false" runat="server" AlternateText="Delete"
                                    CommandName="GETEMPLOYEE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit">
                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>--%>

                                <asp:LinkButton ID="imgSendMail" runat="server" CommandName="SENDMAIL"
                                    ToolTip="Send Mail" CommandArgument="<%# Container.DataSetIndex %>">
                                     <span class="icon"><i class="fas fa-envelope"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton ID="imgActivity" CommandName="ACTIVITY" runat="server"
                                    ToolTip="Activities">
                                    <span class="icon"><i class="fa fa-pencil-ruler"></i></span> 
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="PD Form" CommandName="PDFORM"
                                    ID="cmdPDForm" ToolTip="PD Form">
                                     <span class="icon"><i class="fas fa-file"></i></span>  
                                </asp:LinkButton>

                                <asp:LinkButton ID="imgSuitableCheck" runat="server" CommandName="SUITABILITYCHECK"
                                    ToolTip="Suitability Check">
                                    <span class="icon"><i class="fas fa-user-astronaut"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton ID="imgRemark" runat="server" AlternateText="Remarks"
                                    CommandName="REMARKS" CommandArgument='<%# Container.DataSetIndex %>'
                                    ToolTip="Remarks">
                                    <span class="icon"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete">
                                     <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="SEAFARERLOGIN" ID="cmdSeafarerLogin" ToolTip="Create Seafarer Login">
                                    <span class="icon"><i class="fas fa-sign-in-alt-user"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <%--<NestedViewTemplate>
                        <table style="font-size: 11px; width: 60%">
                            <tr>

                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="Applied On :"></telerik:RadLabel>

                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblAppliedOn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPLIEDON","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </td>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="D.O.A :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblDOA" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDOA", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </td>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel3" runat="server" Text="D.O.B :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="RadLabel4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDATEOFBIRTH", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel5" runat="server" Text="Last Remarks :"></telerik:RadLabel>
                                </td>
                                <td colspan="5">
                                    <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' TargetControlId="lblRemarks" />
                                </td>
                            </tr>
                        </table>
                    </NestedViewTemplate>--%>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowExpandCollapse="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>


        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
