<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseStrategySheet.aspx.cs"
    Inherits="PurchaseStrategySheet" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Strategy Sheet</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStrategySheet" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReceivedFromDate" runat="server" Text="Received From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucFromDate" runat="server" CssClass="input"></eluc:Date>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReceivedToDate" runat="server" Text="Received To Date"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Date ID="ucToDate" runat="server" CssClass="input"></eluc:Date>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table width="100%">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblGroup" runat="server" Text="Group"></telerik:RadLabel>
                                </td>
                                <td>
                                    <div runat="server" id="divGroup" class="input" style="overflow: auto; width: 60%; height: 80px"
                                        onscroll="javascript:setGroupScroll();">
                                        <asp:HiddenField ID="hdnScrollGroup" runat="server" />
                                        <telerik:RadCheckBoxList ID="chkGroupList" runat="server" AutoPostBack="true" Height="100%" DataBindings-DataTextField="FLDHARDNAME"
                                            DataBindings-DataValueField="FLDHARDCODE" OnSelectedIndexChanged="ChkGroupList_Changed" RepeatColumns="1" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                        </telerik:RadCheckBoxList>
                                    </div>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                                </td>
                                <td>
                                    <div runat="server" id="divFleet" class="input" style="overflow: auto; width: 60%; height: 80px"
                                        onscroll="javascript:setFleetScroll();">
                                        <asp:HiddenField ID="hdnScrollFleet" runat="server" />
                                        <telerik:RadCheckBoxList ID="chkFleetList" runat="server" AutoPostBack="true" Height="100%" DataBindings-DataTextField="FLDFLEETDESCRIPTION"
                                            DataBindings-DataValueField="FLDFLEETID" OnSelectedIndexChanged="chkFleetList_Changed" RepeatColumns="1" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                        </telerik:RadCheckBoxList>
                                    </div>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                                </td>
                                <td>
                                    <div runat="server" id="dvVessel" class="input" style="overflow: auto; width: 40%; height: 80px"
                                        onscroll="javascript:setVesselScroll();">
                                        <asp:HiddenField ID="hdnScrollVessel" runat="server" />
                                        <telerik:RadCheckBoxList ID="chkVesselList" runat="server" Height="100%" AutoPostBack="true" DataBindings-DataTextField="FLDVESSELNAME"
                                            DataBindings-DataValueField="FLDVESSELID" OnSelectedIndexChanged="chkVesselList_Changed" RepeatColumns="1" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                        </telerik:RadCheckBoxList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShowAllPOswithDate" runat="server" Text="Show All POs with Date"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="cbShowNonBlank" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="3">
                        <font color="blue">
                        <telerik:RadLabel ID="lblSelect" runat="server" Text="Select the option below to analyze the POs that are not acted upon."></telerik:RadLabel>
                    </font>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="3">
                        <telerik:RadRadioButtonList runat="server" ID="rbDates" Direction="Horizontal"
                            AutoPostBack="true">
                            <Items>
                                <telerik:ButtonListItem Text="Reqn. Not Processed." Value="1" Selected="True"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="Awaiting all quotes" Value="2"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="Awaiting PO Approval" Value="3"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="PO to be Issued" Value="4"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="Awaiting Vessel Receipt" Value="5"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="3">
                        <font color="blue">
                        <telerik:RadLabel ID="lblToknow" runat="server" Text="To know the number of days for which the POs are pending, specifythe number of days and click on Search."></telerik:RadLabel>
                    </font>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <eluc:Decimal ID="txtDays" runat="server" Width="90px" Mask="99999" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStockType" runat="server" Text="Stock Type"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlStockType" runat="server" CssClass="input" AutoPostBack="true">
                            <Items>
                                <telerik:DropDownListItem Value="Dummy" Text="ALL" />
                                <telerik:DropDownListItem Value="SPARE" Text="SPARE" />
                                <telerik:DropDownListItem Value="STORE" Text="STORE" />
                                <telerik:DropDownListItem Value="SERVICE" Text="SERVICE" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblForwarder" runat="server" Text="Forwarder"></telerik:RadLabel>

                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVenderCode" runat="server" Width="100px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" BorderWidth="1px" Width="200px" CssClass="input"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnPickVender" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=141&txtsupcode='+ document.getElementById('txtVenderCode').value +'&txtsupname='+ document.getElementById('txtVenderName').value, true);"
                                Text=".." />
                            <telerik:RadTextBox ID="txtVenderID" runat="server" Width="1" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsBottom" Text=".." OnClick="cmdClear_Click" />
                    </td>
                </tr>
            </table>
            <asp:Panel runat="server" ID="pnlAnalysisReport" GroupingText="Performance Report">
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOldpending" runat="server" Text="Pending in Begining of Month"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOldpending" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblNewCreated" runat="server" Text="Received by End of the Month"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtNewCreated" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTotalPending" runat="server" Text="Total Pending"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTotalPending" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblTotalCleared" runat="server" Text="Total Cleared"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTotalCleared" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />

            <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvFormDetails" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvFormDetails" runat="server" AllowSorting="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvFormDetails_NeedDataSource" OnSortCommand="gvFormDetails_SortCommand"
                OnItemDataBound="gvFormDetails_ItemDataBound" OnItemCommand="gvFormDetails_ItemCommand" 
                GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderText="Req. No."  SortExpression="FLDFORMNO">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkFormNumberName" CommandName="SELECT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Stock Type" HeaderStyle-Width="70px" AllowSorting="true" SortExpression="FLDSTOCKTYPE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblStockType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkStockType" runat="server" OnClientClick="javascript:setVessel()" CommandName="FORM"
                                 Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Form Title" HeaderStyle-Width="115px" AllowSorting="true" SortExpression="FLDTITLE">
                            <ItemTemplate>
                               <telerik:RadLabel ID="lblFormTitle" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTITLE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received on Phoenix" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDIMPORTEDDATE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReceivedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPORTEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active on Phoenix" HeaderStyle-Width="75px" SortExpression="FLDACTIVEDATE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveDate" runat="server" 
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="75px" HeaderText="Sent for quote" SortExpression="FLDSENTDATE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSentQuote" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSENTDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Days" AllowSorting="true" HeaderStyle-Width="40px" SortExpression="FLDDAYDIFFRECDANDACTIVE" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQtnSentVsReqRecd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAYDIFFRECDANDACTIVE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Quotation received" AllowSorting="true" HeaderStyle-Width="75px" SortExpression="FLDRECEIVEDDATE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuotationReceived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Days" AllowSorting="true" HeaderStyle-Width="40px" SortExpression="FLDDAYDIFFRECDANDSENT">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQtnRecdVsSent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAYDIFFRECDANDSENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Approved date" AllowSorting="true" HeaderStyle-Width="75px" SortExpression="FLDPURCHASEAPPROVEDATE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApprovedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEAPPROVEDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Days" AllowSorting="true" HeaderStyle-Width="40px" SortExpression="FLDDAYDIFFRECDANDAPPROVED">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApprdVsQtnRecd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAYDIFFRECDANDAPPROVED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Issued" AllowSorting="true" HeaderStyle-Width="75px" SortExpression="FLDORDEREDDATE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPOIssued" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Days" AllowSorting="true" HeaderStyle-Width="40px" SortExpression="FLDDAYDIFFORDERANDAPPROVED">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPOIssuedVsApprd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAYDIFFORDERANDAPPROVED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Forwarder" AllowSorting="true" HeaderStyle-Width="30px" SortExpression="FLDFORWARDERNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblForwarder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORWARDERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Forwarder Recd." AllowSorting="true" HeaderStyle-Width="75px" SortExpression="FLDFORWARDERRECEIVEDDATE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblForwarderRecd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORWARDERRECEIVEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Days" AllowSorting="true" HeaderStyle-Width="40px" SortExpression="FLDDAYSWITHFORWARDER">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDaysWithForwarder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAYSWITHFORWARDER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vendor" AllowSorting="true" HeaderStyle-Width="30px" SortExpression="FLDFORWARDERVENDORNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORWARDERVENDORNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Weight" AllowSorting="true" HeaderStyle-Width="30px" SortExpression="FLDTOTALWEIGHT">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWeight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALWEIGHT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received Onboard" AllowSorting="true" HeaderStyle-Width="75px" SortExpression="FLDVENDORDELIVERYDATE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReceivedOnboard" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORDELIVERYDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Delivery Port" AllowSorting="true" HeaderStyle-Width="30px" SortExpression="FLDSEAPORTNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDeliveryPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Amount (USD)" AllowSorting="true" HeaderStyle-Width="30px" SortExpression="FLDPRICE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRICE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnablePostBackOnRowClick="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <script type="text/javascript">

            // Maintain scroll position on list box. 
            //var xPos, yPos;
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            function setVesselScroll() {
                var div = $get('<%=dvVessel.ClientID %>');
                var hdn = $get('<%= hdnScrollVessel.ClientID %>');
                hdn.value = div.scrollTop;
            }

            function setFleetScroll() {
                var div1 = $get('<%=divFleet.ClientID %>');
                var hdn1 = $get('<%= hdnScrollFleet.ClientID %>');
                hdn1.value = div1.scrollTop;
            }

            function setGroupScroll() {
                var div2 = $get('<%=divGroup.ClientID %>');
                var hdn2 = $get('<%= hdnScrollGroup.ClientID %>');
                hdn2.value = div2.scrollTop;
            }

            function setVessel() {
                parent.document.frames["applicationtitle"].location.reload();
            }

            //        function BeginRequestHandler(sender, args) {
            //            var listBox = $get('<%= dvVessel.ClientID %>');
            //            var hdn = $get('<%= hdnScrollVessel.ClientID %>');

            //            if (listBox != null) {
            //                xPos = listBox.scrollLeft;
            //                yPos = listBox.scrollTop;
            //            }
            //        }

            function EndRequestHandler(sender, args) {

                var listBox = $get('<%= dvVessel.ClientID %>');
                var hdn = $get('<%= hdnScrollVessel.ClientID %>');
                listBox.scrollTop = hdn.value;

                var listBox1 = $get('<%= divFleet.ClientID %>');
                var hdn1 = $get('<%= hdnScrollFleet.ClientID %>');
                listBox1.scrollTop = hdn1.value;

                var listBox2 = $get('<%= divGroup.ClientID %>');
                var hdn2 = $get('<%= hdnScrollGroup.ClientID %>');
                listBox2.scrollTop = hdn2.value;
            }

            //        prm.add_beginRequest(BeginRequestHandler);
            prm.add_endRequest(EndRequestHandler);
        </script>
    </telerik:RadCodeBlock>
</body>
</html>
