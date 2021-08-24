<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsWageCostPlanning.aspx.cs"
    Inherits="CrewReportsWageCostPlanning" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Wage Cost Planning</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
       <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvWageCost.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
       
            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewWageCostPlanning" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuWageCostPlanning" runat="server" OnTabStripCommand="MenuWageCostPlanning_TabStripCommand"></eluc:TabStrip>
            <font color="blue">
            &nbsp Notes:<br />
            &nbsp  
                           &nbsp 1) For adding as seafarer-please use the last line<br />
            &nbsp
                           &nbsp 2) For replacing a seafarer kindly slect the edit button and choose the seafarer from drop down<br />
            &nbsp
                           &nbsp 3) For removing-please use the delete button &nbsp     </font>                      
                <br />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="padding-left: 10px">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" runat="server" CssClass="dropdown_mandatory" VesselsOnly="true" Width="210px"
                            AppendDataBoundItems="true" AssignedVessels="true" Entitytype="VSL" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrentWageScenarioAsOnDate" runat="server" Text="Current wage scenario as on date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtCurrentWageDate" runat="server" CssClass="input_mandatory" Width="210px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCrewChangePlanDate" runat="server" Text="Crew Change Plan Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtCrewChangeDate" runat="server" Width="210px" />
                    </td>
                </tr>

                <tr>
                    <td style="padding-left: 10px">
                        <telerik:RadLabel ID="lblBudgetedYTD" runat="server" Text="Budgeted YTD"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtBudgetedYTD" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            DecimalPlace="2" Width="210px" />
                    </td>
                    <td rowspan="3">
                        <telerik:RadLabel ID="lblManual" runat="server" Text="Manual"></telerik:RadLabel>
                    </td>
                    <td rowspan="3">
                        <telerik:RadCheckBox ID="chkManual" runat="server" AutoPostBack="true" OnCheckedChanged="chkManual_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 10px">
                        <telerik:RadLabel ID="lblActualYTD" runat="server" Text="Actual YTD"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtActualYTD" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            DecimalPlace="2" Width="210px" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 10px">
                        <telerik:RadLabel ID="lblVarianceYTD" runat="server" Text="Variance YTD"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVarianceYTD" runat="server" CssClass="txtNumber readonlytextbox"
                            ReadOnly="true" Width="210px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <br />

            <eluc:TabStrip ID="MenuWCPExcel" runat="server" OnTabStripCommand="MenuWCPExcel_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvWageCost" runat="server" AutoGenerateColumns="False" Font-Size="10px" AllowPaging="true" AllowCustomPaging="true"
                CellPadding="2" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvWageCost_ItemCommand" ShowFooter="true"
                OnItemDataBound="gvWageCost_ItemDataBound" CellSpacing="0" GridLines="None"  GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvWageCost_NeedDataSource" RenderMode="Lightweight" AllowSorting="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ForeColor="Black" ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <ColumnGroups>
                        
                        <telerik:GridColumnGroup HeaderText="Current Wage Scenario as on Date" Name="wage scenario" ParentGroupName="Part - A - Actual Onboard"
                            HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        
                        <telerik:GridColumnGroup HeaderText="Part - A - Actual Onboard" Name="Part - A - Actual Onboard"   HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        
                        <telerik:GridColumnGroup HeaderText="Crew Change Plan Date" Name="crew change" ParentGroupName="Part - B - Planner"     HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>

                        <telerik:GridColumnGroup HeaderText="Part - B - Planner" Name="Part - B - Planner"    HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>

                        <telerik:GridColumnGroup HeaderText="" Name="a"    HeaderStyle-HorizontalAlign="Center" ParentGroupName="b" >
                        </telerik:GridColumnGroup>

                        <telerik:GridColumnGroup HeaderText="" Name="b"    HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>

                    </ColumnGroups>
                    <HeaderStyle Width="100px" />
                   <FooterStyle VerticalAlign="Middle"/>

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="SrNo." ColumnGroupName="wage scenario">
                            
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSRNO")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" ColumnGroupName="wage scenario">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" ></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExtraCrew" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEXTRACREW")%>'></telerik:RadLabel>
                                <telerik:RadLabel
                                    ID="lblRankid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRANKID")%>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel
                                    ID="lblRefEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREFEMPLOYEEID")%>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel
                                    ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>'
                                    Width="90px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Employee Name" ColumnGroupName="wage scenario">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRefEmployeeName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREFEMPLOYEENAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank exp in Months" ColumnGroupName="wage scenario">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankExp" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRANKEXP")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="S/on Date" ColumnGroupName="wage scenario">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOnDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE")%>'
                                    Width="80px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="EOC Date" ColumnGroupName="wage scenario">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEOCDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEOCDATE")%>'
                                    Width="80px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Current Wages as on Date" ColumnGroupName="wage scenario">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWageDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>'
                                    Width="80px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="" ColumnGroupName="crew change"    >
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWageTotal" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBTOTAL")%>'
                                    Width="80px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Proposed Action" ColumnGroupName="crew change"  >

                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDPROPOSEDACTION")%> &nbsp;
                                    <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDTKEY")%>'></telerik:RadLabel>

                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="REVOKE" ID="cmdRevoke" ToolTip="Revoke Action">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" ColumnGroupName="crew change"  >
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="90px"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNewEmpid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'></telerik:RadLabel>
                                <telerik:RadLabel
                                    ID="lblRelieverRankid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDADDITIONALRANKID")%>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel
                                    ID="lblRelieverRank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDADDITIONALRANKNAME")%>'
                                    Width="90px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Rank ID="ucRankEdit" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    CssClass="input_mandatory" Width="90px" RankList='<%# PhoenixRegistersRank.ListRank() %>'
                                    SelectedRank='<%# DataBinder.Eval(Container, "DataItem.FLDRANKID")%>' Enabled="false" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" ColumnGroupName="crew change"  >
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px"></ItemStyle>
                            <FooterStyle VerticalAlign="Middle" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmpid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkCrew" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="ddlEmployeeidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlEmployeeEdit" runat="server" CssClass="input_mandatory" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Name"  Width="80%">
                                </telerik:RadComboBox>

                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListEmployeeAdd" runat="server" >
                                    <br /> <telerik:RadTextBox ID="txtEmployeeNameAdd" runat="server" CssClass="input_mandatory" Width="70%"></telerik:RadTextBox>
                                    <asp:LinkButton ID="btnShowEmployeeAdd" runat="server" Text=".." >
                                                <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtEmployeeIdAdd" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank Exp in Months" ColumnGroupName="crew change"  >

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRelieverRankExp" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNEWRANKEXP")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Combined Rank Exp with Onboard Officer" ColumnGroupName="crew change"  >

                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCOMBINEDRANKEXP")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DOA" ColumnGroupName="crew change"  >
                            <ItemStyle Wrap="true" HorizontalAlign="left" Width="80px"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRelieverDOA" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDOA")%>'
                                    Width="80px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Wages Cost will be" ColumnGroupName="crew change"  >
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRelieverWagesCost" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDWAGECOST")%>'
                                    Width="80px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtWageCostEdit" runat="server" DecimalPlace="2" Width="120px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtWageCostAdd" runat="server" CssClass="input_mandatory" IsInteger="true" Width="85%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="" ColumnGroupName="a"  >
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRelieverWageTotal" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPLANSUBTOTAL")%>'
                                    Width="80px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" ColumnGroupName="a">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />

                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Remove the seafarer from this list">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>

                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave"
                                    ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel"
                                    ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>

                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Add New" CommandName="Add" ID="cmdAdd">
                                        <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>

                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="false" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

             <table >
                    <tr>
                        <td>
                            <table>
                                <tr class="rowred">
                                    <td width="5px" height="10px">
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            * Extra Crew
                        </td>
                    </tr>
                </table>
        
         <eluc:Status ID="ucStatus" runat="server" Visible="false" />
        </telerik:RadAjaxPanel>
       
    </form>
</body>
</html>
