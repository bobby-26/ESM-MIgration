<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreSignOnDateCorrection.aspx.cs"
    Inherits="CrewOffshore_CrewOffshoreSignOnDateCorrection" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOnReason" Src="~/UserControls/UserControlSignOnReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew List</title>


    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
          <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvSignOff.ClientID %>"));
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
    <form id="form1" runat="server">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="CrewQuery" runat="server" TabStrip="true" OnTabStripCommand="CrewQuery_TabStripCommand"></eluc:TabStrip>

            <div>
                <table>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="UcVessel" runat="server"  AppendDataBoundItems="true"
                                AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" Width="200px" />
                        </td>
                    </tr>
                </table>
            </div>
           
           <eluc:TabStrip ID="CrewSignOff" runat="server" OnTabStripCommand="CrewSignOff_TabStripCommand"></eluc:TabStrip>



            <%--  <asp:GridView ID="gvSignOff" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="gvSignOff_RowDataBound" OnRowCommand="gvSignOff_RowCommand"
                    OnRowEditing="gvSignOff_RowEditing" OnRowCancelingEdit="gvSignOff_RowCancelingEdit"
                    AllowSorting="true" OnSorting="gvSignOff_Sorting" OnRowUpdating="gvSignOff_RowUpdating"
                    ShowHeader="true" EnableViewState="false" DataKeyNames="FLDSIGNONOFFID">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />--%>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvSignOff" runat="server" Height="100%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvSignOff_NeedDataSource"
                OnItemDataBound="gvSignOff_ItemDataBound"
                OnItemCommand="gvSignOff_ItemCommand"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
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
                      
                        <telerik:GridTemplateColumn HeaderText="Vessel">

                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELNAME"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No">

                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                         
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDFILENO"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" SortExpression="FLDNAME" AllowSorting="true">

                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOnOffid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONOFFID"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"]%>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkEmployeeName" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                    Text='<%# ((DataRowView)Container.DataItem)["FLDNAME"]%>' CommandName="GETEMPLOYEE"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                          
                        
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONRANKNAME"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                       
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNationality" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNATIONALITYNAME"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passport No">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                          
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassportNo" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDPASSPORTNO"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Daily Rate (USD)">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                          
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDailyRate" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDAILYRATE"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDailyRate" runat="server" Text=' <%# ((DataRowView)Container.DataItem)["FLDDAILYRATE"]%>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Daily DP Allowance (USD)">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                          
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDPRate" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDPALLOWANCE"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign On Date">
                            <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                         
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateJoined" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtSignOnDate" runat="server" CssClass="input_mandatory" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE"))%>'
                                    AutoPostBack="true" OnTextChangedEvent='txtSignOffDate_TextChanged' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-Off Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                         
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOffDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSeaPortId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONSEAPORTID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSignonreason" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONREASONID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSignOnOffIdAdd" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONOFFID"] %>'></telerik:RadLabel>

                            </ItemTemplate>
                            <%--<EditItemTemplate>
                                    <telerik:RadLabel ID="lblSignOnOffIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONOFFID"] %>'></telerik:RadLabel>
                                    <eluc:Date ID="txtSignOffDate" runat="server" CssClass="input_mandatory" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE"))%>'
                                        AutoPostBack="true" OnTextChangedEvent='txtSignOffDate_TextChanged' />
                                </EditItemTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-Off Reason">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                         
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignoffReason" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDSIGNOFFREASON") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSignoffReasonId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNOFFREASONID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSignOffIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNOFFREASONID"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%--<EditItemTemplate>
                                    <telerik:RadLabel ID="lblSignOffIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNOFFREASONID"] %>'></telerik:RadLabel>
                                    <eluc:SignOffReason runat="server" ID="ucSignOffReasonEdit" CssClass="input_mandatory"
                                        AppendDataBoundItems="true" SignOffReasonList='<%#SouthNests.Phoenix.Registers.PhoenixRegistersreasonssignoff.Listreasonssignoff() %>' />
                                </EditItemTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-Off Port">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                         
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOffPort" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNOFFSEAPORTNAME"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblEstimatedTravelDateHeader" runat="server" Text="Estimated Travel End Date"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEstimatedTravelDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDETOD")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtEstimatedTravelEndDate" runat="server" CssClass="input_mandatory"
                                    Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDETOD"))%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="End of Contract">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReliefDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblReliefDateEdit" runat="server" Visible="true" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE"))%>'></telerik:RadLabel>
                                <eluc:Date ID="txtReliefDateEdit" runat="server" Visible="false" CssClass="input_mandatory"
                                    Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDRELIEFDUEDATE"))%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Tour of Duty">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletionDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLD90RELIEFDATE"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lbl90ReliefDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLD90RELIEFDATE")) %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                      
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="EDIT"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit">
                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel">
                                     <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>




        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
