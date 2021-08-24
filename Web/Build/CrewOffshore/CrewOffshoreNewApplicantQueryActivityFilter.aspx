<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreNewApplicantQueryActivityFilter.aspx.cs" Inherits="CrewOffshore_CrewOffshoreNewApplicantQueryActivityFilter" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlNationalityList" Src="../UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlRankList" Src="../UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlZone" Src="../UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlRank" Src="../UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVesselTypeList" Src="../UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlEngineType" Src="../UserControls/UserControlEngineType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDocuments" Src="../UserControls/UserControlDocuments.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OtherCompany" Src="~/UserControls/UserControlOtherCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlMultiColumnUser.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Applicant Query Filter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">


        <eluc:TabStrip ID="MenuActivityFilterMain" Title="New Applicant" runat="server" OnTabStripCommand="NewApplicantFilterMain_TabStripCommand"></eluc:TabStrip>
        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <div id="divFind">
            <table cellpadding="2" cellspacing="2" width="100%">
           
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server"  Width="240px" MaxLength="200"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFileNumber" runat="server" Text="File No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileNumber" runat="server"  Width="240px" MaxLength="10"></telerik:RadTextBox>
                   
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassportNumber" runat="server" Text="Passport"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassortNo" runat="server"  Width="240px" MaxLength="200"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCDCNumber" runat="server" Text="CDC Number">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeamanbookNo" runat="server"  Width="240px" MaxLength="200"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofAvailabilityBetween" runat="server" Text="DOA Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtDOAStartDate"  runat="server" />
                        &nbsp;to&nbsp;
                            <eluc:UserControlDate ID="txtDOAEndDate"  runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlZone ID="ddlZone" runat="server" AppendDataBoundItems="true" 
                            Width="240px" />
                    </td>
                 
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlRankList ID="lstRank" runat="server"  AppendDataBoundItems="true"
                            Width="240px" />
                        <br />
                        <font color="blue">(Press "ctrl" for Multiple Selection)</font>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlNationalityList ID="lstNationality" runat="server" 
                            AppendDataBoundItems="true" Width="240px" />
                        <br />
                        <font color="blue">(Press "ctrl" for Multiple Selection)</font>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAppliedBetween" runat="server" Text="Applied Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtAppliedStartDate"  runat="server" />
                        &nbsp;to&nbsp;
                            <eluc:UserControlDate ID="txtAppliedEndDate"  runat="server" />
                    </td>

                </tr>
       
                <tr>
                    <td colspan="4">
                        <hr />
                        <b><telerik:RadLabel runat="server" ID="RadLabel1" Text="Experience"></telerik:RadLabel> </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblSailedRank" Text="Sailed Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlRank ID="ddlSailedRank" runat="server" AppendDataBoundItems="true"
                             Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" Text="Vessel Type" runat="server"></telerik:RadLabel>
                    </td>
                    <td rowspan="2">
                        <eluc:UserControlVesselTypeList ID="ddlVesselTypeList" runat="server" AppendDataBoundItems="true"
                             />
                        <br />
                        <font color="blue">(Press "ctrl" for Multiple Selection)</font>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEngineType" runat="server" Text="Engine Type"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:UserControlEngineType ID="ddlEngineType" runat="server" AppendDataBoundItems="true"
                             Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPreviousCompany" runat="server" Text="Previous Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:OtherCompany ID="ddlPrevCompany" runat="server"  AppendDataBoundItems="true" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                        <b><telerik:RadLabel runat="server" ID="RadLabel2" Text="Documents"></telerik:RadLabel> </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDocuments ID="ddlCourse" runat="server" DocumentType="COURSE" AppendDataBoundItems="true"
                             Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLicenses" runat="server" Text="License"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDocuments ID="ddlLicences" runat="server" DocumentType="LICENCE"
                            AppendDataBoundItems="true"  Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVisa" runat="server" Text="Visa"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Country ID="ddlVisa" runat="server" AppendDataBoundItems="true"  Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4"></td></tr>
                        <hr />
                
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActiveInactive" runat="server" Text="Active / In-Active"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlInActive" runat="server"  AppendDataBoundItems="true" Width="240px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlInActive_SelectedIndex" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                                <telerik:RadComboBoxItem Text="Active" Value="1"/>
                                <telerik:RadComboBoxItem Text="In-Active" Value="0"/>
                                <telerik:RadComboBoxItem Text="Deleted" Value="2"/>
                            </Items>
                            
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadListBox ID="lstStatus" runat="server"  SelectionMode="Multiple" Width="240px"
                            DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE">
                         
                        </telerik:RadListBox>
                    </td>
                </tr>
            </table>
        </div>
      
    </form>
</body>
</html>
