<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSupdtEventFeedbackBanner.aspx.cs"
    Inherits="InspectionSupdtEventFeedbackBanner" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Event Feedback</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script language="Javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
        </script>

        <script language="javascript" type="text/javascript">
            function TxtMaxLength(text, maxLength) {
                text.value = text.value.substring(0, maxLength);
            }


            function CheckAll(id) {
                var masterTable = $find("<%= gvAssignedTo.ClientID %>").get_masterTableView();
              var row = masterTable.get_dataItems();
              if (id.checked == true) {
                  for (var i = 0; i < row.length; i++) {
                      masterTable.get_dataItems()[i].findElement("chkSelect").checked = true;
                  }
              }
              else {
                  for (var i = 0; i < row.length; i++) {
                      masterTable.get_dataItems()[i].findElement("chkSelect").checked = false;
                  }
              }
          }

        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSupdtEventFeedbackBanner" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <%-- <eluc:Title runat="server" ID="ucTitle" Text="Event Feedback" ShowMenu="false"></eluc:Title>--%>

            <eluc:TabStrip ID="MenuSupdtEventFeedback" runat="server" OnTabStripCommand="MenuSupdtEventFeedback_TabStripCommand"></eluc:TabStrip>

            <table id="tblSupdtEventFeedback" width="100%" cellpadding="2" cellspacing="2">
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblEventTitle" runat="server" Text="Event"></telerik:RadLabel>
                        </b>
                    </td>
                    <td colspan="3"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselByCompany runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input_mandatory"
                            Width="200px" AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEventDate" runat="server" Text="Event Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucEventDate" runat="server" CssClass="input_mandatory" DatePicker="true"
                            AutoPostBack="true" OnTextChangedEvent="ucEventDate_Changed"></eluc:Date>
                        <asp:ImageButton ID="cmdHiddenSubmit" runat="server" ImageUrl="<%$ PhoenixTheme:images/search.png%>"
                            OnClick="cmdHiddenSubmit_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEvent" runat="server" Text="Event"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadDropDownList ID="ddlEvent" runat="server" CssClass="input_mandatory" Width="200px">
                        </telerik:RadDropDownList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucRecordedDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSupdt" runat="server" Text="Superintendent"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSupdtName" runat="server" CssClass="readonlytextbox" MaxLength="200"
                            Width="196px" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td></td>

                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblFeedback" runat="server" Text="Feedback"></telerik:RadLabel>
                        </b>
                    </td>
                    <td colspan="3"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFeedbackCategory" runat="server">Feedback Category</telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadDropDownList ID="ddlFeedbackCategory" runat="server" CssClass="input_mandatory"
                            Width="200px" OnSelectedIndexChanged="ddlFeedback_Changed" AutoPostBack="true">
                        </telerik:RadDropDownList>
                    </td>
                    <%--  <td colspan="2"></td>--%>
                    <td>
                        <telerik:RadLabel ID="lblKeyAnchors" runat="server" Text="Key Anchors"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucKeyAnchor" runat="server" AppendDataBoundItems="true" Width="250px"
                            CssClass="dropdown_mandatory" QuickTypeCode="155" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFeedbackSubCategory" runat="server">SubCategory</telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadDropDownList ID="ddlFeedbackSubCategory" runat="server" CssClass="input" Width="200px">
                        </telerik:RadDropDownList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblExpectedClosingDate" runat="server">Expected Closing Date</telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <eluc:Date ID="ucExpectedClosingDate" runat="server" />
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="chkaction" runat="server" Enabled="false" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblactionreq" runat="server">Action Required</telerik:RadLabel>
                                </td>
                            </tr>
                        </table>

                    </td>


                    <td colspan="2"></td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server">Superintendent Remarks</telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtRemarks" CssClass="input_mandatory" runat="server" TextMode="MultiLine"
                            onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)" Rows="5"
                            Width="670px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblAssignedTo" runat="server" Text="Assigned To"></telerik:RadLabel>
                    </td>
                </tr>
            </table>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvAssignedTo" Height="88%" runat="server" AllowSorting="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvAssignedTo_NeedDataSource"
                ShowFooter="False" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSIGNONOFFID">
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

                        <telerik:GridTemplateColumn>
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkAllAssignedTo" runat="server" Text="Select All" AutoPostBack="true" onclick="CheckAll(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" EnableViewState="true" />
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignonoffId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONOFFID"]%>'></telerik:RadLabel>
                                <%# ((DataRowView)Container.DataItem)["FLDNAME"]%>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="true" SortExpression="FLDSIGNONRANKNAME">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDSIGNONRANKNAME"]%>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
