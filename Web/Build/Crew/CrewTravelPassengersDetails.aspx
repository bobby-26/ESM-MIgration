<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelPassengersDetails.aspx.cs"
    Inherits="CrewTravelPassengersDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Time" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew travel</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

   </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:status ID="ucstatus" runat="server" Visible="false" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Passenger Details" ShowMenu="false">
                    </eluc:Title>
                    <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div id="divMain" runat="server" style="width: 100%;">
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td colspan="4">
                                <asp:Panel ID="pnlpersonal" runat="server" GroupingText="Passenger Details">
                                    <table width="100%" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td width="15%">
                                                <asp:Literal ID="lblFirstName" runat="Server" Text="First Name"></asp:Literal>
                                            </td>
                                            <td width="35%">                                               
                                                <asp:TextBox ID="txtFirstname" runat="server" CssClass="input" MaxLength="200"
                                                    Width="80%"></asp:TextBox>                                                   
                                            </td>
                                            <td width="15%">
                                                <asp:Literal ID="lblMiddleName" runat="Server" Text="Middle Name"></asp:Literal>
                                            </td>
                                            <td width="35%">
                                                <asp:TextBox ID="txtmiddlename" runat="server" CssClass="input" Width="80%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblLastName" runat="server" Text="Last Name"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtlastname" runat="server" CssClass="input" Width="80%"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblDateofBirth" runat="Server" Text="Date of Birth"></asp:Literal>
                                            </td>
                                            <td>
                                                <eluc:Date ID="txtdob" runat="server" CssClass="input" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblPassportNo" runat="server" Text="Passport No"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPassport" runat="server" CssClass="input" Width="80%"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblDateofIssue" runat="server" Text="Date of issue"></asp:Literal>
                                            </td>
                                            <td>
                                                <eluc:Date ID="txtpdateofissue" runat="server" CssClass="input" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblPlaceofIssue" runat="server" Text="Place of Issue"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtpplaceodissue" runat="server" CssClass="input" Width="80%"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblDateofExpiry1" runat="Server" Text="Date of Expiry"></asp:Literal>
                                            </td>
                                            <td>
                                                <eluc:Date ID="txtpdateofexpiry" runat="server" CssClass="input" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblCDCNo" runat="server" Text="CDC No."></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtcdcno" runat="server" CssClass="input" Width="80%"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblDateofIssue1" runat="server" Text="Date of issue"></asp:Literal>
                                            </td>
                                            <td>
                                                <eluc:Date ID="txtcdcdateofissue" runat="server" CssClass="input" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblPlaceofIssue1" runat="server" Text="Place of Issue"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtcdcplaceofissue" runat="server" CssClass="input" Width="80%"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblDateofExpiry" runat="server" Text=" Date of Expiry"></asp:Literal>
                                            </td>
                                            <td>
                                                <eluc:Date ID="txtcdcdateofexpiry" runat="server" CssClass="input" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:literal ID="lblUSVISA" runat="server" Text="US VISA"></asp:literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtusvisa" runat="server" CssClass="input" Width="80%"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblOtherVisaDetails" runat="server" Text="Other Visa Details"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtothervisa" runat="server" CssClass="input" Width="80%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblNationality" runat="server" Text="Nationality"></asp:Literal>
                                            </td>
                                            <td>
                                                <eluc:Nationality ID="ucnationality" runat="server" AppendDataBoundItems="true" CssClass="input" />
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblOnOffSigner" runat="server" Text="On/Off Signer"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlonsigner" runat="server" CssClass="input">
                                                    <asp:ListItem Text="NA" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="off-signer" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="on-signer" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblPaymentMode" runat="server" Text="Payment Mode"></asp:Literal>
                                            </td>
                                            <td>
                                                <eluc:Hard ID="ucPaymentmodeAdd" runat="server" AppendDataBoundItems="true" CssClass="input"
                                                    HardList="<%# PhoenixRegistersHard.ListHard(1,185) %>" HardTypeCode="185" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Panel ID="Panel1" runat="server" GroupingText="Travel Details">
                                    <asp:GridView ID="gvCTBreakUp" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        OnRowDeleting="gvCTBreakUp_RowDeleting" OnRowDataBound="gvCTBreakUp_RowDataBound"
                                        OnRowEditing="gvCTBreakUp_RowEditing" OnRowCancelingEdit="gvCTBreakUp_RowCancelingEdit"
                                        OnRowUpdating="gvCTBreakUp_RowUpdating" Width="100%" CellPadding="3" ShowHeader="true"
                                        EnableViewState="false" OnRowCommand="gvCTBreakUp_RowCommand">
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="10px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No.">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblSNoHeader" runat="server">S.No.</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblTravelReqNo" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></asp:Label>
                                                    <%#DataBinder.Eval(Container, "DataItem.FLDSERIALNO")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Origin">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblOrigHeader" runat="server">Orig</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblTravelRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblOnSignerYN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYN") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblBreakUpId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBREAKUPID") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblIsEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISEDIT")%>'
                                                        Visible="false"></asp:Label>
                                                    <%#DataBinder.Eval(Container, "DataItem.FLDORIGINNAME")%>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblEmployeeIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblTravelRequestIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblVesselIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblOnSignerYNEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYN") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblBreakUpIdEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBREAKUPID") %>'
                                                        Visible="false"></asp:Label>
                                                    <span id="spnPickListOriginOldbreakup">
                                                        <asp:TextBox ID="txtOriginNameOldBreakup" runat="server" Width="80%" Enabled="False"
                                                            CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'></asp:TextBox>
                                                        <asp:ImageButton ID="btnShowOriginoldbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                            ImageAlign="Top" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItemIndex %>"
                                                            OnClientClick="return showPickList('spnPickListOriginOldbreakup', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                                        <asp:TextBox ID="txtOriginIdOldBreakup" runat="server" Width="0px" CssClass="hidden"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>'></asp:TextBox>
                                                    </span>
                                                    <br />
                                                    <br />
                                                    <span id="spnPickListOriginbreakup">
                                                        <asp:TextBox ID="txtOriginNameBreakup" runat="server" Width="80%" Enabled="False"
                                                            CssClass="input_mandatory"></asp:TextBox>
                                                        <asp:ImageButton ID="btnShowOriginbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                            ImageAlign="Top" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItemIndex %>"
                                                            OnClientClick="return showPickList('spnPickListOriginbreakup', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                                        <asp:TextBox ID="txtOriginIdBreakup" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                                                    </span>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Destination">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblDestHeader" runat="server">Dest</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#DataBinder.Eval(Container, "DataItem.FLDDESTINATIONNAME")%>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <span id="spnPickListDestinationOldbreakup">
                                                        <asp:TextBox ID="txtDestinationNameOldBreakup" runat="server" Width="80%" Enabled="False"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>' CssClass="input_mandatory"></asp:TextBox>
                                                        <asp:ImageButton ID="btnShowDestinationOldbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                            ImageAlign="Top" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItemIndex %>"
                                                            OnClientClick="return showPickList('spnPickListDestinationOldbreakup', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                                        <asp:TextBox ID="txtDestinationIdOldBreakup" runat="server" Width="0px" CssClass="hidden"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'></asp:TextBox>
                                                    </span>
                                                    <br />
                                                    <br />
                                                    <span id="spnPickListDestinationbreakup">
                                                        <asp:TextBox ID="txtDestinationNameBreakup" runat="server" Width="80%" Enabled="False"
                                                            CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'></asp:TextBox>
                                                        <asp:ImageButton ID="btnShowDestinationbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                            ImageAlign="Top" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItemIndex %>"
                                                            OnClientClick="return showPickList('spnPickListDestinationbreakup', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                                        <asp:TextBox ID="txtDestinationIdBreakup" runat="server" Width="0px" CssClass="hidden"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'></asp:TextBox>
                                                    </span>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Departure Date">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblDepDateHeader" runat="server">Dep Date</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDepartureDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDEPARTUREDATE", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                    <asp:Label ID="LBLDEPARTUREAMPM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREAMPM") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <eluc:Date runat="server" ID="txtDepartureDateOld" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE") %>'>
                                                    </eluc:Date>
                                                    <asp:DropDownList ID="ddldepartureampmold" runat="server" CssClass="dropdown_mandatory">
                                                        <asp:ListItem Text="AM" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="PM" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <br />
                                                    <br />
                                                    <eluc:Date runat="server" ID="txtDepartureDate" CssClass="input_mandatory"></eluc:Date>
                                                    <asp:DropDownList ID="ddldepartureampm" runat="server" CssClass="dropdown_mandatory">
                                                        <asp:ListItem Text="AM" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="PM" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Arrival Date">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblArrDateHeader" runat="server">Arr Date</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblArrivalDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                    <asp:Label ID="lblARRIBALAMPM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALAMPM") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <eluc:Date runat="server" ID="txtArrivalDateOld" CssClass="input_mandatory"></eluc:Date>
                                                    <asp:DropDownList ID="ddlarrivalampmold" runat="server" CssClass="dropdown_mandatory">
                                                        <asp:ListItem Text="AM" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="PM" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <br />
                                                    <br />
                                                    <eluc:Date runat="server" ID="txtArrivalDate" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}") %>'>
                                                    </eluc:Date>
                                                    <asp:DropDownList ID="ddlarrivalampm" runat="server" CssClass="dropdown_mandatory">
                                                        <asp:ListItem Text="AM" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="PM" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Purpose">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblPurposeHeader" runat="Server">Purpose</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblPurpose" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPURPOSENAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <eluc:TravelReason ID="ucPurposeOld" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                                        ReasonList="<%# PhoenixCrewTravelRequest.ListTravelReason(null) %>" />
                                                    <br />
                                                    <br />
                                                    <eluc:TravelReason ID="ucPurpose" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                                        ReasonList="<%# PhoenixCrewTravelRequest.ListTravelReason(null) %>" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblActionHeader" runat="server">
                                                         Action
                                                    </asp:Label>
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Select" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                        CommandName="EDITROW" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                                        ToolTip="Edit"></asp:ImageButton>
                                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                    <asp:ImageButton runat="server" AlternateText="Select" ImageUrl="<%$ PhoenixTheme:images/travel-breakup.png %>"
                                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdTravelBreakUp"
                                                        ToolTip="Add Break Journey"></asp:ImageButton>
                                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                                        ToolTip="Delete"></asp:ImageButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                                        ToolTip="Save Break Journey"></asp:ImageButton>
                                                    <asp:ImageButton runat="server" AlternateText="Save" Visible="false" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                        CommandName="Save" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdRowSave"
                                                        ToolTip="Save"></asp:ImageButton>
                                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                                        ToolTip="Cancel"></asp:ImageButton>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
