<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardCommercialAuxiliaryBoiler.aspx.cs"
    Inherits="Dashboard_DashboardCommercialAuxiliaryBoiler" %>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var msg = <%=this.responseMsg%>;
        if(msg != 'success')
        {
            eval(msg);
        }
        ChartData = <%=this.seriesData%>;
    dateseries = <%=this.dateList%>;


    </script>
</telerik:RadCodeBlock>
