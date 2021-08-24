<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalAuxiliaryPerformanceSFOC.aspx.cs"
    Inherits="DashboardTechnicalAuxiliaryPerformanceSFOC" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
<script type="text/javascript">
     seriesData = <%=this.SeriesData%>;
     dateseries = <%=this.dateseries%>;
    shoptestSfoc = <%=this.shoptestSFOC%>;
    seriesData2 = <%=this.SeriesData1%>;
    seriesData3 = <%=this.SeriesData2%>;
    seriesData4 = <%=this.overhauldata%>;
    seriesData5 = <%=this.cwEngineData%>;

</script>
    </telerik:RadCodeBlock>