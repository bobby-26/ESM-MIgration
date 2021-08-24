using System;
using SouthNests.Phoenix.Framework;

public partial class VesselPositionSIPDocuments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {        
            
        }
    }

    protected void lnkAnnexer1_Click(object sender, EventArgs e)
    {
        string filePath = Server.MapPath("~/Template/VesselPosition/Appendix 1 - detailed_paper_0_50S_fuel MANBW.pdf");
        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=pdf");
    }

    protected void lnkAppendix2_Click(object sender, EventArgs e)
    {
        string filePath = Server.MapPath("~/Template/VesselPosition/Appendix 2 (Sl2019-671 MANBW).pdf");
        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=pdf");
    }

    protected void lnkAppendix3_Click(object sender, EventArgs e)
    {
        string filePath = Server.MapPath("~/Template/VesselPosition/Appendix 3 - Tank Stripping.doc.pdf");
        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=pdf");
    }

    protected void lnkAppendix4_Click(object sender, EventArgs e)
    {
        string filePath = Server.MapPath("~/Template/VesselPosition/Appendix 4 (Bimco-2020-fuel-transition-clause-for-time-charter-parties).pdf");
        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=pdf");
    }

    protected void lnkAppendix5_Click(object sender, EventArgs e)
    {
        string filePath = Server.MapPath("~/Template/VesselPosition/Appendix 5 (MEPC.1-Circ.864-Rev.1).pdf");
        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=pdf");
    }

    protected void lnkUserManual_Click(object sender, EventArgs e)
    {
        string filePath = Server.MapPath("~/Template/VesselPosition/Phoenix_Ship Implementaiton Plan_User Manual_v2.0.pdf");
        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=pdf");
    }

    protected void lnkSIPGuideLines_Click(object sender, EventArgs e)
    {
        string filePath = Server.MapPath("~/Template/VesselPosition/SIP Manual and Instruction.pdf");
        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=pdf");
    }
}