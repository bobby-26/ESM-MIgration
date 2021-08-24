<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockQuotation.aspx.cs"
    Inherits="DryDockQuotation" EnableEventValidation="false" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="../UserControls/UserControlCurrency.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:radcodeblock runat="server">
       <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
        function OffWindowH() {
            var OffWindowH = 0;

            window.scrollTo(0, 10000000);

            if (typeof self.pageYOffset != 'undefined')
                OffWindowH = self.pageYOffset;
            else if (document.compatMode && document.compatMode != 'BackCompat')
                OffWindowH = document.documentElement.scrollTop;
            else if (document.body && typeof (document.body.scrollTop) != 'undefined')
                OffWindowH = document.body.scrollTop;
            window.scrollTo(0, 0);

            return OffWindowH;
        }

        function WindowHeight() {
            var WindowHeight = 0;
            if (typeof (window.innerWidth) == 'number')
                WindowHeight = window.innerHeight;
            else if (document.documentElement && document.documentElement.clientHeight)
                WindowHeight = document.documentElement.clientHeight;
            else if (document.body && document.body.clientHeight)
                WindowHeight = document.body.clientHeight;

            return WindowHeight;
        }

        function pHeight() {
            var pHeight = OffWindowH() + WindowHeight();
            return pHeight;
        }

        function ModelDialog() {
            var Modal = document.getElementById("modal");
            if (Modal == null) {
                Modal = document.createElement("div");
                Modal.setAttribute("id", "modal");
                Modal.setAttribute("class", "modal");
                document.body.appendChild(Modal);
            }
            Modal.style.height = pHeight() + 'px';
            Modal.style.display = "block";
        }
        function Loading() {
            var Element = document.getElementById("loading");
            objh = parseFloat(Element.style.height) / 2;
            objw = parseFloat(Element.style.width) / 2;
            Element.style.top = Math.floor(Math.round((document.documentElement.offsetHeight / 2) + document.documentElement.scrollTop) - objh) + 'px';
            Element.style.left = Math.floor(Math.round((document.documentElement.offsetWidth / 2) + document.documentElement.scrollLeft) - objw) + 'px';
            Element.style.display = "block";
        }
        function ShowProgress() {
            ModelDialog();
            Loading();
        }
        function HideProgress() {
            var loading = document.getElementById("loading");
            loading.style.display = "none";
            var modal = document.getElementById("modal");
            if (modal != null)
                modal.style.display = "none";
        }
        var fileDownloadCheckTimer = null
        function ExportDryDock(orderid, quotationid, vesselid, usercode, exportoption) {
            document.cookie = "fileDownloadToken=" + new Date() + ";path=/";
            ShowProgress();
            fileDownloadCheckTimer = setInterval(function () { ClearInterval() }, 100);
            document.getElementById("ifrGeneratedFile").src = "../Options/OptionsDryDock.ashx?methodname=DRYDOCKEXPORT2XL&exportoption=" + exportoption + "&orderid=" + orderid + "&quotationid=" + quotationid + "&vslid=" + vesselid + "&usercode=" + usercode;
        }
        function ExportDryDockMaker(orderid, quotationid, vesselid, usercode, exportoption) {
            document.cookie = "fileDownloadToken=" + new Date() + ";path=/";
            ShowProgress();
            fileDownloadCheckTimer = setInterval(function () { ClearInterval() }, 100);
            document.getElementById("ifrGeneratedFile").src = "../Options/OptionsDryDock.ashx?methodname=DRYDOCKMAKEREXPORT2XL&exportoption=" + exportoption + "&orderid=" + orderid + "&quotationid=" + quotationid + "&vslid=" + vesselid + "&usercode=" + usercode;
        }
        function getCookie(cname) {
            var name = cname + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') c = c.substring(1);
                if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
            }
            return "";
        }
        function ClearInterval() {
            ShowProgress();
            if (getCookie("fileDownloadToken") == "") {
                HideProgress();
                window.clearInterval(fileDownloadCheckTimer);
            }
        }
<%--function Edit(quotationid) {
    setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest(quotationid); }, 50);
        }--%>
    </script>
    </telerik:radcodeblock>
    <style type="text/css">
        body, html {
            width: 100%;
            height: 100%;
            margin: 0px;
            padding: 0px;
            background-color: #FFFFFF;
        }

        .modal {
            border: 0px;
            padding: 0px;
            margin: 0px;
            top: 0px;
            left: 0px;
            width: 100%;
            position: absolute;
            background-color: #000000;
            opacity: 0.5;
            filter: Alpha(opacity:50);
            z-index: 11;
            -moz-opacity: 0.8;
            min-height: 100%;
        }

        .loading {
            border: 0px;
            padding: 0px;
            display: none;
            position: absolute;
            border: 5px solid darkgrey;
            background-color: #FFFFFF;
            z-index: 12;
        }
    </style>


    
</head>
<body>
    <form id="frmQuotation" runat="server" autocomplete="off">
        <telerik:radscriptmanager id="radscript1" runat="server"></telerik:radscriptmanager>
        <%--<telerik:radskinmanager id="radskin1" runat="server"></telerik:radskinmanager>--%>

        <script type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            //Raised before processing of an asynchronous postback starts and the postback request is sent to the server.
            prm.add_beginRequest(BeginRequestHandler);
            // Raised after an asynchronous postback is finished and control has been returned to the browser.
            prm.add_endRequest(EndRequestHandler);
            function BeginRequestHandler(sender, args) {
                ShowProgress()
            }

            function EndRequestHandler(sender, args) {
                HideProgress()
            }
        </script>
        

        <iframe runat="server" id="ifrGeneratedFile" src="about:blank"
            width="0" height="0" style="display: none" />
        <div id="loading" class="loading" style="width: 200px; height: 100px; line-height: 9em" align="center">
            <asp:Image ID="imgProgress" runat="server" ImageUrl="<%$ PhoenixTheme:images/uploading.gif %>" />
            Processing. Please wait.                                            
        </div>
       
          <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
           
           
        </telerik:RadAjaxManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            
            <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuQuotation" runat="server" OnTabStripCommand="Quotation_TabStripCommand" TabStrip="true"></eluc:TabStrip>

       
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="text-wrap: true">
                        <telerik:radlabel id="RadLabel1" runat="server" text="Project No"></telerik:radlabel>
                    </td>
                    <td><b>
                        <telerik:radlabel id="ucTitle" runat="server" text="" />
                    </b></td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:radlabel id="lblYard" runat="server" text="Yard"></telerik:radlabel>
                    </td>
                    <td>
                        <span id="spnPickListYard">
                            <telerik:radtextbox id="txtYardCode" runat="server" width="60px" cssclass="input_mandatory"></telerik:radtextbox>
                            <telerik:radtextbox id="txtYardName" runat="server" borderwidth="1px" width="380px" cssclass="input_mandatory"></telerik:radtextbox>
                            <asp:LinkButton ID="btnPickVender" runat="server"><span class="icon"><i class="fas fa-tasks"></i></span></asp:LinkButton>

                            <telerik:radtextbox id="txtYardID" runat="server" width="1"></telerik:radtextbox>
                        </span>
                    </td>
                    <td>
                        <telerik:radlabel id="lblValidUntil" runat="server" text="Valid Until"></telerik:radlabel>

                    </td>
                    <td>
                        <eluc:Date ID="txtValidUntil" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:radlabel id="lblYardRef" runat="server" text="Yard Ref."></telerik:radlabel>

                    </td>
                    <td>
                        <telerik:radtextbox id="txtYardReferenceno" runat="server" width="90px"></telerik:radtextbox>
                    </td>
                    <td>
                        <telerik:radlabel id="lblRecievedDate" runat="server" text="Recieved Date"></telerik:radlabel>

                    </td>
                    <td>
                        <eluc:Date ID="ucRecievedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:radlabel id="lblCurrency" runat="server" text="Currency"></telerik:radlabel>

                    </td>
                    <td>
                        <eluc:Currency ID="ucCurrency" AppendDataBoundItems="true" runat="server" />
                        &nbsp;
                               <telerik:radlabel id="lblRate" runat="server" text="Rate"></telerik:radlabel>
                        &nbsp;<eluc:Number ID="txtExchangeRate" runat="server" Width="50px" IsInteger="false" CssClass="input txtNumber" />
                    </td>
                    <td>
                        <telerik:radlabel id="lblExpiryDate" runat="server" text="Expiry Date"></telerik:radlabel>

                    </td>
                    <td>
                        <eluc:Date ID="ucExpiryDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:radlabel id="lblSentDate" runat="server" text="Sent Date"></telerik:radlabel>

                    </td>
                    <td>
                        <eluc:Date ID="ucSentDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                    <td>
                        <telerik:radlabel id="lblSentby" runat="server" text="Sent by"></telerik:radlabel>

                    </td>
                    <td>
                        <telerik:radtextbox id="txtSentBy" runat="server" cssclass="readonlytextbox" readonly="true"></telerik:radtextbox>
                    </td>
                </tr>
            </table>
            <hr />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:radlabel id="lblTotalPriceQuotedCurrency" runat="server" text="Total Price (Quoted Currency)"></telerik:radlabel>

                    </td>
                    <td>
                        <eluc:Decimal ID="txtTotalPrice" runat="server" Width="120px" Mask="99,999,999.99" />
                    </td>
                    <td>
                        <telerik:radlabel id="lblDiscountQuoted" runat="server" text="Yard"></telerik:radlabel>

                    </td>
                    <td>
                        <eluc:Decimal ID="ucTotalDiscount" runat="server" Width="90px" ReadOnly="true" Mask="99,999,999.99" />
                    </td>
                    <td>
                        <telerik:radlabel id="lblLumpSumDiscount" runat="server" text="Lump Sum Discount"></telerik:radlabel>

                    </td>
                    <td>%&nbsp;<eluc:Number ID="ucAgreedpercent" runat="server" IsInteger="false" MaxLength="6" Width="50px" />
                        &nbsp;
                                &nbsp;
                               <telerik:radlabel id="lblValue" runat="server" text="Value"></telerik:radlabel>
                        &nbsp;<eluc:Number ID="ucAgreedLumpsum" runat="server" IsInteger="false" MaxLength="6" Width="50px" />
                    </td>
                    <td>
                        <telerik:radlabel id="lblNetPrice" runat="server" text="Net Price"></telerik:radlabel>

                    </td>
                    <td>
                        <eluc:Decimal ID="txtNetPrice" runat="server" Width="90px" ReadOnly="true" Mask="99,999,999.99" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:radlabel id="lblTotalPriceUSD" runat="server" text="Total Price (USD)"></telerik:radlabel>

                    </td>
                    <td>
                        <eluc:Decimal ID="txtUsdPrice" runat="server" Width="120px" ReadOnly="true" Mask="99,999,999.99" />
                    </td>
                    <td>
                        <telerik:radlabel id="lblDiscountUSD" runat="server" text="Discount (USD)"></telerik:radlabel>

                    </td>
                    <td>
                        <eluc:Decimal ID="txtTotalDiscount" runat="server" Width="90px" ReadOnly="true" Mask="99,999,999.99" />
                    </td>
                    <td>
                        <telerik:radlabel id="lblIncludeYardinOwnersReportYN" runat="server" text="Include Yard in Owners Report Y/N"></telerik:radlabel>

                    </td>
                    <td>
                        <asp:CheckBox ID="chkReportOwnerYN" runat="server" />
                    </td>
                </tr>
            </table>
            <hr />
            <table width="100%">
                <tr>
                    <td width="40%">
                        <table cellpadding="1" cellspacing="1" width="100%">
                            <tr>
                                <td></td>
                                <td>
                                    <telerik:radlabel id="lblYardQuote" runat="server" text="Yard Quote"></telerik:radlabel>

                                </td>
                                <td>
                                    <telerik:radlabel id="lblAdjustment" runat="server" text="Adjustment"></telerik:radlabel>

                                </td>
                                <td>
                                    <telerik:radlabel id="lblTotal" runat="server" text="Total"></telerik:radlabel>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:radlabel id="lblNumberofDaysalongside" runat="server" text="Number of Days alongside"></telerik:radlabel>

                                </td>
                                <td>
                                    <eluc:Number ID="YardQuote2" runat="server" MaxLength="6" Width="50px" />
                                </td>
                                <td>
                                    <eluc:Number ID="Adjustment2" runat="server" MaxLength="6" Width="50px" />
                                </td>
                                <td>
                                    <eluc:Number ID="Total2" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="50px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:radlabel id="lblNumberofDaysinDrydock" runat="server" text="Number of Days in Drydock"></telerik:radlabel>

                                </td>
                                <td>
                                    <eluc:Number ID="YardQuote1" runat="server" MaxLength="6" Width="50px" />
                                </td>
                                <td>
                                    <eluc:Number ID="Adjustment1" runat="server" MaxLength="6" Width="50px" />
                                </td>
                                <td>
                                    <eluc:Number ID="Total1" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="50px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:radlabel id="lblTotalDaysinYard" runat="server" text="Total Days in Yard"></telerik:radlabel>

                                </td>
                                <td>
                                    <eluc:Number ID="TotalYardQuote" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                        MaxLength="6" Width="50px" />
                                </td>
                                <td>
                                    <eluc:Number ID="TotalAdjustment" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                        MaxLength="6" Width="50px" />
                                </td>
                                <td>
                                    <eluc:Number ID="Total" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="50px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="60%">
                        <table width="100%">
                            <tr>
                                <td>
                                    <telerik:radlabel id="lblServiceSpeed" runat="server" text="Service Speed"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Number ID="ucServiceSpeed" runat="server"
                                        MaxLength="8" Width="50px" />
                                </td>
                                <td>
                                    <telerik:radlabel id="lblDeviationDays" runat="server" text="Deviation Days"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Number ID="ucDeviationDays" runat="server"
                                        MaxLength="8" Width="50px" />
                                </td>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:radlabel id="lblDeviationfromDeliveryPoint" runat="server" text="Deviation from Delivery Point"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Number ID="ucDeviationDeliveryPoint" runat="server"
                                        MaxLength="8" Width="50px" />
                                </td>
                                <td>
                                    <telerik:radlabel id="lblHFOCostMT" runat="server" text="HFO Cost/MT"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Number ID="ucHFOCost" runat="server"
                                        MaxLength="8" Width="50px" />
                                </td>
                                <td>
                                    <telerik:radlabel id="lblMDOCostMT" runat="server" text="MDO Cost/MT"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Number ID="ucMDOCost" runat="server"
                                        MaxLength="8" Width="50px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:radlabel id="lblDeviationfromReDeliveryPoint" runat="server" text="Deviation from Re-Delivery Point"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Number ID="ucDeviationReDeliveryPoint" runat="server"
                                        MaxLength="8" Width="50px" />
                                </td>
                                <td>
                                    <telerik:radlabel id="lblHFOConsumptionDay" runat="server" text="HFO Consumption/Day"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Number ID="ucHFOConsumption" runat="server"
                                        MaxLength="8" Width="50px" />
                                </td>
                                <td>
                                    <telerik:radlabel id="lblMDOConsumptionDay" runat="server" text="MDO Consumption/Day"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Number ID="ucMDOConsumption" runat="server"
                                        MaxLength="8" Width="50px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:radlabel id="lblTotalDeviation" runat="server" text="Total Deviation"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Number ID="ucTotalDeviation" runat="server"
                                        MaxLength="8" Width="50px" />
                                </td>
                                <td>
                                    <telerik:radlabel id="lblHFOTotalCost" runat="server" text="HFO Total Cost"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Number ID="ucHFOTotalCost" runat="server"
                                        MaxLength="8" Width="50px" />
                                </td>
                                <td>
                                    <telerik:radlabel id="lblMDOTotalCost" runat="server" text="MDO Total Cost"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Number ID="ucMDOTotalCost" runat="server"
                                        MaxLength="8" Width="50px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuQuotationGrid" runat="server" OnTabStripCommand="QuotationGrid_TabStripCommand"></eluc:TabStrip>

            <telerik:radgrid rendermode="Lightweight" id="gvQuotation" runat="server" allowcustompaging="true" allowsorting="true" allowpaging="true"
                cellspacing="0" gridlines="None"
                onneeddatasource="gvQuotation_NeedDataSource"   EnableViewState="false"
                onitemcommand="gvQuotation_ItemCommand"
                onitemdatabound="gvQuotation_ItemDataBound" 
                enableheadercontextmenu="true" groupingenabled="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" Height="10px"   DataKeyNames="FLDQUOTATIONID" >
                           <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <HeaderStyle Width="102px" />
                    
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                     
                            <telerik:GridTemplateColumn HeaderText="Include"          HeaderStyle-HorizontalAlign="center">
                                <HeaderStyle Width="4%" />
                                <ItemTemplate>
                                    <telerik:RadCheckBox ID="chkSelect" Checked="false" runat="server"  EnableViewState="true" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText=" "        HeaderStyle-HorizontalAlign="center">
                                  <HeaderStyle Width="3%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="imgFlag" runat="server" Enabled="false" ><span class="icon" style="color:yellow"><i class="fas fa-star"></i></span></asp:LinkButton>
                                </ItemTemplate>
    <%--                            <asp:ImageButton ID="ImageButton1" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" />--%>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reference Number"         HeaderStyle-HorizontalAlign="center">
                                <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblpoissued" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOISSUED") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblOrderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblQuotationid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkNumber" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDYARDREFNO") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Yard Name"    HeaderStyle-HorizontalAlign="center">
                                <HeaderStyle Width="20%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                           
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblYardName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' CommandName="Select" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                             <telerik:RadLabel ID="lblvesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>

                                     </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="MakerY/N"     HeaderStyle-HorizontalAlign="center" >
                                  <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                           
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblMakerYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERYN") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Currency"      HeaderStyle-HorizontalAlign="center">
                                  <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                          
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCurrencyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Received Date"     HeaderStyle-HorizontalAlign="center">
                                  <HeaderStyle Width="12%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                           
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblQuotedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                           
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"    Width="15%"></HeaderStyle>
                           
                                <ItemStyle Wrap="False" HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Select" 
                                        CommandName="SELECTYARD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSelect"
                                        ToolTip="Select">
                                        <span class="icon"><i class="fa fa-check-circle"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="De-Select" 
                                        CommandName="DESELECT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDeSelect"
                                        ToolTip="De-Select">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img6" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3"  alt=""  />
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="select" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3"  alt="" />
                                    <asp:LinkButton  runat="server" AlternateText="Refresh" 
                                        CommandName="REFRESH" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRefresh"
                                        ToolTip="Refresh">
                                        <span class="icon"><i class="fas fa-sync"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3"  alt="" />
                                    <asp:LinkButton  runat="server" AlternateText="Export XL"
                                        CommandName="EXPORT2XL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdExport2XL"
                                        ToolTip="Export XL">
                                        <span class="icon"><i class="fas fa-file-excel"></i> </span>
                                    </asp:LinkButton>
                                    <asp:LinkButton  runat="server" AlternateText="Export XL" 
                                        CommandName="EXPORT2XLINCREMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdIncrement"
                                        ToolTip="Export XL (Pending Quote)">
                                        <span class="icon"><i class="fas fa-file-excel"></i> </span>
                                    </asp:LinkButton>
                                    <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3"  alt="" />
                                    <asp:LinkButton  runat="server" AlternateText="Prepare Quote" 
                                        CommandName="PREPAREQUOTE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPrepareQuote"
                                        ToolTip="Prepare Quote">
                                        <span class="icon"><i class="fas fa-file-invoice-dollar"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3"  alt=""  />
                                    <asp:LinkButton  runat="server" AlternateText="Send Quote" 
                                        CommandName="SENDQUOTE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSendQuery"
                                        ToolTip="Send Quote">
                                        <span class="icon"><i class="fas fa-envelope"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img5" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3"  alt="" />
                                    <asp:LinkButton  runat="server" AlternateText="JOBS" Visible="false"
                                        CommandName="JOBS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdJobs"
                                        ToolTip="Jobs">
                                        <span class ="icon" ><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                     <img id="Img7" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3"  alt="" />
                                    <asp:LinkButton  runat="server" AlternateText="JOBS"
                                        CommandName="JOBPROGRESS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdjobprogress"
                                        ToolTip="Job Progress">
                                        <span class ="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:radgrid>

            <eluc:Status ID="ucStatus" runat="server" />
        
    </form>
</body>
</html>
