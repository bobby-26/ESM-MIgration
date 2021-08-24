<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDepartmentList.ascx.cs" Inherits="UserControlDepartmentList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script type="text/javascript">

    // Maintain scroll position on list box. 
    var xPos, yPos; 
    var prm = Sys.WebForms.PageRequestManager.getInstance(); 

    function setDeparmentScroll()
    {
        var div = $get('<%=divCheckboxList.ClientID %>');
        var hdn = $get('<%= hdnScrollDepartment.ClientID %>');
        hdn.value = div.scrollTop;
    }

    function BeginRequestHandler(sender, args) 
    { 
        var listBox = $get('<%= divCheckboxList.ClientID %>'); 
        var hdn = $get('<%= hdnScrollDepartment.ClientID %>');
        
        if (listBox != null) 
        { 
            xPos = listBox.scrollLeft; 
            yPos = listBox.scrollTop; 
        } 
    } 

    function EndRequestHandler(sender, args) 
    { 
        var listBox = $get('<%= divCheckboxList.ClientID %>'); 
        var hdn = $get('<%= hdnScrollDepartment.ClientID %>');
        listBox.scrollTop = hdn.value; 
    } 

    prm.add_beginRequest(BeginRequestHandler);
    prm.add_endRequest(EndRequestHandler); 


</script>

<%--<div runat="server" id="divCheckboxList" class="input" style="overflow: auto;height: 140px" onscroll="javascript:setDeparmentScroll();">
    <asp:HiddenField ID="hdnScrollDepartment" runat="server" />
    <asp:Label ID="lblCheckboxList" runat="server" Text="Label" Visible="false"></asp:Label>
     <asp:CheckBoxList ID="UcDEP" runat="server"  OnSelectedIndexChanged="UcDEP_SelectedIndexChanged"
         DataTextField="FLDDEPARTMENTNAME"   DataValueField="FLDDEPARTMENTID" OnDataBound="UcDEP_DataBound" Height="40px" RepeatColumns="2">
    </asp:CheckBoxList>    
</div>--%>

<div runat="server" id="divCheckboxList" class="input" style="overflow: auto;height: 140px" onscroll="javascript:setDeparmentScroll();">
    <asp:HiddenField ID="hdnScrollDepartment" runat="server" />
    <telerik:RadLabel ID="lblCheckboxList" runat="server" Text="Label" Visible="false"></telerik:RadLabel>
    <telerik:RadListBox RenderMode="Lightweight" ID="UcDEP" DataTextField="FLDDEPARTMENTNAME" DataValueField="FLDDEPARTMENTID"
        runat="server" CheckBoxes="true" ShowCheckAll="true" SelectionMode="Multiple" OnDataBound="UcDEP_DataBound">
    </telerik:RadListBox>   
</div>