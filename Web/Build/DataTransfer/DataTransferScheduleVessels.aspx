<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTransferScheduleVessels.aspx.cs" Inherits="DataTransfer_DataTransferScheduleVessels" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Synchronizer Setting</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <style type="text/css">
            .divFloatLeft {
                height: 53px;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
       
            <br clear="all" />
            <telerik:RadTextBox ID="lblGuidance" runat="server" Text="" BorderStyle="None" BorderWidth="0px" ReadOnly="true" Rows="4" TextMode="MultiLine" Width="100%" Font-Bold="true"></telerik:RadTextBox>

            <br />


            <table id="tblVesselNameSearch" width="50%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtvesselName" runat="server" MaxLength="200" CssClass="input" OnTextChanged="txtVesselName_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucTechFleet" Width="80%" CssClass="input" AppendDataBoundItems="true" OnTextChangedEvent="ucFleet_TextChanged" AutoPostBack="true" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuDataSynchronizer" runat="server" OnTabStripCommand="MenuDataSynchronizer_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselList" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemDataBound="gvVesselList_ItemDataBound" OnItemCommand="gvVesselList_ItemCommand" OnNeedDataSource="gvVesselList_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name" AllowSorting="true" SortExpression="FLDVESSELNAME">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkVesselName" runat="server" CommandName="SELECT" 
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="  Active Y/N">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDataTransferYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATATRANSFER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSyncAttachmentsYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTTRANSFER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSyncDocumentsYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTANSFER") %>'></telerik:RadLabel>
                               <%-- <Telerik:RadCheckBox runat="server" ID="chkActiveYN" OnCheckedChanged="CheckBoxActiveClicked" AutoPostBack="true" BackColor="Transparent" ForeColor="Transparent" />--%>
                              <Telerik:RadcheckBox runat="server" ID="chkActiveYN" CommandName="UPDATEACTIVEYN" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDACTIVE").ToString().Equals("1") ? true : false %>' 
                                        AutoPostBack="true" EnableViewState="true"/>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=" Data Tranfer Y/N">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                               <%-- <Telerik:RadCheckBox runat="server" ID="chkDataTransferYN" OnCheckedChanged="CheckBoxActiveClicked" AutoPostBack="true" BackColor="Transparent" ForeColor="Transparent" />--%>
                                 <Telerik:RadcheckBox runat="server" ID="chkDataTransferYN" CommandName="UPDATEDATATRANSFERYN"
                                        AutoPostBack="true" EnableViewState="true"/>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=" Sync. Attachments Y/N">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                              <%--  <Telerik:RadCheckBox runat="server" ID="chkAttachmentsYN" OnCheckedChanged="AttachmentCheckBoxClicked" AutoPostBack="true" BackColor="Transparent" ForeColor="Transparent" />--%>
                                  <Telerik:RadcheckBox runat="server" ID="chkAttachmentsYN" CommandName="UPDATEATTACHMENTSYN"
                                        AutoPostBack="true" EnableViewState="true"/>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText=" Sync. Documents Y/N">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%--<Telerik:RadCheckBox runat="server" ID="chkDocumentsYN" OnCheckedChanged="DocumentCheckBoxClicked" AutoPostBack="true" BackColor="Transparent" ForeColor="Transparent" />--%>
                                  <Telerik:RadcheckBox runat="server" ID="chkDocumentsYN" CommandName="UPDATEDOCUMENTSYN"
                                        AutoPostBack="true" EnableViewState="true"/>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

