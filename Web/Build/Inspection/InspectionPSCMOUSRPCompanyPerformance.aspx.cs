using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Data;
using Telerik.Web.UI;
using System.Drawing;

public partial class Inspection_InspectionPSCMOUSRPCompanyPerformance : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Calculate", "SAVE", ToolBarDirection.Right);
        MenuShipRiskProfile.AccessRights = this.ViewState;
        MenuShipRiskProfile.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            if (Request.QueryString["processid"] != null)
                ViewState["processid"] = Request.QueryString["processid"].ToString();
            else
                ViewState["processid"] = "";

            ViewState["COMPANYID"] = "";
            BindPSCMOU();
            BindCompanyParameters();

        }

    }

    protected void BindPSCMOU()
    {
        ddlCompany.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        ddlCompany.DataTextField = "FLDCOMPANYNAME";
        ddlCompany.DataValueField = "FLDCOMPANYID";
        ddlCompany.DataBind();        
        ddlCompany.SelectedValue = "1";
    }

    private void BindData()
    {


        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDTYPEDESCRIPTION", "FLDPSCMOU", "FLDSCORE" };
        string[] alCaptions = { "Ship Type", "PSC MOU", "Weightage" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionPSCMOUMatrix.PSCMOUCompanyPerformanceScoreSearch(
            General.GetNullableGuid(ddlCompany.SelectedValue),
            sortexpression, sortdirection,
            gvMenuPSCMOU.CurrentPageIndex + 1,
            gvMenuPSCMOU.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvMenuPSCMOU", "Company Performance ", alCaptions, alColumns, ds);

        gvMenuPSCMOU.DataSource = ds;
        gvMenuPSCMOU.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvMenuPSCMOU_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
            //BindCompanyParameters();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlCompany_TextChanged(object sender, EventArgs e)
    {
        BindCompanyParameters();
        gvMenuPSCMOU.Rebind();
    }

    protected void gvMenuPSCMOU_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataBoundItem1 = e.Item as GridDataItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            Label lblDefindex = (Label)e.Item.FindControl("lblDefindex");

            if (lblDefindex.Text == "Below Average")              //Below Average
            {
                lblDefindex.ForeColor = Color.DarkGreen;
            }
            if (lblDefindex.Text == "Above Average")              //Below Average
            {
                lblDefindex.ForeColor = Color.Red;
            }
            if (lblDefindex.Text == "Average")              //Below Average
            {
                lblDefindex.ForeColor = Color.Orange;
            }
            Label lblDetindex = (Label)e.Item.FindControl("lblDetindex");

            if (lblDetindex.Text == "Below Average")              //Below Average
            {
                lblDetindex.ForeColor = Color.DarkGreen;
            }
            if (lblDetindex.Text == "Above Average")              //Below Average
            {
                lblDetindex.ForeColor = Color.Red;
            }
            if (lblDetindex.Text == "Average")              //Below Average
            {
                lblDetindex.ForeColor = Color.Orange;
            }
            Label lblCompanygperf = (Label)e.Item.FindControl("lblCompanygperf");

            if (lblCompanygperf.Text == "High")              //High
            {
                lblCompanygperf.ForeColor = Color.DarkGreen;
            }
            if (lblCompanygperf.Text == "Low" || lblCompanygperf.Text == "Very Low")              //Low
            {
                lblCompanygperf.ForeColor = Color.Red;
            }
            if (lblCompanygperf.Text == "Medium")              //Medium
            {
                lblCompanygperf.ForeColor = Color.Orange;
            }

            //GridDecorator.MergeRows(gvMenuPSCMOU, e);

        }
    }

    protected void MenuShipRiskProfile_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixInspectionPSCMOUMatrix.PSCMOUSRPCompanyPerformanceInsert(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                     General.GetNullableInteger(txtnoofinsp.Text),
                                                     General.GetNullableInteger(txtnoofdetention.Text),
                                                     General.GetNullableInteger(txtnoofnonismdef.Text),
                                                     General.GetNullableInteger(txtnoofismdef.Text),
                                                     General.GetNullableInteger(rblrefusal.SelectedValue),
                                                     General.GetNullableGuid(ddlCompany.SelectedValue)
                                                     //ref colorscoreid
                                                     );
                BindCompanyParameters();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCompanyParameters()
    {
        DataSet ds = PhoenixInspectionPSCMOUMatrix.SRPCompanyPerformanceEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ddlCompany.SelectedValue));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtnoofinsp.Text = dr["FLDNOOFINSPECTION"].ToString();
            txtnoofdetention.Text = dr["FLDNOOFDETENTION"].ToString();
            txtnoofnonismdef.Text = dr["FLDNOOFNONISMDEF"].ToString();
            txtnoofismdef.Text = dr["FLDNOOFISMDEF"].ToString();
            txtcompdetratio.Text = dr["FLDDETINDEXRATIO"].ToString();
            txtcompdefratio.Text = dr["FLDDEFINDEXRATIO"].ToString();
            txtdetmouavg.Text = dr["FLDDETMOUAVERAGE"].ToString();
            txtavgdef.Text = dr["FLDDEFMOUAVERAGE"].ToString();
            lbldetresult.Text = dr["FLDDETENTIONINDEX"].ToString();
            lbldefindexresult.Text = dr["FLDDEFICIENCYINDEX"].ToString();
            lblcompperf.Text = dr["FLDCOMPANYPERFORMANCE"].ToString();
            lbldeflimitaboverange.Text = dr["FLDDEFABOVELIMIT"].ToString();
            lbldeflimitbelowrange.Text = dr["FLDDEFBELOWLIMIT"].ToString();
            lbldetabvelimit.Text = dr["FLDDETABOVELIMIT"].ToString();
            lbldetbelowlimit.Text = dr["FLDDETBELOWLIMIT"].ToString();

            if (dr["FLDDEFINDEX"].ToString() == "3")              //Below Average
            {
                lbldefindexresult.ForeColor = Color.DarkGreen;
            }
            if (dr["FLDDEFINDEX"].ToString() == "2")              //Above Average
            {
                lbldefindexresult.ForeColor = Color.Red;
            }
            if (dr["FLDDEFINDEX"].ToString() == "1")              // Average
            {
                lbldefindexresult.ForeColor = Color.Orange;
            }           

            if (dr["FLDDETINDEX"].ToString() == "3")              //Below Average
            {
                lbldetresult.ForeColor = Color.DarkGreen;
            }
            if (dr["FLDDETINDEX"].ToString() == "2")              //Below Average
            {
                lbldetresult.ForeColor = Color.Red;
            }
            if (dr["FLDDETINDEX"].ToString() == "1")              //Below Average
            {
                lbldetresult.ForeColor = Color.Orange;
            }           

            if (dr["FLDCOMPANYPERF"].ToString() == "1")              //High
            {
                lblcompperf.ForeColor = Color.DarkGreen;
            }
            if (dr["FLDCOMPANYPERF"].ToString() == "2" || dr["FLDCOMPANYPERF"].ToString() == "4")              //Low
            {
                lblcompperf.ForeColor = Color.Red;
            }
            if (dr["FLDCOMPANYPERF"].ToString() == "3")              //Medium
            {
                lblcompperf.ForeColor = Color.Orange;
            }
            idtooltip.ToolTip = "(No. of Detention/No. of Inspection)*100";
            iddeftooltip.ToolTip = "((No. of ISM def * 5) + (No. non ISM def * 1))/No. of Inspection";
        }

    }
}