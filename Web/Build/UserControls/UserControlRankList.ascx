<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlRankList.ascx.cs"
    Inherits="UserControlRankList" %>
<script type="text/javascript">
    // Maintain scroll position on list box. 
    var xPosRank, yPosRank;
    var prmRank = Sys.WebForms.PageRequestManager.getInstance();

    function BeginRequestHandler(sender, args) {
        var listBox = $get('<%= lstRank.ClientID %>');

        if (listBox != null) {
            xPosRank = listBox.scrollLeft;
            yPosRank = listBox.scrollTop;
        }
    }

    function EndRequestHandler(sender, args) {
        var listBox = $get('<%= lstRank.ClientID %>');

        if (listBox != null) {
            listBox.scrollLeft = xPosRank;
            listBox.scrollTop = yPosRank;
        }
    }

    prmRank.add_beginRequest(BeginRequestHandler);
    prmRank.add_endRequest(EndRequestHandler); 
</script>
<%--<div runat="server" id="divRankList" style="overflow-y: auto; overflow-x: hidden;
    height: 80px">
    <asp:ListBox ID="lstRank" DataTextField="FLDRANKNAME" DataValueField="FLDRANKID"
        SelectionMode="Multiple" runat="server" Width="240px" OnDataBound="lstRank_DataBound"
        OnTextChanged="lstRank_TextChanged"></asp:ListBox>
</div>--%>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div runat="server" id="divRankList" style="overflow-y: auto; overflow-x: hidden; height: 80px">
    <telerik:RadListBox RenderMode="Lightweight" ID="lstRank" DataTextField="FLDRANKNAME" DataValueField="FLDRANKID"  Localization-CheckAll="--Check All--"
        CheckBoxes="true" ShowCheckAll="true" runat="server" OnDataBound="lstRank_DataBound"
        OnTextChanged="lstRank_TextChanged"></telerik:RadListBox>
</div>
