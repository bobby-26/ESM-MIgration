<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSeniorRankList.ascx.cs"
    Inherits="UserControls_UserControlSeniorRankList" %>
<script type="text/javascript">
    // Maintain scroll position on list box. 
    var xPosRank, yPosRank;
    var prmRank = Sys.WebForms.PageRequestManager.getInstance();

    function BeginRequestHandler(sender, args) {
        var listBox = $get('<%= lstSeniorRank.ClientID %>');

        if (listBox != null) {
            xPosRank = listBox.scrollLeft;
            yPosRank = listBox.scrollTop;
        }
    }

    function EndRequestHandler(sender, args) {
        var listBox = $get('<%= lstSeniorRank.ClientID %>');

        if (listBox != null) {
            listBox.scrollLeft = xPosRank;
            listBox.scrollTop = yPosRank;
        }
    }

    prmRank.add_beginRequest(BeginRequestHandler);
    prmRank.add_endRequest(EndRequestHandler); 
</script>
<%--<div runat="server" id="DivSeniorRankList" style="overflow-y: auto; overflow-x: hidden; height:80px">
    <asp:ListBox ID="lstSeniorRank" runat="server" DataTextField="FLDRANKNAME" DataValueField="FLDRANKID"
        OnDataBound="lstSeniorRank_DataBound" OnTextChanged="lstSeniorRank_TextChanged"
        SelectionMode="Multiple"></asp:ListBox>
</div>--%>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="DivSeniorRankList" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstSeniorRank" DataTextField="FLDRANKNAME" DataValueField="FLDRANKID"  Localization-CheckAll="--Check All--"
        CheckBoxes="true" ShowCheckAll="true" runat="server" OnDataBound="lstSeniorRank_DataBound"
        OnTextChanged="lstSeniorRank_TextChanged"></telerik:RadListBox> 
</div>
