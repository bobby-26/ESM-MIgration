<%@ Page Language="C#" EnableViewState="false" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    DataSet ds = new DataSet();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ds = PhoenixRegistersVessel.PrintVessel((Filter.CurrentVesselMasterFilter == null) ? 0 : int.Parse(Filter.CurrentVesselMasterFilter));
        }
        catch (Exception ex)
        {
            Response.Write("Refresh the screen and try again!<br/>" + ex.Message );
        }
    }
</script>
<html>
<head>
    <script language="javascript" type="text/javascript">
        function cmdPrint_Click()
        {
            document.getElementById('cmdPrint').style.visibility = "hidden";
            window.print();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <h3><asp:Literal ID="lblVesselParticulars" runat="server" Text="Vessel Particulars"></asp:Literal></h3>
    <table>
        <tr>
            <td>
                <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" />
            </td>
        </tr>
    </table>     
     
    <table border='1' cellpadding='2' cellspacing='0' width='100%'>
        <%        
            DataRow dr = ds.Tables[0].Rows[0];        
        %>
        <tr>
            <td>
                <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDVESSELNAME"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDTYPENAME"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblIMONumber" runat="server" Text="IMO Number"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDIMONUMBER"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblOfficialNumber" runat="server" Text="Official Number"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDOFFICIALNUMBER"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblCallSign" runat="server" Text="Call Sign"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDCALLSIGN"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblMMSINo" runat="server" Text="MMSI No."></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDMMSINUMBER"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblClassification" runat="server" Text="Classification"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDCLASSIFICATION"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblClassNotation" runat="server" Text="Class Notation"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDCLASSNOTATION"]%></b>
            </td>
        </tr>
        <tr>            
            <td>
                <asp:Literal ID="lblVesselShortCode" runat="server" Text="Vessel Short Code"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDVESSELCODE"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblIceClass" runat="server" Text="Ice Class"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDICECLASSED"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblFlag" runat="server" Text="Flag"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDFLAGNAME"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblPortofRegistry" runat="server" Text="Port of Registry"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDPORTNAME"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDOWNERNAME"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblDisponentOwner" runat="server" Text="Disponent Owner"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDDISPONENTOWNERNAME"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblManager" runat="server" Text="Manager"></asp:Literal>
            </td>
            <td>
               <b><% =dr["FLDPRIMARYMANAGERNAME"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblCharterer" runat="server" Text="Charterer"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDCHARTERERNAME"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDPRINCIPALNAME"] %></b>
            </td>
            <td>
                <asp:Literal ID="lblHullNo" runat="server" Text="Hull No."></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDHULLNUMBER"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblKeelLaid" runat="server" Text="Keel Laid"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDDATEENTERED"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblLaunched" runat="server" Text="Launched"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDDATELEFT"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblDelivery" runat="server" Text="Delivery"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDORGDATEENTERED"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblESMTakeover" runat="server" Text="Takeover"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDORGDATELEFT"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblNavigationArea" runat="server" Text="Navigation Area"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDNAVIGATIONAREA"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblServiceSpeed" runat="server" Text="Service Speed"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDSPEED"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblBuilder" runat="server" Text="Builder"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDYARDNAME"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblFittedwithFramo" runat="server" Text="Fitted with Framo"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDFITWITHFRAMOYN"]%></b>
            </td>
        </tr>
    </table>
    <br />
    <table border='1' cellpadding='2' cellspacing='0' width='100%'>
        <tr>
            <td colspan="4">
                <b><asp:Literal ID="lblPrincipalDimensions" runat="server" Text="Principal Dimensions"></asp:Literal></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblLOA" runat="server" Text="LOA"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDLOA"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblLBP" runat="server" Text="LBP"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDLBP"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblBreadthext" runat="server" Text="Breadth (ext)"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDBREADTH"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblDepthmld" runat="server" Text="Depth (mld)"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDDEPTH"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblHeightmax" runat="server" Text="Height (max)"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDHEIGHT"]%></b>
            </td>
        </tr>
    </table>
    <br />
    <table border='1' cellpadding='2' cellspacing='0' width='100%'>
        <tr>
            <td colspan="4">
                <b><asp:Literal ID="lblTonnage" runat="server" Text="Tonnage"></asp:Literal></b>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Literal ID="lblRegistered" runat="server" Text="Registered"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="lblSuez" runat="server" Text="Suez"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="lblPanama" runat="server" Text="Panama"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblGross" runat="server" Text="Gross"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDREGISTEREDGT"]%></b>
            </td>
            <td>
                <b><% =dr["FLDSUEZGT"]%></b>
            </td>
            <td>
                <b><% =dr["FLDPANAMAGT"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblNet" runat="server" Text="Net"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDREGISTEREDNT"]%></b>
            </td>
            <td>
                <b><% =dr["FLDSUEZNT"]%></b>
            </td>
            <td>
                <b><% =dr["FLDPANAMANT"]%></b>
            </td>
        </tr>
    </table>
    <br />
    <table border='1' cellpadding='2' cellspacing='0' width='100%'>
        <tr>
            <td colspan="4">
                <b><asp:Literal ID="lblLoadLine" runat="server" Text="Load Line"></asp:Literal></b>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Literal ID="lblFreeboard" runat="server" Text="Freeboard"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="lblDraft" runat="server" Text="Draft"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="lblDWT" runat="server" Text="DWT"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblTropical" runat="server" Text="Tropical"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDFREEBOARDTROPICAL"]%></b>
            </td>
            <td>
                <b><% =dr["FLDDRAFTTROPICAL"]%></b>
            </td>
            <td>
                <b><% =dr["FLDDWTTROPICAL"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblSummer" runat="server" Text="Summer"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDFREEBOARDSUMMER"]%></b>
            </td>
            <td>
                <b><% =dr["FLDDRAFTSUMMER"]%></b>
            </td>
            <td>
                <b><% =dr["FLDDWTSUMMER"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblWinter" runat="server" Text="Winter"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDFREEBOARDWINTER"]%></b>
            </td>
            <td>
                <b><% =dr["FLDDRAFTWINTER"]%></b>
            </td>
            <td>
                <b><% =dr["FLDDWTWINTER"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblLightship" runat="server" Text="Lightship"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDFREEBOARDLIGHTSHIP"]%></b>
            </td>
            <td>
                <b><% =dr["FLDDRAFTLIGHTSHIP"]%></b>
            </td>
            <td>
                <b><% =dr["FLDDWTLIGHTSHIP"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblBallastCond" runat="server" Text="Ballast Cond"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDFREEBOARDBALLASTCOND"]%></b>
            </td>
            <td>
                <b><% =dr["FLDDRAFTBALLASTCOND"]%></b>
            </td>
            <td>
                <b><% =dr["FLDDWTBALLASTCOND"]%></b>
            </td>
        </tr>
    </table>
    <br />
    <table border='1' cellpadding='2' cellspacing='0' width='100%'>
        <tr>
            <td colspan="4">
                <b><asp:Literal ID="lblMainMachinery" runat="server" Text="Main Machinery"></asp:Literal></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblEngineType" runat="server" Text="Engine Type"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDENGINENAME"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblEngineModel" runat="server" Text="Engine Model"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDENGINEMODEL"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblMainEngine" runat="server" Text="Main Engine"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDMAINENGINE"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblMCR" runat="server" Text="MCR"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDMCR"]%></b>
            </td>
        </tr>
        <tr>
            <td>
               <asp:Literal ID="lblAuxEngine" runat="server" Text="Aux Engine"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDAUXENGINE"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblAuxBoiler" runat="server" Text="Aux Boiler"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDAUXBOILER"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblLifeBoat" runat="server" Text="Life Boat"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDLIFEBOATQUANTITY"]%></b>
            </td>
            <td>
                <asp:Literal ID="lblCapacity" runat="server" Text="Capacity"></asp:Literal>
            </td>
            <td>
                <b><% =dr["FLDLIFEBOATCAPACITY"]%></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
            </td>
            <td colspan="3" style="max-width: 30px">
                <b><% =dr["FLDREMARKS"]%></b>
            </td>
        </tr>
    </table>
    </form>    
</body>
</html>
