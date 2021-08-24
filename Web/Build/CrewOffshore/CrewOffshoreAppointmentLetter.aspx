<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreAppointmentLetter.aspx.cs" Inherits="CrewOffshoreAppointmentLetter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BankAccount" Src="~/UserControls/UserControlEmployeeBankAccount.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Appointment Letter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCrewAppointmentLetter" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">

                <%--<eluc:Title runat="server" ID="ucTitle" Text="Appointment Letter" ShowMenu="false" />--%>
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

                <eluc:TabStrip ID="CrewMenu" runat="server" Title="Appointment Letter" OnTabStripCommand="CrewMenu_TabStripCommand"></eluc:TabStrip>

                <table runat="server" id="tblPersonalMaster" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDOB" runat="server" Text="DOB"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucDOB" runat="server" CssClass="readonlytextbox" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPlaceofBirth" runat="server" Text="Place of Birth"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtPlaceOfBirth" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRankNationality" runat="server" Text="Rank / Nationality"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtRank" runat="server" CssClass="input readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            /
                            <telerik:RadTextBox ID="txtNationality" runat="server" CssClass="input readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblSeamanBookNo" runat="server" Text="Seaman Book No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSeamanBook" runat="server" CssClass="input readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAddress" runat="server" Text="Address"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtAddress" runat="server" CssClass="readonlytextbox" Width="670px"
                                ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Proposed Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRegOwner" runat="server" Text="Reg. Owner"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtRegOwner" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVesselAddress" runat="server" Text="Address"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtVesselAddress" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="670px"></telerik:RadTextBox>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblIMONo" runat="server" Text="IMO No."></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtIMONo" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td colspan="3"></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPlaceofEngagement" runat="server" Text="Place of Engagement"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:SeaPort ID="ddlSignOnSeaPort" runat="server"  AppendDataBoundItems="true" Width="150px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPlaceOfRepatriation" runat="server" Text="Place of Repatriation"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:SeaPort ID="ddlSignOffPort" runat="server"  AppendDataBoundItems="true" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblContractStartDate" runat="server" Text="Contract commencement date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucContractStartDate" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblContractCancellationDate" runat="server" Text="Contract cancellation date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucContractCancellationDate" runat="server"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDailyWages" runat="server" Text="Daily Wages"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblcurrency" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtDailyWages" runat="server" CssClass="readonlytextbox" IsInteger="true" ReadOnly="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblContractPeriodDays" runat="server" Text="Tenure (Days)"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtContractPeriodDays" runat="server" CssClass="readonlytextbox" IsInteger="true" ReadOnly="true" />
                            <telerik:RadLabel ID="lblPlusMinus" runat="server" Text="+/-"></telerik:RadLabel>
                            <eluc:Number ID="txtPlusMinusPeriod" runat="server" CssClass="readonlytextbox" IsInteger="true" ReadOnly="true" />
                            <telerik:RadLabel ID="lblPlusMinusDays" runat="server" Text="(Days)"></telerik:RadLabel>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDPAllowance" runat="server" Text="DP Allowance"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblcurrencyAllowance" runat="server"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCurrencyid" Visible="false" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtDPAllowance" runat="server" CssClass="readonlytextbox" IsInteger="true" ReadOnly="true" />
                        </td>
                        <td colspan="2">
                            <b>
                                <telerik:RadLabel ID="lblNote" runat="server" EnableViewState="false" Text="Note : Tenure commences from the date of sign on." 
                                    BorderStyle="None" ForeColor="Blue">
                                </telerik:RadLabel>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSignOffTravelDays" runat="server" Text="SignOff Travel Days"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtSignOffTravelDays" runat="server"  IsInteger="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblBankAccount" runat="server" Text="Bank Account"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:BankAccount ID="ucBankAccount" runat="server"  AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>
                <b>
                    <telerik:RadLabel ID="lblHeading" runat="server" Text="Work & OT hours Configuration"></telerik:RadLabel>
                </b>
                <br />
                <div id="divGrid" style="position: relative; z-index: 0">
                    <%-- <asp:GridView ID="gvOffshoreComponent" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="true" OnRowCommand="gvOffshoreComponent_RowCommand"
                        OnRowEditing="gvOffshoreComponent_RowEditing" OnRowCancelingEdit="gvOffshoreComponent_RowCancelingEdit"
                        OnRowUpdating="gvOffshoreComponent_RowUpdating" ShowHeader="true" EnableViewState="false"
                        DataKeyNames="FLDCONTRACTCOMPONENETID" OnRowDataBound="gvOffshoreComponent_RowDataBound" OnRowDeleting="gvOffshoreComponent_OnRowDeleting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvOffshoreComponent" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOffshoreComponent_NeedDataSource"
                        OnItemCommand="gvOffshoreComponent_ItemCommand"
                        OnItemDataBound="gvOffshoreComponent_ItemDataBound"
                        
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed"  DataKeyNames="FLDCONTRACTCOMPONENETID" ShowFooter="true">
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
                             
                                <telerik:GridTemplateColumn HeaderText="Component">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTCOMPONENETID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtComponentNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                            CssClass="gridinput_mandatory">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtComponentNameAdd" runat="server" CssClass="gridinput_mandatory"></telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Hours Per Week">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="200px"></ItemStyle>
                                  
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblHours" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOURS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtHoursEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOURS") %>'
                                            CssClass="gridinput_mandatory" IsInteger="true" Width="200px" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Number ID="txtHoursAdd" runat="server" CssClass="gridinput_mandatory" IsInteger="true" Width="200px" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Remarks">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                  
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtRemarksEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' >
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtRemarksAdd" runat="server" ></telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                   
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit" 
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit">
                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                      
                                        <asp:LinkButton runat="server" AlternateText="Delete" 
                                            CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                            ToolTip="Delete">
                                             <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" 
                                            CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                            ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                      
                                        <asp:LinkButton runat="server" AlternateText="Cancel" 
                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" 
                                            CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                            ToolTip="Add New">
                                            <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                        </asp:LinkButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>
                
            </div>
            <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="ucConfirm_Click" OKText="Ok"
                CancelText="Cancel" Visible="false" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
