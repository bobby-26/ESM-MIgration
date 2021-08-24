<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPlanEventDetail.aspx.cs" Inherits="CrewPlanEventDetail" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Airport" Src="~/UserControls/UserControlAirport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Change Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function pageLoad(sender, eventArgs) {
                fade('statusmessage');
            }
            function Remarks() {
                showDialog();
            }
        </script>
        <script type="text/javascript">
            $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager2" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <telerik:RadWindow RenderMode="Lightweight" ID="modalPopup" runat="server" Modal="true" OffsetElementID="main"
            Style="z-index: 100001;">
            <ContentTemplate>
                <table id="Table2" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Remarks:"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="100%" Rows="8" Resize="Vertical">
                                <%--   <ClientEvents OnKeyPress="setHeight" OnLoad="setHeight" />--%>
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadPushButton ID="lnkSaveRemarks" runat="server" OnClick="lnkSaveRemarks_Click" Text="Save"></telerik:RadPushButton>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </telerik:RadWindow>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="80%">
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="CrewTab" runat="server" OnTabStripCommand="CrewTab_TabStripCommand"></eluc:TabStrip>
            <table style="border-collapse: collapse; width: 100%;" cellpadding="1px">
                <tr>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory"
                            OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID"
                            EnableLoadOnDemand="True" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <telerik:RadComboBox ID="ddlStatus" runat="server" AppendDataBoundItems="true" Enabled="false"
                            AutoPostBack="true" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                                <telerik:RadComboBoxItem Text="Open" Value="1" />
                                <telerik:RadComboBoxItem Text="Close" Value="0" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <eluc:MultiPort runat="server" ID="ucPort" AddressType='1255' Width="100%" />
                    </td>
                    <td style="width: 5%;">
                        <telerik:RadLabel ID="lblRefNo" runat="server" Text="Ref. No."></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <telerik:RadTextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" Width="100%" ReadOnly="true"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblEventDate" runat="server" Text="Starting"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <eluc:Date ID="txtEventDate" runat="server" CssClass="input_mandatory" />
                    </td>

                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblVesselETA" runat="server" Text="ETA"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <eluc:Date ID="txtVesselArrival" runat="server" />
                    </td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblVesselETB" runat="server" Text="ETB"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <eluc:Date ID="txtETB" runat="server" />
                    </td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblAirport" runat="server" Text="Airport"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <eluc:Airport ID="ucAirport" runat="server" AirportList='<%# PhoenixRegistersAirport.ListAirport(null)%>' Width="100%"
                            AppendDataBoundItems="true" />
                    </td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblCity" runat="server" Text="City"></telerik:RadLabel>
                    </td>

                    <td style="width: 10%">
                        <eluc:MultiCity ID="ucMultiCity" runat="server" AppendDataBoundItems="true" Width="100%" />
                    </td>

                </tr>
                <tr>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblEventToDate" runat="server" Text="Ending"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <eluc:Date ID="txtEventToDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblVesselETD" runat="server" Text="ETD"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <eluc:Date ID="txtVesselDepature" runat="server" />
                    </td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblVesselETC" runat="server" Text="ETC"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <eluc:Date ID="txtETC" runat="server" />
                    </td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblPortAgent" runat="server" Text="Port Agent"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <eluc:MultiAddress runat="server" ID="ucPortAgent" AddressType='1255'
                            Width="100%" />
                    </td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblTravelAgent" runat="server" Text="Travel Agent"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <eluc:MultiAddress runat="server" ID="ucTravelAgent" AddressType='1255'
                            Width="100%" />
                    </td>

                </tr>
                <tr>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblVoyage" runat="server" Text="Voyage"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <telerik:RadTextBox ID="txtVoyage" runat="server" Enabled="false" Text=""></telerik:RadTextBox>
                    </td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblQuotation" runat="server" Text="Quotation"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <telerik:RadTextBox ID="txtQuotation" runat="server" Enabled="false" Text=""></telerik:RadTextBox>
                    </td>
                     <td style="width: 5%">
                        <telerik:RadLabel ID="lblCost" runat="server" Text="Cost"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">                        
                         <eluc:Number ID="txtCost" runat="server" CssClass="input txtNumber" MaxLength="8" Width="80%" />
                    </td>                   
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblSubAgent" runat="server" Text="Sub Agent"></telerik:RadLabel>
                    </td>
                    <td style="width: 10%">
                        <eluc:MultiAddress runat="server" ID="ucSubAgent" AddressType='1255'
                            Width="100%" />
                    </td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <asp:LinkButton runat="server" AlternateText="Remarks" CommandName="REMARKS" ID="LinkButton1"
                            ToolTip="Remarks" Width="20PX" Height="20PX" OnClientClick="Remarks()">
                                    <span class="icon"><i class="fas fa-info-circle"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="gvEventTab" runat="server" OnTabStripCommand="gvEventTab_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvEvent" runat="server" AllowCustomPaging="true" AllowSorting="true" Height="80%"
                CellSpacing="0" GridLines="None" OnItemCommand="gvEvent_ItemCommand" OnNeedDataSource="gvEvent_NeedDataSource"
                OnItemDataBound="gvEvent_ItemDataBound" EnableViewState="true" GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText=" " Visible="false">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text=""></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCrewEventDetailId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWEVENTDETAILID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblCrewEventId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWEVENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tick" HeaderStyle-Width="3%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkAllOffSigner" OnCheckedChanged="chkAllOffSigner_CheckedChanged"
                                    AutoPostBack="True" />
                            </HeaderTemplate>
                            <ItemStyle Wrap="false" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkOffSigner" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="5%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add"
                                    CommandName="ADDOFFSIGNER" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAddOffSigner"
                                    ToolTip="Add Off-Signer" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemStyle Wrap="false" HorizontalAlign="Left" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" Screen='<%#"Crew/CrewTravelMoreInfoList.aspx?empId=" + DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERID").ToString() +"&familyId="%>' />
                                <asp:LinkButton runat="server" ID="cmdoffSignerTravel" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="OFFTRAVELREQ" CommandArgument="<%# Container.DataSetIndex %>" ToolTip="Off-Signer Travel">                                
                                <span class="icon"><i class="fas fa-plane-approval"></i></span>
                                </asp:LinkButton>
                                <telerik:RadLabel runat="server" ID="lblOffSignerTravelReq" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISOFFSIGNERTRVREQUESTED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No." HeaderStyle-Width="7%">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOffSignerFileNo" runat="server" Text="File No."></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERFILENO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Off-Signer" HeaderStyle-Width="23%">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOffSigner" runat="server" Text="Off-Signer"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOffSignerId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lbloffsignerrankname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERRANKCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbloffsignercon" runat="server" Text=" - "></telerik:RadLabel>
                                <asp:LinkButton ID="lnkOffSigner" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tick" HeaderStyle-Width="3%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkAllOnSigner" OnCheckedChanged="chkAllOnSigner_CheckedChanged"
                                    AutoPostBack="True" />
                            </HeaderTemplate>
                            <ItemStyle Wrap="false" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkOnSigner" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="5%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add"
                                    CommandName="ADDONSIGNER" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAddOnSigner"
                                    ToolTip="Add On-Signer" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemStyle Wrap="false" HorizontalAlign="Left" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <eluc:CommonToolTip ID="ucCommonToolTiponsigner" runat="server" Screen='<%# "Crew/CrewTravelMoreInfoList.aspx?empId=" + DataBinder.Eval(Container,"DataItem.FLDONSIGNERID").ToString() +"&familyId="%>' />
                                <asp:LinkButton runat="server" ID="cmdOnSignerTravel" AlternateText="Travel" Width="20PX" Height="20PX"
                                    CommandName="ONTRAVELREQ" CommandArgument="<%# Container.DataSetIndex %>" ToolTip="On-Signer Travel">                                
                                <span class="icon"><i class="fas fa-plane-approval"></i></span>
                                </asp:LinkButton>
                                <telerik:RadLabel runat="server" ID="lblOnSignerTravelReq" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISONSIGNERTRVREQUESTED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No." HeaderStyle-Width="7%">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="File No."></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOnsignerFileno" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERFILENO")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="On-Signer" HeaderStyle-Width="23%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCrewHeader" runat="server" Text="On-Signer"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPDStatusID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUSID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCrewPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERRANKID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblonsignerrankname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERRANKCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblonsignercon" runat="server" Text=" - "></telerik:RadLabel>
                                <asp:LinkButton ID="lnkOnSigner" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERNAME") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblJoinDate" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE", "{0:dd/MMM/yyyy}") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="23%">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Remove Plan"
                                    CommandName="REMOVEPLAN" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRemovePlan"
                                    ToolTip="Remove From Event" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DEPLAN" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="De-Plan">
                                    <span class="icon"><i class="fas fa-calendar-times-deplan"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="PD Form"
                                    ID="cmdPDForm" CommandName="PDFORM" ToolTip="PD Form">
                                     <span class="icon"><i class="fas fa-file"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Offer Letter"
                                    ID="cmdOfferLetter" Visible="false" CommandName="OFFERLETTER" CommandArgument="<%# Container.DataSetIndex %>"
                                    ToolTip="Offer Letter">
                                    <span class="icon"><i class="fas fa-file-signature-ol"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Document Checklist"
                                    CommandName="DOCUMENTCHECKLIST" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDocChecklist"
                                    ToolTip="Documents Checklist" Visible="false">
                                     <span class="icon"><i class="fas fa-list-ul"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Document Checklist"
                                    CommandName="DOCUMENTCHECKLISTMAIL" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDocumentCheckLIstmail"
                                    ToolTip="Documents Checklist E-Mail" Visible="false">
                                    <span class="icon"><i class="fas fa-envelope"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Approve Travel"
                                    ID="cmdApproveTravel" Visible="false" CommandName="APPROVETRAVEL" CommandArgument="<%# Container.DataSetIndex %>"
                                    ToolTip="Approve Travel">
                                    <span class="icon"><i class="fas fa-plane-departure"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Generate Appointment Letter"
                                    ID="cmdAppLetter" Visible="false" CommandName="APPOINTMENTLETTER" CommandArgument="<%# Container.DataSetIndex %>"
                                    ToolTip="Appointment Letter">
                                    <span class="icon"><i class="fas fa-file-signature-ol"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="CANCELAPPOINTMENTLETTER" CommandArgument='<%# Container.DataSetIndex %>'
                                    ID="cmdCancelAppointment" Visible="false" ToolTip="Cancel Appointment">
                                    <span class="icon"><i class="fas fa-times-circle-cancel"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Approve Sign/On"
                                    ID="cmdApproveSignOn" Visible="false" CommandName="APPROVESIGNON" CommandArgument="<%# Container.DataSetIndex %>"
                                    ToolTip="Approve Sign/On">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Medical Request"
                                    CommandName="MEDICALREQUEST" CommandArgument='<%# Container.DataSetIndex %>'
                                    ID="cmdMedical" ToolTip="Initiate Medical Request" Visible="false">
                                    <span class="icon"><i class="fas fa-briefcase-medical"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Course Request"
                                    CommandName="COURSEREQUEST" CommandArgument='<%# Container.DataSetIndex %>'
                                    ID="cmdCourse" ToolTip="Initiate Course Request" Visible="false">
                                    <span class="icon"><i class="fas fa-book"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" Visible="false" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="APPOINTMENTLETTERPDF" ID="cmdAppointmentLetter"
                                    ImageAlign="AbsMiddle" Text=".." ToolTip="Show Pdf">
                                    <span class="icon"><i class="fas fa-file-pdf"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="cmdRemark" CommandName="REMARK" runat="server"
                                    ToolTip="Remarks" AlternateText="Remarks" CommandArgument='<%# Container.DataSetIndex %>'>
                                    <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
