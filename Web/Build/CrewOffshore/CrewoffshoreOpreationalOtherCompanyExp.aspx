<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewoffshoreOpreationalOtherCompanyExp.aspx.cs"
    Inherits="CrewOffshore_CrewoffshoreOpreationalOtherCompanyExp" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCrewOtherExperienceList" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <%--    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CrewAppraisalMain" runat="server" OnTabStripCommand="CrewAppraisalMain_TabStripCommand">
                    </eluc:TabStrip>
                </div>--%>


                <eluc:TabStrip ID="MenuCrewOtherExperienceList" Title="Crew Other Experience" runat="server" OnTabStripCommand="CrewAppraisalMain_TabStripCommand"></eluc:TabStrip>

                <div>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
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
                                <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="Employee Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <hr />
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <th>
                            <b>
                                <telerik:RadLabel ID="lblOperationalExperience" runat="server" Text="Operational Experience"></telerik:RadLabel>
                            </b>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNumberofDryDockingsattended" runat="server" Text="Dry Dockings attended"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select value" ID="ddlDryDockingsattended" runat="server" CssClass="input">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                    <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                    <telerik:RadComboBoxItem Text="No" Value="2" />
                                </Items>

                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblNumberofOVIDInspections" runat="server" Text="Number of OVID inspections"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="uctxtNumberofOVIDInspections" runat="server" CssClass="input" MaxLength="7"
                                IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNumberofOSVISInspections" runat="server" Text="Number of OSVIS inspections"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="uctctNumberofOSVISInspections" runat="server" CssClass="input txtNumber"
                                MaxLength="7" IsInteger="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblClassofROV" runat="server" Text="Class of ROV"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:CheckBoxList ID="cblRovType" runat="server" CssClass="input" DataTextField="FLDDMRROVTYPENAME"
                                DataValueField="FLDDMRROVTYPEID" Height="40%" RepeatDirection="Vertical"
                                Width="80%">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNumberofOceanTows" runat="server" Text="Number of Ocean Tows"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtNumberofOceanTows" runat="server" CssClass="input txtNumber"
                                MaxLength="7" IsInteger="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblNumberofRigMoves" runat="server" Text="Number of Infield Moves"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtNumberofRigMoves" runat="server" CssClass="input txtNumber" MaxLength="7"
                                IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFSIPSC" runat="server" Text="FSI/PSC"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select value" ID="ddlFSIPSC" runat="server" CssClass="input">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                    <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                    <telerik:RadComboBoxItem Text="No" Value="2" />
                                </Items>

                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblFMEA" runat="server" Text="FMEA"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select value" ID="ddlFMEA" runat="server" CssClass="input">
                                 <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                    <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                    <telerik:RadComboBoxItem Text="No" Value="2" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="25%">
                            <telerik:RadLabel ID="lblDPAnnuals" runat="server" Text="DP Annuals"></telerik:RadLabel>
                        </td>
                        <td width="25%">
                            <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select value"  ID="ddlDPAnnuals" runat="server" CssClass="input">
                               <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                    <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                    <telerik:RadComboBoxItem Text="No" Value="2" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td width="25%">
                            <telerik:RadLabel ID="lblDeliveryOrTakeover" runat="server" Text="New Delivery / Take over"></telerik:RadLabel>
                        </td>
                        <td width="25%">
                            <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select value" ID="ddlDeliveryOrTakeover" runat="server" CssClass="input">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                    <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                    <telerik:RadComboBoxItem Text="No" Value="2" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <b>
                                <telerik:RadLabel ID="lblCargoHandling" runat="server" Text="Special Cargo Handling Experience"></telerik:RadLabel>
                            </b>
                        </th>
                    </tr>
                    <tr>
                        <td width="25%">
                            <telerik:RadLabel ID="lblHeavyLift" runat="server" Text="Heavy Lift / Project cargoes"></telerik:RadLabel>
                        </td>
                        <td width="25%">
                            <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select value" ID="ddlHeavyLift" runat="server" CssClass="input">
                               <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                    <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                    <telerik:RadComboBoxItem Text="No" Value="2" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td width="25%">
                            <telerik:RadLabel ID="lblDKDMud" runat="server" Text="DKD Mud"></telerik:RadLabel>
                        </td>
                        <td width="25%">
                            <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select value" ID="ddlDKDMud" runat="server" CssClass="input">
                                 <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                    <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                    <telerik:RadComboBoxItem Text="No" Value="2" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="25%">
                            <telerik:RadLabel ID="lblMethanol" runat="server" Text="Methanol"></telerik:RadLabel>
                        </td>
                        <td width="25%">
                            <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select value" ID="ddlMethanol" runat="server" CssClass="input">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                    <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                    <telerik:RadComboBoxItem Text="No" Value="2" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td width="25%">
                            <telerik:RadLabel ID="lblGlycol" runat="server" Text="Glycol"></telerik:RadLabel>
                        </td>
                        <td width="25%">
                            <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select value" ID="ddlGlycol" runat="server" CssClass="input">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                    <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                    <telerik:RadComboBoxItem Text="No" Value="2" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <b>
                                <telerik:RadLabel ID="lblAnchorHandling" runat="server" Text="Anchor Handling"></telerik:RadLabel>
                            </b>
                        </th>
                    </tr>
                    <tr>
                        <td width="25%">
                            <telerik:RadLabel ID="lblTypesofanchorshandled" runat="server" Text="Types of anchors handled"></telerik:RadLabel>
                        </td>
                        <td width="25%" hight="10%">
                            <asp:CheckBoxList ID="chkValue" runat="server" CssClass="input" DataTextField="FLDANCHORHANDLINGTYPENAME"
                                DataValueField="FLDANCHORHANDLINGTYPEID" Height="40%" RepeatDirection="Vertical"
                                Width="80%">
                            </asp:CheckBoxList>
                        </td>
                        <td width="25%">
                            <telerik:RadLabel ID="lblExperienceingranchors" runat="server" Text="Experience in grappling for anchors"></telerik:RadLabel>
                        </td>
                        <td width="25%">
                            <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select value" ID="ddlExperienceingranchors" runat="server" CssClass="input">
                              <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                    <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                    <telerik:RadComboBoxItem Text="No" Value="2" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="25%">
                            <telerik:RadLabel ID="lblExperiencestowage" runat="server" Text="Experience in Handling and stowage of Chains"></telerik:RadLabel>
                        </td>
                        <td width="25%">
                            <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select value" ID="ddlExperiencestowage" runat="server" CssClass="input">
                              <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                    <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                    <telerik:RadComboBoxItem Text="No" Value="2" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAnchorhandlingdepth" runat="server" Text="Maximum Anchor handling depth"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucAnchorhandlingdepth" runat="server" CssClass="input txtNumber"
                                MaxLength="6" />
                            M
                        </td>
                    </tr>
                    <tr>
                        <td width="25%">
                            <telerik:RadLabel ID="lblSizelengthofchain" runat="server" Text="Experience in Chain Handling"></telerik:RadLabel>
                        </td>
                        <td width="25%">
                            <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select value" ID="ddlExperienceChainHandling" runat="server" CssClass="input">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                    <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                    <telerik:RadComboBoxItem Text="No" Value="2" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
