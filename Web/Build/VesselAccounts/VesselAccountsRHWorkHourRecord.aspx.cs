using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Generic;
using Telerik.Web.UI;
public partial class VesselAccountsRHWorkHourRecord : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Close", "CLOSE", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuWorkHour.AccessRights = this.ViewState;
            MenuWorkHour.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["EMPID"] = null;
                ViewState["RHSTARTID"] = null;
                ViewState["CALENDERID"] = null;

                if (Request.QueryString["EMPID"] != null)
                    ViewState["EMPID"] = Request.QueryString["EMPID"].ToString();

                if (Request.QueryString["RHStartId"] != null)
                    ViewState["RHSTARTID"] = Request.QueryString["RHStartId"].ToString();

                if (Request.QueryString["CalenderId"] != null)
                    ViewState["CALENDERID"] = Request.QueryString["CalenderId"].ToString();

                if (Request.QueryString["SHIPCALENDERID"] != null)
                    ViewState["SHIPCALENDERID"] = Request.QueryString["SHIPCALENDERID"].ToString();

                if (ViewState["CALENDERID"] != null)
                    BindDetails();
                BindPreviousDay();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuWorkHour_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixVesselAccountsRH.WorkHoursRemarksUpdate(
                    General.GetNullableGuid(ViewState["CALENDERID"].ToString())
                    , int.Parse(ViewState["SHIPCALENDERID"].ToString())
                    , txtRemarks.Text);

                ucStatus.Text = "Remarks updated successfully.";
            }
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }
    private void BindPreviousDay()
    {
        DataSet ds = PhoenixVesselAccountsRH.PreviousDayWorkHourEdit
                  (new Guid(ViewState["CALENDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblprevoius.Text = "Previous Day - Hours of Work " + ds.Tables[0].Rows[0]["FLDWORKPLACENAME"].ToString();
                PreSeaWSheet.TotalHours = decimal.Parse(ds.Tables[0].Rows[0]["FLDHOURPERDAY"].ToString());
                PreSeaWSheet.TWorkHours = decimal.Parse(ds.Tables[0].Rows[0]["FLDWH"].ToString());
                PreSeaWSheet.TRestHours = decimal.Parse(ds.Tables[0].Rows[0]["FLDRH"].ToString());
                PreSeaWSheet.FieldToBind = "FLDHOURSATSEA";
                PreSeaWSheet.FieldValue = "FLDRESTHOURWORKID";
                PreSeaWSheet.SetTimeList((DataTable)ds.Tables[0]);
            }
        }
    }
    private void BindDetails()
    {
        try
        {
            DataSet ds = PhoenixVesselAccountsRH.RHWorkCalenderEdit(new Guid(ViewState["CALENDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtEmpName.Text = dr["FLDNAME"].ToString();
                ucRank.SelectedRank = dr["FLDRANKID"].ToString();
                txtDate.Text = string.Format("{0:dd/MMM/yyyy}", dr["FLDDATE"].ToString());
                txtHour.Text = dr["FLDHOURS"].ToString();
                txtReportingday.Text = dr["FLDREPORTINGDAY"].ToString();

                rbnhourchange.SelectedValue = dr["FLDADVANCERETARD"].ToString();
                if (dr["FLDHALFHOURYN"].ToString() != "0" && dr["FLDHALFHOURYN"].ToString() != "")
                    rbnhourvalue.SelectedValue = dr["FLDHALFHOURYN"].ToString();
                rbtnadvanceretard.SelectedValue = dr["FLDCLOCK"].ToString();
                rbtnworkplace.SelectedValue = dr["FLDWORKPLACE"].ToString();
                lblCurrent.Text = "Hours of Work " + dr["FLDWORKPLACENAME"].ToString();
                ViewState["TOTALHOURS"] = dr["FLDTOTALHOURS"].ToString();
                txtRemarks.Text = dr["FLDWORKHOURSREMARK"].ToString();
            }

            ds = PhoenixVesselAccountsRH.WorkHourEdit(new Guid(ViewState["CALENDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(txtHour.Text))
                    SeaWSheet.TotalHours = decimal.Parse(txtHour.Text);
                SeaWSheet.TWorkHours = ViewState["TOTALHOURS"] != null ? decimal.Parse(ViewState["TOTALHOURS"].ToString()) : 0;
                SeaWSheet.TRestHours = ViewState["TOTALHOURS"] != null ? (decimal.Parse(txtHour.Text)) - (decimal.Parse(ViewState["TOTALHOURS"].ToString())) : 0;
                SeaWSheet.FieldToBind = "FLDHOURSATSEA";
                SeaWSheet.FieldValue = "FLDRESTHOURWORKID";
                SeaWSheet.SetTimeList((DataTable)ds.Tables[0]);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixVesselAccountsRH.ComplianceList(new Guid(ViewState["CALENDERID"].ToString()), int.Parse(ViewState["EMPID"].ToString()));

            gvWorkHourRecord.DataSource = ds;
            gvWorkHourRecord.DataBind();

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkHourRecord_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            RadLabel lblhour = (RadLabel)e.Item.FindControl("lblHour");
            int a = 0;
            if (lblhour != null)//SHOWING HOUR TEXT AS '00-01','01-02'......
            {
                int.TryParse(lblhour.Text, out a);
                lblhour.Text = (a - 1).ToString().PadLeft(2, '0') + "-" + lblhour.Text.PadLeft(2, '0');
            }

            Image S1 = (Image)e.Item.FindControl("imgS1");
            Image S2 = (Image)e.Item.FindControl("imgS2");
            Image S3 = (Image)e.Item.FindControl("imgS3");
            Image S4 = (Image)e.Item.FindControl("imgS4");
            Image S5 = (Image)e.Item.FindControl("imgS5");
            Image O1 = (Image)e.Item.FindControl("imgO1");
            Image O2 = (Image)e.Item.FindControl("imgO2");
            RadLabel lblexception = (RadLabel)e.Item.FindControl("lblException");
            UserControlToolTip uct1 = (UserControlToolTip)e.Item.FindControl("ucToolTipnc1");
            UserControlToolTip uct2 = (UserControlToolTip)e.Item.FindControl("ucToolTipnc2");
            UserControlToolTip uct3 = (UserControlToolTip)e.Item.FindControl("ucToolTipnc3");
            UserControlToolTip uct4 = (UserControlToolTip)e.Item.FindControl("ucToolTipnc4");
            UserControlToolTip uct5 = (UserControlToolTip)e.Item.FindControl("ucToolTipnc5");
            UserControlToolTip uct6 = (UserControlToolTip)e.Item.FindControl("ucToolTipnc6");
            UserControlToolTip uct7 = (UserControlToolTip)e.Item.FindControl("ucToolTipnc7");

            if (S1 != null && lblexception != null && uct1 != null)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string s = drv["FLDNONCOMPLIANCE"].ToString();
                string[] legend = drv["FLDLEGEND"].ToString().TrimEnd(',').Split(',');
                string[] exp = lblexception.Text.ToString().Replace("<BR/>", "#").Split('#');
                List<string> nc = new List<string>();
                List<int> opa = new List<int>();
                for (int c = 0; c < exp.Length; c++)
                    nc.Add(exp[c].ToString());

                for (int i = 0; i < legend.Length; i++)
                {
                    if (legend[i].ToString().ToUpper() == "S1")
                    {
                        S1.ImageUrl = Session["images"] + "/case1.png";
                        uct1.Text = nc[i].ToString();
                        //S1.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct1.ToolTip + "', 'visible');");
                        //S1.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct1.ToolTip + "', 'hidden');");
                        uct1.Position = ToolTipPosition.TopCenter;
                        uct1.TargetControlId = S1.ClientID;
                    }
                    if (legend[i].ToString().ToUpper() == "S2")
                    {
                        S2.ImageUrl = Session["images"] + "/case2.png";
                        uct2.Text = nc[i].ToString();
                        //S2.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct2.ToolTip + "', 'visible');");
                        //S2.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct2.ToolTip + "', 'hidden');");
                        uct2.Position = ToolTipPosition.TopCenter;
                        uct2.TargetControlId = S2.ClientID;
                    }
                    if (legend[i].ToString().ToUpper() == "S3")
                    {
                        S3.ImageUrl = Session["images"] + "/case3.png";
                        uct3.Text = nc[i].ToString();
                        //S3.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct3.ToolTip + "', 'visible');");
                        //S3.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct3.ToolTip + "', 'hidden');");
                        uct3.Position = ToolTipPosition.TopCenter;
                        uct3.TargetControlId = S3.ClientID;
                    }
                    if (legend[i].ToString().ToUpper() == "S4")
                    {
                        S4.ImageUrl = Session["images"] + "/case4.png";
                        uct4.Text = nc[i].ToString();
                        //S4.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct4.ToolTip + "', 'visible');");
                        //S4.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct4.ToolTip + "', 'hidden');");
                        uct4.Position = ToolTipPosition.TopCenter;
                        uct4.TargetControlId = S4.ClientID;
                    }
                    if (legend[i].ToString().ToUpper() == "S5")
                    {
                        S5.ImageUrl = Session["images"] + "/case5.png";
                        uct5.Text = nc[i].ToString();
                        //S5.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct5.ToolTip + "', 'visible');");
                        //S5.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct5.ToolTip + "', 'hidden');");
                        uct5.Position = ToolTipPosition.TopCenter;
                        uct5.TargetControlId = S5.ClientID;
                    }
                    if (legend[i].ToString().ToUpper() == "O1")
                    {
                        O1.ImageUrl = Session["images"] + "/case6.png";
                        uct6.Text = nc[i].ToString();
                        //O1.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct6.ToolTip + "', 'visible');");
                        //O1.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct6.ToolTip + "', 'hidden');");
                        uct6.Position = ToolTipPosition.TopCenter;
                        uct6.TargetControlId = O1.ClientID;
                    }
                    if (legend[i].ToString().ToUpper() == "O2")
                    {
                        O2.ImageUrl = Session["images"] + "/case7.png";
                        uct7.Text = nc[i].ToString();
                        //O2.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct7.ToolTip + "', 'visible');");
                        //O2.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct7.ToolTip + "', 'hidden');");
                        uct7.Position = ToolTipPosition.TopCenter;
                        uct7.TargetControlId = O2.ClientID;
                    }
                }
            }
        }
    }
    protected void SeaWSheet_OnTimeStripCommand(object sender, EventArgs e)
    {
        try
        {
            PhoenixVesselAccountsRH.SeaWorkHourUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        new Guid(SeaWSheet.Id),
                                                        SeaWSheet.WorkHours,
                                                        PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                        int.Parse(ViewState["SHIPCALENDERID"].ToString()),
                                                        int.Parse(ViewState["MONTHID"].ToString()),
                                                        int.Parse(ViewState["YEAR"].ToString())
                                                        );
            updateworkhourRule(new Guid(SeaWSheet.Id), int.Parse(ViewState["EMPID"].ToString()));

            BindDetails();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindDetails();
            BindData();
        }
    }
    private void updateworkhourRule(Guid WorkHourID, int empid)
    {
        try
        {
            PhoenixVesselAccountsRH.WorkHourRuleUpdate(WorkHourID, empid);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
