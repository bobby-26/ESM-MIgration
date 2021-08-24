<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewMedicalRequest.aspx.cs" Inherits="CrewMedicalRequest" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Src="~/UserControls/UserControlTabsTelerik.ascx" TagPrefix="eluc" TagName="TabStrip" %>
<%@ Register Src="~/UserControls/UserControlTitle.ascx" TagPrefix="eluc" TagName="Title" %>
<%@ Register Src="~/UserControls/UserControlErrorMessage.ascx" TagPrefix="eluc" TagName="Error" %>
<%@ Register Src="~/UserControls/UserControlStatus.ascx" TagName="UserControlStatus" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Warn List</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmLicReq" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">

                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

                <eluc:TabStrip ID="MRMenu" runat="server" OnTabStripCommand="MRMenu_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvReq" runat="server" Height="550px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvReq_NeedDataSource"
                        OnItemCommand="gvReq_ItemCommand"
                        OnItemDataBound="gvReq_ItemDataBound"
                        OnSortCommand="gvReq_SortCommand"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="None">
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <NestedViewSettings>
                                <ParentTableRelation>
                                    <telerik:GridRelationFields MasterKeyField="FLDREQUESTID" DetailKeyField="FLDREQUESTID" />
                                </ParentTableRelation>
                            </NestedViewSettings>
                            <NestedViewTemplate>
                                <table width="100%" cellpadding="1">
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel4" runat="server" Font-Bold="true" Text="Tentative Vessel:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel5" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE") %>'></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel6" runat="server" Font-Bold="true" Text="Joined Vessel:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel7" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOININGVESSELCODE") %>'></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel8" runat="server" Font-Bold="true" Text="Invoice No:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel9" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENO") %>'></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel1" runat="server" Font-Bold="true" Text="Created By:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblCreatedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBY") %>'></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel2" runat="server" Font-Bold="true" Text="Created Date:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr></tr>
                                </table>
                            </NestedViewTemplate>
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Request No">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblMedStatus" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblAttendedStatus" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDATTENDEDYN")%>'></telerik:RadLabel>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDREFNUMBER")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name/ File No">
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEmployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME")%>'></asp:LinkButton>
                                        <br />
                                        /&nbsp<%#DataBinder.Eval(Container, "DataItem.FLDFILENO")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rank">
                                    <HeaderStyle Width="50px" />
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Clinic">
                                    <HeaderStyle Width="140px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReqType" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREQUESTTYPE")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblReqId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREQUESTID")%>'></telerik:RadLabel>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDCLINICADDRESSNAME")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Family Member">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkFamilyEmployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERNAME")%>'></asp:LinkButton>
                                        <telerik:RadLabel ID="lblFamilyid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFAMILYID")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Attended">
                                    <HeaderStyle Width="70px" Wrap="false" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAttended" runat="server"
                                            Text='<%# DataBinder.Eval(Container, "DataItem.FLDATTENDEDYN").ToString() == "1" ? "Yes" : "No"%>'>
                                        </telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="chkAttendedEdit" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDATTENDEDYN").ToString() == "1" ? true : false%>'
                                            Enabled='<%# DataBinder.Eval(Container, "DataItem.FLDATTENDEDYN").ToString() == "1" ? false : true%>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Received">
                                    <HeaderStyle Width="70px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReport" runat="server"
                                            Text='<%# DataBinder.Eval(Container, "DataItem.FLDRECEIVEDYN").ToString() == "1" ? "Yes" : "No"%>'>
                                        </telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="chkReceivedEdit" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDRECEIVEDYN").ToString() == "1" ? true : false%>'
                                            Enabled='<%# DataBinder.Eval(Container, "DataItem.FLDRECEIVEDYN").ToString() == "1" ? false : true%>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Confirm">
                                    <HeaderStyle Width="65px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVerified" runat="server"
                                            Text='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDYN").ToString() == "1" ? "Yes" : "No"%>'>
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVerifiedby" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDBYNAME").ToString()%>'></telerik:RadLabel>
                                        <eluc:ToolTip ID="ucToolTipLicense" runat="server" Text='<%# "Verified By : " + DataBinder.Eval(Container,"DataItem.FLDVERIFIEDBYNAME") +"<br/>Verified On :  " + DataBinder.Eval(Container,"DataItem.FLDVERIFIEDON","{0:dd/MMM/yyyy}") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="chkVerifiedEdit" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDYN").ToString() == "1" ? true : false%>'
                                            Enabled='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDYN").ToString() == "1" ? false : true%>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="chkVerifiedAdd" runat="server" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Cost">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDMEDICALCOST")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status">
                                    <HeaderStyle Width="80px" />
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDHARDNAME")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Medical Request"
                                            CommandName="MEDICAL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdMedical"
                                            ToolTip="Medical Request"><span class="icon"><i class="fas fa-briefcase-medical"></i></span></asp:LinkButton>

                                        <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" Screen='<%# "Crew/CrewMedicalMoreInfoList.aspx?requestid=" + DataBinder.Eval(Container,"DataItem.FLDREQUESTID").ToString()%>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                            ToolTip="Save"><span class="icon"><i class="fas fa-save"></i></span></asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel">
                                        <span class="icon"><i class="fa fa-trash"></i></span></asp:LinkButton>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>

                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
