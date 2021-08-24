<%@ Page Language="C#" AutoEventWireup="True" CodeFile="OptionsCrewDeBriefing.aspx.cs" Inherits="OptionsCrewDeBriefing"
    MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonAddress" Src="../UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="../UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToBeDoneBy" Src="~/UserControls/UserControlOffshoreToBeDoneBy.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>De-Briefing</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</telerik:RadCodeBlock></head>

<body>
    <div>
        <form id="frmRegistersState" runat="server">
            <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
                runat="server">
            </ajaxToolkit:ToolkitScriptManager>
            <asp:UpdatePanel runat="server" ID="pnlStateEntry">
                <ContentTemplate>
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                        <div class="subHeader" style="position: relative">
                            <div id="divHeading">
                                <eluc:Title runat="server" ID="ucTitle" ShowMenu="false" Text="De-Briefing" />
                            </div>
                        </div>
                        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                            <eluc:TabStrip ID="FeedBackTabs" runat="server" OnTabStripCommand="FeedBackTabs_TabStripCommand"
                                TabStrip="false"></eluc:TabStrip>
                        </div>
                        <div id="divPrimarySection" runat="server">
                            <table width="100%" cellpadding="5" cellspacing="1">
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblEmployeeNumber" runat="server" Text="File No"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                    </td>

                                    <td>
                                        <asp:Literal ID="lblFirstName" runat="server" Text="Name"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFirstName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>


                                    <td>
                                        <asp:Literal ID="Vessel" runat="server" Text="Vessel"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtvesselName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblSignOnDate" runat="server" Text="Sign On"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSignOnDate" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                        <%--<eluc:Date ID="txtSignOnDate" runat="server" CssClass="readonly_textbox" ReadOnly="true" />--%>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblSigonOffdate" runat="server" Text="Sign Off"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSignOffDate" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                    </td>


                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblreviewd" Visible ="false" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <th nowrap>
                                        <h3>
                                            <asp:Literal ID="lblavdetail" runat="server" Text="Availability and Contact Details"></asp:Literal></h3>
                                    </th>
                                </tr>
                            </table>
                            <table id="Table1" width="100%" style="color: black; font-size: small; font-family: Arial">
                                <tr>
                                    <td>In order that we can plan your next assignment you are requested to kindly advise us your 
                                        <br />
                                        availability date and confirm your contact details by filling up the template below.
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="20%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td>
                                        <asp:Literal ID="lbldoddate" runat="server" Text="Date of Availability"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucdoa" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                            OnTextChangedEvent='txtDOA_TextChanged' />
                                    </td>
                                </tr>

                            </table>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td width="50%">
                                        <b>
                                            <asp:Literal ID="lblPermanentAddress" runat="server" Text="Permanent Address"></asp:Literal>
                                        </b>
                                    </td>

                                    <td colspan="4">
                                        <b>
                                            <asp:Literal ID="lblPermanentContact" runat="server" Text="Permanent Contact"></asp:Literal></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <eluc:CommonAddress ID="PermanentAddress" runat="server" />
                                    </td>
                                    <td style="vertical-align: text-top;">
                                        <table style="vertical-align: text-top;">
                                            <tr>
                                                <td width="50%">
                                                    <asp:Literal ID="lblPhoneNumber" runat="server" Text="Phone Number"></asp:Literal>
                                                </td>
                                                <td>
                                                    <eluc:PhoneNumber ID="txtPhoneNumber" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="lblMobileNumber" runat="server" Text="Mobile Number"></asp:Literal>
                                                </td>
                                                <td>
                                                    <eluc:MobileNumber ID="txtMobileNumber" IsMobileNumber="true" runat="server"
                                                        CssClass="input" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="lblEMail" runat="server" Text="E-Mail"></asp:Literal>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtEmail" runat="server" Width="100%" CssClass="input_mandatory"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td nowrap>
                                                    <asp:Literal ID="lblPlaceofEngagement" runat="server" Text="Port of Engagement"></asp:Literal>
                                                </td>
                                                <td>
                                                    <eluc:SeaPort ID="ddlPortofEngagement" runat="server" CssClass="input" AppendDataBoundItems="true"
                                                        Width="150px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <hr />
                        <div>
                            <table>
                                <tr>
                                    <h3>
                                        <asp:Literal ID="lbldebriefinghead" runat="server" Text="De-Briefing:"></asp:Literal>

                                    </h3>
                                </tr>
                            </table>

                            <table id="Table2" width="100%" style="color: black; font-size: small; font-family: Arial">
                                <tr>
                                    <td>We would also like feedback of your tenure onboard your last vessel and would appreciate if you could kindly fill up the Debriefing template below.
                                        <br />
                                        This will allow us to address any issues or concerns that you may have as well as improve our systems
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </div>
                        <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                            <asp:GridView ID="gvFeedBackQst" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" ShowHeader="true"
                                OnPreRender="gvFeedBackQst_PreRender" EnableViewState="false" OnRowDataBound="gvFeedBackQst_RowDataBound">
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <%-- <asp:TemplateField>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <HeaderTemplate>
                                            S.No
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle Width="40px" HorizontalAlign="Center" />
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <HeaderTemplate>
                                            Questions
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <table cellspacing="10">

                                                <tr>
                                                    <td style="font-weight: bold;">

                                                        <asp:Label ID="lblcategorynameG" Visible="false" Font-Underline="true" runat="server" Font-Size="Small" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME")%>'></asp:Label><br />
                                                        <asp:Label ID="lblQuestionId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUESTIONID")%>'></asp:Label>
                                                        <%#Container.DataItemIndex+1 %> .
                                                        <asp:Label ID="lblQuestionName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUESTIONNAME")%>'></asp:Label>
                                                        <asp:Label ID="lblRequirRemark" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREQUIREREMARK")%>'></asp:Label>
                                                        <asp:Label ID="lblCategoryId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCATEGORYID")%>'></asp:Label>
                                                        <asp:Label ID="lblOrder" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSORTORDER")%>'
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblOptions" runat="server" DataValueField="FLDOPTIONID"
                                                            DataTextField="FLDOPTIONNAME" DataSource='<%# PhoenixCrewDeBriefing.GetdeBriefingoptions(General.GetNullableInteger((DataBinder.Eval(Container,"DataItem.FLDQUESTIONID")).ToString())) %>'
                                                            RepeatDirection="Horizontal">
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="trcomments">
                                                    <td>
                                                        <asp:Label ID="lblcomment" Visible="true" runat="server" Text="Comments (If Any)"></asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="txtComments" Visible="true" runat="server" CssClass="input" TextMode="MultiLine"
                                                            onkeyDown="checkTextAreaMaxLength(this,event,'500');" Width="300px" Height="50px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                         <div>
                            <%--<hr />--%>
                            <h3>
                                <asp:Literal ID="lblGeneralComments" runat="server" Text="General Comments:"></asp:Literal></h3>
                               <asp:TextBox ID="txtGeneralComments" CssClass="input" runat="server" TextMode="MultiLine"
                                                        onkeyDown="checkTextAreaMaxLength(this,event,'500');" Width="800px" Height="50px"></asp:TextBox>
                        </div>
                        <div>
                            <hr />
                            <h3>
                                <asp:Literal ID="lblPendingTrainingneeds" runat="server" Text="Recommended Courses:"></asp:Literal></h3>
                        </div>
                        <div id="div2" style="position: relative; z-index: 1">
                            <table id="Table8" width="100%" style="color: black; font-size: small; font-family: Arial">
                                <tr>
                                    <td>The following Courses have been identified for you.You are requested to kindly get in touch with your 
                                        <br />
                                        designated manning office so that these can addressed promptly.
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div id="divGrid1" style="position: relative; z-index: +1">
                                <asp:GridView ID="gvCrewRecommendedCourses" runat="server" AutoGenerateColumns="False"
                                    Font-Size="11px" Width="100%" CellPadding="3"
                                    ShowFooter="false"
                                    ShowHeader="true" EnableViewState="false" AllowSorting="true">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                    <RowStyle Height="10px" />
                                    <Columns>
                                        <asp:ButtonField Text="DoubleClick" CommandName="SELECT" Visible="false" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblSlNoHeader" runat="server">Sl.No</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDROWNUMBER")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="true"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblCourseHeader" runat="server">Course</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCourseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Left" Width="5%" Wrap="true"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblRecommendedDateHeader" runat="server">Recommended Date</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                                <asp:Label ID="lblrecommendedid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECOMMENDEDID") %>'></asp:Label>
                                                <asp:Label ID="lblEmpid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                                <asp:Label ID="lblRankid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></asp:Label>
                                                <asp:Label ID="lblCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEID") %>'></asp:Label>
                                                <asp:Label ID="lblRecommendedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECOMMENDEDDATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Left" Width="13%" Wrap="true"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblRecommendedByHeader" runat="server">Recommended By</asp:Label>

                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRecommendedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECOMMENDEDBYNAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="true"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblLastdoneDateHeader" runat="server">Last done Date</asp:Label>

                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container,"DataItem.FLDCOURSELASTDONE") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="true"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblStatusHeader" runat="server">Status</asp:Label>

                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDYN") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Left" Width="3%" Wrap="true"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblApprovedHeader" runat="server">Approved</asp:Label>

                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDAPPROVEDYN")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlApproveEdit" runat="server" CssClass="input" OnTextChanged="ddlApprovalType_TextChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="To be done"></asp:ListItem>
                                                    <asp:ListItem Value="0" Text="Not reqd"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="To be done prior next s/on"></asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Left" Width="5%" Wrap="true"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblApprovedDateHeader" runat="server">Approved Date</asp:Label>

                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblNominatedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOMINATEDDATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblRemarksHeader" runat="server">Remarks</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDAPPROVALREMARKS")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblDtkeyEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="input" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALREMARKS") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <input type="hidden" id="isouterpage" name="isouterpage" />
                            <eluc:Status ID="ucStatus" runat="server" />
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
