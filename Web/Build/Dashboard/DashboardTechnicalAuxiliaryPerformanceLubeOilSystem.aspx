<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalAuxiliaryPerformanceLubeOilSystem.aspx.cs" Inherits="DashboardTechnicalAuxiliaryPerformanceLubeOilSystem" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
<script type="text/javascript">
        seriesData =<%=this.seriesData%>;
        dateseries = <%=this.dateseries%>;
</script>
</telerik:RadCodeBlock>