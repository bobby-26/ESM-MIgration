<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVesselCertificateCategoryList.ascx.cs"
    Inherits="UserControlVesselCertificateCategoryList" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

    <script type="text/javascript"> 
    // Maintain scroll position on list box. 
    var xPosRank, yPosRank; 
    var prmRank = Sys.WebForms.PageRequestManager.getInstance(); 

    function BeginRequestHandler(sender, args) 
    {
        var listBox = $get('<%# lstCategory.ClientID %>'); 

        if (listBox != null) 
        { 
            xPosRank = listBox.scrollLeft; 
            yPosRank = listBox.scrollTop; 
        } 
    } 

    function EndRequestHandler(sender, args) 
    {
        var listBox = $get('<%# lstCategory.ClientID %>'); 

        if (listBox != null) 
        { 
            listBox.scrollLeft = xPosRank; 
            listBox.scrollTop = yPosRank; 
        } 
    } 

    prmRank.add_beginRequest(BeginRequestHandler); 
    prmRank.add_endRequest(EndRequestHandler); 
    </script>   

<div runat="server" id="chkboxlist" style="overflow-y: auto; overflow-x: hidden; height: 80px"> 
    <telerik:RadListBox RenderMode="Lightweight" ID="lstCategory" DataTextField="FLDCATEGORYNAME" DataValueField="FLDCATEGORYID" 
        CheckBoxes="true" ShowCheckAll="true" runat="server" OnDataBound="lstCategory_DataBound" 
        OnTextChanged="lstCategory_TextChanged"></telerik:RadListBox>
</div>
