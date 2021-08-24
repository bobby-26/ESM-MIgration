using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOffshore;
using System.Text;
using Telerik.Web.UI;
public partial class CrewOffshoreSummary : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreSummary.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        MenuOprExpExcel.AccessRights = this.ViewState;
        MenuOprExpExcel.MenuList = toolbar1.Show();

        PhoenixToolbar toolbar2 = new PhoenixToolbar();
        toolbar2.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreSummary.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        MenuSplCrgorxp.AccessRights = this.ViewState;
        MenuSplCrgorxp.MenuList = toolbar2.Show();

        PhoenixToolbar toolbar3 = new PhoenixToolbar();
        toolbar3.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreSummary.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        MenuAnchorHandlingexcel.AccessRights = this.ViewState;
        MenuAnchorHandlingexcel.MenuList = toolbar3.Show();


        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            if (Request.QueryString["personalmaster"] != null)
            {
                ViewState["empid"] = Filter.CurrentCrewSelection;
                SetEmployeePrimaryDetails();
            }
            else if (Request.QueryString["newapplicant"] != null)
            {
                ViewState["empid"] = Filter.CurrentNewApplicantSelection;
                SetNewApplicantPrimaryDetails();
            }
            PhoenixCrewOffshoreCrewList.EmployeeExperienceupdate(General.GetNullableInteger(ViewState["empid"].ToString()));
            
        }
        FillData();
        BindOperationalExp();

    }


    private void FillData()
    {
        DataSet ds = new DataSet();

        if (rbVesselTypeRank.Checked == true)
            ds = PhoenixCrewSummary.ListVesselTypeRankSummary(int.Parse(ViewState["empid"].ToString()), rblMonths.Checked == true ? 0 : 1, rbCompany.Checked == true ? 1 : 0);
        if (rbEngineTypeRank.Checked == true)
            ds = PhoenixCrewSummary.ListEngineTypeRankSummary(int.Parse(ViewState["empid"].ToString()), rblMonths.Checked == true ? 0 : 1, rbCompany.Checked == true ? 1 : 0);
        if (rbPrincipalRank.Checked == true)
            ds = PhoenixCrewSummary.ListPrincipalRankSummary(int.Parse(ViewState["empid"].ToString()), rblMonths.Checked == true ? 0 : 1, rbCompany.Checked == true ? 1 : 0);
        if (rbOwnerRank.Checked == true)
            ds = PhoenixCrewSummary.ListOwnerRankSummary(int.Parse(ViewState["empid"].ToString()), rblMonths.Checked == true ? 0 : 1, rbCompany.Checked == true ? 1 : 0);
        if (rbCompanyRank.Checked == true)
            ds = PhoenixCrewSummary.ListCompanyRankSummary(int.Parse(ViewState["empid"].ToString()), rblMonths.Checked == true ? 0 : 1, rbCompany.Checked == true ? 1 : 0);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                //lblExpInMonths.Visible = true;
                DataTable dt2 = ds.Tables[2];

                StringBuilder sb = new StringBuilder();

                sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");

                //Printing the Header

                DataTable dtTempHeader = ds.Tables[0];

                DataRow[] drvHeader = dtTempHeader.Select("FLDROWID = " + dt2.Rows[0]["FLDROWID"].ToString());

                //sb.Append("<tr class=\"DataGrid-HeaderStyle\"><td></td>");
                sb.Append("<tr><td></td>");

                foreach (DataRow drTempHeader in drvHeader)
                {
                    sb.Append("<td align='right'>");
                    sb.Append(drTempHeader["FLDCOLUMNHEADER"].ToString());
                    sb.Append("</td>");
                }

                sb.Append("<td align='right'>Total</td></tr>");

                //Printing the Data

                foreach (DataRow dr in dt2.Rows)
                {
                    DataTable dtTemp = ds.Tables[0];

                    DataRow[] drv = dtTemp.Select("FLDROWID = " + dr["FLDROWID"].ToString());

                    sb.Append("<tr style=\"height:10px;\" ><td align='left'>" + drv[0]["FLDROWHEADER"].ToString() + "</td>");

                    foreach (DataRow drTemp in drv)
                    {
                        sb.Append("<td align='right'>");
                        sb.Append(drTemp["FLDEXPERIENCE"].ToString());
                        sb.Append("</td>");
                    }

                    sb.Append("<td align='right'><b>" + dr["FLDROWEXPERIENCE"].ToString() + "</b></td></tr>");
                }

                sb.Append("<tr><td align='left'><b>Total</b></td>");

                foreach (DataRow drTemp in ds.Tables[1].Rows)
                {
                    sb.Append("<td align='right'><b>");
                    sb.Append(drTemp["FLDCOLUMNEXPERIENCE"].ToString());
                    sb.Append("</b></td>");
                }
                foreach (DataRow drTemp in ds.Tables[3].Rows)
                {
                    sb.Append("<td align='right'><b>");
                    sb.Append(drTemp["FLDROWTOTAL"].ToString());
                    sb.Append("</b></td>");
                }

                sb.Append("</table>");

                ltGrid.Text = sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");

                sb.Append("<tr style=\"height:10px;\"><td style=\"height:15px;\"></td></tr>");
                sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

                sb.Append("</table>");

                ltGrid.Text = sb.ToString();
            }
        }
    }
   

    private void BindOperationalExp()
    {
        try
        {
            DataSet ds = PhoenixCrewSummary.ListofOperationalexp(int.Parse(ViewState["empid"].ToString()), rbCompany.Checked == true ? 1 : 0);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];
            DataTable dt2 = ds.Tables[2];

            gvOprExp.DataSource = ds.Tables[0];

            gvSplCrgorexp.DataSource = ds.Tables[1];

            GvAncrHndlng.DataSource = ds.Tables[2];

           
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void OprExpExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExceloprexp();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    public void SplCrgorxpExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelSplCrgorxp();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void MenuAnchorHandlExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelAnchorHandlexp();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowExceloprexp()
    {
        string[] alColumns = { "FLDOTHEREXPERIANCESUMMARYNAME", "FLDPROCVALUE", "FLDPRVALUE", "FLDOCVALUE", "FLDVALUE" };
        string[] alCaptions = { "Experience", "Other experience for previous ranks", "Company experience for previous ranks", "Other experience for current rank", "Company Experience for current rank" };

        DataSet ds = PhoenixCrewSummary.ListofOperationalexp(int.Parse(ViewState["empid"].ToString()), rbCompany.Checked == true ? 1 : 0);
        DataTable dt = ds.Tables[0];


        if (ds.Tables[0].Rows.Count > 0)
        {

            Response.AddHeader("Content-Disposition", "attachment; filename=Operational Experience.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Operational Experience</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td width='20%'>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in dt.Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
    }

    private void ShowExcelSplCrgorxp()
    {
        string[] alColumns = { "FLDOTHEREXPERIANCESUMMARYNAME", "FLDPROCVALUE", "FLDPRVALUE", "FLDOCVALUE", "FLDVALUE" };
        string[] alCaptions = { "Experience", "Other experience for previous ranks", "Company experience for previous ranks", "Other experience for current rank", "Company Experience for current rank" };

        DataSet ds = PhoenixCrewSummary.ListofOperationalexp(int.Parse(ViewState["empid"].ToString()), rbCompany.Checked == true ? 1 : 0);
        DataTable dt1 = ds.Tables[1];
        if (ds.Tables[1].Rows.Count > 0)
        {
            Response.AddHeader("Content-Disposition", "attachment; filename=Special Cargo Handling Experience.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Special Cargo Handling Experience</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td width='20%'>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in dt1.Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
    }
    private void ShowExcelAnchorHandlexp()
    {
        string[] alColumns = { "FLDOTHEREXPERIANCESUMMARYNAME", "FLDPROCVALUE", "FLDPRVALUE", "FLDOCVALUE", "FLDVALUE" };
        string[] alCaptions = { "Experience", "Other experience for previous ranks", "Company experience for previous ranks", "Other experience for current rank", "Company Experience for current rank" };

        DataSet ds = PhoenixCrewSummary.ListofOperationalexp(int.Parse(ViewState["empid"].ToString()), rbCompany.Checked == true ? 1 : 0);
        DataTable dt2 = ds.Tables[2];
        if (ds.Tables[2].Rows.Count > 0)
        {
            Response.AddHeader("Content-Disposition", "attachment; filename=Anchor Handling Experience.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Anchor Handling Experience</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td width='20%'>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in dt2.Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }

    }
    private void ShowExcel()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<tr><td colspan='4' align='left'><b>Compay Expericence in Month</b></td></tr>");
        sb.Append("<tr><td colspan='4' align='left'><b>Vessel Type-Rank</td></b></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListVesselTypeRankSummary(int.Parse(ViewState["empid"].ToString()), 0, 0)));
        sb.Append("<tr><td colspan='4' align='left'><b>Engine Type-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListEngineTypeRankSummary(int.Parse(ViewState["empid"].ToString()), 0, 0)));
        sb.Append("<tr><td colspan='4' align='left'><b>Principal-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListPrincipalRankSummary(int.Parse(ViewState["empid"].ToString()), 0, 0)));
        sb.Append("<tr><td colspan='4' align='left'><b>Owner-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListOwnerRankSummary(int.Parse(ViewState["empid"].ToString()), 0, 0)));
        sb.Append("<tr><td colspan='4' align='left'><b>Company-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListCompanyRankSummary(int.Parse(ViewState["empid"].ToString()), 0, 0)));

        sb.Append("<tr><td colspan='4' align='left'><b>Full Expericence in Month</b><br/></td></tr>");
        sb.Append("<tr><td colspan='4' align='left'><b>Vessel Type-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListVesselTypeRankSummary(int.Parse(ViewState["empid"].ToString()), 1, 0)));
        sb.Append("<tr><td colspan='4' align='left'><b>Engine Type-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListEngineTypeRankSummary(int.Parse(ViewState["empid"].ToString()), 1, 0)));
        sb.Append("<tr><td colspan='4' align='left'><b>Principal-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListPrincipalRankSummary(int.Parse(ViewState["empid"].ToString()), 1, 0)));
        sb.Append("<tr><td colspan='4' align='left'><b>Owner-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListOwnerRankSummary(int.Parse(ViewState["empid"].ToString()), 1, 0)));
        sb.Append("<tr><td colspan='4' align='left'><b>Company-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListCompanyRankSummary(int.Parse(ViewState["empid"].ToString()), 1, 0)));

        sb.Append("<tr><td colspan='4' align='left'><b>Compay Expericence in Decimal</b><br/></td></tr>");
        sb.Append("<tr><td colspan='4' align='left'><b>Vessel Type-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListVesselTypeRankSummary(int.Parse(ViewState["empid"].ToString()), 0, 1)));
        sb.Append("<tr><td colspan='4' align='left'><b>Engine Type-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListEngineTypeRankSummary(int.Parse(ViewState["empid"].ToString()), 0, 1)));
        sb.Append("<tr><td colspan='4' align='left'><b>Principal-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListPrincipalRankSummary(int.Parse(ViewState["empid"].ToString()), 0, 1)));
        sb.Append("<tr><td colspan='4' align='left'><b>Owner-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListOwnerRankSummary(int.Parse(ViewState["empid"].ToString()), 0, 1)));
        sb.Append("<tr><td colspan='4' align='left'><b>Company-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListCompanyRankSummary(int.Parse(ViewState["empid"].ToString()), 0, 1)));

        sb.Append("<tr><td colspan='4' align='left'><b>Full Expericence in Month</b><br/></td></tr>");
        sb.Append("<tr><td colspan='4' align='left'><b>Vessel Type-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListVesselTypeRankSummary(int.Parse(ViewState["empid"].ToString()), 1, 1)));
        sb.Append("<tr><td colspan='4' align='left'><b>Engine Type-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListEngineTypeRankSummary(int.Parse(ViewState["empid"].ToString()), 1, 1)));
        sb.Append("<tr><td colspan='4' align='left'><b>Principal-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListPrincipalRankSummary(int.Parse(ViewState["empid"].ToString()), 1, 1)));
        sb.Append("<tr><td colspan='4' align='left'><b>Owner-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListOwnerRankSummary(int.Parse(ViewState["empid"].ToString()), 1, 1)));
        sb.Append("<tr><td colspan='4' align='left'><b>Company-Rank</b></td></tr>");
        sb.Append(GenerateHistory(PhoenixCrewSummary.ListCompanyRankSummary(int.Parse(ViewState["empid"].ToString()), 1, 1)));
        Response.Clear();
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + 8 + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /><h5><center> SHIP MANAGEMENT</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + 8 + "'><h5><center>Crew Summary</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + 8 + "' align='left'>Date:" + DateTime.Now.ToShortDateString() + "</td></tr>");
        Response.Write("<tr><td colspan='" + 8 + "'>&nbsp;</td>");
        Response.AddHeader("Content-Disposition", "attachment; filename=CrewSummary.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write(sb.ToString());
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.End();
    }

    private StringBuilder GenerateHistory(DataSet ds)
    {
        StringBuilder sb = new StringBuilder();

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblExpInMonths.Visible = true;
                DataTable dt2 = ds.Tables[2];

                sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" border=\"1\"> ");

                //Printing the Header

                DataTable dtTempHeader = ds.Tables[0];

                DataRow[] drvHeader = dtTempHeader.Select("FLDROWID = " + dt2.Rows[0]["FLDROWID"].ToString());

                sb.Append("<tr><td></td>");

                foreach (DataRow drTempHeader in drvHeader)
                {
                    sb.Append("<td style='font-family:Arial; font-size:10px;' width='20%' align='right'><b>");
                    sb.Append(drTempHeader["FLDCOLUMNHEADER"].ToString());
                    sb.Append("</b></td>");
                }

                sb.Append("<td style='font-family:Arial; font-size:10px;' width='20%' align='right'><b>Total</b></td></tr>");

                //Printing the Data

                foreach (DataRow dr in dt2.Rows)
                {
                    DataTable dtTemp = ds.Tables[0];

                    DataRow[] drv = dtTemp.Select("FLDROWID = " + dr["FLDROWID"].ToString());

                    sb.Append("<tr><td style='font-family:Arial; font-size:10px;' align='left'>" + drv[0]["FLDROWHEADER"].ToString() + "</td>");

                    foreach (DataRow drTemp in drv)
                    {
                        sb.Append("<td align='right'>");
                        sb.Append(drTemp["FLDEXPERIENCE"].ToString());
                        sb.Append("</td>");
                    }

                    sb.Append("<td style='font-family:Arial; font-size:12px;' align='right'><b>" + dr["FLDROWEXPERIENCE"].ToString() + "</b></td></tr>");
                }

                sb.Append("<tr><td style='font-family:Arial; font-size:12px;' align='left'><b>Total</b></td>");

                foreach (DataRow drTemp in ds.Tables[1].Rows)
                {
                    sb.Append("<td style='font-family:Arial; font-size:12px;'align='right'><b>");
                    sb.Append(drTemp["FLDCOLUMNEXPERIENCE"].ToString());
                    sb.Append("</b></td>");
                }
                foreach (DataRow drTemp in ds.Tables[3].Rows)
                {
                    sb.Append("<td style='font-family:Arial; font-size:12px;'align='right'><b>");
                    sb.Append(drTemp["FLDROWTOTAL"].ToString());
                    sb.Append("</b></td>");
                }

                sb.Append("</table>");
                ltGrid.Text = sb.ToString();
            }
            else
            {
                sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" border=\"1\"> ");
                sb.Append("<tr><td style='font-family:Arial; font-size:10px;'></td></tr>");
                sb.Append("<tr><td style='font-family:Arial; font-size:10px;' align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");
                sb.Append("</table>");

                ltGrid.Text = sb.ToString();
            }
        }
        return sb;
    }

    public void SetNewApplicantPrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(ViewState["empid"].ToString()));

            tdempno.Visible = false;
            tdempnum.Visible = false;

            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(ViewState["empid"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvOprExp_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindOperationalExp();
    }

    protected void gvSplCrgorexp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindOperationalExp();
    }

    protected void GvAncrHndlng_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindOperationalExp();
    }
}
