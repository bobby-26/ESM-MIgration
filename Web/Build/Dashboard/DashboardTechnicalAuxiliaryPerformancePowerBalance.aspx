<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalAuxiliaryPerformancePowerBalance.aspx.cs" Inherits="DashboardTechnicalAuxiliaryPerformancePowerBalance" %>
<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
<script type="text/javascript">

    //var pumpIndexData = [
	//	[119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119],
	//	[119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119],
	//	[119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119],
	//	[121, 121, 121, 121, 121, 121, 121, 121, 121, 121, 121, 121],
	//	[120, 120, 120, 120, 120, 120, 120, 120, 120, 120, 120, 120],
	//	[119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119],
	//	[119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119],
	//	[119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119]
    //];

    //var pmaxData = [
	//	[14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14],
	//	[14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14],
	//	[14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14],
	//	[14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14],
	//	[14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14],
	//	[14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14],
	//	[14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14],
	//	[14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14]
    //];

    //var exhaustTempData = [
	//	[285, 285, 285, 285, 285, 285, 285, 285, 285, 285, 285, 285],
	//	[300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300],
	//	[290, 290, 290, 290, 290, 290, 290, 290, 290, 290, 290, 290],
	//	[290, 290, 290, 290, 290, 290, 290, 290, 290, 290, 290, 290],
	//	[290, 290, 290, 290, 290, 290, 290, 290, 290, 290, 290, 290],
	//	[285, 285, 285, 285, 285, 285, 285, 285, 285, 285, 285, 285],
	//	[300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300],
	//	[290, 290, 290, 290, 290, 290, 290, 290, 290, 290, 290, 290]
    //];

    //var cfwData = [
	//	[79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79],
	//	[79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79],
	//	[79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79],
	//	[79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79],
	//	[79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79],
	//	[79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79],
	//	[79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79],
	//	[79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79, 79]
    //];

     pmaxData = <%=this.pmaxData%>;
     pumpIndexData = <%=this.pumpIndexData%>;   
     exhaustTempData = <%=this.exhaustTempData%>;
     cfwData = <%=this.cfwData%>;
     dateseries = <%=this.dateseries%>;

</script>
</telerik:RadCodeBlock>