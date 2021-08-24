using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using Telerik.Web.UI;
using System.Web;

public partial class CrewSummary : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar1 = new PhoenixToolbar();  
        toolbar1.AddFontAwesomeButton("../Crew/CrewSummary.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        MenuShowExcel.AccessRights = this.ViewState;
        MenuShowExcel.MenuList = toolbar1.Show();

        PhoenixToolbar toolbarMenu = new PhoenixToolbar();
        MenuCompanySummary.AccessRights = this.ViewState;
        MenuCompanySummary.MenuList = toolbarMenu.Show();

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
        }
        FillData();
        BindSTSSireData();
    }
    private void BindSTSSireData()
    {
        DataSet ds = PhoenixCrewSummary.ListSTSSireCount(int.Parse(ViewState["empid"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ltSTSValue.Text = ds.Tables[0].Rows[0]["FLDSTSCOUNT"].ToString();
            ltSireValue.Text = ds.Tables[0].Rows[0]["FLDSIRECOUNT"].ToString();
        }
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
                lblExpInMonths.Visible = true;
                DataTable dt2 = ds.Tables[2];

                StringBuilder sb = new StringBuilder();

                sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");

                //Printing the Header

                DataTable dtTempHeader = ds.Tables[0];

                DataRow[] drvHeader = dtTempHeader.Select("FLDROWID = " + dt2.Rows[0]["FLDROWID"].ToString());

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

                sb.Append("<tr style=\"height:10px;\" ><td style=\"height:15px;\"></td></tr>");
                sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

                sb.Append("</table>");

                ltGrid.Text = sb.ToString();
            }
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
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + 8 + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
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
}
